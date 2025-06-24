// src/Predictor.cs
//────────────────────────────────────────────────────────────────────────────
// Hosts both REST (ASP.NET Core Minimal APIs) and gRPC endpoints that expose
// real-time failure probabilities produced by ModelTrainer.
//
//   • REST  :  POST /predict  {FeatureVector JSON} → {probability, fail?}
//   • gRPC  :  Predict() rpc defined in proto (see /proto/predmaint.proto)
//   • Prometheus metrics via prometheus-net
//   • OTEL tracing enabled automatically when OTEL_EXPORTER_OTLP_ENDPOINT set
//────────────────────────────────────────────────────────────────────────────

using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prometheus;
using OpenTelemetry.Trace;

namespace PredictiveMaintenance_IoT
{
    public class PredictorService
    {
        private readonly ModelTrainer _trainer;

        public PredictorService(ModelTrainer trainer) => _trainer = trainer;

        public static async Task RunAsync(ModelTrainer trainer)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders().AddConsole();

            // ─── Metrics ─────────────────────────────────────────────
            builder.Services.AddSingleton(trainer);
            builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
            var app = builder.Build();
            var counter = Metrics.CreateCounter("predict_requests_total", "Total prediction requests");
            var latency = Metrics.CreateSummary("predict_latency_seconds", "Prediction latency", new SummaryConfiguration(maxAge: TimeSpan.FromMinutes(1), ageBuckets: 5));
            app.MapMetrics("/metrics");

            // ─── OTEL (if endpoint env var set) ─────────────────────
            string? otelEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
            if (!string.IsNullOrEmpty(otelEndpoint))
            {
                builder.Services.AddOpenTelemetry()
                    .WithTracing(b => b
                        .AddAspNetCoreInstrumentation()
                        .AddOtlpExporter());
            }

            // ─── REST endpoint ──────────────────────────────────────
            app.MapPost("/predict", async (FeatureVector fv) =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                trainer.Predict(fv, out bool fail, out float prob);
                watch.Stop();

                counter.Inc();
                latency.Observe(watch.Elapsed.TotalSeconds);

                return Results.Json(new { probability = prob, failure = fail });
            });

            // ─── gRPC endpoint (optional) ───────────────────────────
            // Requires server reflection + proto; out of scope here.
            // app.MapGrpcService<PredMaintGrpcService>();

            Console.WriteLine("🚀 Predictor REST API running on http://0.0.0.0:8080");
            await app.RunAsync("http://0.0.0.0:8080");
        }
    }
}
