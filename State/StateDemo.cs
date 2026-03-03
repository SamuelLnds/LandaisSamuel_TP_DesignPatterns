namespace LandaisSamuel_TP_DesignPatterns.State;

public interface ITrackingState
{
    void Receive(Article context, int quantity);
    void Pick(Article context, int quantity);
    string GetIdentity(Article context);
}

public class Article(string reference, ITrackingState initialState)
{
    public string Reference { get; } = reference;
    public int Stock { get; set; }
    public string? CurrentLot { get; set; }
    public string? CurrentSerial { get; set; }

    // Représente le changement de mode de suivi d'un article
    public void TransitionTo(ITrackingState newState)
    {
        string from = initialState.GetType().Name;
        string to = newState.GetType().Name;
        initialState = newState;
        ConsoleHelper.WriteStep($"[Article] Transition : {from} -> {to}");
    }

    // Le contexte délègue tout à l'état courant
    public void Receive(int quantity) => initialState.Receive(this, quantity);

    public void Pick(int quantity) => initialState.Pick(this, quantity);

    public string GetIdentity() => initialState.GetIdentity(this);
}

public class NoStockMonitoringState : ITrackingState
{
    public void Receive(Article context, int quantity)
    {
        context.Stock += quantity;
        ConsoleHelper.WriteStep(
            $"[NoStockMonitoring] Réception de {quantity} x {context.Reference} (pas de contrôle)"
        );
    }

    public void Pick(Article context, int quantity)
    {
        context.Stock -= quantity;
        ConsoleHelper.WriteStep(
            $"[NoStockMonitoring] Prélèvement de {quantity} x {context.Reference} (pas de contrôle)"
        );
    }

    public string GetIdentity(Article context) => context.Reference;
}

public class StandardState : ITrackingState
{
    public void Receive(Article context, int quantity)
    {
        context.Stock += quantity;
        ConsoleHelper.WriteStep(
            $"[Standard] Réception de {quantity} x {context.Reference} (stock mis à jour)"
        );
    }

    public void Pick(Article context, int quantity)
    {
        if (context.Stock < quantity)
        {
            ConsoleHelper.WriteStep(
                $"[Standard] Stock insuffisant pour {context.Reference} ({context.Stock} disponibles)"
            );
            return;
        }
        context.Stock -= quantity;
        ConsoleHelper.WriteStep(
            $"[Standard] Prélèvement de {quantity} x {context.Reference} (stock vérifié)"
        );
    }

    public string GetIdentity(Article context) => $"{context.Reference} [Stock: {context.Stock}]";
}

public class BatchState : ITrackingState
{
    public void Receive(Article context, int quantity)
    {
        context.CurrentLot = $"LOT-{DateTime.Now:yyyyMMdd}-{new Random().Next(100, 999)}";
        context.Stock += quantity;
        ConsoleHelper.WriteStep(
            $"[Batch] Réception de {quantity} x {context.Reference} sous {context.CurrentLot}"
        );
    }

    public void Pick(Article context, int quantity)
    {
        if (context.CurrentLot == null)
        {
            ConsoleHelper.WriteStep(
                $"[Batch] Aucun lot affecté pour {context.Reference}, prélèvement refusé"
            );
            return;
        }
        context.Stock -= quantity;
        ConsoleHelper.WriteStep(
            $"[Batch] Prélèvement de {quantity} x {context.Reference} depuis {context.CurrentLot}"
        );
    }

    public string GetIdentity(Article context) =>
        $"{context.Reference} [{context.CurrentLot ?? "Aucun lot"}]";
}

public class SerialState : ITrackingState
{
    public void Receive(Article context, int quantity)
    {
        context.CurrentSerial = $"SN-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        context.Stock += 1;
        ConsoleHelper.WriteStep(
            $"[Serial] Réception unitaire de {context.Reference} avec {context.CurrentSerial}"
        );
        if (quantity > 1)
            ConsoleHelper.WriteStep(
                $"[Serial] Suivi série impose la réception unitaire ({quantity - 1} restants à traiter)"
            );
    }

    public void Pick(Article context, int quantity)
    {
        if (context.CurrentSerial == null)
        {
            ConsoleHelper.WriteStep(
                $"[Serial] Aucun numéro de série pour {context.Reference}, prélèvement refusé"
            );
            return;
        }
        context.Stock -= 1;
        ConsoleHelper.WriteStep(
            $"[Serial] Prélèvement unitaire de {context.Reference} ({context.CurrentSerial})"
        );
        context.CurrentSerial = null;
    }

    public string GetIdentity(Article context) =>
        $"{context.Reference} [{context.CurrentSerial ?? "Aucun SN"}]";
}

public class StateDemo : IDemo
{
    public void Run()
    {
        var article = new Article("VIS-M6-100", new NoStockMonitoringState());

        // Test de l'article sans suivi de stock
        article.Receive(500);
        article.Pick(50);
        ConsoleHelper.WriteStep($"[Article] Identité : {article.GetIdentity()}");

        // Transition vers le suivi de stock standard
        article.TransitionTo(new StandardState());
        article.Receive(200);
        article.Pick(100);
        ConsoleHelper.WriteStep($"[Article] Identité : {article.GetIdentity()}");

        // Transition vers le suivi par lot
        article.TransitionTo(new BatchState());
        article.Receive(300);
        article.Pick(50);
        ConsoleHelper.WriteStep($"[Article] Identité : {article.GetIdentity()}");

        // Transition vers le suivi en série
        article.TransitionTo(new SerialState());
        article.Receive(3);
        article.Pick(1);
        ConsoleHelper.WriteStep($"[Article] Identité : {article.GetIdentity()}");
    }
}
