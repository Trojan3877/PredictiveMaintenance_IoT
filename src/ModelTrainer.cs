// src/ModelTrainer.cs
//────────────────────────────────────────────────────────────────────────────
// Consumes FeatureVector records, updates an incremental ML.NET model, and
// exposes a Predict() API for the Predictor service.
//
// • Uses Stochastic Dual Coordinate Ascent (SDCA) logistic regression.
// • Online learning: updates every `batchSize` examples per asset.
// • Saves model file to `models/latest.zip` on each update cycle.
//
// NOTE: Failure label is assumed to be provided in a control channel or
//       backfilled; for demo we generate a synthetic label (vibration Z > 2).
//────────────────────────────────────────────────────────────────────────────

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace PredictiveMaintenance_IoT
{
    // ML.NET input schema
    public class ModelInput
    {
        public float VibMean { get; set; }
        public float VibStd { get; set; }
        public float VibZ { get; set; }
        public float TempMean { get; set; }
        public float TempStd { get; set; }
        public float TempZ { get; set; }
        public bool  Label { get; set; }      // failure? true/false
    }

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")] public bool Prediction { get; set; }
        public float Probability { get; set; }
    }

    public interface IModelSink
    {
        void Predict(FeatureVector fv, out bool failure, out float prob);
    }

    public class ModelTrainer : IFeatureSink, IModelSink, IDisposable
    {
        private readonly MLContext _ml = new(2025);
        private ITransformer? _model;
        private PredictionEngine<ModelInput, ModelOutput>? _engine;
        private readonly List<ModelInput> _buffer = new();
        private readonly int _batch;

        private static readonly string ModelDir = Path.Combine("models");
        private static readonly string ModelPath = Path.Combine(ModelDir, "latest.zip");

        public ModelTrainer(int batchSize = 256)
        {
            _batch = batchSize;
            Directory.CreateDirectory(ModelDir);
            if (File.Exists(ModelPath))
            {
                _model = _ml.Model.Load(ModelPath, out _);
                _engine = _ml.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);
            }
        }

        // IFeatureSink implementation
        public void Write(FeatureVector fv)
        {
            bool label = fv.VibZ > 2 || fv.TempZ > 2.5;          // synthetic failure flag
            _buffer.Add(ToInput(fv, label));

            if (_buffer.Count >= _batch)
            {
                TrainOnBuffer();
                _buffer.Clear();
            }
        }

        private void TrainOnBuffer()
        {
            IDataView data = _ml.Data.LoadFromEnumerable(_buffer);

            var pipeline = _ml.Transforms.Concatenate("Features",
                    nameof(ModelInput.VibMean), nameof(ModelInput.VibStd),
                    nameof(ModelInput.VibZ), nameof(ModelInput.TempMean),
                    nameof(ModelInput.TempStd), nameof(ModelInput.TempZ))
                .Append(_ml.BinaryClassification.Trainers.SdcaLogisticRegression());

            _model = pipeline.Fit(data);
            _engine = _ml.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);

            _ml.Model.Save(_model, data.Schema, ModelPath);
            Console.WriteLine($"🧠 Model updated • {DateTime.UtcNow:u}");
        }

        // IModelSink implementation
        public void Predict(FeatureVector fv, out bool failure, out float prob)
        {
            if (_engine is null)
            {
                failure = false;
                prob = 0;
                return;
            }
            var output = _engine.Predict(ToInput(fv, false));
            failure = output.Prediction;
            prob = output.Probability;
        }

        private static ModelInput ToInput(FeatureVector fv, bool label) => new()
        {
            VibMean   = (float)fv.VibMean,
            VibStd    = (float)fv.VibStd,
            VibZ      = (float)fv.VibZ,
            TempMean  = (float)fv.TempMean,
            TempStd   = (float)fv.TempStd,
            TempZ     = (float)fv.TempZ,
            Label     = label
        };

        public void Dispose() => (_engine as IDisposable)?.Dispose();
    }
}
