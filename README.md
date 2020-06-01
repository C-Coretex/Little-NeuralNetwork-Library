# Real Neural Network

![Training example](https://github.com/C-Coretex/Little-NeuralNetwork-Library/blob/master/NeuralNetworkExample_FishersIris/TrainingAndTest/TrainingPhoto.png)

A library that is made for community. `NeuralNetworkExample_FishersIris` and `NeuralNetworkExample_ImageRecognition` are examples of how to use the library, `NeuralNetwork` is the project of the library and you can find the .dll in the folder `"DLL is HERE"`. There you can find 2 folders with dll files for `.Net Core` and `.Net Framework`.

## Table of contents

* [Optimizations that has been done](#optimizations-that-has-been-done)
* [Setting up the library](#setting-up-the-library)
* [NeuralNetwork creation](#neuralnetwork-creation)
* [NeuralNetwork usage](#neuralnetwork-usage)
* [NeuralNetwork teaching example](#example-of-each-teach-iteration)
* [Contributing](#contributing)
* [Notes](#notes)

---

## Optimizations that has been done

- `Structs` are more preferable than `classes` and `objects`.

- All the variables are introduced in the body of the class, so `garbage collector` will execute less often.

- A `reference` is used for passing structs in the arguments, therefore the program does not copy the struct every time the function is called.

- `For` loop is more preferable than `foreach` loop. Also you can unroll loop.

- Sometimes `serial code` is more preferable than functions.

---

## Setting up the library

**If you are using `.Net Framework` you must uncheck `Prefer 32-bit` in project preferences. [Uncheck #1](https://www.neovolve.com/2015/07/31/disable-prefer-32-bit/) or [Uncheck #2](https://www.codeofclimber.ru/2015/make-sure-prefer-32-bit-option-is-turned-off-for-net-4-5-executables/)**

Add the DLL to your project ([how to add dll in VS](https://www.c-sharpcorner.com/UploadFile/1e050f/creating-and-using-dll-class-library-in-C-Sharp/)) and type this:

```C#
using NN;
```

---

## How to use the library

### Neuron layer types

There are 3 types of neuron layers:

* Input layer
* Hidden layer
* Output layer

There may be one bias in each layer (except output). It's value is always '1' in every scenario. 

### NeuralNetwork creation

Firstly, you have to declare the struct of the Neural Network.

```C#
const string NeuronsAndLayers = "4+ 15+ 6 3"; 
// 4+  — input neurons and 1 bias neuron ([0]-InputNeurons)
// 15+ — hidden neurons in 1-st hidden layer and 1 bias neuron ([1]-Neurons In 1-st HiddenLayer) ([..])
// 6  — neurons in 2-nd hidden layer ([n-1]-Neurons In 2-nd HiddenLayer)
// 3  — output neurons ([n]-OutputNeurons)
// put + in each layer (except OUTPUT) to add bias
```

*You **must** declare at least 2 layers of the Neural Network. [0] (Input) and [n] (Output).*

- To create a Neural Network you need to create an object of a class `"NN.NeuralNetwork"` using its constructor:
  
  ```C#
  NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, -1, 1);
  ```
  
  *The first argument is the struct of the Neural Network (read above), after it you need to declare the bounds of random weights of synapses.*

- To change the **Moment** and **LearningRate** of the Neural Network:
  
  ```C#
  NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, -1, 1)
  {
      Moment = MyMoment, // from 0 to 1, for example 0.5
      LearningRate = MyLearningRate // from 0.1 to ∞, for example 1.2
  };
  ```

---

### NeuralNetwork usage

- To run the Neural Network use:
  
  ```C#
  Neuron[] endValue = network.RunNetwork(trainingData[i].IN);
  ```
  
  `[Neuron[]](#neuron-struct)` is a struct of a NeuralNetwork class. Each neuron has it's current value and outgoing synapse weights.
  The only argument of the function `RunNetwork()` is an array with Input values of Neurons. In this case, it may be `[1, 1, 0, 1]` (as it was written earlier, the current Neural Network has only 4 Input Neurons).
  The function returns Output value of Neurons, so you can interpret it as you wish

- To teach the Network by current example (to make one iteration of teaching by current training example) use this function:

```C#
network.TeachNetwork(trainingData[i].OUT);
```

*Where `trainingData[i].OUT` is an array with expected output value of neurons. In this case, it may be `[0, 1, 0]` (as it was written earlier, the current Neural Network has only 3 output neurons).*

---

### NeuralNetwork Load/Save

- To Load the Neural Network use this constructor:
  
  ```C#
  NeuralNetwork network = new NeuralNetwork(@"C:\s\MyNeural.aaa");
  ```
  
  *The only argument of this function is a path to the Neural Network file (as you could guess).*

- To Save the Neural Network use this method:
  
  ```C#
  network.SaveNetwork(@"C:\s\Neural.aaa");
  ```
  
  *The only argument of this function is a path where the Network will be saved and the name of the Neural Network file (as you could guess).*

---

### Example of each teach iteration

- Creating a new Neural Network (or you can load it if you already have one):
  
  ```C#
  string NeuronsAndLayers = "7 8+ 6 5 3";
  NeuralNetwork network = new NeuralNetwork(NeuronsAndLayers, -1, 1)
  {
    Moment = 0.5,
    LearningRate = 0.7
  };
  ```

- Declaring input data:
  
  ```C#
  double[] inputData = new double[] { 1, 0, 1, 1, 0, 1, 1 };
  ```

- Declaring output data:
  
  ```C#
  double[] outputData = new double[] { 1, 0, 1 };
  ```

- Neural network teaching:
  
  ```C#
  Neuron[] outputNeurons = network.RunNetwork(inputData);
  network.TeachNetwork(outputData);
  ```

- Alternative teaching (so you don't have to `Run` the Network manually before Teaching):
  
  ```C#
  network.TeachNetwork(inputData, outputData);
  ```

- Neural network running:
  
  ```C#
  Neuron[] outputNeurons = network.RunNetwork(inputData);
  ```

- Counting the error of the outpt
  
  ```C#
  Neuron[] outputNeurons = network.RunNetwork(inputData);

//Counting an error of current unit
end = 0;
for (uint neuronIndex = 0; neuronIndex < outputNeurons.Length; ++neuronIndex)
         end += Math.Pow(outputData[neuronIndex] - outputNeurons[neuronIndex].value, 2);
error = end / outputData.Length; //((i1[expected]-a1[output])*(i1-a1)+...+(in-an)*(in-an))/n
errorSum += error;
```
  
Do it in a loop for greater efficiency. For more information check the example in `Program.cs`.

---

## Contributing
#### You can freely use the library if you want.

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

---

# Notes

### Neuron struct

```C#
[Serializable]
public struct Neuron
{
      public double value;
      public double[] weights;
}
```

---
