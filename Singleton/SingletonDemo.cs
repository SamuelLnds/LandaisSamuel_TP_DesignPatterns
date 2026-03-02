namespace LandaisSamuel_TP_DesignPatterns.Singleton;

public sealed class WarehouseLogger
{
    // Lazy<T> gère la thread safety et la lazy initialization nativement
    // C'est la manière simple de faire un singleton en C#
    private static readonly Lazy<WarehouseLogger> _instance = new(() => new WarehouseLogger());

    // Compteur partagé : nombre de colis scannés en réception
    private int _receivedPackagesCount;

    // Constructeur privé pour éviter toute instanciation externe
    private WarehouseLogger()
    {
        // Ce message s'affiche au premier appel de l'instance
        ConsoleHelper.WriteStep("[WMS] Réception commencée");
    }

    public static WarehouseLogger Instance => _instance.Value;

    public void ScanPackage()
    {
        // Incrément thread-safe : plusieurs opérateurs peuvent scanner en même temps
        Interlocked.Increment(ref _receivedPackagesCount);
    }

    public int ReceivedPackagesCount => _receivedPackagesCount;
}

public class SingletonDemo : IDemo
{
    public string Name => "Singleton";

    public void Run()
    {
        // Plusieurs opérateurs scannent des colis en même temps sur un quai
        const int operators = 5;
        const int scansPerOperator = 1000;

        int totalExpected = operators * scansPerOperator;
        ConsoleHelper.WriteStep($"Colis attendus : {totalExpected}");

        // Simulation de l'activité de scan en parallèle
        Parallel.For(
            0,
            operators,
            _ =>
            {
                for (int i = 0; i < scansPerOperator; i++)
                    WarehouseLogger.Instance.ScanPackage();

                ConsoleHelper.WriteStep(
                    $"Opérateur terminé : [ID : {Environment.CurrentManagedThreadId}]"
                );
            }
        );

        // Vérification : les valeurs doivent être correctes
        ConsoleHelper.WriteStep(
            $"Colis scannés : {WarehouseLogger.Instance.ReceivedPackagesCount}"
        );

        if (WarehouseLogger.Instance.ReceivedPackagesCount != totalExpected)
            ConsoleHelper.WriteError("Erreur : le nombre de colis scannés est incorrect !");

        // Vérification : même instance partout
        var wms1 = WarehouseLogger.Instance;
        var wms2 = WarehouseLogger.Instance;
        ConsoleHelper.WriteStep($"\nMême instance ? {ReferenceEquals(wms1, wms2)}");
    }
}
