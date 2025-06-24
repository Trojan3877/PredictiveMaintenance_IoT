// src/DataIngest.cs
//────────────────────────────────────────────────────────────────────────────
// Consumes high-frequency sensor messages from Apache Kafka (or MQTT fallback),
// validates the payload, and writes a normalized record to the local ring buffer.
//
//  • Designed for 50 k msg/s per pod on minimal CPU.
//  • Replace <KAFKA_BROKER> env var at runtime.
//  • Unit-test friendly: IKafkaConsumer interface injected.
//────────────────────────────────────────────────────────────────────────────

using System;
using System.Text.Json;
using System.Threading;
using Confluent.Kafka;

namespace PredictiveMaintenance_IoT
{
    public record SensorMessage(long AssetId, DateTime Timestamp, double Vibration, double Temperature);

    public interface IMessageSink
    {
        void Write(SensorMessage msg);
    }

    public interface IKafkaConsumer : IDisposable
    {
        void Subscribe(string topic);
        ConsumeResult<string, string>? Poll(TimeSpan timeout);
    }

    public class KafkaConsumerWrapper : IKafkaConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        public KafkaConsumerWrapper()
        {
            var cfg = new ConsumerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKER") ?? "localhost:9092",
                GroupId = "predmaint_group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(cfg).Build();
        }

        public void Subscribe(string topic) => _consumer.Subscribe(topic);

        public ConsumeResult<string, string>? Poll(TimeSpan timeout) => _consumer.Consume(timeout);

        public void Dispose() => _consumer.Close();
    }

    public class DataIngest
    {
        private readonly IMessageSink _sink;
        private readonly IKafkaConsumer _consumer;
        private readonly CancellationToken _token;

        public DataIngest(IMessageSink sink, IKafkaConsumer consumer, CancellationToken token)
        {
            _sink = sink;
            _consumer = consumer;
            _token = token;
        }

        public void Run(string topic = "sensor-stream")
        {
            _consumer.Subscribe(topic);
            Console.WriteLine($"📡 Listening on topic '{topic}'…");

            while (!_token.IsCancellationRequested)
            {
                var cr = _consumer.Poll(TimeSpan.FromMilliseconds(200));
                if (cr == null) continue;

                try
                {
                    var msg = JsonSerializer.Deserialize<SensorMessage>(cr.Value);
                    if (msg is null) continue;

                    // Basic validation
                    if (msg.Vibration < 0 || msg.Temperature < -50 || msg.Temperature > 150)
                        continue;

                    _sink.Write(msg);
                }
                catch (JsonException e)
                {
                    Console.Error.WriteLine($"Malformed JSON: {e.Message}");
                }
            }
        }
    }
}
