namespace CCC_CS_Template;

internal static class Program
{
    class SolutionExample1 : ISolution<List<int>>
    {
        public List<int> Solve(string[] input)
        {
            var res = new List<int>();
            foreach (var line in input)
            {
                var nums = line.Split(" ").Select(int.Parse).ToArray();
                int last = nums[0];
                for (int i = 1; i < nums.Length; i++)
                {
                    if (last + 1 < nums[i])
                    {
                        res.Add(last + 2);
                        break;
                    }

                    last = nums[i];
                }
            }

            return res;
        }

        public string Format(List<int> output) => string.Join("\n", output);
    }

    private static void Main()
    {
        var levelHandler = new LevelHandler.LevelHandlerBuilder()
            .SetInputDir("/home/julian/RiderProjects/CCC_CS_Template/CCC_CS_Template/Input")
            .SetOutputDir("/home/julian/RiderProjects/CCC_CS_Template/CCC_CS_Template/Output")
            .ShowTestDiff()
            .Build();

        levelHandler.TestLevel(1, new SolutionExample1());
    }
}