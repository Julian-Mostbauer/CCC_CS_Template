using System.Text;

namespace CCC_CS_Template;

/// <summary>
/// Contains the input files and the example input/output for a level
/// Input constrains:
/// <br/>
/// 5 input files, none being empty
/// <br/>
/// ID must be in bounds
/// </summary>
public record LevelInputFile
{
    private const int MAX_ID = 5;
    private const int MIN_ID = 1;
    public string[] Lines { get; }
    public int Id { get; }

    public LevelInputFile(string[] lines, int id)
    {
        if (lines.Length == 0) throw new ArgumentException("Empty input file");
        if (id is < MIN_ID or > MAX_ID) throw new ArgumentException("Invalid id");

        Lines = lines;
        Id = id;
    }
}

/// <summary>
/// Contains all the data from a given level
/// </summary>
public record LevelInput
{
    private const int INPUT_FILE_COUNT = 5;
    public List<LevelInputFile> InputFiles { get; }
    public string[] ExampleInput { get; }
    public string ExampleOutput { get; }

    /// <summary>
    /// Creates a new LevelInput object
    /// Throws an exception if the input files are not valid
    /// </summary>
    /// <param name="inputFiles"></param>
    /// <param name="exampleInput"></param>
    /// <param name="exampleOutput"></param>
    /// <exception cref="ArgumentException"></exception>
    public LevelInput(List<LevelInputFile> inputFiles, string[]? exampleInput, string? exampleOutput)
    {
        if (inputFiles.Count != INPUT_FILE_COUNT) throw new ArgumentException($"Expected {INPUT_FILE_COUNT} inputs");
        if (inputFiles.Any(x => x.Lines.Length == 0)) throw new ArgumentException("Empty input file");
        if (exampleOutput == null || exampleInput == null)
            throw new ArgumentException("Example input/output not found");
        if (exampleOutput.Length == 0 || exampleInput.Length == 0)
            throw new ArgumentException("Empty example input/output");

        InputFiles = inputFiles;
        ExampleInput = exampleInput;
        ExampleOutput = exampleOutput;
    }
};

/// <summary>
/// Handles the reading and writing of input and output files, as well as testing and solving levels
/// </summary>
public static partial class LevelHandler
{
    private static string? _inputDir = null;
    private static string? _outputDir = null;

    [System.Text.RegularExpressions.GeneratedRegex(@"^level\d+_\d+\.in$")]
    private static partial System.Text.RegularExpressions.Regex FILE_NAME_REGEX();

    private static bool IsValidFileName(string name) =>
        FILE_NAME_REGEX().IsMatch(name);

    /// <summary>
    /// Initializes the directories for the input and output files
    /// Required before using <see cref="TestLevel{T}"/> and <see cref="SolveLevel{T}"/>
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir"></param>
    public static void Initialize(string inputDir, string outputDir)
    {
        _inputDir = inputDir;
        _outputDir = outputDir;
    }

    private static LevelInput ReadLvlInput(int level)
    {
        if (_inputDir == null) throw new Exception("Input Directory not initialized");
        string dir = $"{_inputDir}/level{level}";
        string[] foundFiles = Directory.GetFiles(dir);

        List<LevelInputFile> inputs = [];
        string[]? exampleInp = null;
        string? exampleOut = null;

        foreach (var file in foundFiles)
        {
            // handle the example files separately
            if (file.Contains("example"))
            {
                if (file.EndsWith(".in")) exampleInp = File.ReadAllLines(file);
                else exampleOut = File.ReadAllText(file).Trim();
                continue;
            }

            // skip files that don't match the pattern
            var fileName = Path.GetFileName(file);
            if (!IsValidFileName(fileName)) continue;

            // read the file contents and save it along with the id, because the order of the input files is not certain
            var contents = File.ReadAllLines(file);

            // the id is the first digit in the file name after the underscore
            // should always work because the file name is validated by IsValidFileName
            var id = int.Parse(fileName.Split('_')[1][0].ToString());
            inputs.Add(new LevelInputFile(contents, id));
        }

        var fileContents = new LevelInput(inputs, exampleInp, exampleOut);
        return fileContents;
    }

    private static void WriteLvlOutput<T>(int level, LevelInput input, ISolution<T> solution)
    {
        if (_outputDir == null) throw new Exception("Output directory not initialized");
        var dir = $"{_outputDir}/level{level}";
        Directory.CreateDirectory(dir);

        // use the solution on each file and write the output
        foreach (var inputFile in input.InputFiles)
        {
            var output = solution.Solve(inputFile.Lines);
            var formattedOutput = solution.Format(output);

            // ensure the output ends with a newline
            if (!formattedOutput.EndsWith('\n')) formattedOutput += '\n';
            File.WriteAllText($"{dir}/level{level}_{inputFile.Id}.out", formattedOutput);
        }
    }

    /// <summary>
    /// Reads the input files for a level, solves them and writes the output files
    /// </summary>
    /// <param name="level"></param>
    /// <param name="solution"></param>
    /// <typeparam name="T"></typeparam>
    public static void SolveLevel<T>(int level, ISolution<T> solution)
    {
        try
        {
            var levelInput = ReadLvlInput(level);
            WriteLvlOutput(level, levelInput, solution);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Tests a level using the given example input and output
    /// Prints the expected and actual output
    /// Results: <br/>
    /// Green: Test passed <br/>
    /// Yellow: Test failed <br/>
    /// Red: Test errored
    /// </summary>
    /// <param name="level"></param>
    /// <param name="solution"></param>
    /// <typeparam name="T"></typeparam>
    public static void TestLevel<T>(int level, ISolution<T> solution)
    {
        try
        {
            var levelInput = ReadLvlInput(level);
            var exampleInput = levelInput.ExampleInput;
            var exampleOutput = levelInput.ExampleOutput;

            var output = solution.Solve(exampleInput);
            var formattedOutput = solution.Format(output);

            Console.WriteLine("Expected:");
            Console.WriteLine(exampleOutput);
            Console.WriteLine("Got:");
            Console.WriteLine(formattedOutput);

            if (CleanStringEqual(exampleOutput, formattedOutput, out var diff))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Test passed");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(diff);
                Console.WriteLine("Test failed");
            }
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.WriteLine("Test errored");
        }

        Console.ResetColor();
    }

    private static bool CleanStringEqual(string a, string b, out string diff)
    {
        string NormalizeNewlines(string input)
        {
            return input.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        StringBuilder diffBuilder = new();
        a = NormalizeNewlines(a).Trim();
        b = NormalizeNewlines(b).Trim();

        for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
        {
            char expectedChar = i < a.Length ? a[i] : ' ';
            char actualChar = i < b.Length ? b[i] : ' ';
            if (actualChar != expectedChar)
            {
                diffBuilder.Append(
                    $"({i}.idx)\tExpected: |Got:\n\t{expectedChar}\t  |{actualChar}\n{new string('-', 23)}\n");
            }
        }

        diff = diffBuilder.ToString();
        return diffBuilder.Length == 0;
    }
}

/// <summary>
/// ISolution is a generic interface that defines the Solve and Format methods
/// Generic type T: the type of the output of the Solve method
/// Methods: <br/>
/// Solve: takes an array of strings as input and returns an object of type T <br/>
/// Format: takes the output of Solve and returns a string representation of it
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISolution<T>
{
    public T Solve(string[] input);
    public string Format(T output);
}