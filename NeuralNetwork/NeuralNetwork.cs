using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NN
{
    //---------------------------------------------------------------
    //
    //
    //  This file is a library with public methods to use Neural Network.
    //  Check another file to understand how to use the library.
    //  You are free to use the library in yout own needs
    //
    //
    //---------------------------------------------------------------
    [Serializable]
    public class NeuralNetwork //output  =  sum (weights * inputs) + bias 
    {
        public double Moment { get; set; } = 0;
        public double LearningRate { get; set; } = 1;

        [Serializable]
        /// <summary>
        /// The struct 'Neuron[]'. Has double 'value' - value of the neuron and double[] 'w' - weights outhoing from this neuron
        /// </summary>
        public struct Neuron
        {
            public double value; // Value of the neuron
            public double[] w;  //  All weights outgoing from this neuron
        }
        /// <summary>
        /// The public struct(Neuron[][]) network. To access to the certain neuron type: network[i][j]
        /// </summary>
        public Neuron[][] network { get; set; }
        private Neuron[][] deltaNetwork;

        private double[][] previousWeights;
        private uint[] withoutBiasLength;

        //Introducing all the variables that are used in for-loops here, so garbage collector will no longer execute (it is used for perfomance optimization)
        private int j;
        private int i;
        private int k;
        private int a;

        #region Network Initialization/Load/Save
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the network.
        /// </summary>
        /// <param name="NeuronsAndLayers">The struct of the network. For instance, "4+ 15+ 5 3" whare '+' means bias.</param>
        /// <param name="randMin">Minimal random weight of synapse.</param>
        /// <param name="randMax">Maximal random weight of synapse.</param>
        public NeuralNetwork(string NeuronsAndLayers, double randMin, double randMax) //Initializing a neural network
        { 
            string[] neuronsSTR = NeuronsAndLayers?.Split(' ');
            network = new Neuron[neuronsSTR.Length][];
            withoutBiasLength = new uint[neuronsSTR.Length];
            ArrayList biases = new ArrayList();
            for (i = 0; i < neuronsSTR.Length; ++i) //Count of neurons in each layer
            {
                try //Check if there are biases
                {
                    uint index = Convert.ToUInt32(neuronsSTR[i]);
                    network[i] = new Neuron[index];
                    withoutBiasLength[i] = index;
                }
                catch (Exception) //If bias is in the layer then
                {
                    if (i == neuronsSTR.Length - 1)
                        throw new Exception("You cannot add biases to OUTPUT layers");
                    else
                    {
                        biases.Add(i);
                        uint index = Convert.ToUInt32(neuronsSTR[i][0..^1]) + 1;
                        network[i] = new Neuron[index]; //Convert only count of neurons without bias(+)
                        network[i][index - 1].value = 1;
                        withoutBiasLength[i] = index - 1;
                    }
                }
            }

            previousWeights = new double[network.Length][];
            //Distribution of values in weights of Neural Network and initializing PreviousWeights
            for (i = 0; i < network.Length - 1; ++i) //             Every layer in this NeuralNetwork
            {
                previousWeights[i] = new double[network[i].Length * withoutBiasLength[i + 1]];
                uint countOfWeights = 0;

                for (j = 0; j < network[i].Length; ++j) //          Every neuron in layer[i]
                {
                    Random rand = new Random();
                    network[i][j].w = new double[withoutBiasLength[i + 1]];
                    for (uint a = 0; a < network[i][j].w.Length; ++a) // Every weight from neuron[i][j]
                    {
                        previousWeights[i][countOfWeights] = 0; //PrewiosWeight on the first iteration = 0
                        ++countOfWeights;

                    weightWasZeroREPEAT:
                        network[i][j].w[a] = Math.Round(rand.NextDouble() * (randMax - randMin) + randMin, 5); //Random value for the weight
                        if (network[i][j].w[a] == 0)
                            goto weightWasZeroREPEAT; //Value of weight cannot be 0
                    }
                }
            }

            //Creating a copy of NeuralNetwork to work with it
            deltaNetwork = new Neuron[network.Length][];
            for (i = 0; i < network.Length; ++i)
            {
                deltaNetwork[i] = new Neuron[network[i].Length];
                for (j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j] = network[i][j];
            }
        }
        /// <summary>
        /// Loads the network.
        /// </summary>
        /// <param name="pathAndName">The path to the file with it's name.</param>
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
                    Moment = n.Moment;
                    LearningRate = n.LearningRate;
                }

                //Creating a copy of NeuralNetwork to work with it
                deltaNetwork = new Neuron[network.Length][];
                for (i = 0; i < network.Length; ++i)
                {
                    deltaNetwork[i] = new Neuron[network[i].Length];
                    for (j = 0; j < network[i].Length; ++j)
                        deltaNetwork[i][j] = network[i][j];
                }
            }
            else
                throw new ArgumentException("This file does not exist.\rTry to recheck the path or just save new Neural Network with <SaveNetwork> method", "NeuralNetwork");
        }

        /// <summary>
        /// Saves the network.
        /// </summary>
        /// <param name="pathAndName">The path to the file with it's name.</param>
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
        /// <summary>
        /// Runs the network and returns Output neurons with the struct Neuron[].
        /// </summary>
        /// <param name="training">This is array that contains ont training set(iteration).</param>
        /// <returns>Neuron[]</returns>
        public virtual Neuron[] RunNetwork(double[] training) //Function to Run the network
        {
            //INPUT assignment
            for (j = 0; j < training.Length; ++j)
                network[0][j].value = training[j];

            double output;
            for (i = 1; i < network.Length; ++i) //                Every layer in this NeuralNetwork
            {
                for (j = 0; j < withoutBiasLength[i]; ++j) //         Every neuron in layer[i]
                {
                    //Calculation value of the neuron, depending on all values of neurons and weights of synapses connected to this neuron
                    output = 0;
                    for (k = 0; k < network[i - 1].Length; ++k) // Summ of every neuron in layer[i-1] * every neuron of weight[i-1] = weight[j]
                        output += network[i - 1][k].value * network[i - 1][k].w[j];

                    //Value activation 
                    network[i][j].value = Sigmoid(output);
                }
            }

            return network[network.GetLength(0) - 1];
        }
