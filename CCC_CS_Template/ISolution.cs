namespace CCC_CS_Template;

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

/// <summary>
/// Default ISolution interface for string output
/// </summary>
public interface ISolution
{
    public string Solve(string[] input);
    public string Format(string output);
}