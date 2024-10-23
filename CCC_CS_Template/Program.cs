namespace CCC_CS_Template;

internal static class Program
{
    private static void Main()
    {
        LevelHandler.Initialize("/home/julian/RiderProjects/CCC_CS_Template/CCC_CS_Template/Input",
            "/home/julian/RiderProjects/CCC_CS_Template/CCC_CS_Template/Output");
        LevelHandler.TestLevel(3, new SolutionExamples.Solution4());
        LevelHandler.SolveLevel(3, new SolutionExamples.Solution4());
    }
}