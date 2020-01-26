using System;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        public double Moment = 1;
        public double LearningRate = 1;
        public struct Neuron
        {
            public double value;
            public double[] w;
        }
        private double[][] previousWeights;

        public Neuron[][] network;

        #region Network Initialization
        public NeuralNetwork(string NeuronsAndLayers)
        {
            string[] neuronsSTR = NeuronsAndLayers.Split(' ');
            int[] neurons = new int[neuronsSTR.Length];
            for (int i = 0; i < neuronsSTR.Length; ++i)
                neurons[i] = Convert.ToInt32(neuronsSTR[i]);

            network = new Neuron[neurons.Length][];
            previousWeights = new double[neurons.Length][];

            for (int i = 0; i < neurons.Length; ++i)
                network[i] = new Neuron[neurons[i]];

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
            return (1.0 / (1 + Math.Pow(Math.E, -num)));
        }
        public Neuron[] RunNetwork(double[] training)
        {
            for(int i = 0; i < training.Length; ++i)
                network[0][i].value = training[i];

            for (int i = 1; i < network.Length; ++i)
            {
                for (int j = 0; j < network[i].Length; ++j)
                {
                    double output = 0;
                    for (int k = 0; k < network[i - 1].Length; ++k)
                        output += network[i - 1][k].value * network[i - 1][k].w[j];

                    network[i][j].value = Sigmoid(output);
                }
            }

            return network[network.GetLength(0) - 1];
        }
        #endregion

        #region NeuralNetwork Teach
        private static double SigmoidError(double output)
        {
            return ((1 - output) * output);
        }
        private static double DeltaOut(double outIdeal, double outActual)
        {
            return ((outIdeal - outActual) * SigmoidError(outActual));
        }
        private static double DeltaHidden(double[] weights, double output, Neuron[] delta)
        {
            double actualSum = 0;

            for (int i = 0; i < weights.Length; ++i)
                actualSum += weights[i] * delta[i].value;

            return (SigmoidError(output) * actualSum);
        }
        private static double GRAD(double output, double delta)
        {
            return (output * delta);
        }
        private double deltaW(double Grad, double prWeight)
        {
            return (LearningRate * Grad + Moment * prWeight);
        }
        public void TeachNetwork(double[] ideal, Neuron[] output)
        {
            Neuron[][] deltaNetwork = new Neuron[network.Length][];
            for (int i = 0; i < network.Length; ++i)
            {
                deltaNetwork[i] = new Neuron[network[i].Length];
                for (int j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j] = network[i][j];
            }

            for(int i = 0; i < ideal.Length; ++i)
                deltaNetwork[deltaNetwork.Length - 1][i].value = DeltaOut(ideal[i], output[i].value);

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
