using System;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        public double Moment;
        public double LearningRate;
        public struct Neuron
        {
            public double value;
            public double[] w;
        }
        private double[][] previousWeights;

        public Neuron[][] network;

        #region Network Initialization
        public NeuralNetwork(double moment, double learningRate)
        {
            Moment = moment;
            LearningRate = learningRate;

            network = new Neuron[3][];
            previousWeights = new double[3][];

            network[0] = new Neuron[2];
            network[1] = new Neuron[3];
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

                    weightWasZero:
                        network[i][j].w[a] = Math.Round(rand.NextDouble(), 1);
                        if (network[i][j].w[a] == 0)
                            goto weightWasZero;
                        // Console.WriteLine("Weigth " + (i + 1) + " " + (j+1) + " = " + network[i][j].w[a]);
                    }
                }
            }
        }
        #endregion

        #region NeuralNetwork Run
        private static double Sigmoid(double num)
        {
            num = 1.0 / (1 + Math.Pow(Math.E, -num));

            return num;
        }

        public double RunNetwork(int I1, int I2)
        {
            network[0][0].value = I1;
            network[0][1].value = I2;

            for (int i = 1; i < network.Length; ++i)
            {
                for (int j = 0; j < network[i].Length; ++j)
                {
                    double output = 0;
                    for (int k = 0; k < network[i - 1].Length; ++k)
                        output += network[i - 1][k].value * network[i - 1][k].w[j];

                    network[i][j].value = Sigmoid(Math.Round(output, 5));
                }
            }

            return network[network.GetLength(0) - 1][0].value;
        }
        #endregion

        #region NeuralNetwork Teach
        private static double SigmoidError(double output)
        {
            output = (1 - output) * output;
            return output;
        }
        private static double DeltaOut(double outIdeal, double outActual)
        {
            outActual = (outIdeal - outActual) * SigmoidError(outActual);
            return outActual;
        }
        private static double DeltaHidden(double[] weights, double output, Neuron[] delta)
        {
            double actualSum = 0;

            for (int i = 0; i < weights.Length; ++i)
            {
                actualSum += weights[i] * delta[i].value;
            }

            output = SigmoidError(output) * actualSum;
            return output;
        }
        private static double GRAD(double output, double delta)
        {
            output = output * delta;
            return output;
        }
        private double deltaW(double Grad, double prWeight)
        {
            prWeight = LearningRate * Grad + Moment * prWeight;
            return prWeight;
        }
        public void TeachNetwork(double ideal, double output)
        {
            Neuron[][] deltaNetwork = new Neuron[network.Length][];
            for (int i = 0; i < network.Length; ++i)
            {
                deltaNetwork[i] = new Neuron[network[i].Length];
                for (int j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j] = network[i][j];
            }

            deltaNetwork[deltaNetwork.Length - 1][deltaNetwork[deltaNetwork.Length - 1].Length - 1].value = DeltaOut(ideal, output);

            for (int i = deltaNetwork.Length - 2; i >= 1; --i)
                for (int j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j].value = DeltaHidden(deltaNetwork[i][j].w, network[i][j].value, deltaNetwork[i + 1]);

            for (int i = deltaNetwork.Length - 2; i >= 0; --i)
                for (int j = 0; j < network[i].Length; ++j)
                    for (int a = 0; a < network[i][j].w.Length; ++a)
                    {
                        double Grad = GRAD(network[i][j].value, deltaNetwork[i + 1][a].value);
                        double delta = deltaW(Grad, previousWeights[i][j]);
                        network[i][j].w[a] += delta;
                        previousWeights[i][j] = delta;
                    }
        }
        #endregion

    }
}
