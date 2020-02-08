using System;
using System.IO;
using System.Reflection;

namespace NeuralNetwork
{
    class NeuralNetworkPROBuilderXOR
    {
        const double Moment = 0.01;
        const double LearningRate = 0.1;
        const string NeuronsAndLayers = "8 4 2 1"; //"[0]-InputNeurons, [1]-Neurons In 1-st HiddenLayer,
                                                  //  [2]-Neurons In 2-nd HiddenLayer,[..],[n-1]-Neurons In (n-1)-th HiddenLayer, [n]-OutputNeurons"
        public struct TrainAndTest
        {
            public double[] IN;   //All Input values 
            public double[] OUT; // All output values
        }

        static void Main()
        {
            #region Training&Test initialization
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\numbers.txt");
            string[] lines = File.ReadAllLines(filePath);

            //TRAIN units
            TrainAndTest[] trainingData = new TrainAndTest[lines.Length / 2];
            //Training and test files are .txt, where non-odd lines are INPUT values and odd lines are OUTPUT values

            uint countOfUnit = 0;
            for (uint i = 1; i <= lines.Length; ++i)
            {
                string[] line = lines[i - 1].Split(' ');
                if (i % 2 != 0)
                {
                    trainingData[countOfUnit].IN = new double[line.Length];
                    for (uint j = 0; j < line.Length; ++j) //Every INPUT value in the line must be readen
                        trainingData[countOfUnit].IN[j] = Convert.ToDouble(line[j]);
                }
                else
                {
                    trainingData[countOfUnit].OUT = new double[line.Length];
                    for (uint j = 0; j < line.Length; ++j) //Every OUTPUT value in the line must be readen
                        trainingData[countOfUnit].OUT[j] = Convert.ToDouble(line[j]);
                    ++countOfUnit;
                }
            }

            //The same for TEST units
            filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\numbersTEST.txt");
            lines = File.ReadAllLines(filePath);

            TrainAndTest[] testData = new TrainAndTest[lines.Length / 2];

            countOfUnit = 0;
            for (uint i = 1; i <= lines.Length; ++i)
            {
                string[] line = lines[i - 1].Split(' ');
                if (i % 2 != 0)
                {
                    testData[countOfUnit].IN = new double[line.Length];
                    for (uint j = 0; j < line.Length; ++j)
                        testData[countOfUnit].IN[j] = Convert.ToDouble(line[j]);
                }
                else
                {
                    testData[countOfUnit].OUT = new double[line.Length];
                    for (uint j = 0; j < line.Length; ++j)
                        testData[countOfUnit].OUT[j] = Convert.ToDouble(line[j]);
                    ++countOfUnit;
                }
            }
            #endregion

            #region Main part - network training
            double terminatingErrorProcents = 0.001; //The average error procent on which we want to end training
            uint refreshSpeed = 10000;

            //Creating an object of NeuralNetwork with same parameters as we described in variables
            //NeuralNetwork network = new NeuralNetwork(@"C:\s\Neural.aaa");
            NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, 0, 1);
            network.Moment = Moment;
            network.LearningRate = LearningRate;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            double errorSum;
            uint iteration = 0;
            double end;
            double error;
            do
            {
                errorSum = 0;
                for (uint i = 0; i < trainingData.Length; ++i) //Run through all TRAIN units
                {
                    //Running the network with current INPUT values of this unit
                    NeuralNetwork.Neuron[] endValue = network.RunNetwork(trainingData[i].IN);

                    //Counting an error of current unit
                    end = 0;
                    for (uint j = 0; j < endValue.Length; ++j)
                        end += Math.Pow(trainingData[i].OUT[j] - endValue[j].value, 2);
                    error = end / trainingData.Length; //((i-a1)*(i1-a1)+...+(in-an)*(in-an))/n
                    errorSum += error;

                    network.TeachNetwork(trainingData[i].OUT, endValue);
                }
                if (iteration++ % refreshSpeed == 0)
                    Console.WriteLine(iteration + " average error = " + Math.Round((errorSum / trainingData.GetLength(0)) * 100, 5) + "%");

            } while ((errorSum / trainingData.GetLength(0)) * 100 > terminatingErrorProcents); //while average error procent is greater tnah TEP - continue
            sw.Stop();
            #endregion

            #region Output
            uint errCount = 0;
            for (uint i = 0; i < testData.GetLength(0); ++i) //Run through all TEST units
            {
                NeuralNetwork.Neuron[] answer = network.RunNetwork(testData[i].IN);
                double[] _answer = new double[answer.Length];

                for (uint j = 0; j < answer.Length; ++j) //Normalizing answers in this unit
                    _answer[j] = answer[j].value > 0.5 ? 1 : 0;

                for (uint j = 0; j < answer.Length; ++j) //Checking answers in this unit for a mistake
                {
                    if (_answer[j] != testData[i].OUT[j]) //If a mistake was made then:
                    {
                        for (uint a = 0; a < testData[i].IN.Length; ++a) // -Write down all INPUTs 
                            Console.Write($"I{a}={testData[i].IN[a]} ");
                        Console.WriteLine();
                        for (uint k = 0; k < answer.Length; ++k) // -Write down all OUTPUTS + activated OUTPUTS + ideal OUTPUTS
                            Console.WriteLine("Real answer = " + answer[k].value + " Activated answer = " + _answer[k] + " Ideal = " + testData[i].OUT[k]);
                        ++errCount;
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("\nError count = " + errCount);
            Console.WriteLine("\nNumber of iterations = " + iteration);
            Console.WriteLine("Training time = " + ((double)sw.ElapsedMilliseconds/1000).ToString() + " sec");

            //network.SaveNetwork(@"C:\s\Neural.aaa");
            #endregion
        }
    }
}