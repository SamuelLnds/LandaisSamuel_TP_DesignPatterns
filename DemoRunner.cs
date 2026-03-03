using System.Text.RegularExpressions;

namespace LandaisSamuel_TP_DesignPatterns;

public static partial class DemoRunner
{
    public static void ShowMenu(params IDemo[] demos)
    {
        // Construire les options du menu
        var demoNames = demos.Select(GetDemoName).ToArray();
        var options = new string[demoNames.Length + 1];
        options[0] = "Exécuter toutes les démos";
        for (int i = 0; i < demoNames.Length; i++)
            options[i + 1] = demoNames[i];

        int choice = ConsoleHelper.ReadMenuChoice("Menu des Design Patterns", options);

        Console.Clear();

        if (choice == 0)
        {
            RunAll(demos);
        }
        else
        {
            RunSingle(demos[choice - 1]);
        }
    }

    public static void RunAll(params IDemo[] demos)
    {
        ConsoleHelper.WriteHeader("Exécution des différents exemples de Design Pattern");

        try
        {
            for (int i = 0; i < demos.Length; i++)
            {
                var demo = demos[i];
                var name = GetDemoName(demo);

                ConsoleHelper.WriteStart(name);
                demo.Run();
                ConsoleHelper.WriteEnd($"Fin de la démonstration du {name}");

                if (i < demos.Length - 1)
                    Thread.Sleep(1000);
            }
        }
        finally
        {
            ConsoleHelper.WriteFooter("Fin de l'exécution de tous les exemples de Design Pattern.");
        }
    }

    private static void RunSingle(IDemo demo)
    {
        var name = GetDemoName(demo);

        ConsoleHelper.WriteHeader($"Démonstration du {name}");

        ConsoleHelper.WriteStart(name);
        demo.Run();
        ConsoleHelper.WriteEnd($"Fin de la démonstration du {name}");

        ConsoleHelper.WriteFooter($"Fin de la démonstration du {name}.");
    }

    private static string GetDemoName(IDemo demo)
    {
        var typeName = demo.GetType().Name;

        // Retire le suffixe "Demo" du nom de la classe
        if (typeName.EndsWith("Demo"))
            typeName = typeName[..^4];

        // Insère un espace avant chaque majuscule (sauf la première) pour séparer les mots du PascalCase
        return PascalCaseSeparator().Replace(typeName, " ");
    }

    [GeneratedRegex(@"(?<=.)(?=[A-Z])")]
    private static partial Regex PascalCaseSeparator();
}
