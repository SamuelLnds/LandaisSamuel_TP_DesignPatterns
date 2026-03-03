namespace LandaisSamuel_TP_DesignPatterns.Decorator;

// Contrat commun
public interface IPickingOperation
{
    void Execute();
}

// Classe de base : l'opération de picking standard
public class PickingOperation(string item, int quantity) : IPickingOperation
{
    private readonly string _item = item;
    private readonly int _quantity = quantity;

    public void Execute()
    {
        ConsoleHelper.WriteStep($"[Picking] Prélèvement de {_quantity} x {_item}");
    }
}

// Décorateur de base : enveloppe n'importe quelle IPickingOperation
// La classe abstraite facilite la création de décorateurs
public abstract class PickingDecorator(IPickingOperation wrapped) : IPickingOperation
{
    // Le décorateur de base stocke une référence à l'opération enveloppée
    protected readonly IPickingOperation _wrapped = wrapped;

    // Virtual est utilisé pour permettre aux décorateurs d'override
    public virtual void Execute() => _wrapped.Execute();
}

// Décorateur 1 : vérification du poids
public class WeightCheckDecorator(IPickingOperation wrapped) : PickingDecorator(wrapped)
{
    public override void Execute()
    {
        ConsoleHelper.WriteStep("[WeightCheck] Vérification du poids de la palette...");
        _wrapped.Execute();
        ConsoleHelper.WriteStep("[WeightCheck] Poids conforme.");
    }
}

// Décorateur 2 : traçabilité par scan
public class ScanTrackingDecorator(IPickingOperation wrapped) : PickingDecorator(wrapped)
{
    public override void Execute()
    {
        ConsoleHelper.WriteStep("[ScanTracking] Scan de l'article enregistré.");
        _wrapped.Execute();
        ConsoleHelper.WriteStep("[ScanTracking] Traçabilité mise à jour.");
    }
}

public class DecoratorDemo : IDemo
{
    // Suppression de l'avertissement de performance parce qu'il n'est pas pertinent dans le contexte de l'exemple
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1859:Utiliser des types concrets si possible pour améliorer les performances",
        Justification = "Dans le cadre de l'exemple, on utilise l'implémentation afin de représenter le wrapper"
    )]
    public void Run()
    {
        IPickingOperation operation = new PickingOperation("Carton de vis M6", 3);

        // Opération simple
        ConsoleHelper.WriteStep("=== Picking simple ===");
        operation.Execute();

        // Avec vérification de poids uniquement
        ConsoleHelper.WriteStep("\n=== Picking + poids ===");
        IPickingOperation withWeight = new WeightCheckDecorator(operation);
        withWeight.Execute();

        // Avec les deux décorateurs empilés
        ConsoleHelper.WriteStep("\n=== Picking + poids + scan ===");
        IPickingOperation withBoth = new ScanTrackingDecorator(new WeightCheckDecorator(operation));
        withBoth.Execute();
    }
}
