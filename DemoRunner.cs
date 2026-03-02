namespace LandaisSamuel_TP_DesignPatterns;

public static class DemoRunner
{
    public static void RunAll(params IDemo[] demos)
    {
        ConsoleHelper.WriteHeader("Exécution des différents exemples de Design Pattern");

        try
        {
            for (int i = 0; i < demos.Length; i++)
            {
                var demo = demos[i];

                ConsoleHelper.WriteStart(demo.Name);
                demo.Run();
                ConsoleHelper.WriteEnd($"Fin de la démonstration du {demo.Name}");

                if (i < demos.Length - 1)
                    Thread.Sleep(1000);
            }
        }
        finally
        {
            ConsoleHelper.WriteFooter("Fin de l'exécution de tous les exemples de Design Pattern.");
        }
    }
}