//-------------------------------------------------------------------------------------------------------------------------------------------------
        private double Sigmoid(double num)
        {
            return (1.0 / (1 + Math.Pow(Math.E, -num)));
        } //Activation function for RUN
        #endregion

        #region NeuralNetwork Train
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Teaches the network.
        /// </summary>
        /// <param name="ideal">The expected values(double[]).</param>
        /// <param name="output">The actual values(Neuron[]).</param>
        ///
        public virtual void TeachNetwork(double[] ideal, Neuron[] output) //Function to Train the network
        {
            //Calculating Delta(OUT) for OUTPUT neurons of the NeuralNetwork 
            for (i = 0; i < ideal.Length; ++i)
                 deltaNetwork[^1][i].value = DeltaOut(ideal[i], output[i].value);

            //Calculating Delta(HIDDEN) for HIDDEN neurons the NeuralNetwork 
            for (i = deltaNetwork.Length - 2; i >= 1; --i) //Start - from the last HIDDEN layer | End - to the firs HIDDEN layer
                for (j = 0; j < network[i].Length; ++j)
                    deltaNetwork[i][j].value = DeltaHidden(network[i][j].w, deltaNetwork[i + 1], network[i][j].value);

            //Calculating delta of all the weights to change them
            double Grad;
            double delta;
            for (i = deltaNetwork.Length - 2; i >= 0; --i) //        Start - from the last HIDDEN layer | End - to the firs layer (INPUT)
                for (j = 0; j < network[i].Length; ++j) //          Every neuron in layer[i]
                    for (a = 0; a < network[i][j].w.Length; ++a) // Every weight from neuron[i][j]
                    {
                        Grad = GRAD(network[i][j].value, deltaNetwork[i + 1][a].value);
                        delta = DeltaW(Grad, previousWeights[i][j]);
                        network[i][j].w[a] += delta; //Change the weight of synapse (ActualWeight + DeltaOfThisWeight)
                        previousWeights[i][j] = delta;
                    }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        private double SigmoidError(double output)
        {
            return ((1 - output) * output);
        } //Activation function for TRAIN
        private double DeltaOut(double outIdeal, double outActual)
        {
            return ((outIdeal - outActual) * SigmoidError(outActual));
        } //Calculation delta of OUT neuron
        private double DeltaHidden(double[] weights, Neuron[] delta, double output)
        {
            double actualSum = 0;

            for (k = 0; k < weights.Length; ++k) //((w1 * d1) + (w2 * d2) + .. + (wn * dn))
                actualSum += weights[k] * delta[k].value;

            return (SigmoidError(output) * actualSum);
        } //Calculation delta of HIDDEN neuron
        private double GRAD(double output, double delta)
        {
            return (output * delta);
        } //Calculation gradient(gradient descent) for the weight
        private double DeltaW(double Grad, double prWeight)
        {
            return (LearningRate * Grad + Moment * prWeight);
        } //Calculation delta of the weight
        #endregion

    }
}
