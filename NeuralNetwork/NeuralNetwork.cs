using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NeuralNetwork
{
    [Serializable]
    class NeuralNetwork
    {
        public double Moment = 0;
        public double LearningRate = 1;
        [Serializable]
        public struct Neuron
        {
            public double value; // Value of the neuron
            public double[] w;  //  All weights outgoing from this neuron
        }
        private double[][] previousWeights;

        public Neuron[][] network;

        #region Network Initialization/Load/Save
//-------------------------------------------------------------------------------------------------------------------------------------------------
        public NeuralNetwork(string NeuronsAndLayers, double randMin, double randMax) //Initializing a neural network
        { 
            string[] neuronsSTR = NeuronsAndLayers.Split(' ');
            uint[] neurons = new uint[neuronsSTR.Length];
            for (uint i = 0; i < neuronsSTR.Length; ++i) //Count of neurons in each layer
                neurons[i] = Convert.ToUInt32(neuronsSTR[i]);

            network = new Neuron[neurons.Length][];
            previousWeights = new double[neurons.Length][];

            for (uint i = 0; i < neurons.Length; ++i) //Initializing a Neural Network
                network[i] = new Neuron[neurons[i]];

            //Distribution of values in weights of Neural Network and initializing PreviousWeights
            for (uint i = 0; i < network.Length - 1; ++i) //             Every layer in this NeuralNetwork
            {
                previousWeights[i] = new double[network[i].Length * network[i + 1].Length];
                uint countOfWeights = 0;

                for (uint j = 0; j < network[i].Length; ++j) //          Every neuron in layer[i]
                {
                    Random rand = new Random();
                    network[i][j].w = new double[network[i + 1].Length];
                    for (uint a = 0; a < network[i][j].w.Length; ++a) // Every weight from neuron[i][j]
                    {
                        previousWeights[i][countOfWeights] = 0; //PrewiosWeight on the first iteration = 0
                        ++countOfWeights;

                    weightWasZeroREPEAT:
                        network[i][j].w[a] = Math.Round(rand.NextDouble() * (randMax- randMin) + randMin, 5); //Random value for the weight
                        if (network[i][j].w[a] == 0)
                            goto weightWasZeroREPEAT; //Value of weight cannot be 0
                         //Console.WriteLine("Weigth " + (i + 1) + " " + (j+1) + " = " + network[i][j].w[a]);
                    }
                }
            }
        }
        public NeuralNetwork(string pathAndName) //Loading a neural network
        {
            if (File.Exists(pathAndName))
            {
                using (FileStream fs = new FileStream(pathAndName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    NeuralNetwork n = (NeuralNetwork)formatter.Deserialize(fs);
                    network = n.network;
                    previousWeights = n.previousWeights;
                }
            }
            else
                throw new ArgumentException("This file does not exist.\rTry to recheck the path or just save new Neural Network with <SaveNetwork> method", "NeuralNetwork");
        }

        public void SaveNetwork(string pathAndName) //Saving a neural network
        {
            using (FileStream fs = new FileStream(pathAndName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region NeuralNetwork Run
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public Neuron[] RunNetwork(double[] training) //Function to Run the network
        {
            //INPUT assignment
            for (uint j = 0; j < training.Length; ++j)
                network[0][j].value = training[j];

            double output;
            for (uint i = 1; i < network.Length; ++i) //                Every layer in this NeuralNetwork
            {
                for (uint j = 0; j < network[i].Length; ++j) //         Every neuron in layer[i]
                {
                    //Calculation of value of the neuron, depending on all values of neurons and weights of synapses connected to this neuron
                    output = 0;
                    for (uint k = 0; k < network[i - 1].Length; ++k) // Summ of every neuron in layer[i-1] * every neuron of neuron[i-1] and it's weight[j]
                        output += network[i - 1][k].value * network[i - 1][k].w[j];

                    //Value activation 
                    network[i][j].value = Sigmoid(output);
                }
            }

            return network[network.GetLength(0) - 1];
        }
//-------------------------------------------------------------------------------------------------------------------------------------------------
        private static double Sigmoid(double num)
        {
            return (1.0 / (1 + Math.Pow(Math.E, -num)));
        } //Activation function for RUN
        #endregion

        #region NeuralNetwork Train
//-------------------------------------------------------------------------------------------------------------------------------------------------
        public void TeachNetwork(double[] ideal, Neuron[] output) //Function to Train the network
        {
            //Creating a copy of NeuralNetwork to work with it
            Neuron[][] deltaNetwork = new Neuron[network.Length][];
            for (uint i = 0; i < network.Length; ++i)
            {
                deltaNetwork[i] = new Neuron[network[i].Length];
                for (uint j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j] = network[i][j];
            }

            //Calculating Delta(OUT) for OUTPUT neurons of the NeuralNetwork 
            for (uint i = 0; i < ideal.Length; ++i)
                deltaNetwork[deltaNetwork.Length - 1][i].value = DeltaOut(ideal[i], output[i].value);

            //Calculating Delta(HIDDEN) for HIDDEN neurons the NeuralNetwork 
            for (int i = deltaNetwork.Length - 2; i >= 1; --i) //Start - from the last HIDDEN layer | End - to the firs HIDDEN layer
                for (uint j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j].value = DeltaHidden(deltaNetwork[i][j].w, deltaNetwork[i + 1], network[i][j].value);

            //Calculating delta of all the weights to change them
            double Grad;
            double delta;
            for (int i = deltaNetwork.Length - 2; i >= 0; --i) //        Start - from the last HIDDEN layer | End - to the firs layer (INPUT)
                for (uint j = 0; j < network[i].Length; ++j) //          Every neuron in layer[i]
                    for (uint a = 0; a < network[i][j].w.Length; ++a) // Every weight from neuron[i][j]
                    {
                        Grad = GRAD(network[i][j].value, deltaNetwork[i + 1][a].value);
                        delta = deltaW(Grad, previousWeights[i][j]);
                        network[i][j].w[a] += delta; //Change the weight of synapse (ActualWeight + DeltaOfThisWeight)
                        previousWeights[i][j] = delta;
                    }
        }
//-------------------------------------------------------------------------------------------------------------------------------------------------
        private static double SigmoidError(double output)
        {
            return ((1 - output) * output);
        } //Activation function for TRAIN
        private static double DeltaOut(double outIdeal, double outActual)
        {
            return ((outIdeal - outActual) * SigmoidError(outActual));
        } //Calculation delta of OUT neuron
        private static double DeltaHidden(double[] weights, Neuron[] delta, double output)
        {
            double actualSum = 0;

            for (uint i = 0; i < weights.Length; ++i) //((w1 * d1) + (w2 * d2) + .. + (wn * dn))
                actualSum += weights[i] * delta[i].value;

            return (SigmoidError(output) * actualSum);
        } //Calculation delta of HIDDEN neuron
        private static double GRAD(double output, double delta)
        {
            return (output * delta);
        } //Calculation gradient(gradient descent) for the weight
        private double deltaW(double Grad, double prWeight)
        {
            return (LearningRate * Grad + Moment * prWeight);
        } //Calculation delta of the weight
        #endregion

    }
}
