namespace LandaisSamuel_TP_DesignPatterns;

public static class ConsoleHelper
{
    public static void WriteHeader(string message)
    {
        WithColor(
            ConsoleColor.Cyan,
            () =>
            {
                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine(message);
                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine(Environment.NewLine);
            }
        );
    }

    public static void WriteStart(string message)
    {
        WithColor(
            ConsoleColor.White,
            () =>
            {
                Console.WriteLine(message);
                Console.WriteLine(new string('-', (int)(message.Length * 1.5)));
                Console.WriteLine(Environment.NewLine);
            }
        );
    }

    public static void WriteStep(string message)
    {
        WithColor(ConsoleColor.Gray, () => Console.WriteLine(message));
    }

    public static void WriteEnd(string message)
    {
        WithColor(
            ConsoleColor.Green,
            () =>
            {
                Console.WriteLine(Environment.NewLine);
                int length =
                    message.Length > Console.WindowWidth ? Console.WindowWidth : message.Length;
                Console.WriteLine(new string('-', length));
                Console.WriteLine(message);
                Console.WriteLine(Environment.NewLine);
            }
        );
    }

    public static void WriteError(string message)
    {
        WithColor(
            ConsoleColor.Red,
            () =>
            {
                Console.WriteLine(message);
                int length =
                    message.Length > Console.WindowWidth ? Console.WindowWidth : message.Length;
                Console.WriteLine(new string('-', length));
                Console.WriteLine(Environment.NewLine);
            }
        );
    }

    private static void WithColor(ConsoleColor color, Action action)
    {
        var previous = Console.ForegroundColor;
        try
        {
            if (previous != color)
                Console.ForegroundColor = color;

            action();
        }
        finally
        {
            Console.ForegroundColor = previous; // reset
        }
    }
}
