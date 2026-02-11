using LandaisSamuel_TP_DesignPatterns;
using LandaisSamuel_TP_DesignPatterns.Mediator;

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
