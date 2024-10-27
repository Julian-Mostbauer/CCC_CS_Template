using System.Numerics;

namespace CCC_CS_Template;

public static class CCCUtils
{
    public static class IO
    {
        public static void DebugPrint(string[] names, params object[] objs)
        {
            if (objs.Length != names.Length) throw new ArgumentException("Length of names and objs must be equal");
            var header = $"{new string('-', 20)} <{DateTime.Now.TimeOfDay}> {new string('-', 20)}";
            Console.WriteLine(header);
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{names[i]}: {objs[i]}");
            }

            Console.WriteLine(header.Replace("<", "</") + "\n");
        }
    }

    public static class Parsing
    {
        public static int[][] ParseIntLinesAfter(int n, string[] l, string sep = " ")
        {
            return l.Skip(n).Select(x => x.Split(sep).Select(int.Parse).ToArray()).ToArray();
        }
    }

    public static class Sequences
    {
        public static int SequenceBreaker(int[] nums, Func<int,int> pattern)
        {
            if (nums.Length < 2) throw new ArgumentException("Array must have at least 2 elements");
            for (int i = 1; i < nums.Length; i++)
            {
                if (pattern(nums[i-1]) != nums[i])
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int SequenceBreaker(double[] nums, Func<double, double> pattern, double tolerance = 0)
        {
            if (nums.Length < 2) throw new ArgumentException("Array must have at least 2 elements");
            for (int i = 1; i < nums.Length; i++)
            {
                if (Math.Abs(pattern(nums[i - 1]) - nums[i]) > tolerance)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}