namespace ConsoleApp1.ConsoleInterface;
public class ConsoleReader : IReader
{
    public string? Read()
    {
        return Console.ReadLine();
    }
}
