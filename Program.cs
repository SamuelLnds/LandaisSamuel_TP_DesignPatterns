using LandaisSamuel_TP_DesignPatterns;
using LandaisSamuel_TP_DesignPatterns.Adapter;
using LandaisSamuel_TP_DesignPatterns.Command;
using LandaisSamuel_TP_DesignPatterns.Decorator;
using LandaisSamuel_TP_DesignPatterns.Mediator;
using LandaisSamuel_TP_DesignPatterns.Observer;
using LandaisSamuel_TP_DesignPatterns.Singleton;

// Ce fichier sera destiné à exécuter l'ensemble des scripts des différents dossiers de design pattern
// Chaque design pattern sera annoncé clairement dans la console, et sa finalisation également
// L'exécution du Program.cs permettra alors de vérifier l'ensemble du bon fonctionnement du projet

ConsoleHelper.WriteHeader("Exécution des différents exemples de Design Pattern");

try
{
    // Mediator
    ConsoleHelper.WriteStart("Mediator");
    MediatorDemo.ExampleExecution();
    ConsoleHelper.WriteEnd("Fin de la démonstration du Mediator");

    Thread.Sleep(1000);

    // Singleton
    ConsoleHelper.WriteStart("Singleton");
    SingletonDemo.ExampleExecution();
    ConsoleHelper.WriteEnd("Fin de la démonstration du Singleton");

    Thread.Sleep(1000);

    // Observer
    ConsoleHelper.WriteStart("Observer");
    ObserverDemo.ExampleExecution();
    ConsoleHelper.WriteEnd("Fin de la démonstration de l'Observer");

    Thread.Sleep(1000);

    // Decorator
    ConsoleHelper.WriteStart("Decorator");
    DecoratorDemo.ExampleExecution();
    ConsoleHelper.WriteEnd("Fin de la démonstration du Decorator");

    Thread.Sleep(1000);

    // Command
    ConsoleHelper.WriteStart("Command");
    CommandDemo.ExampleExecution();
    ConsoleHelper.WriteEnd("Fin de la démonstration du Command");

    // Adapter
    ConsoleHelper.WriteStart("Adapter");
    AdapterDemo.ExampleExecution();
    ConsoleHelper.WriteEnd("Fin de la démonstration de l'Adapter");
}
catch (Exception ex)
{
    ConsoleHelper.WriteError(ex.Message);
}
finally
{
    ConsoleHelper.WriteFooter("Fin de l'exécution de tous les exemples de Design Pattern.");
    Console.WriteLine("Appuyez sur une touche pour quitter...");
    Console.ReadKey();
}
