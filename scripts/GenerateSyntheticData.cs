// scripts/GenerateSyntheticData.cs
//────────────────────────────────────────────────────────────────────────────
// Generates a configurable stream of SensorMessage JSON objects and publishes
// them to Apache Kafka.  Use --stdout flag to print to console instead.
//
// Build & run:
//   dotnet build -o out scripts/GenerateSyntheticData.csproj
//   dotnet out/GenerateSyntheticData.dll --broker localhost:9092 --rate 100 --stdout
//────────────────────────────────────────────────────────────────────────────

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using PredictiveMaintenance_IoT;

const string topic = "sensor-stream";
int    rate   = 100;      // msgs per second
string broker = "localhost:9092";
bool   stdout = false;

foreach (var a in args)
{
    if (a.StartsWith("--rate="))    rate   = int.Parse(a.Split("=")[1]);
    if (a.StartsWith("--broker="))  broker = a.Split("=")[1];
    if (a == "--stdout")            stdout = true;
}

Console.WriteLine($"🌡  Generating synthetic sensor data → {(stdout ? "stdout" : broker)} ({rate} msg/s)");

IProducer<string, string>? producer = null;
if (!stdout)
{
    producer = new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = broker }).Build();
}

var rnd = new Random();
long assetCount = 50;

var delay = TimeSpan.FromMilliseconds(1000.0 / rate);
var cts   = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

while (!cts.IsCancellationRequested)
{
    long assetId = rnd.NextInt64(1, assetCount);
    double vibration  = Math.Round(rnd.NextDouble() * 4 + 10, 2);   // 10-14 mm/s
    double temperature = Math.Round(rnd.NextDouble() * 10 + 45, 2); // 45-55 °C

    var msg = new SensorMessage(assetId, DateTime.UtcNow, vibration, temperature);
    string json = JsonSerializer.Serialize(msg);

    if (stdout)
        Console.WriteLine(json);
    else
        await producer!.ProduceAsync(topic, new Message<string, string> { Key = assetId.ToString(), Value = json });

    await Task.Delay(delay, cts.Token);
}

producer?.Flush();
Console.WriteLine("Generator stopped.");
