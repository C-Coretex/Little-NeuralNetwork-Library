# My-Real-Neural-Network
A library that is made by me. `Program.cs` is an example of how to use the library, `NeuralNetwork.cs` is the solution for the library and you can find the .dll in the folder `"DLL is HERE"`.

## Table of contents
* [Setting up the library](#setting-up)
* [NeuralNetwork creation](#neuralnetwork-creation)
* [NeuralNetwork usage](#NeuralNetwork-usage)
* [Contributing](#contributing)
* [Notes](#notes)
---


## Setting up the library

Add the DLL to your project ([how to add dll in VS](https://www.c-sharpcorner.com/UploadFile/1e050f/creating-and-using-dll-class-library-in-C-Sharp/)) and type this:
```C#
using NN;
using static NN.NeuralNetwork;
```
---


## How to use the library
### NeuralNetwork creation

Firstly, you have to declare the struct of the Neural Network.
```C#
const string NeuronsAndLayers = "4+ 15+ 6 3"; //"[0]-InputNeurons, [1]-Neurons In 1-st HiddenLayer,
// [2]-Neurons In 2-nd HiddenLayer,[..],[n-1]-Neurons In (n-1)-th HiddenLayer, [n]-OutputNeurons"
// put + in each layer (except OUTPUT) to add bias
```
*You **must** declare at least 2 layers of the Neural Network. [0] (Input) and [n] (Output).*

- To create a Neural Network you need to create an object of a class `"NN.NeuralNetwork"`:
```C#
NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, -1, 1);
```
*The first argument is the struct of the Neural Network (read above), after it you need to declare the bounds of random weights of synapses.*

- To change the **Moment** and **LearningRate** of the Neural Network:
```C#
NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, -1, 1)
{
      Moment = MyMoment,
      LearningRate = MyLearningRate
};
```
---


### NeuralNetwork usage
- To run the Neural Network use:
```C#
Neuron[] endValue = network.RunNetwork(trainingData[i].IN);
```
*[Neuron[]](#neuron-struct) is a struct of a NeuralNetwork class. Each neuron has it's current value and outgoing synapse weights.*
*The only argument of the function `RunNetwork()` is an array with Input values of Neurons. In this case, it may be `[1, 1, 0, 1]` (as it was written earlier, the current Neural Network has only 4 Input Neurons).*
*The function returns Output value of Neurons, so you can interpret it as you wish*

- To teach the Network by current example (to make one iteration of teaching by current training example) use this function:

```C#
network.TeachNetwork(trainingData[i].OUT, endValue);
```
*Where `trainingData[i].OUT` is an array with expected Output value of Neurons. In this case, it may be `[0, 0, 0]` (as it was written earlier, the current Neural Network has only 3 Output Neurons). `endValue` is an array with actual Output values of Neurons (that you've got after running the Network).*

---


### NeuralNetwork Load/Save

- To Load the Neural Network use this method:
```C#
NeuralNetwork network = new NeuralNetwork(@"C:\s\MyNeural.aaa");
``` 
*The only argument of this function is a path to the Neural Network file (as you could guess).*

- To Save the Neural Network use this method:
```C#
network.SaveNetwork(@"C:\s\Neural.aaa");
``` 
*The only argument of this function is a path to the place where the Network will be saved and the name of the Neural Network (as you could guess).*

---



## Contributing
#### You can freely use the library if you want.

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

---

# Notes

### Neuron struct

```C#
public struct Neuron
{
    public double value; // Value of the neuron
    public double[] w;  //  All weights outgoing from this neuron
}
```
---
