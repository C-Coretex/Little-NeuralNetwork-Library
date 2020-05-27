using NN;
using System;

namespace NeuralNetworkExample2
{
    public struct NeuralNetworkLearnResult
    {
        public int AnswerExpected;
        public int AnswerActual;

        public double[] NeuronsOutput;
        public Neuron[] OutputNeurons;

        public NeuralNetworkLearnResult(int answerExpected, int answerActual, double[] output, Neuron[] neurons)
        {
            AnswerExpected = answerExpected;
            AnswerActual = answerActual;
            this.NeuronsOutput = output ?? throw new ArgumentNullException(nameof(output));
            OutputNeurons = neurons ?? throw new ArgumentNullException(nameof(neurons));
        }
    }
}
