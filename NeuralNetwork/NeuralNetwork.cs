using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace NN
{
    /// <summary>
    /// This file is a library with public methods to use Neural Network.
    /// Check another file to understand how to use the library.
    /// You are free to use the library in yout own needs
    /// </summary>
    [Serializable]
    public class NeuralNetwork //output  =  sum (weights * inputs) + bias 
    {
        [Serializable]
        public struct Neuron
        {
            public double value;
            public double[] weights;
        }

        /// <summary>
        /// The public struct(Neuron[][]) network. To access to the certain neuron type: network[i][j]
        /// </summary>
        public Neuron[][] Network { get; set; }
        public double Moment { get; set; } = 0;
        public double LearningRate { get; set; } = 1;
        public int LayersCount => Network.Length;


        private Neuron[][] deltaNetwork;
        private double[][] previousWeights;
        private uint[] withoutBiasLength;

        //Introducing all the variables that are used in the functions, so garbage collector will no longer execute (it is used for perfomance optimization)
        private int j;
        private int i;
        private int k;
        private int a;
        private double output;
        private double Grad;
        private double delta;

        #region Network Initialization/Load/Save
        /// <summary>
        /// Creates the network.
        /// </summary>
        /// <param name="NeuronsAndLayers">The struct of the network. For instance, "4+ 15+ 5 3" whare '+' means bias. Watch readme</param>
        /// <param name="randMin">Minimal random weight of synapse.</param>
        /// <param name="randMax">Maximal random weight of synapse.</param>
        public NeuralNetwork(string NeuronsAndLayers, double randMin, double randMax)
        {
            Random rand = new Random();

            string[] neuronsSTR = NeuronsAndLayers?.Split(' ');
            Network = new Neuron[neuronsSTR.Length][];
            withoutBiasLength = new uint[neuronsSTR.Length];
            System.Collections.ArrayList biases = new System.Collections.ArrayList();
            for (i = 0; i < neuronsSTR.Length; ++i) //Count of neurons in each layer
            {
                try //Check if there are biases
                {
                    uint index = Convert.ToUInt32(neuronsSTR[i]);
                    Network[i] = new Neuron[index];
                    withoutBiasLength[i] = index;
                }
                catch (Exception) //If bias is in the layer then
                {
                    if (i == neuronsSTR.Length - 1)
                        throw new Exception("You cannot add biases to OUTPUT layers");
                    else
                    {
                        biases.Add(i);
                        uint index = Convert.ToUInt32(neuronsSTR[i].Substring(0, neuronsSTR[i].Length-1)) + 1;
                        Network[i] = new Neuron[index]; //Convert only count of neurons without bias(+)
                        Network[i][index - 1].value = 1;
                        withoutBiasLength[i] = index - 1;
                    }
                }
            }

            previousWeights = new double[Network.Length][];
            //Distribution of values in weights of Neural Network and initializing PreviousWeights
            for (i = 0; i < Network.Length - 1; ++i) //             Every layer in this NeuralNetwork
            {
                previousWeights[i] = new double[Network[i].Length * withoutBiasLength[i + 1]];
                uint countOfWeights = 0;

                for (j = 0; j < Network[i].Length; ++j) //          Every neuron in layer[i]
                {
                    Network[i][j].weights = new double[withoutBiasLength[i + 1]];
                    for (uint a = 0; a < Network[i][j].weights.Length; ++a) // Every weight from neuron[i][j]
                    {
                        previousWeights[i][countOfWeights] = 0; //PrewiosWeight on the first iteration = 0
                        ++countOfWeights;

                    weightWasZeroREPEAT:
                        Network[i][j].weights[a] = Math.Round(rand.NextDouble() * (randMax - randMin) + randMin, 5); //Random value for the weight
                        if (Network[i][j].weights[a] == 0)
                            goto weightWasZeroREPEAT; //Value of weight cannot be 0
                    }
                }
            }

            //Creating a copy of NeuralNetwork to work with it
            deltaNetwork = new Neuron[Network.Length][];
            for (i = 0; i < Network.Length; ++i)
            {
                deltaNetwork[i] = new Neuron[Network[i].Length];
                for (j = 0; j < Network[i].Length; ++j)
                    deltaNetwork[i][j] = Network[i][j];
            }
        }
        /// <summary>
        /// Loads the network.
        /// </summary>
        /// <param name="pathAndName">The path to the file with its name.</param>
        public NeuralNetwork(string pathAndName)
        {
            if (System.IO.File.Exists(pathAndName))
            {
                try
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(pathAndName, System.IO.FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        NeuralNetwork n = (NeuralNetwork)formatter.Deserialize(fs);
                        Network = n.Network;
                        previousWeights = n.previousWeights;
                        Moment = n.Moment;
                        LearningRate = n.LearningRate;
                    }
                }
                catch(Exception ex)
                {
                    throw new System.IO.IOException($"Couldn't open file {pathAndName}\r\n{ex.ToString()}");
                }

                //Creating a copy of NeuralNetwork to work with it
                deltaNetwork = new Neuron[Network.Length][];
                for (i = 0; i < Network.Length; ++i)
                {
                    deltaNetwork[i] = new Neuron[Network[i].Length];
                    for (j = 0; j < Network[i].Length; ++j)
                        deltaNetwork[i][j] = Network[i][j];
                }
            }
            else
                throw new System.IO.FileNotFoundException("This file does not exist.\rTry to recheck the path or just save new Neural Network with <SaveNetwork> method", pathAndName);
        }

        /// <summary>
        /// Saves the network.
        /// </summary>
        /// <param name="pathAndName">The path to the file with its name.</param>
        public void SaveNetwork(string pathAndName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(pathAndName, System.IO.FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }
        #endregion

        #region NeuralNetwork Run
        /// <summary>
        /// Runs the network and returns Output neurons with the struct Neuron[].
        /// </summary>
        /// <param name="inputValues">This is array that contains input set.</param>
        /// <returns>Neuron[]</returns>
        public virtual Neuron[] RunNetwork(double[] inputValues)
        {
            //INPUT neurons assignment
            for (j = 0; j < inputValues.Length; ++j)
                Network[0][j].value = inputValues[j];

            for (i = 1; i < LayersCount; ++i)
            {
                for (j = 0; j < withoutBiasLength[i]; ++j) // Every neuron in layer[i]
                {
                    // Calculation value of the neuron, depending on all values of neurons and weights of synapses connected to this neuron
                    output = 0;
                    for (k = 0; k < Network[i - 1].Length; ++k) // Summ of every neuron in layer[i-1] * every neuron of weight[i-1] = weight[j]
                        output += Network[i - 1][k].value * Network[i - 1][k].weights[j];

                    //Value activation 
                    Network[i][j].value = Sigmoid(output);
                }
            }

            return Network[Network.GetLength(0) - 1];
        }

        //===========================================================================================================
        private double Sigmoid(double num)
        {
            return (1.0 / (1 + Math.Pow(Math.E, -num)));
        }
        #endregion

        #region NeuralNetwork Train
        /// <summary>
        /// Function to Train the network after run
        /// </summary>
        /// <param name="ideal">Expected values(double[]).</param>
        public virtual void TeachNetwork(double[] ideal)
        {
            //Calculating Delta(OUT) for OUTPUT neurons of the NeuralNetwork 
            for (i = 0; i < ideal.Length; ++i)
                 deltaNetwork[LayersCount - 1][i].value = DeltaOut(ideal[i], Network[LayersCount - 1][i].value);

            //Calculating Delta(HIDDEN) for HIDDEN neurons the NeuralNetwork 
            for (i = LayersCount - 2; i >= 1; --i) //Start - from the last HIDDEN layer | End - to the firs HIDDEN layer
                for (j = 0; j < Network[i].Length; ++j)
                    deltaNetwork[i][j].value = DeltaHidden(Network[i][j].weights, deltaNetwork[i + 1], Network[i][j].value);

            //Calculating delta of all the weights to change them
            for (i = LayersCount - 2; i >= 0; --i) //        Start - from the last HIDDEN layer | End - to the firs layer (INPUT)
                for (j = 0; j < Network[i].Length; ++j) //          Every neuron in layer[i]
                    for (a = 0; a < Network[i][j].weights.Length; ++a) // Every weight from neuron[i][j]
                    {
                        Grad = Network[i][j].value * deltaNetwork[i + 1][a].value; //Calculating gradient(gradient descent) for the weight
                        delta = LearningRate*Grad + Moment*previousWeights[i][j]; //Calculating delta of the weight
                        Network[i][j].weights[a] += delta; //Change the weight of synapse (ActualWeight + DeltaOfThisWeight)
                        previousWeights[i][j] = delta;
                    }
        }

        /// <summary>
        /// Function to Train the network.
        /// </summary>
        /// <param name="input"> Values for INPUT neurons </param>
        /// <param name="ideal"> Expected values for OUTPUT neurons </param>
        public virtual void TeachNetwork(double[] input, double[] ideal)
        {
            RunNetwork(input);
            TeachNetwork(ideal);
        }

        //===========================================================================================================
        private double SigmoidError(double output)
        {
            return (1 - output) * output;
        }
        private double DeltaOut(double outIdeal, double outActual)
        {
            return ((outIdeal - outActual) * SigmoidError(outActual));
        }
        private double DeltaHidden(double[] weights, Neuron[] delta, double output)
        {
            double actualSum = 0;

            for (k = 0; k < weights.Length; ++k) //((w1 * d1) + (w2 * d2) + .. + (wn * dn))
                actualSum += weights[k] * delta[k].value;

            return (SigmoidError(output) * actualSum);
        }
        #endregion
    }
}
