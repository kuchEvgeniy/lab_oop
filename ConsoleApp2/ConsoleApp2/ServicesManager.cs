namespace ConsoleApp1
{
    public static class ServicesManager
    {
        public static ILogger Writer { get; set; }
        public static IReader Reader { get; set; }

    }
}

public interface ILogger
{
    void Log(string? message);
}

public interface IReader
{
    string? Read();
}