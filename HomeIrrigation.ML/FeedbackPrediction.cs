using Microsoft.ML.Data;

namespace HomeIrrigation.ML
{
    public class FeedbackPrediction
    {
        [ColumnName(name: "PredictedLabel")]
        public bool TurnOnSprinklers { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
