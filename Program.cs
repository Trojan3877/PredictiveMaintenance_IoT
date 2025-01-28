using System;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace PredictiveMaintenance_IoT
{
    class Program
    {
        public class IoTData
        {
            [LoadColumn(0)] public float Temperature { get; set; }
            [LoadColumn(1)] public float Vibration { get; set; }
            [LoadColumn(2)] public float Pressure { get; set; }
            [LoadColumn(3)] public bool Failure { get; set; }
        }

        public class IoTPrediction
        {
            [ColumnName("PredictedLabel")] public bool PredictedFailure { get; set; }
            public float Probability { get; set; }
            public float Score { get; set; }
        }

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            string dataPath = "sensor_data.csv";
            IDataView dataView = mlContext.Data.LoadFromTextFile<IoTData>(
                dataPath,
                separatorChar: ',',
                hasHeader: true);

            var dataSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = dataSplit.TrainSet;
            var testData = dataSplit.TestSet;

            var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", nameof(IoTData.Temperature), nameof(IoTData.Vibration), nameof(IoTData.Pressure))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"));

            var trainer = mlContext.BinaryClassification.Trainers.FastTree();
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            Console.WriteLine("Training the model...");
            var model = trainingPipeline.Fit(trainData);

            Console.WriteLine("Evaluating the model...");
            var predictions = model.Transform(testData);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions);

            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");
            Console.WriteLine($"Area Under Curve: {metrics.AreaUnderRocCurve:P2}");

            string modelPath = "PredictiveMaintenanceModel.zip";
            mlContext.Model.Save(model, trainData.Schema, modelPath);
            Console.WriteLine($"Model saved to: {modelPath}");

            Console.WriteLine("Making predictions...");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<IoTData, IoTPrediction>(model);

            var sampleData = new IoTData
            {
                Temperature = 75,
                Vibration = 0.03f,
                Pressure = 1.8f
            };

            var prediction = predictionEngine.Predict(sampleData);

            Console.WriteLine($"Predicted Failure: {prediction.PredictedFailure}");
            Console.WriteLine($"Failure Probability: {prediction.Probability:P2}");
            Console.WriteLine($"Score: {prediction.Score}");
        }
    }
}
