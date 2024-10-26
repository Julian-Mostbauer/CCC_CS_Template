using System.Text;

namespace CCC_CS_Template;

public partial class LevelHandler
{
    private readonly FileHandler _fileHandler;
    private readonly bool _showTestDiff;

    /// <summary>
    /// Initializes a new instance of the <see cref="LevelHandler"/> class.
    /// </summary>
    /// <param name="inputDir">The input directory.</param>
    /// <param name="outputDir">The output directory.</param>
    /// <param name="showTestDiff">Indicates whether to show test differences.</param>
    private LevelHandler(string inputDir, string outputDir, bool showTestDiff)
    {
        _fileHandler = new FileHandler(inputDir, outputDir);
        _showTestDiff = showTestDiff;
    }

    /// <summary>
    /// Reads the input files for a level, solves them and writes the output files.
    /// </summary>
    /// <param name="level">The level number.</param>
    /// <param name="solution">The solution to apply.</param>
    /// <typeparam name="T">The type of the solution output.</typeparam>
    public void SolveLevel<T>(int level, ISolution<T> solution)
    {
        try
        {
            var levelInput = _fileHandler.ReadLvlInput(level);
            _fileHandler.WriteLvlOutput(level, levelInput, solution);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Tests a level using the given example input and output.
    /// Prints the expected and actual output.
    /// Results: <br/>
    /// Green: Test passed <br/>
    /// Yellow: Test failed <br/>
    /// Red: Test errored
    /// </summary>
    /// <param name="level">The level number.</param>
    /// <param name="solution">The solution to apply.</param>
    /// <typeparam name="T">The type of the solution output.</typeparam>
    public void TestLevel<T>(int level, ISolution<T> solution)
    {
        try
        {
            var levelInput = _fileHandler.ReadLvlInput(level);
            var exampleInput = levelInput.ExampleInput;
            var exampleOutput = levelInput.ExampleOutput;

            var output = solution.Solve(exampleInput);
            var formattedOutput = solution.Format(output);

            Console.WriteLine("Expected:");
            Console.WriteLine(exampleOutput);
            Console.WriteLine("Got:");
            Console.WriteLine(formattedOutput);

            if (CalcStringDiff(exampleOutput, formattedOutput, out var diff))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Test passed");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (_showTestDiff) Console.WriteLine(diff);
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

    /// <summary>
    /// Calculates the differences between two strings and returns whether they are identical.
    /// </summary>
    /// <param name="a">The first string to compare.</param>
    /// <param name="b">The second string to compare.</param>
    /// <param name="diff">The output string that contains the differences between the two strings.</param>
    /// <returns>True if the strings are identical; otherwise, false.</returns>
    private static bool CalcStringDiff(string a, string b, out string diff)
    {
        StringBuilder diffBuilder = new();
        a = NormalizeNewlines(a).Trim();
        b = NormalizeNewlines(b).Trim();

        for (var i = 0; i < Math.Max(a.Length, b.Length); i++)
        {
            var expectedChar = i < a.Length ? a[i] : ' ';
            var actualChar = i < b.Length ? b[i] : ' ';
            if (actualChar != expectedChar)
            {
                diffBuilder.Append(
                    $"({i}.idx)\tExpected: |Got:\n" +
                    $"\t{expectedChar}\t  |{actualChar}\n" +
                    $"{new string('-', 23)}\n");
            }
        }

        diff = diffBuilder.ToString();
        return diffBuilder.Length == 0;

        string NormalizeNewlines(string input) => input.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    public class LevelHandlerBuilder
    {
        private string? _inputDir = null;
        private string? _outputDir = null;
        private bool _showTestDiff = false;

        /// <summary>
        /// Sets the input directory.
        /// </summary>
        /// <param name="inputDir">The input directory.</param>
        /// <returns>The <see cref="LevelHandlerBuilder"/> instance.</returns>
        public LevelHandlerBuilder SetInputDir(string inputDir)
        {
            _inputDir = inputDir;
            return this;
        }

        /// <summary>
        /// Sets the output directory.
        /// </summary>
        /// <param name="outputDir">The output directory.</param>
        /// <returns>The <see cref="LevelHandlerBuilder"/> instance.</returns>
        public LevelHandlerBuilder SetOutputDir(string outputDir)
        {
            _outputDir = outputDir;
            return this;
        }

        /// <summary>
        /// Enables the display of test differences.
        /// </summary>
        /// <returns>The <see cref="LevelHandlerBuilder"/> instance.</returns>
        public LevelHandlerBuilder ShowTestDiff()
        {
            _showTestDiff = true;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="LevelHandler"/> instance.
        /// </summary>
        /// <returns>The <see cref="LevelHandler"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the input or output directory is not set.</exception>
        public LevelHandler Build()
        {
            if (string.IsNullOrEmpty(_inputDir)) throw new ArgumentException("Input directory not set");
            if (string.IsNullOrEmpty(_outputDir)) throw new ArgumentException("Output directory not set");
            return new LevelHandler(_inputDir, _outputDir, _showTestDiff);
        }
    }

    private partial class FileHandler(string inputDir, string outputDir)
    {
        [System.Text.RegularExpressions.GeneratedRegex(@"^level\d+_\d+\.in$")]
        private static partial System.Text.RegularExpressions.Regex FILE_NAME_REGEX();

        /// <summary>
        /// Validates the file name against the regex pattern.
        /// </summary>
        /// <param name="name">The file name to validate.</param>
        /// <returns>True if the file name is valid; otherwise, false.</returns>
        private static bool IsValidFileName(string name) =>
            FILE_NAME_REGEX().IsMatch(name);

        /// <summary>
        /// Reads the input files for a level.
        /// </summary>
        /// <param name="level">The level number.</param>
        /// <returns>The <see cref="LevelInput"/> instance containing the input files.</returns>
        internal LevelInput ReadLvlInput(int level)
        {
            string dir = $"{inputDir}/level{level}";
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

        /// <summary>
        /// Writes the output files for a level.
        /// </summary>
        /// <typeparam name="T">The type of the solution output.</typeparam>
        /// <param name="level">The level number.</param>
        /// <param name="input">The input data for the level.</param>
        /// <param name="solution">The solution to apply.</param>
        /// <exception cref="Exception">Thrown when the output directory is not initialized.</exception>
        internal void WriteLvlOutput<T>(int level, LevelInput input, ISolution<T> solution)
        {
            if (outputDir == null) throw new Exception("Output directory not initialized");
            var dir = $"{outputDir}/level{level}";
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
    }
}

public record LevelInput
{
    private const int InputFileCount = 5;
    public List<LevelInputFile> InputFiles { get; }
    public string[] ExampleInput { get; }
    public string ExampleOutput { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LevelInput"/> class.
    /// </summary>
    /// <param name="inputFiles">The list of input files.</param>
    /// <param name="exampleInput">The example input.</param>
    /// <param name="exampleOutput">The example output.</param>
    /// <exception cref="ArgumentException">Thrown when the input files count is incorrect or example input/output is invalid.</exception>
    public LevelInput(List<LevelInputFile> inputFiles, string[]? exampleInput, string? exampleOutput)
    {
        if (inputFiles.Count != InputFileCount) throw new ArgumentException($"Expected {InputFileCount} inputs");

        if (exampleOutput == null) throw new ArgumentException("Example output not found");
        if (exampleOutput.Length == 0) throw new ArgumentException("Empty example output");
        if (exampleInput == null) throw new ArgumentException("Example input not found");
        if (exampleInput.Length == 0) throw new ArgumentException("Empty example input");

        InputFiles = inputFiles;
        ExampleInput = exampleInput;
        ExampleOutput = exampleOutput;
    }
};

public record LevelInputFile
{
    private const int MaxId = 5;
    private const int MinId = 1;
    public string[] Lines { get; }
    public int Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LevelInputFile"/> class.
    /// </summary>
    /// <param name="lines">The lines of the input file.</param>
    /// <param name="id">The identifier of the input file.</param>
    /// <exception cref="ArgumentException">Thrown when the input file is empty or the id is invalid.</exception>
    public LevelInputFile(string[] lines, int id)
    {
        if (lines.Length == 0) throw new ArgumentException("Empty input file");
        if (id is < MinId or > MaxId) throw new ArgumentException("Invalid id");

        Lines = lines;
        Id = id;
    }
}