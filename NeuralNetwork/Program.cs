using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using NN;
using static NN.NeuralNetwork;

namespace NeuralNetworkExample
{
    //---------------------------------------------------------------
    //
    //
    //  This file is just an example how to use the NeuralNetwork library.
    //  You are free to use the library in yout own needs
    //
    //
    //---------------------------------------------------------------

    class NeuralNetworkPROBuilder
    {
        const double MomentTemp = 0.7;
        const double LearningRateTemp = 0.1;
        const string NeuronsAndLayers = "4+ 15+ 6 3"; //"[0]-InputNeurons, [1]-Neurons In 1-st HiddenLayer,
        //  [2]-Neurons In 2-nd HiddenLayer,[..],[n-1]-Neurons In (n-1)-th HiddenLayer, [n]-OutputNeurons"
        //   put + in each layer (except OUTPUT) to add bias
        static double terminatingErrorProcents = 0.001; //The average error procent on which we want to end training
        static uint refreshSpeed = 1800;
        public struct TrainAndTest
        {
            public double[] IN;   //All Input values 
            public double[] OUT; // All output values
        }

        private static TrainAndTest[] AddTrainOrTest(string fullPath)
        {
            string[] lines = File.ReadAllLines(fullPath);
            //TRAIN or TEST units
            TrainAndTest[] Data = new TrainAndTest[lines.Length / 2];
            //Training and test files are .txt, where non-odd lines are INPUT values and odd lines are OUTPUT values

            uint countOfUnit = 0;
            for (uint i = 1; i <= lines.Length; ++i)
            {
                if (i % 2 != 0)
                {
                    string[] line = lines[i - 1].Split('	');
                    Data[countOfUnit].IN = new double[line.Length];
                    for (uint j = 0; j < line.Length; ++j) //Every INPUT value in the line must be readen
                        Data[countOfUnit].IN[j] = Convert.ToDouble(line[j]);
                }
                else
                {
                    string[] line = lines[i - 1].Split(' ');
                    Data[countOfUnit].OUT = new double[line.Length];
                    for (int j = 0; j < line.Length; ++j) //Every OUTPUT value in the line must be readen
                        Data[countOfUnit].OUT[j] = Convert.ToDouble(line[j]);
                    ++countOfUnit;
                }
            }

            return Data;
          //  return Data;
        }
        private static uint CheckForMistakes (ref NeuralNetwork network, ref TrainAndTest[] testData)
        {
            uint errCount = 0;

            for (uint i = 0; i < testData.GetLength(0); ++i) //Run through all TEST units
            {
                Neuron[] answer = network.RunNetwork(testData[i].IN);
                double[] _answer = new double[answer.Length];

                for (uint j = 0; j < answer.Length; ++j) //Normalizing answers in this unit
                    _answer[j] = answer[j].value > 0.5 ? 1 : 0;

                for (uint j = 0; j < _answer.Length; ++j) //Checking answers in this unit for a mistake
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
                        break;
                    }
                }
            }

            return errCount;
        }


        private static bool pressedENTER = false;
        private static void CheckForEnter()
        {
           while (!pressedENTER)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    pressedENTER = true;
                    return;
                }
            }
        }
        static void Main()
        {
            var sepByThous = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            sepByThous.NumberGroupSeparator = " ";

            #region Training&Test initialization
            string firstPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            firstPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\TrainingAndTest\");

            TrainAndTest[] trainingData = AddTrainOrTest(firstPath + "numbers.txt");
            //The same for TEST units
            TrainAndTest[] testData = AddTrainOrTest(firstPath + "numbersTEST.txt");

            #endregion

            #region Main part - network training
            new Thread(new ThreadStart(CheckForEnter)).Start();

            //Creating an object of NeuralNetwork with same parameters as we described in variables
            //NeuralNetwork network = new NeuralNetwork(@"C:\s\Neural.aaa");
            NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, -1, 1)
            {
                Moment = MomentTemp,
                LearningRate = LearningRateTemp
            };

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
                    Neuron[] endValue = network.RunNetwork(trainingData[i].IN);

                    //Counting an error of current unit
                    end = 0;
                    for (uint j = 0; j < endValue.Length; ++j)
                        end += Math.Pow(trainingData[i].OUT[j] - endValue[j].value, 2);
                    error = end / trainingData.Length; //((i-a1)*(i1-a1)+...+(in-an)*(in-an))/n
                    errorSum += error;

                    network.TeachNetwork(trainingData[i].OUT, endValue);
                }
                if (iteration++ % refreshSpeed == 0)
                    Console.WriteLine(iteration.ToString("#,0", sepByThous) + "  average error = " + Math.Round((errorSum / trainingData.GetLength(0)) * 100, 5) + "%" + "    =    " + Math.Round(errorSum * 100, 5) + "% general,    " + ((double)sw.ElapsedMilliseconds / 1000).ToString("#,0.000", sepByThous) + " sec");
            } while (((errorSum / trainingData.GetLength(0)) * 100 > terminatingErrorProcents) && !pressedENTER); //while average error procent is greater tnah TEP - continue
            sw.Stop();
            #endregion

            #region Output
            uint errCount = CheckForMistakes(ref network, ref testData);

            Console.WriteLine("\nAccuracy = " + Math.Round((double)(testData.GetLength(0)-(errCount)) / testData.GetLength(0) * 100, 3) + "%");
            Console.WriteLine("Right answers from the test = " + (testData.GetLength(0)-errCount).ToString("#,0", sepByThous) + " of " + testData.GetLength(0).ToString("#,0", sepByThous));
            Console.WriteLine("\nNumber of iterations = " + iteration.ToString("#,0", sepByThous));
            Console.WriteLine("Training time = " + ((double)sw.ElapsedMilliseconds/1000).ToString("#,0.000", sepByThous) + " sec");

            //network.SaveNetwork(@"C:\s\Neural.aaa");
            #endregion
        }
    }
}