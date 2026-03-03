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
        new StrategyDemo(),
        new CompositeDemo(),
        new FacadeDemo(),
        new BuilderDemo(),
        new StateDemo(),
        new AbstractFactoryDemo(),
        new FactoryMethodDemo()
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
    ConsoleHelper.WriteStep("Appuyez sur une touche pour quitter...");
    Console.ReadKey();
}
