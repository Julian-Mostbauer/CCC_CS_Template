# CCC_CS_Template

## About

Template for the "Cloudflight coding contest" made for C# programming language.

## Usage

1. Clone this repository
2. (Optional) Delete `SolutionExamples.cs` file if you don't need them
3. Edit the `Program.cs` file to implement the solution
4. Run the program with `dotnet run` command or in an IDE

## Using the LevelHandler

The LevelHandler class is responsible for reading, writing and processing of the input and output
of the problem, as well as testing it. It is used by giving it the level number and the ISolution implementation.
Before using it, it needs to be initialized with the input and output file paths.

The input should be in the following format:

```plaintext
Input_Top_Path_Folder
|- levelx
||- levelx_y.in
||    ...
||- levelx_example.in
||- levelx_example.out
```

Which is the default format of the CCC problems, when you unzip them and move them to a folder like the creatively named
`Input_Top_Path_Folder`.

The output will be in the following format:

```plaintext
Output_Top_Path_Folder
|- levelx
||- levelx_y.out
||    ...
```

Note that the example files are not processed by the LevelHandler, they are only used for testing.

Example of usage for solving:

```csharp
private static void Main()
{
    LevelHandler.Initialize("your input top path", "your output top path");
    LevelHandler.SolveLevel(3, new SolutionExamples.Solution4());
}    
```

Example of usage for testing:

```csharp
private static void Main()
{
    LevelHandler.Initialize("your path", "your path");
    LevelHandler.TestLevel(3, new SolutionExamples.Solution4());
}
```

## Using ISolution

The LevelHandler uses the ISolution interface to solve and format the output of the problem.
The ISolution has the following implementation in the `LevelHandler.cs` File:

```csharp
public interface ISolution<T>
{
    public T Solve(string[] input);
    public string Format(T output);
}
```

Examples are given in the `SolutionExamples.cs` file.
They showcase different levels of type complexity, to showcase how formating can be used.
Here is a simple example:

```csharp
public class Solution1 : ISolution<string>
{
    public string Solve(string[] input) => "Test";
    public string Format(string output) => output;
}
```

This example is a simple solution that returns the string "Test" and formats it to the same string.