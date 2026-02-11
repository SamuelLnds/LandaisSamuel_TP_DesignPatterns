namespace LandaisSamuel_TP_DesignPatterns.Command;

// Receiver : contient la logique métier
public class StockService
{
    private readonly Dictionary<string, int> _stock = new() { { "A1", 100 }, { "B2", 50 } };

    public void Move(string from, string to, int quantity)
    {
        _stock[from] -= quantity;
        _stock[to] += quantity;
        ConsoleHelper.WriteStep($"[StockService] Déplacé {quantity} unités de {from} à {to}");
    }

    public void UndoMove(string from, string to, int quantity)
    {
        _stock[to] -= quantity;
        _stock[from] += quantity;
        ConsoleHelper.WriteStep(
            $"[StockService] Annulation : {quantity} unités remises de {to} à {from}"
        );
    }

    public void PrintStock()
    {
        ConsoleHelper.WriteStep("\n--- Stock actuel ---");
        foreach (var entry in _stock)
            ConsoleHelper.WriteStep($"  {entry.Key} : {entry.Value} unités");
    }
}

// Interface commune
public interface ICommand
{
    void Execute();
    void Undo();
}

// ConcreteCommand : déplacement de stock
public class MoveStockCommand(StockService stockService, string from, string to, int quantity)
    : ICommand
{
    private readonly StockService _stockService = stockService;
    private readonly string _from = from;
    private readonly string _to = to;
    private readonly int _quantity = quantity;

    public void Execute() => _stockService.Move(_from, _to, _quantity);

    public void Undo() => _stockService.UndoMove(_from, _to, _quantity);
}

// Invoker : terminal opérateur
public class OperatorTerminal
{
    private readonly Stack<ICommand> _history = new();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void UndoLast()
    {
        if (_history.Count == 0)
        {
            ConsoleHelper.WriteStep("[Terminal] Aucune action à annuler.");
            return;
        }
        var last = _history.Pop(); // Pop permet de récupérer et retirer la dernière commande de la liste
        last.Undo();
    }
}

public class CommandDemo
{
    public static void ExampleExecution()
    {
        // La logique ici correspond au client
        // C'est là où les commandes sont créées et là où l'invoker est utilisé
        var stockService = new StockService();
        var terminal = new OperatorTerminal();

        stockService.PrintStock();

        // On peut imaginer que ces commandes sont déclenchées par des actions de l'utilisateur sur une interface
        // Par exemple, un opérateur qui clique sur un bouton sur le terminal
        terminal.ExecuteCommand(new MoveStockCommand(stockService, "A1", "B2", 30));
        terminal.ExecuteCommand(new MoveStockCommand(stockService, "B2", "A1", 10));

        stockService.PrintStock();

        ConsoleHelper.WriteStep("\n--- Annulation du dernier mouvement ---");
        terminal.UndoLast();

        stockService.PrintStock();
    }
}
