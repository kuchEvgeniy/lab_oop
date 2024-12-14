namespace ConsoleApp1.ConsoleInterface;
public class ConsoleLogger : ILogger
{
    public void Log(string? message)
    {
        Console.WriteLine(message);
    }
}
