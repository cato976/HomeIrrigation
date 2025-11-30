using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Newtonsoft.Json.Linq;

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
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .1F,
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .9F,
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .9F,
                Temperature = 33,
                WindSpeed = 1,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .3F,
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .3F,
                Temperature = 50,
                WindSpeed = 1,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 0F,
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .7F,
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 2F,
                Temperature = 80,
                WindSpeed = 1,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 4F,
                Temperature = 55,
                WindSpeed = 1,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 2F,
                Temperature = 72,
                WindSpeed = 1,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 5F,
                Temperature = 72,
                WindSpeed = 1,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .25F,
                Temperature = 72,
                WindSpeed = 7,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .25F,
                Temperature = 72,
                WindSpeed = 17,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .25F,
                Temperature = 72,
                WindSpeed = .7F,
                TurnOnSprinklers = true
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .25F,
                Temperature = 50,
                WindSpeed = .7F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = 3.5F,
                Temperature = 72,
                WindSpeed = .7F,
                TurnOnSprinklers = false
            });
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .25F,
                Temperature = 72,
                WindSpeed = 7.5F,
                TurnOnSprinklers = false
            });

            return testData;
        }

        public static List<FeedbackTrainingData> LoadTestDataFromFile(List<FeedbackTrainingData> testData)
        {
            var data = ReadDataForPassXDays();
            var count = 50;
            for (int i = 0; i < count; i++)
            {
                testData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = data[i].FeedbackRainfall,
                    Temperature = data[i].Temperature,
                    WindSpeed = data[i].WindSpeed,
                    TurnOnSprinklers = data[i].TurnOnSprinklers
                });
            }
            testData.Add(new FeedbackTrainingData()
            {
                FeedbackRainfall = .2F,
                Temperature = 73,
                WindSpeed = 1F,
                TurnOnSprinklers = true
            });

            return testData;
        }

        public static TransformerChain<BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.FastTree.FastTreeBinaryModelParameters,
               Microsoft.ML.Calibrators.PlattCalibrator>>> TrainMachine(MLContext mlContext, IDataView dataView)
        {
            // Step 4 :- We need to create the pipeline and define the workflows in it.
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(FeedbackTrainingData.TurnOnSprinklers))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(FeedbackTrainingData.FeedbackRainfall), nameof(FeedbackTrainingData.Temperature), nameof(FeedbackTrainingData.WindSpeed)))
                .Append(mlContext.BinaryClassification.Trainers.FastTree(numberOfLeaves: 50, numberOfTrees: 50, minimumExampleCountPerLeaf: 1));

            // Step 5 :- Train the algorithm and we want the model out
            var model = pipeline.Fit(dataView);

            return model;
        }

        public static List<FeedbackTrainingData> LoadTrainingDataFromFile(List<FeedbackTrainingData> trainingData)
        {
            var newData = ReadDataForPassXDays();
            return newData;
        }

        public static List<FeedbackTrainingData> ReadDataForPassXDays()
        {
            List<FeedbackTrainingData> trainingData = new List<FeedbackTrainingData>();
            String path = "c:\\temp\\WeatherData.txt";
            // Open the file to read from.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadToEnd()) != string.Empty)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        JObject weatherEntry = JObject.Parse(s);
                        double rainfall = 0;
                        for (var day = 0; day < weatherEntry["daily"]["time"].Count(); day++)
                        {
                            var rain = (double)weatherEntry["daily"]["rain_sum"][day];
                            var temp = (double)weatherEntry["daily"]["temperature_2m_max"][day];
                            var windSpeed = (double)weatherEntry["daily"]["wind_speed_10m_max"][day];
                            trainingData.Add(new FeedbackTrainingData()
                            {
                                FeedbackRainfall = (float)rain,
                                Temperature = (float)temp,
                                TurnOnSprinklers = rainfall < 1 && temp > 70 && windSpeed < 5 ? true : false
                            });
                        }
                        //double temperature = (double)weatherEntry["daily"]["temperature_2m_max"][0];
                        //var windSpeed = (double)weatherEntry["daily"]["wind_speed_10m_max"][0];
                        //Console.WriteLine($"rainfall: {rainfall}");
                        //Console.WriteLine($"temperature: {temperature}");
                        //var trfl = rainfall < 1 && temperature > 70 && windSpeed < 5 ? true : false;
                        //Console.WriteLine($"TurnOnSprinklers : {trfl}");
                    }
                    //Console.WriteLine(s);
                }
                //
                // Add some false entries
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 1F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 1.9F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 2F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 4F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 2F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 5F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 5F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = 50F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = .350F,
                    Temperature = 86,
                    WindSpeed = 2,
                    TurnOnSprinklers = true,
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = .50F,
                    Temperature = 40,
                    WindSpeed = 2,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = .50F,
                    Temperature = 90,
                    WindSpeed = 2,
                    TurnOnSprinklers = true
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = .50F,
                    Temperature = 90,
                    WindSpeed = 6,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = .50F,
                    Temperature = 90,
                    WindSpeed = 6.3F,
                    TurnOnSprinklers = false
                });
                trainingData.Add(new FeedbackTrainingData()
                {
                    FeedbackRainfall = .2F,
                    Temperature = 73,
                    WindSpeed = 1F,
                    TurnOnSprinklers = true
                });
            }

            return trainingData;
        }
    }
}
