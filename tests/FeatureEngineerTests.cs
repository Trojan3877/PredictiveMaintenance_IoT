// tests/FeatureEngineerTests.cs
//────────────────────────────────────────────────────────────────────────────
// Validates rolling mean/std/z-score logic and ModelTrainer train/predict
// without Kafka or network dependencies. Uses xUnit.
//────────────────────────────────────────────────────────────────────────────

using System;
using PredictiveMaintenance_IoT;
using Xunit;

namespace PredMaint.Tests
{
    // Mock sinks for isolation
    public class DummyFeatureSink : IFeatureSink
    {
        public FeatureVector? Last { get; private set; }
        public void Write(FeatureVector fv) => Last = fv;
    }

    public class DummyModelSink : IMessageSink, IModelSink
    {
        public FeatureVector? Last { get; private set; }
        public void Write(SensorMessage msg) => throw new NotImplementedException();
        public void Predict(FeatureVector fv, out bool failure, out float prob)
        {
            Last = fv;
            failure = fv.VibZ > 2;
            prob = failure ? 0.9f : 0.1f;
        }
    }

    public class FeatureEngineerTests
    {
        [Fact]
        public void RollingWindow_Computes_Correct_Mean_Std()
        {
            var sink = new DummyFeatureSink();
            var fe   = new FeatureEngineer(sink, windowSize: 3);

            var msgs = new[]
            {
                new SensorMessage(1, DateTime.UtcNow, 10, 50),
                new SensorMessage(1, DateTime.UtcNow, 12, 52),
                new SensorMessage(1, DateTime.UtcNow, 14, 54)
            };

            foreach (var m in msgs) fe.Write(m);

            Assert.NotNull(sink.Last);
            Assert.Equal(12, Math.Round(sink.Last!.VibMean, 0));
            Assert.True(sink.Last!.VibStd > 1);
            Assert.InRange(sink.Last!.VibZ, -1.1, 1.1);
        }

        [Fact]
        public void ModelTrainer_Predicts_Failure_For_High_Z()
        {
            using var trainer = new ModelTrainer(batchSize: 1);
            // Craft a vector with high vibration Z
            var fv = new FeatureVector(1, DateTime.UtcNow, 0, 1, 3.5, 0, 1, 0);

            trainer.Predict(fv, out bool fail, out float prob);

            Assert.True(fail);
            Assert.InRange(prob, 0.5, 1.0);
        }
    }
}
