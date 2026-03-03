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
        ConsoleHelper.WriteStep($"[Validation] Fournisseur : {supplierId}");
        ConsoleHelper.WriteStep($"[Validation] Articles attendus : {expectedItems.Count}");
        ConsoleHelper.WriteStep($"[Validation] Articles reçus : {receivedItems.Count}");

        bool isValid = expectedItems.Count == receivedItems.Count;
        ConsoleHelper.WriteStep(
            $"[Validation] Résultat : {(isValid ? "CONFORME" : "NON CONFORME")}"
        );
        return isValid;
    }
}

// Sous-système 2 : Gestion du stock
public class StockManager
{
    private readonly Dictionary<string, int> _stock = [];

    public void UpdateStock(List<string> items, int quantityPerItem)
    {
        foreach (var item in items)
        {
            if (!_stock.ContainsKey(item))
                _stock[item] = 0;

            _stock[item] += quantityPerItem;
            ConsoleHelper.WriteStep(
                $"[Stock] {item} : +{quantityPerItem} (total : {_stock[item]})"
            );
        }
    }
}

// Sous-système 3 : Génération de documents
public class DocumentGenerator
{
    public void GenerateReceiptDocument(string deliveryId)
    {
        var document = $"BON_RECEPTION_{deliveryId}_{DateTime.Now:yyyyMMdd}.pdf";
        ConsoleHelper.WriteStep($"[Document] Bon de réception généré : {document}");
    }

    public void PrintLabel(string labelContent)
    {
        ConsoleHelper.WriteStep($"[Document] Étiquette imprimée : {labelContent}");
    }
}

// Sous-système 4 : Notifications
public class NotificationService
{
    public void NotifyWarehouseManager(string message)
    {
        ConsoleHelper.WriteStep($"[Notification] Responsable entrepôt : {message}");
    }

    public void NotifyPurchasingDepartment(string supplierId, bool isConform)
    {
        string status = isConform ? "Conforme" : "Anomalie détectée";
        ConsoleHelper.WriteStep(
            $"[Notification] Département achats : fournisseur {supplierId} - {status}"
        );
    }
}

// Façade : orchestre le workflow complet de réception
public class ReceptionFacade
{
    private readonly DeliveryValidator _validator = new();
    private readonly StockManager _stockManager = new();
    private readonly DocumentGenerator _documentGenerator = new();
    private readonly NotificationService _notificationService = new();

    public void ProcessReception(
        string deliveryId,
        string supplierId,
        List<string> expectedItems,
        List<string> receivedItems
    )
    {
        ConsoleHelper.WriteStep($"[Réception] Début du traitement de {deliveryId}");

        bool isValid = _validator.ValidateDelivery(supplierId, expectedItems, receivedItems);

        if (!isValid)
        {
            _notificationService.NotifyWarehouseManager($"Anomalie sur {deliveryId}");
            _notificationService.NotifyPurchasingDepartment(supplierId, false);
            ConsoleHelper.WriteStep($"[Réception] Processus interrompu pour {deliveryId}");
            return;
        }

        _stockManager.UpdateStock(receivedItems, 100);

        _documentGenerator.GenerateReceiptDocument(deliveryId);
        _documentGenerator.PrintLabel($"LIV-{deliveryId}");

        _notificationService.NotifyWarehouseManager(
            $"{deliveryId} finalisée - {receivedItems.Count} références"
        );
        _notificationService.NotifyPurchasingDepartment(supplierId, true);

        ConsoleHelper.WriteStep($"[Réception] Traitement de {deliveryId} terminé avec succès");
    }
}

public class FacadeDemo : IDemo
{
    public void Run()
    {
        var facade = new ReceptionFacade();

        ConsoleHelper.WriteStep("[Scénario] Réception conforme");

        var expectedItems = new List<string> { "VIS-M6", "ECROU-M6", "RONDELLE-M6" };
        var receivedItems = new List<string> { "VIS-M6", "ECROU-M6", "RONDELLE-M6" };

        facade.ProcessReception("LIV-2025-001", "FOURNISSEUR-A", expectedItems, receivedItems);

        ConsoleHelper.WriteStep("");
        ConsoleHelper.WriteStep("[Scénario] Réception non conforme");

        var expectedItems2 = new List<string> { "BOULON-M12", "ECROU-M12" };
        var receivedItems2 = new List<string> { "BOULON-M12" };

        facade.ProcessReception("LIV-2025-002", "FOURNISSEUR-B", expectedItems2, receivedItems2);
    }
}
