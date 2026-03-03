namespace LandaisSamuel_TP_DesignPatterns.Composite;

// Interface commune : tous les éléments emballables
public interface IPackagingComponent
{
    string GetLabel();
    double GetTotalWeight();
    void Display(int indent = 0);
    string GetPackagingType();
}

// Leaf : un article avec sa quantité
public class Article(string code, int quantity, double unitWeight) : IPackagingComponent
{
    public string GetLabel() => $"{code} (x{quantity})";

    public double GetTotalWeight() => quantity * unitWeight;

    public string GetPackagingType() => "Article";

    public void Display(int indent = 0)
    {
        ConsoleHelper.WriteStep(
            $"{new string(' ', indent)}|-- Article: {GetLabel()} - {GetTotalWeight():F2} kg"
        );
    }
}

// Composite : un carton
public class Carton(string reference) : IPackagingComponent
{
    private readonly List<IPackagingComponent> _contents = [];
    private const double CARTON_WEIGHT = 0.5;

    public void Add(IPackagingComponent component) => _contents.Add(component);

    public void Remove(IPackagingComponent component) => _contents.Remove(component);

    public string GetLabel() => $"Carton {reference}";

    public string GetPackagingType() => "Carton";

    public double GetTotalWeight()
    {
        double contentWeight = _contents.Sum(c => c.GetTotalWeight());
        return CARTON_WEIGHT + contentWeight;
    }

    public void Display(int indent = 0)
    {
        string prefix = indent == 0 ? "[CARTON]" : "|--";
        ConsoleHelper.WriteStep(
            $"{new string(' ', indent)}{prefix} {GetLabel()} - {GetTotalWeight():F2} kg"
        );
        foreach (var item in _contents)
            item.Display(indent + 4);
    }

    public int GetItemCount() => _contents.Count;
}

// Composite : une palette
public class Palette(string reference) : IPackagingComponent
{
    private readonly List<IPackagingComponent> _contents = [];
    private const double PALETTE_WEIGHT = 25.0;

    public void Add(IPackagingComponent component) => _contents.Add(component);

    public void Remove(IPackagingComponent component) => _contents.Remove(component);

    public string GetLabel() => $"Palette {reference}";

    public string GetPackagingType() => "Palette";

    public double GetTotalWeight()
    {
        double contentWeight = _contents.Sum(c => c.GetTotalWeight());
        return PALETTE_WEIGHT + contentWeight;
    }

    public void Display(int indent = 0)
    {
        ConsoleHelper.WriteStep(
            $"{new string(' ', indent)}[PALETTE] {GetLabel()} - {GetTotalWeight():F2} kg"
        );
        foreach (var item in _contents)
            item.Display(indent + 4);
    }

    public int GetItemCount() => _contents.Count;
}

// Expédition : regroupe les colis de premier niveau
public class Shipment(string shipmentId)
{
    private readonly List<IPackagingComponent> _packages = [];

    public void AddPackage(IPackagingComponent package) => _packages.Add(package);

    public double GetTotalWeight() => _packages.Sum(p => p.GetTotalWeight());

    public void DisplayPackingList()
    {
        ConsoleHelper.WriteStep($"[Expédition] Bordereau {shipmentId}");
        ConsoleHelper.WriteStep("");

        foreach (var package in _packages)
            package.Display();

        ConsoleHelper.WriteStep("");
        ConsoleHelper.WriteStep($"[Expédition] Poids total : {GetTotalWeight():F2} kg");
        ConsoleHelper.WriteStep($"[Expédition] Nombre de colis niveau 1 : {_packages.Count}");
    }

    public void DisplayCarrierLabel()
    {
        ConsoleHelper.WriteStep($"[Transporteur] Expédition : {shipmentId}");
        ConsoleHelper.WriteStep($"[Transporteur] Poids total : {GetTotalWeight():F2} kg");

        for (int i = 0; i < _packages.Count; i++)
        {
            var package = _packages[i];
            ConsoleHelper.WriteStep(
                $"[Transporteur] Colis {i + 1} : {package.GetPackagingType()} - {package.GetTotalWeight():F2} kg"
            );
        }
    }
}

public class CompositeDemo : IDemo
{
    public void Run()
    {
        var shipment = new Shipment("EXP-2025-0342");

        // Carton 1 : petits articles
        var carton1 = new Carton("C001");
        carton1.Add(new Article("VIS-M6-INOX", 500, 0.002));
        carton1.Add(new Article("ECROU-M6", 500, 0.003));
        carton1.Add(new Article("RONDELLE-M6", 300, 0.001));
        shipment.AddPackage(carton1);
        ConsoleHelper.WriteStep(
            $"[Colisage] {carton1.GetLabel()} préparé ({carton1.GetItemCount()} types d'articles)"
        );

        // Carton 2 : pièces moyennes
        var carton2 = new Carton("C002");
        carton2.Add(new Article("SUPPORT-METAL-A", 12, 0.85));
        carton2.Add(new Article("SUPPORT-METAL-B", 8, 1.2));
        shipment.AddPackage(carton2);
        ConsoleHelper.WriteStep(
            $"[Colisage] {carton2.GetLabel()} préparé ({carton2.GetItemCount()} types d'articles)"
        );

        // Palette 1 : articles lourds + carton imbriqué
        var palette1 = new Palette("P001");
        palette1.Add(new Article("PLAQUE-ACIER-100x50", 20, 8.5));

        var carton3 = new Carton("C003");
        carton3.Add(new Article("BOULON-M12", 100, 0.05));
        carton3.Add(new Article("ECROU-M12", 100, 0.04));
        palette1.Add(carton3);

        shipment.AddPackage(palette1);
        ConsoleHelper.WriteStep(
            $"[Colisage] {palette1.GetLabel()} préparée (articles directs + carton imbriqué)"
        );

        // Palette 2 : plusieurs cartons imbriqués
        var palette2 = new Palette("P002");

        var carton4 = new Carton("C004");
        carton4.Add(new Article("CABLE-2.5mm-ROUGE", 10, 0.15));
        carton4.Add(new Article("CABLE-2.5mm-BLEU", 10, 0.15));

        var carton5 = new Carton("C005");
        carton5.Add(new Article("CONNECTEUR-RJ45", 50, 0.01));
        carton5.Add(new Article("CLIP-FIXATION", 100, 0.005));

        palette2.Add(carton4);
        palette2.Add(carton5);
        shipment.AddPackage(palette2);
        ConsoleHelper.WriteStep($"[Colisage] {palette2.GetLabel()} préparée (2 cartons imbriqués)");

        ConsoleHelper.WriteStep("");

        // Arborescence complète
        shipment.DisplayPackingList();

        ConsoleHelper.WriteStep("");

        // Étiquette transporteur
        shipment.DisplayCarrierLabel();
    }
}
