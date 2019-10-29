using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace HomeIrrigation.ML
{
    public class SprinklerLab
    {
        public static List<FeedbackTrainingData> LoadTrainingData(List<FeedbackTrainingData> trainingData)
        {
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .1F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .1F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .9F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .3F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 0F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .7F,
                TurnOnSprinklers = true
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 1F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 1.9F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 2F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 4F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 2F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 5F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 5F,
                TurnOnSprinklers = false
            });
            trainingData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 50F,
                TurnOnSprinklers = false
            });

            return trainingData;
        }

        public static List<FeedbackTrainingData> LoadTestData(List<FeedbackTrainingData> testData)
        {
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .1F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .1F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .9F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .3F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 0F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .7F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 2F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 4F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 2F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 5F,
                TurnOnSprinklers = false
            });

            return testData;
        }

        public static TransformerChain<BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.FastTree.FastTreeBinaryModelParameters,
               Microsoft.ML.Calibrators.PlattCalibrator>>> TrainMachine(MLContext mlContext, IDataView dataView)
        {
            // Step 4 :- We need to create the pipeline and define the workflows in it.
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(FeedbackTrainingData.TurnOnSprinklers))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(FeedbackTrainingData.FeedbackRainfall)))
                .Append(mlContext.BinaryClassification.Trainers.FastTree(numberOfLeaves: 50, numberOfTrees: 50, minimumExampleCountPerLeaf: 1));

            // Step 5 :- Train the algorithm and we want the model out
            var model = pipeline.Fit(dataView);
            
            return model;
        }
    }
}
