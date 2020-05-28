using NN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace NeuralNetworkExample2
{
    public partial class TeachNetworkNumber : Form
    {
        Random rnd = new Random();
        public const int NUM_OF_PICTURES_IN_DATASET = 1000;

        NeuralNetwork network;
        NeuralNetworkStatistic statistic = new NeuralNetworkStatistic();
        DatasetZip datasetZip = new DatasetZip("dataset.zip");

        Drawer numberDrawer;
        Graphic graphicDrawer;

        public TeachNetworkNumber()
        {
            InitializeComponent();

            network = new NeuralNetwork("400 16+ 16 8 10", -1, 1)
            {
                LearningRate = 0.8,
                Moment = 0.3
            };

            lbl_learningRate.Text = $"Learning rate: {network.LearningRate}";
            lbl_moment.Text = $"Moment: {network.Moment}";

            numberDrawer = new Drawer(pnl_draw);
            graphicDrawer = new Graphic(pnl_graphic, graphicStep:1);

            datasetZip.Extract();
        }
        
        void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;

            new Thread(() =>
            {
                for (int i = 0; i < 50000; i++)
                {
                    int inputNumOfNeuralNetwork = rnd.Next(0, 10);

                    Bitmap imageWithNumber = datasetZip.GetRandomImageByNum(inputNumOfNeuralNetwork);

                    NeuralNetworkLearnResult iterationResult = LearnNeuralnetwork(imageWithNumber, inputNumOfNeuralNetwork);


                    if (iterationResult.AnswerExpected == iterationResult.AnswerActual)
                    {
                        statistic.RightAnswers += 1;
                    }
                    else
                    {
                        statistic.WrongAnswers += 1;
                    }

                    try
                    {
                        if (i % 5 == 0) // show info every 5 iterations
                        {
                            double error = GetError(iterationResult.OutputNeurons);
                            graphicDrawer.DrawPoint((float)(error));

                            this.Invoke(new MethodInvoker(() =>
                            {
                                lbl_iteration.Text = $"Iteration: {i}";
                                lbl_right.Text = $"Right: {statistic.RightAnswers}";
                                lbl_mistakes.Text = $"Mistakes: {statistic.WrongAnswers}";
                                lbl_percent.Text = $"Right answers: {statistic.PercentOfRight.ToString("P")}";
                            }));
                        }

                        if (i % 100 == 0) // show picture every 100 iterations
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                picturebx_currentNum.Image = imageWithNumber;
                                lbl_ActualNum.Text = ((int)iterationResult.AnswerActual).ToString();
                            }));
                        }
                    }
                    catch { }
                }
            })
            { IsBackground = true }.Start();
        }

        NeuralNetworkLearnResult LearnNeuralnetwork(Bitmap img, int num)
        {
            Bitmap compressedimage = new Bitmap(img, 20, 20);
            double[] imageData = CovertPicture(compressedimage);

            double[] output = GetOuputArrayForNumber(num);

            Neuron[] neurons = network.RunNetwork(imageData);
            network.TeachNetwork(output);

            int anserGiven = (int)GetAnswer(neurons).Item2;

            return new NeuralNetworkLearnResult(num, anserGiven, output, neurons);
        }

        double GetError(Neuron[] neurons)
        {
            double error = GetAnswer(neurons).Item1.value;
            return error;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neurons"> output neurons</param>
        /// <returns> Neuron — neuron with biggest value; int — index of this neuron (answer)</returns>
        (Neuron, int) GetAnswer(Neuron[] neurons)
        {
            double max = neurons[0].value;
            double maxIndex = 0;
            for (int index = 0; index < neurons.Length; index++)
            {
                if (neurons[index].value > max)
                {
                    max = neurons[index].value;
                    maxIndex = index;
                }
            }


            for (double index = maxIndex; index < neurons.Length; index++)
            {
                return (neurons[(int)index], (int)index);
            }
            throw new Exception();
        }

        double[] GetOuputArrayForNumber(int num)
        {
            /* Example:
             * for num 0 you get: [1, 0, 0, 0, 0, 0, 0, 0, 0, 0]
             * for num 1 you get: [0, 1, 0, 0, 0, 0, 0, 0, 0, 0]
             * for num 9 you get: [0, 0, 0, 0, 0, 0, 0, 0, 0, 1]
             */
            List<double> result = new List<double>(10);
            for (int i = 0; i < 10; i++)
            {
                if (i == num)
                {
                    result.Add(1);
                }
                else
                {
                    result.Add(0);
                }
            }
            return result.ToArray();
        }

        double[] CovertPicture(Bitmap bitmap)
        {
            List<double> result = new List<double>(400);

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var pixel = bitmap.GetPixel(j, i);
                    if (pixel.R == 255 &&
                        pixel.G == 255 &&
                        pixel.B == 255)
                    {
                        result.Add(0);
                    }
                    else
                    {
                        result.Add(1);
                    }
                }
            }
            if (result.Count == 400)
            {
                return result.ToArray();
            }
            throw new Exception("wrong image. must be 20x20");
        }

        void btn_check_Click(object sender, EventArgs e)
        {
            picturebx_currentNum.Image = numberDrawer.bmp;
            lbl_ActualNum.Text = GetAnswer(network.RunNetwork(CovertPicture(new Bitmap(numberDrawer.bmp, 20, 20)))).Item2.ToString();

        }

        void btn_clear_Click(object sender, EventArgs e)
        {
            numberDrawer.Clear(pnl_draw.BackColor);
        }
    }
}