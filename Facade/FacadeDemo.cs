namespace LandaisSamuel_TP_DesignPatterns.Facade;

// Sous-système 1 : Validation de livraison
public class DeliveryValidator
{
    public bool ValidateDelivery(
        string supplierId,
        List<string> expectedItems,
        List<string> receivedItems
    )
    {
        ConsoleHelper.WriteStep("[DeliveryValidator] Validation de la livraison...");
        ConsoleHelper.WriteStep($"  Fournisseur : {supplierId}");
        ConsoleHelper.WriteStep($"  Articles attendus : {expectedItems.Count}");
        ConsoleHelper.WriteStep($"  Articles reçus : {receivedItems.Count}");

        bool isValid = expectedItems.Count == receivedItems.Count;
        ConsoleHelper.WriteStep($"  Résultat : {(isValid ? "CONFORME" : "NON CONFORME")}");
        return isValid;
    }
}

// Sous-système 2 : Gestion du stock
public class StockManager
{
    private readonly Dictionary<string, int> _stock = [];

    public void UpdateStock(List<string> items, int quantityPerItem)
    {
        ConsoleHelper.WriteStep("\n[StockManager] Mise à jour du stock...");
        foreach (var item in items)
        {
            if (!_stock.ContainsKey(item))
                _stock[item] = 0;

            _stock[item] += quantityPerItem;
            ConsoleHelper.WriteStep($"  {item} : +{quantityPerItem} (total : {_stock[item]})");
        }
    }
}

// Sous-système 3 : Génération de documents
public class DocumentGenerator
{
#pragma warning disable IDE0060 // Supprimer le paramètre inutilisé - ici on simule une génération
    public string GenerateReceiptDocument(string deliveryId, string supplierId, int itemCount)
#pragma warning restore IDE0060 // Supprimer le paramètre inutilisé
    {
        ConsoleHelper.WriteStep("\n[DocumentGenerator] Génération du bon de réception...");
        var document = $"BON_RECEPTION_{deliveryId}_{DateTime.Now:yyyyMMdd}.pdf";
        ConsoleHelper.WriteStep($"  Document généré : {document}");
        return document;
    }

    public void PrintLabel(string labelContent)
    {
        ConsoleHelper.WriteStep($"  Étiquette imprimée : {labelContent}");
    }
}

// Sous-système 4 : Notifications
public class NotificationService
{
    public void NotifyWarehouseManager(string message)
    {
        ConsoleHelper.WriteStep("\n[NotificationService] Notification envoyée...");
        ConsoleHelper.WriteStep($"  Destinataire : Responsable d'entrepôt");
        ConsoleHelper.WriteStep($"  Message : {message}");
    }

    public void NotifyPurchasingDepartment(string supplierId, bool isConform)
    {
        ConsoleHelper.WriteStep(
            $"  Département achats notifié : Livraison fournisseur {supplierId} - {(isConform ? "Conforme" : "Anomalie détectée")}"
        );
    }
}

// Facade
public class ReceptionFacade
{
    private readonly DeliveryValidator _validator;
    private readonly StockManager _stockManager;
    private readonly DocumentGenerator _documentGenerator;
    private readonly NotificationService _notificationService;

    public ReceptionFacade()
    {
        _validator = new DeliveryValidator();
        _stockManager = new StockManager();
        _documentGenerator = new DocumentGenerator();
        _notificationService = new NotificationService();
    }

    // Méthode de façade : orchestre tout le workflow de réception
    public void ProcessReception(
        string deliveryId,
        string supplierId,
        List<string> expectedItems,
        List<string> receivedItems
    )
    {
        ConsoleHelper.WriteStep($" TRAITEMENT RÉCEPTION - {deliveryId}");

        // Étape 1 : Validation
        bool isValid = _validator.ValidateDelivery(supplierId, expectedItems, receivedItems);

        if (!isValid)
        {
            // Workflow anomalie
            _notificationService.NotifyWarehouseManager(
                $"ANOMALIE : Livraison {deliveryId} non conforme"
            );
            _notificationService.NotifyPurchasingDepartment(supplierId, false);
            ConsoleHelper.WriteStep("\n[FACADE] Processus interrompu - Anomalie détectée");
            return;
        }

        // Étape 2 : Mise à jour stock
        _stockManager.UpdateStock(receivedItems, 100); // 100 unités par article

        // Étape 3 : Génération documents
#pragma warning disable IDE0059 // Assignation inutile d'une valeur - ici on simule une génération
        string receiptDoc = _documentGenerator.GenerateReceiptDocument(
            deliveryId,
            supplierId,
            receivedItems.Count
        );
#pragma warning restore IDE0059 // Assignation inutile d'une valeur
        _documentGenerator.PrintLabel($"LIV-{deliveryId}");

        // Étape 4 : Notifications
        _notificationService.NotifyWarehouseManager(
            $"Réception {deliveryId} finalisée - {receivedItems.Count} références"
        );
        _notificationService.NotifyPurchasingDepartment(supplierId, true);

        ConsoleHelper.WriteStep("\n[FACADE] Processus de réception terminé avec succès");
    }
}

public class FacadeDemo : IDemo
{
    public void Run()
    {
        var facade = new ReceptionFacade();

        ConsoleHelper.WriteStep(">> SCENARIO 1 : Réception conforme\n");

        var expectedItems = new List<string> { "VIS-M6", "ECROU-M6", "RONDELLE-M6" };
        var receivedItems = new List<string> { "VIS-M6", "ECROU-M6", "RONDELLE-M6" };

        facade.ProcessReception("LIV-2025-001", "FOURNISSEUR-A", expectedItems, receivedItems);

        ConsoleHelper.WriteStep("\n>> SCENARIO 2 : Réception non conforme\n");

        var expectedItems2 = new List<string> { "BOULON-M12", "ECROU-M12" };
        var receivedItems2 = new List<string> { "BOULON-M12" }; // Article manquant

        facade.ProcessReception("LIV-2025-002", "FOURNISSEUR-B", expectedItems2, receivedItems2);
    }
}
