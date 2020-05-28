using System;

namespace NeuralNetworkExample2
{
    public class NeuralNetworkStatistic
    {
        public int RightAnswers { get; set; }
        public int WrongAnswers { get; set; }

        public double PercentOfRight
        {
            get
            {
                return Convert.ToDouble(RightAnswers) / (Convert.ToDouble(WrongAnswers) + Convert.ToDouble(RightAnswers));
            }
        }
    }
}
