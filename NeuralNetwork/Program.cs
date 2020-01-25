using System;

namespace NeuralNetwork
{
    class NeuralNetworkPROBuilderXOR
    {
        const double Moment = 0.1;
        const double LearningRate = 0.1;
        static void Main()
        {
            int[,] trainingData = new int[,] { { 1, 1, 0 }, { 1, 0, 1 }, { 0, 1, 1 }, { 0, 0, 0 } };
            int[,] testData = new int[,] { { 1, 1, 0 }, { 1, 0, 1 }, { 0, 1, 1 }, { 0, 0, 0 } };
            double terminatingError = 0.01;
            uint refreshSpeed = 100000;

            NeuralNetwork network = new NeuralNetwork(Moment, LearningRate);

            double errorSum;
            uint iteration = 0;
            do
            {
                errorSum = 0;
                for (int i = 0; i < trainingData.GetLength(0); ++i)
                {
                    double endValue = network.RunNetwork(trainingData[i, 0], trainingData[i, 1]);
                    double error = Math.Pow(trainingData[i, 2] - endValue, 2) / 1; // ((i1-a1)*(i1-a1)+...(in-an)*(in-an))/n
                    errorSum += error;

                    network.TeachNetwork(trainingData[i, 2], endValue);
                }
                if (iteration++ % refreshSpeed == 0)
                    Console.WriteLine("Summ of errors = " + Math.Round((errorSum / trainingData.GetLength(0)) * 100, 5) + "%");

            } while ((errorSum / trainingData.GetLength(0)) > terminatingError);

            for (int i = 0; i < testData.GetLength(0); ++i)
            {
                double answer = network.RunNetwork(testData[i, 0], testData[i, 1]);
                Console.WriteLine("I1 = " + testData[i, 0] + " I2 = " + testData[i, 1]);
                Console.Write("Ideal = " + testData[i, 2] + "\nAnswer = " + answer);

                answer = answer > 0.5 ? 1 : 0;
                Console.WriteLine(" activated answer = " + answer);
                Console.WriteLine();
            }

            Console.WriteLine("Number of iterations = " + iteration);
        }
    }
}