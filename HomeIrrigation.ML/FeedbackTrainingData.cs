using Microsoft.ML.Data;

namespace HomeIrrigation.ML
{
    public class FeedbackTrainingData
    {
        [LoadColumn(0)]
        public float FeedbackRainfall { get; set; }
        [LoadColumn(1)]
        public bool TurnOnSprinklers { get; set; }

    }
}
