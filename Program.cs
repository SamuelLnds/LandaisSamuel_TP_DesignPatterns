using LandaisSamuel_TP_DesignPatterns;

try
{
    DemoRunner.RunAll(
        new MediatorDemo(),
        new SingletonDemo(),
        new ObserverDemo(),
        new DecoratorDemo(),
        new CommandDemo(),
        new AdapterDemo(),
        new StrategyDemo()
    );
}
catch (Exception ex)
{
    ConsoleHelper.WriteError(
        $"Une erreur est survenue lors de l'exécution des démonstrations : {ex.Message}"
    );
}
finally
{
    Console.WriteLine("Appuyez sur une touche pour quitter...");
    Console.ReadKey();
}
