using System;
using System.IO;
using System.Reflection;

namespace NeuralNetwork
{
    class NeuralNetworkPROBuilderXOR
    {
        const double Moment = 0.1;
        const double LearningRate = 0.1;
        const string NeuronsAndLayers = "8 4 2 1";
        public struct TrainAndTest
        {
            public double[] IN;
            public double[] OUT;
        }

        static void Main()
        {
            #region Training&Set
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\numbers.txt");
            string[] lines = File.ReadAllLines(filePath);

            int trainingCount = lines.Length;

            TrainAndTest[] trainingData = new TrainAndTest[trainingCount/2];

            int count = 0;
            for(int i = 1; i <= lines.Length; ++i)
            {
                string[] line = lines[i-1].Split(' ');
                if (i % 2 != 0)
                {
                    trainingData[count].IN = new double[line.Length];
                    for (int j = 0; j < line.Length; ++j)
                        trainingData[count].IN[j] = Convert.ToDouble(line[j]);
                }
                else
                {
                    trainingData[count].OUT = new double[line.Length];
                    for (int j = 0; j < line.Length; ++j)
                        trainingData[count].OUT[j] = Convert.ToDouble(line[j]);
                    ++count;
                }
            }

            filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\numbersTEST.txt");
            lines = File.ReadAllLines(filePath);

            int testCount = lines.Length;
            TrainAndTest[] testData = new TrainAndTest[testCount/2];

            count = 0;
            for (int i = 1; i <= lines.Length; ++i)
            {
                string[] line = lines[i - 1].Split(' ');
                if (i % 2 != 0)
                {
                    testData[count].IN = new double[line.Length];
                    for (int j = 0; j < line.Length; ++j)
                        testData[count].IN[j] = Convert.ToDouble(line[j]);
                }
                else
                {
                    testData[count].OUT = new double[line.Length];
                    for (int j = 0; j < line.Length; ++j)
                        testData[count].OUT[j] = Convert.ToDouble(line[j]);
                    ++count;
                }
            }
            #endregion

            #region Main part - network teaching
            double terminatingError = 0.00001;
            uint refreshSpeed = 100000;

            NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers);
            network.Moment = Moment;
            network.LearningRate = LearningRate;


            double errorSum;
            uint iteration = 0;
            do
            {
                errorSum = 0;
                for (int i = 0; i < trainingData.Length; ++i)
                {
                    NeuralNetwork.Neuron[] endValue = network.RunNetwork(trainingData[i].IN);

                    double end = 0;
                    for(int j = 0; j < endValue.Length; ++j)
                        end += Math.Pow(trainingData[i].OUT[j] - endValue[j].value, 2);
                    
                    double error = end / trainingData.Length; // ((i1-a1)*(i1-a1)+...+(in-an)*(in-an))/n
                    errorSum += error;

                    network.TeachNetwork(trainingData[i].OUT, endValue);
                }
                if (iteration++ % refreshSpeed == 0)
                    Console.WriteLine(iteration + " summ of errors = " + Math.Round((errorSum / trainingData.GetLength(0)) * 100, 5) + "%");

            } while ((errorSum / trainingData.GetLength(0)) > terminatingError);
            #endregion

            #region Output
            int errCount = 0;
            for (int i = 0; i < testData.GetLength(0); ++i)
            {
                NeuralNetwork.Neuron[] answer = network.RunNetwork(testData[i].IN);
                double[] _answer = new double[answer.Length];

                for (int j = 0; j < answer.Length; ++j)
                    _answer[j] = answer[j].value > 0.5 ? 1 : 0;
                for (int j = 0; j < answer.Length; ++j)
                {
                    if (_answer[j] != testData[i].OUT[j])
                    {
                        for (int a = 0; a < testData[i].IN.Length; ++a)
                            Console.Write($"I{a}={testData[i].IN[a]} ");
                        Console.WriteLine();
                        Console.WriteLine("Real answer = " + answer[j].value + " Activated answer = " + _answer[j] + " Ideal = " + testData[i].OUT[j]);
                        ++errCount;
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("\nError count = " + errCount);
            Console.WriteLine("\nNumber of iterations = " + iteration);

            #endregion
        }
    }
}