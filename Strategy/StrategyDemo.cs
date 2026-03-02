namespace LandaisSamuel_TP_DesignPatterns.Strategy;

// Interface commune : la stratégie de prélèvement
public interface IPickingStrategy
{
    string SelectLocation(List<StockLocation> availableLocations);
}

// Données : un emplacement de stock
public class StockLocation
{
    public required string Code { get; set; }
    public DateTime ReceivedDate { get; set; }
    public int DistanceFromDock { get; set; } // Ici on se contente d'un int pour une distance en mètres
}

// FIFO (First In First Out)
public class FifoPickingStrategy : IPickingStrategy
{
    public string SelectLocation(List<StockLocation> availableLocations)
    {
        var oldest = availableLocations.OrderBy(l => l.ReceivedDate).First();
        return oldest.Code;
    }
}

// Plus proche du quai
public class NearestPickingStrategy : IPickingStrategy
{
    public string SelectLocation(List<StockLocation> availableLocations)
    {
        var nearest = availableLocations.OrderBy(l => l.DistanceFromDock).First();
        return nearest.Code;
    }
}

// LIFO (Last In First Out)
public class LifoPickingStrategy : IPickingStrategy
{
    public string SelectLocation(List<StockLocation> availableLocations)
    {
        var newest = availableLocations.OrderByDescending(l => l.ReceivedDate).First();
        return newest.Code;
    }
}

// Contexte : le système de prélèvement
public class PickingSystem(IPickingStrategy strategy)
{
    private IPickingStrategy _strategy = strategy;

    // On peut changer la stratégie à l'exécution
    public void SetStrategy(IPickingStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecutePicking(string itemCode, List<StockLocation> locations)
    {
        Console.WriteLine($"\n[PickingSystem] Prélèvement de {itemCode}");
        var selectedLocation = _strategy.SelectLocation(locations);
        Console.WriteLine($"  => Emplacement sélectionné : {selectedLocation}");
    }
}

public class StrategyDemo : IDemo
{
    public string Name => "Strategy";

    public void Run()
    {
        // Liste des emplacements
        var locations = new List<StockLocation>
        {
            new()
            {
                Code = "A1",
                ReceivedDate = new DateTime(2025, 1, 10),
                DistanceFromDock = 50,
            },
            new()
            {
                Code = "B3",
                ReceivedDate = new DateTime(2025, 2, 15),
                DistanceFromDock = 20,
            },
            new()
            {
                Code = "C2",
                ReceivedDate = new DateTime(2025, 3, 2),
                DistanceFromDock = 80,
            },
        };

        var pickingSystem = new PickingSystem(new FifoPickingStrategy());

        // Commande de denrées pouvant périmer -> FIFO
        pickingSystem.ExecutePicking("Yaourts", locations);

        // Commande urgente -> Plus proche
        pickingSystem.SetStrategy(new NearestPickingStrategy());
        pickingSystem.ExecutePicking("Pièces auto urgentes", locations);

        // Commande LIFO
        pickingSystem.SetStrategy(new LifoPickingStrategy());
        pickingSystem.ExecutePicking("Matériel de construction", locations);
    }
}
