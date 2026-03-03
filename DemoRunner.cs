using System.Text.RegularExpressions;

namespace LandaisSamuel_TP_DesignPatterns;

public static partial class DemoRunner
{
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
