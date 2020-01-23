using System;
using System.Numerics;

namespace NeuralNetwork
{
    class NeuralNetworkPROBuilderXOR
    {
        const double Moment = 0.3;
        const double LearningRate = 0.7;
        public struct Neuron
        {
            public double value;
            public double[] w;
        }

        #region NeuralNetwork Run
        public static double Sigmoid(double num)
        {
            num = 1.0 / (1 + Math.Pow(Math.E, -num));

            return num;
        }

        public static double RunNetwork(Neuron[][] network, int I1, int I2)
        {
            network[0][0].value = I1;
            network[0][1].value = I2;

            for (int i = 1; i < network.Length; ++i)
            {
                for (int j = 0; j < network[i].Length; ++j)
                {
                    double output = 0;
                    for (int k = 0; k < network[i - 1].Length; ++k)
                        output += network[i-1][k].value * network[i-1][k].w[j];

                    network[i][j].value = Sigmoid(Math.Round(output, 5));
                }
            }

            return network[network.GetLength(0) - 1][0].value;
        }
        #endregion

        #region NeuralNetwork Teach
        public static double SigmoidError(double output)
        {
            output = (1 - output) * output;
            return output;
        }
        public static double DeltaOut(double outIdeal, double outActual)
        {
            outActual = (outIdeal - outActual) * SigmoidError(outActual);
            return outActual;
        }
        public static double DeltaHidden(double[] weights, double output, Neuron[] delta)
        {
            double actualSum = 0;

            for (int i = 0; i < weights.Length; ++i)
            {
                actualSum += weights[i] * delta[i].value;
            }

            output = SigmoidError(output) * actualSum;
            return output;
        }
        public static double GRAD(double output, double delta)
        {
            output = delta * output;
            return output;
        }
        public static double deltaW(double Grad, double prWeight)
        {
            prWeight = LearningRate * Grad + Moment * prWeight;
            return prWeight;
        }
        public static void TeachNetwork(Neuron[][] network, double ideal, double output, double[][] prWeights)
        {
            Neuron[][] deltaNetwork = new Neuron[network.Length][];
            for (int i = 0; i < network.Length; ++i)
            {
                deltaNetwork[i] = new Neuron[network[i].Length];
                for (int j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j] = network[i][j];
            }

            deltaNetwork[deltaNetwork.Length - 1][deltaNetwork[deltaNetwork.Length - 1].Length - 1].value = DeltaOut(ideal, output);

            for(int i = deltaNetwork.Length - 2; i >= 1; --i)
                for (int j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j].value = DeltaHidden(deltaNetwork[i][j].w, network[i][j].value, deltaNetwork[i+1]);
            //for(int j = 0; j < network[0].Length; ++j)
            //    deltaNetwork[0][j].value = deltaNetwork[1][j].value;

            for (int i = deltaNetwork.Length - 2; i >= 0; --i)
                for (int j = 0; j < network[i].Length; ++j)
                    for (int a = 0; a < network[i][j].w.Length; ++a)
                    {
                        double Grad = GRAD(network[i][j].value, deltaNetwork[i + 1][a].value);
                        double delta = deltaW(Grad, prWeights[i][j]);
                        network[i][j].w[a] += delta;
                        prWeights[i][j] = delta;
                    }
        }
        #endregion

        static void Main()
        {
            int[,] trainingData = new int[,] { { 1, 0, 1 }, { 1, 1, 0 }, { 0, 0, 0 }, { 0, 1, 1 } };
            int[,] testData = new int[,] {{ 1, 0, 1 }, { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
            double[][] previousWeights = new double[3][];

            #region NeuralNetwork inicialization
            Neuron[][] network = new Neuron[3][];

            network[0] = new Neuron[2];
            network[1] = new Neuron[10];
            network[2] = new Neuron[1];

            for (int i = 0; i < network.Length - 1; ++i)
            {
                previousWeights[i] = new double[network[i].Length * network[i + 1].Length];
                int count = 0;

                for (int j = 0; j < network[i].Length; ++j)
                {
                    Random rand = new Random();
                    network[i][j].w = new double[network[i + 1].Length];
                    for (int a = 0; a < network[i][j].w.Length; ++a)
                    {
                        previousWeights[i][count] = 0;
                        ++count;

                        network[i][j].w[a] = Math.Round(rand.NextDouble() * 4 - 2, 2);
                        //Console.WriteLine("Weigth " + (i + 1) + " = " + network[i][j].w[a]);
                    }
                }
            }
            //Console.WriteLine();
            #endregion

            double errorSum;
            uint iteration = 0;
            do
            {
                errorSum = 0;
                for (int i = 0; i < trainingData.GetLength(0); ++i)
                {
                    double error;
                    double endValue = RunNetwork(network, trainingData[i, 0], trainingData[i, 1]);
                    error = Math.Pow(trainingData[i, 2] - endValue, 2) / 1; // ((i1-a1)*(i1-a1)+...(in-an)*(in-an))/n
                    errorSum += error;

                    TeachNetwork(network, trainingData[i, 2], endValue, previousWeights);
                }
                ++iteration;
                // Console.WriteLine();
                // Console.WriteLine(" - Summ of errors = " + Math.Round(errorSum * 100, 2) + " %");
            } while ((errorSum / trainingData.GetLength(0)) > 0.1);

            for (int i = 0; i < testData.GetLength(0); ++i)
            {
                double answer = RunNetwork(network, testData[i, 0], testData[i, 1]);
                Console.WriteLine("I1 = " + testData[i, 0] + " I2 = " + testData[i, 1]);
                Console.Write("Ideal = " + testData[i, 2] + "\nAnswer = " + answer);
                if (answer > 0.5)
                    answer = 1;
                else
                    answer = 0;
                Console.WriteLine(" normalized answer = " + answer);
                Console.WriteLine();
            }

            Console.WriteLine("Number of iterations = " + iteration);
        }
    }
}