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

    /// <summary>
    /// Affiche un menu interactif navigable avec les flèches directionnelles
    /// et sélectionnable par numéro. Retourne l'index choisi (0-based).
    /// </summary>
    public static int ReadMenuChoice(string title, string[] options)
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Pour les flèches

        WriteHeader(title);

        WriteStep("Utilisez ↑/↓ pour naviguer, Entrée pour valider, ou tapez un numéro.");
        Console.WriteLine();

        int startRow = Console.CursorTop;
        int selected = 0;
        string numberBuffer = "";

        RenderMenu(options, selected, startRow, numberBuffer);

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected - 1 + options.Length) % options.Length;
                    numberBuffer = "";
                    break;

                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % options.Length;
                    numberBuffer = "";
                    break;

                case ConsoleKey.Enter:
                    Console.CursorVisible = true;
                    Console.SetCursorPosition(0, startRow + options.Length);
                    Console.WriteLine();
                    return selected;

                case ConsoleKey.Backspace:
                    if (numberBuffer.Length > 0)
                    {
                        numberBuffer = numberBuffer[..^1];
                        if (
                            numberBuffer.Length > 0
                            && int.TryParse(numberBuffer, out int backIdx)
                            && backIdx >= 0
                            && backIdx < options.Length
                        )
                            selected = backIdx;
                    }
                    break;

                default:
                    if (char.IsDigit(key.KeyChar))
                    {
                        string candidate = numberBuffer + key.KeyChar;

                        // Si le candidat dépasse le nombre max, on recommence
                        if (int.TryParse(candidate, out int num) && num < options.Length)
                        {
                            numberBuffer = candidate;
                            selected = num;
                        }
                        else if (
                            int.TryParse(key.KeyChar.ToString(), out int singleNum)
                            && singleNum < options.Length
                        )
                        {
                            // Recommencer avec ce seul chiffre
                            numberBuffer = key.KeyChar.ToString();
                            selected = singleNum;
                        }
                    }
                    break;
            }

            RenderMenu(options, selected, startRow, numberBuffer);
        }
    }

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

    private static void RenderMenu(
        string[] options,
        int selected,
        int startRow,
        string numberBuffer
    )
    {
        for (int i = 0; i < options.Length; i++)
        {
            Console.SetCursorPosition(0, startRow + i);

            bool isSelected = i == selected;
            string prefix = isSelected ? " • " : " ◦ ";
            string line = $"{prefix}{i} - {options[i]}";

            // Effacer la ligne puis écrire
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, startRow + i);

            if (isSelected)
            {
                WithColor(ConsoleColor.Yellow, () => Console.Write(line));
            }
            else
            {
                WithColor(ConsoleColor.Gray, () => Console.Write(line));
            }
        }

        // Afficher le buffer de saisie numérique en bas du menu
        int bufferRow = startRow + options.Length;
        Console.SetCursorPosition(0, bufferRow);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        Console.SetCursorPosition(0, bufferRow);

        if (numberBuffer.Length > 0)
        {
            WithColor(ConsoleColor.DarkGray, () => Console.Write($"  Saisie : {numberBuffer}"));
        }
    }

    #endregion
}
