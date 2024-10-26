# CCC CS Template

This repository provides a structured template for the "Cloudflight Coding Contest" coding contests using the C# programming language.
It includes a `LevelHandler` class to manage input, output, and test your code. 
Solutions for each level can be implemented by creating a class with the `ISolution` interface,
allowing for flexible and customizable handling of contest problems.

## Getting Started

### Prerequisites
- **C# 10.0 or later** installed
- **File structure**: Set up directories for input and output files as expected by the `LevelHandler` (explained in detail below).

### Structure
- **`LevelHandler`**: Manages reading and writing of input/output files and provides methods for testing and solving individual levels.
- **`ISolution`**: Defines the structure for implementing a solution. This interface requires two methods:
    - `Solve(string[] input)`: Solves the problem for a given string array of input file lines.
    - `Format(T output)`: Formats the output for writing to the output file.

## Setup and Usage

### 1. Creating the Input/Output Directory Structure

Input and output files should be organized by level, as `LevelHandler` reads from specific directories:
- **Input files**: Place files in the `Input` directory, e.g., `Input/level1`.
- **Output files**: Generated output files are saved in the `Output` directory, e.g., `Output/level1`.

### 2. Implementing a Solution with `ISolution`

Create a new solution class implementing `ISolution`. For example, here’s a simple solution structure:

```csharp
class SolutionExample : ISolution<List<int>>
{
    public List<int> Solve(string[] input)
    {
        // Implement your solution here
    }

    public string Format(List<int> output)
    {
        // Format output as a string for output file
        return string.Join("\n", output);
    }
}
```

### 3. Initializing `LevelHandler`

Use the `LevelHandlerBuilder` class to configure your directories and enable test diff display, if desired.

```csharp
var levelHandler = new LevelHandler.LevelHandlerBuilder()
    .SetInputDir("/path/to/Input")
    .SetOutputDir("/path/to/Output")
    .ShowTestDiff() // Optional: shows differences if the test fails
    .Build();
```

### 4. Running a Solution

To test or solve a level, use the `TestLevel` or `SolveLevel` methods of `LevelHandler`, passing the level number and solution instance.

```csharp
levelHandler.TestLevel(1, new SolutionExample());
levelHandler.SolveLevel(1, new SolutionExample());
```

- **`TestLevel`**: Compares the solution’s output to the expected output, showing differences if enabled.
- **`SolveLevel`**: Writes the solution's output directly to the output directory.

## Example

Here’s an example of a solution and how to test it:

```csharp
class SolutionExample1 : ISolution<List<int>>
{
    public List<int> Solve(string[] input)
    {
        var res = new List<int>();
        foreach (var line in input)
        {
            var nums = line.Split(" ").Select(int.Parse).ToArray();
            // Implement problem logic here
        }
        return res;
    }

    public string Format(List<int> output) => string.Join("\n", output);
}

// Initialize LevelHandler
var levelHandler = new LevelHandler.LevelHandlerBuilder()
    .SetInputDir("path/to/Input")
    .SetOutputDir("path/to/Output")
    .ShowTestDiff()
    .Build();

// Run tests
levelHandler.TestLevel(1, new SolutionExample1());
```
---
