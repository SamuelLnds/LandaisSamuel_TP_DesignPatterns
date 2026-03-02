namespace LandaisSamuel_TP_DesignPatterns.Observer;

// Contrat de l'observer
public interface IStockObserver
{
    void OnStockChanged(string location, int newQuantity);
}

// Le sujet : l'emplacement de stockage
public class StorageLocation(string locationCode, int initialQuantity)
{
    // Liste d'observers abonnés à cet emplacement
    // private et readonly pour l'encapsulation, éviter les effets de bord
    private readonly List<IStockObserver> _observers = [];
    private int _quantity = initialQuantity;
    public string LocationCode { get; } = locationCode;

    public void Subscribe(IStockObserver observer) => _observers.Add(observer);

    public void Unsubscribe(IStockObserver observer) => _observers.Remove(observer);

    // Méthode qui simule une mise à jour de stock
    public void UpdateQuantity(int newQuantity)
    {
        _quantity = newQuantity;
        ConsoleHelper.WriteStep(
            $"\n[StorageLocation] {LocationCode} : stock mis à jour ({_quantity} unités)"
        );

        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers)
            observer.OnStockChanged(LocationCode, _quantity);
    }
}

// Observer 1 : système de réapprovisionnement
public class ReplenishmentSystem : IStockObserver
{
    public void OnStockChanged(string location, int newQuantity)
    {
        if (newQuantity <= 5)
            ConsoleHelper.WriteStep(
                $"[ReplenishmentSystem] Alerte : {location} en rupture imminente, commande de réapprovisionnement déclenchée."
            );
    }
}

// Observer 2 : tableau de bord
public class WarehouseDashboard : IStockObserver
{
    public void OnStockChanged(string location, int newQuantity)
    {
        ConsoleHelper.WriteStep($"[Dashboard] {location} : {newQuantity} unités restantes.");
    }
}

public class ObserverDemo : IDemo
{
    public string Name => "Observer";

    public void Run()
    {
        var locationA3 = new StorageLocation("A3", 50);
        var locationB5 = new StorageLocation("B5", 30);

        var replenishment = new ReplenishmentSystem();
        var dashboard = new WarehouseDashboard();

        locationA3.Subscribe(replenishment);
        locationA3.Subscribe(dashboard);
        locationB5.Subscribe(dashboard);

        locationA3.UpdateQuantity(20);
        locationB5.UpdateQuantity(15); // Ici, seul le dashboard réagit

        locationB5.Subscribe(replenishment);
        locationB5.UpdateQuantity(3); // Là, les deux réagissent car ils sont abonnés

        // Désabonnement du dashboard, puis nouvelle mise à jour
        locationA3.Unsubscribe(dashboard);
        locationA3.UpdateQuantity(1);
        // Ici, le dashboard ne fait aucun retour
    }
}
