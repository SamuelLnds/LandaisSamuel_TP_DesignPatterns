namespace LandaisSamuel_TP_DesignPatterns.AbstractFactory;

// Interface produit A : document
public interface IDocument
{
    string Print();
}

// Interface produit B : étiquette
public interface ILabel
{
    string Print();
}

// Famille "Réception"
public class ReceptionDocument(string deliveryId) : IDocument
{
    public string Print() => $"BON DE RÉCEPTION - Livraison {deliveryId}";
}

public class ReceptionLabel(string deliveryId) : ILabel
{
    public string Print() => $"ÉTIQUETTE RÉCEPTION - {deliveryId} - Quai déchargement";
}

// Famille "Expédition"
public class ShippingDocument(string orderId) : IDocument
{
    public string Print() => $"BON D'EXPÉDITION - Commande {orderId}";
}

public class ShippingLabel(string orderId) : ILabel
{
    public string Print() => $"ÉTIQUETTE EXPÉDITION - {orderId} - Transporteur";
}

// Abstract Factory
public interface IWarehouseDocumentFactory
{
    IDocument CreateDocument(string reference);
    ILabel CreateLabel(string reference);
}

// Factory concrète : famille Réception
public class ReceptionDocumentFactory : IWarehouseDocumentFactory
{
    public IDocument CreateDocument(string reference) => new ReceptionDocument(reference);

    public ILabel CreateLabel(string reference) => new ReceptionLabel(reference);
}

// Factory concrète : famille Expédition
public class ShippingDocumentFactory : IWarehouseDocumentFactory
{
    public IDocument CreateDocument(string reference) => new ShippingDocument(reference);

    public ILabel CreateLabel(string reference) => new ShippingLabel(reference);
}

// Client
public class WarehouseFlowProcessor(IWarehouseDocumentFactory factory)
{
    public void Process(string reference)
    {
        var document = factory.CreateDocument(reference);
        var label = factory.CreateLabel(reference);

        ConsoleHelper.WriteStep($"[Processeur] {document.Print()}");
        ConsoleHelper.WriteStep($"[Processeur] {label.Print()}");
    }
}

public class AbstractFactoryDemo : IDemo
{
    public void Run()
    {
        ConsoleHelper.WriteStep("[Scénario] Flux de réception");

        // Ici, contrairement à la factory method, on a une implémentation par factory
        // La factory elle-même détermine la logique de création
        var receptionProcessor = new WarehouseFlowProcessor(new ReceptionDocumentFactory());
        receptionProcessor.Process("LIV-2025-042");

        ConsoleHelper.WriteStep("\n[Scénario] Flux d'expédition");

        // On change la factory
        var shippingProcessor = new WarehouseFlowProcessor(new ShippingDocumentFactory());
        shippingProcessor.Process("CMD-78901");
    }
}
