namespace LandaisSamuel_TP_DesignPatterns;

public static class ConsoleHelper
{
    #region API publique
    public static void WriteHeader(string message) => WriteBanner(message, ConsoleColor.Cyan, '=');

    public static void WriteFooter(string message) => WriteBanner(message, ConsoleColor.Cyan, '=');

    public static void WriteStart(string message)
    {
        WithColor(
            ConsoleColor.White,
            () =>
            {
                Console.WriteLine(message);
                Console.WriteLine(Separator('-', Console.WindowWidth / 3));
                Console.WriteLine();
            }
        );
    }

    public static void WriteStep(string message) =>
        WithColor(ConsoleColor.Gray, () => Console.WriteLine(message));

    public static void WriteEnd(string message) =>
        WriteTitledBlock(
            message,
            ConsoleColor.Green,
            topSpacing: true,
            bottomSpacing: true,
            underlineChar: '-',
            showUnderlineAfterMessage: false
        );

    public static void WriteError(string message) =>
        WriteTitledBlock(
            message,
            ConsoleColor.Red,
            topSpacing: false,
            bottomSpacing: true,
            underlineChar: '-',
            showUnderlineAfterMessage: true
        );

    #endregion

    #region Helpers DRY

    private static void WriteBanner(string message, ConsoleColor color, char borderChar)
    {
        WithColor(
            color,
            () =>
            {
                Console.WriteLine(Separator(borderChar, Console.WindowWidth));
                Console.WriteLine(message);
                Console.WriteLine(Separator(borderChar, Console.WindowWidth));
                Console.WriteLine();
            }
        );
    }

    private static void WriteTitledBlock(
        string message,
        ConsoleColor color,
        bool topSpacing,
        bool bottomSpacing,
        char underlineChar,
        bool showUnderlineAfterMessage
    )
    {
        WithColor(
            color,
            () =>
            {
                if (topSpacing)
                    Console.WriteLine();

                int lineLength = Clamp(
                    message.Length,
                    min: Console.WindowWidth / 3,
                    max: Console.WindowWidth
                );

                if (!showUnderlineAfterMessage)
                    Console.WriteLine(Separator(underlineChar, lineLength));

                Console.WriteLine(message);

                if (showUnderlineAfterMessage)
                    Console.WriteLine(Separator(underlineChar, lineLength));

                if (bottomSpacing)
                    Console.WriteLine();
            }
        );
    }

    private static string Separator(char c, int length) => new(c, Math.Max(0, length));

    private static int Clamp(int value, int min, int max) =>
        value < min ? min : (value > max ? max : value);

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
            Console.ForegroundColor = previous;
        }
    }

    #endregion
}
