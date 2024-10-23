namespace CCC_CS_Template;

public static class SolutionExamples
{
    // Example use of ISolution
    public class Solution1 : ISolution<string>
    {
        public string Solve(string[] input) => "Test";
        public string Format(string output) => output;
    }

    public class Solution2 : ISolution<int>
    {
        public int Solve(string[] input) => 15;
        public string Format(int output) => output.ToString();
    }

    public class Solution3 : ISolution<List<int>>
    {
        public List<int> Solve(string[] input) => [Random.Shared.Next(), Random.Shared.Next(), Random.Shared.Next()];
        public string Format(List<int> output) => string.Join(" ", output);
    }

    public class Solution4 : ISolution<Dictionary<int, List<int>>>
    {
        public Dictionary<int, List<int>> Solve(string[] input) => new()
            { { Random.Shared.Next(), [Random.Shared.Next(), Random.Shared.Next(), Random.Shared.Next()] } };

        public string Format(Dictionary<int, List<int>> output) =>
            string.Join(" ", output.Select(kvp => $"{kvp.Key}:\n{string.Join(" ", kvp.Value)}"));
    }
}