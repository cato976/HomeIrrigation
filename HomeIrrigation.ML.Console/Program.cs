using System.Collections.Generic;
using Microsoft.ML;
using HomeIrrigation.ML;

namespace HomeIrrigation.ML.Console
{
    class Program
    {
        static List<FeedbackTrainingData> trainingData = new List<FeedbackTrainingData>();
        static List<FeedbackTrainingData> testData = new List<FeedbackTrainingData>();


        static void Main(string[] args)
        {
            // Step 1 :- We need to load the training data
            //trainingData = SprinklerLab.LoadTrainingData(trainingData);
            trainingData.AddRange(SprinklerLab.ReadDataForPassXDays());
            //System.Console.WriteLine(trainingData.Count);
            //SprinklerLab.LoadTrainingDataFromFile(trainingData);

            // Step 2 :- Create object of MLContext
            var mlContext = new MLContext();

            // Step 3 :- Convert your data in to IDataView
            IDataView dataView = mlContext.Data.LoadFromEnumerable<FeedbackTrainingData>(trainingData);

            var model = SprinklerLab.TrainMachine(mlContext, dataView);

            //// Step 4 :- We need to create the pipeline and define the workflows in it.
            //var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(FeedbackTrainingData.TurnOnSprinklers))
            //    .Append(mlContext.Transforms.Concatenate("Features", nameof(FeedbackTrainingData.FeedbackRainfall)))
            //    .Append(mlContext.BinaryClassification.Trainers.FastTree(numberOfLeaves: 50, numberOfTrees: 50, minimumExampleCountPerLeaf: 1));

            //// Step 5 :- Train the algorithm and we want the model out
            //var model = pipeline.Fit(dataView);

            // Step 6 :- Load the test data and run the test data to check the models accuracy
            testData = SprinklerLab.LoadTestData(testData);
            testData = SprinklerLab.LoadTestDataFromFile(testData);

            IDataView testDataView = mlContext.Data.LoadFromEnumerable<FeedbackTrainingData>(testData);

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
                float output;
                float.TryParse(feedback, out output);
                if (output == 0 && feedback != "0")
                {
                    strcont = feedback;
                }
                System.Console.WriteLine("Enter temperature");
                string temperature = System.Console.ReadLine().ToString();
                System.Console.WriteLine("Enter wind speed");
                string windSpeed = System.Console.ReadLine().ToString();

                var predictionFunction = mlContext.Model.CreatePredictionEngine<FeedbackTrainingData, FeedbackPrediction>(model);

                var feedbackInput = new FeedbackTrainingData();
                try
                {
                    feedbackInput.FeedbackRainfall = float.Parse(feedback);
                    feedbackInput.Temperature = float.Parse(temperature);
                    feedbackInput.WindSpeed = float.Parse(windSpeed);
                    var feedbackPredicted = predictionFunction.Predict(feedbackInput);
                    System.Console.WriteLine($"TurnOnSprinklers: {feedbackPredicted.TurnOnSprinklers} | Prediction: {(System.Convert.ToBoolean(feedbackPredicted.TurnOnSprinklers) ? "Positive" : "Negative")} | Probability: {feedbackPredicted.Probability} ");
                    System.Console.WriteLine($"TurnOnSprinklers :- {feedbackPredicted.TurnOnSprinklers}");
                }
                catch { }
            }
        }
    }
}
