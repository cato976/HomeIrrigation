using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace HomeIrrigation.ML.Console
{
    class FeedBackTrainingData
    {
        //[LoadColumn(0), ColumnName("Features")]
        [LoadColumn(0)]
        public float FeedBackRainfall { get; set; }
        //[LoadColumn(1), ColumnName(name: "Label")]
        //[LoadColumn(1), ColumnName(name: "Label")]
        [LoadColumn(1)]
        public bool TurnOnSprinklers { get; set; }

    }

    class FeedBackPrediction
    {
        [ColumnName(name: "PredictedLabel")]
        public bool TurnOnSprinklers { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }


    class Program
    {
        static List<FeedBackTrainingData> trainingData = new List<FeedBackTrainingData>();
        static List<FeedBackTrainingData> testData = new List<FeedBackTrainingData>();

        static void LoadTrainingData()
        {
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .1F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .1F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .9F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .3F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 0F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .7F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 1F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 1.9F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 2F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 4F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 2F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 5F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 5F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 50F,
                TurnOnSprinklers = false
            });
        }

        static void LoadTestData()
        {
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .1F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .1F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .9F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .3F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 0F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = .7F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 2F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 4F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 2F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedBackTrainingData()
            {
                FeedBackRainfall = 5F,
                TurnOnSprinklers = false
            });
        }

        static void Main(string[] args)
        {
            // Step 1 :- We need to load the training data
            LoadTrainingData();

            // Step 2 :- Create object of MLContext
            var mlContext = new MLContext();

            // Step 3 :- Convert your data in to IDataView
            IDataView dataView = mlContext.Data.LoadFromEnumerable<FeedBackTrainingData>(trainingData);

            // Step 4 :- We need to create the pipeline and define the workflows in it.
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "TurnOnSprinklers")
                .Append(mlContext.Transforms.Concatenate("Features", "FeedBackRainfall"))
                .Append(mlContext.BinaryClassification.Trainers.FastTree(numberOfLeaves: 50, numberOfTrees: 50, minimumExampleCountPerLeaf: 1));

            // Step 5 :- Train the algorithm and we want the model out
            var model = pipeline.Fit(dataView);

            // Step 6 :- Load the test data and run the test data to check the models accuracy
            LoadTestData();

            IDataView testDataView = mlContext.Data.LoadFromEnumerable<FeedBackTrainingData>(testData);

            var predictions = model.Transform(testDataView);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
            System.Console.WriteLine($"metrics accuracy: {metrics.Accuracy}");
            System.Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            System.Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            System.Console.WriteLine($"F1Score: {metrics.F1Score:P2}");

            // Step 7 :- use the model
            string strcont = "Y";
            while (strcont == "Y")
            {
                System.Console.WriteLine("Enter rainfall");
                string feedback = System.Console.ReadLine().ToString();

                var predictionFunction = mlContext.Model.CreatePredictionEngine<FeedBackTrainingData, FeedBackPrediction>(model);

                var feedbackInput = new FeedBackTrainingData();
                feedbackInput.FeedBackRainfall = float.Parse(feedback);
                var feedbackPredicted = predictionFunction.Predict(feedbackInput);
                System.Console.WriteLine($"TurnOnSprinklers: {feedbackPredicted.TurnOnSprinklers} | Prediction: {(System.Convert.ToBoolean(feedbackPredicted.TurnOnSprinklers) ? "Positive" : "Negative")} | Probability: {feedbackPredicted.Probability} ");
                System.Console.WriteLine($"TurnOnSprinklers :- {feedbackPredicted.TurnOnSprinklers}");
            }
        }
    }
}
