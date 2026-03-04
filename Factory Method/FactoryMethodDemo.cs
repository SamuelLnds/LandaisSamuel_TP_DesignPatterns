namespace LandaisSamuel_TP_DesignPatterns.FactoryMethod;

// Product : interface commune des documents
public interface IWarehouseDocument
{
    string GetTitle();
    string GetContent();
}

// Produit concret : bon de réception
public class ReceptionDocument(string deliveryId) : IWarehouseDocument
{
    public string GetTitle() => "BON DE RÉCEPTION";

    public string GetContent() => $"Réception de la livraison {deliveryId} - Contrôle requis";
}

// Produit concret : bon de préparation
public class PickingDocument(string orderId) : IWarehouseDocument
{
    public string GetTitle() => "BON DE PRÉPARATION";

    public string GetContent() => $"Préparation de la commande {orderId} - Zone A puis Zone B";
}

// Creator : contient la logique métier commune
public abstract class DocumentProcessor(string reference)
{
    // Factory Method : la méthode qui est overridée par les sous-classes
    protected abstract IWarehouseDocument CreateDocument();

    // Logique métier commune qui utilise le produit sans connaître son type
    public void Process()
    {
        var document = CreateDocument();

        ConsoleHelper.WriteStep($"[Processeur] Traitement du document pour : {reference}");
        ConsoleHelper.WriteStep($"[Document] Type    : {document.GetTitle()}");
        ConsoleHelper.WriteStep($"[Document] Contenu : {document.GetContent()}");
        ConsoleHelper.WriteStep($"[Document] Horodatage : {DateTime.Now:dd/MM/yyyy HH:mm}");
        ConsoleHelper.WriteStep($"[Processeur] Document envoyé à l'impression");
    }
}

// Creator concret : traitement des réceptions
public class ReceptionProcessor : DocumentProcessor
{
    private readonly string deliveryId;

#pragma warning disable IDE0290 // Utiliser le constructeur principal -- ici, le constructeur principal empêche d'utiliser deliveryId
    public ReceptionProcessor(string deliveryId)
#pragma warning restore IDE0290 // Utiliser le constructeur principal
        : base(deliveryId)
    {
        this.deliveryId = deliveryId;
    }

    protected override IWarehouseDocument CreateDocument() => new ReceptionDocument(deliveryId);
}

// Creator concret : traitement des préparations
public class PickingProcessor : DocumentProcessor
{
    private readonly string orderId;

#pragma warning disable IDE0290 // Utiliser le constructeur principal -- ici, le constructeur principal empêche d'utiliser orderId
    public PickingProcessor(string orderId)
#pragma warning restore IDE0290 // Utiliser le constructeur principal
        : base(orderId)
    {
        this.orderId = orderId;
    }

    protected override IWarehouseDocument CreateDocument() => new PickingDocument(orderId);
}

public class FactoryMethodDemo : IDemo
{
    public void Run()
    {
        ConsoleHelper.WriteStep("[Scénario] Traitement d'une réception");

        // Ici, processor a comme type une classe abstraite
        // SUivant la logique du polymorphisme, on peut utiliser les classes concrètes qui héritent de celle-ci
        DocumentProcessor processor = new ReceptionProcessor("LIV-2025-042");
        processor.Process();

        ConsoleHelper.WriteStep("\n[Scénario] Traitement d'une préparation");

        // On change la classe concrète
        processor = new PickingProcessor("CMD-78901");
        processor.Process();
    }
}
