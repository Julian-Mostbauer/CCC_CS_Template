namespace CCC_CS_Template;

internal static class Program
{
    class SolutionExample1 : ISolution<List<int>>
    {
        public List<int> Solve(string[] input) => CCCUtils.Parsing.ParseIntLinesAfter(1, input)
            .Select(nums => nums[CCCUtils.Sequences.SequenceBreaker(nums, x => x + 1) - 1] + 1)
            .ToList();

        public string Format(List<int> output) => string.Join("\n", output);
    }

    private static void Main()
    {
        var levelHandler = new LevelHandler.LevelHandlerBuilder()
            .SetInputDir("/home/julian/RiderProjects/CCC_CS_Template/CCC_CS_Template/Input")
            .SetOutputDir("/home/julian/RiderProjects/CCC_CS_Template/CCC_CS_Template/Output")
            .Build();

        levelHandler.TestLevel(1, new SolutionExample1());
    }
}