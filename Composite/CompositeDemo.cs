namespace LandaisSamuel_TP_DesignPatterns.Composite;

// Interface commune : tous les éléments emballables
public interface IPackagingComponent
{
    string GetLabel();
    double GetTotalWeight(); // Poids total en kg
    void Display(int indent = 0); // L'affichage pour chaque composite/feuille avec l'identation
    string GetPackagingType(); // "Article", "Carton", "Palette"
}

// Leaf : un article avec sa quantité
public class Article(string code, int quantity, double unitWeight) : IPackagingComponent
{
    public string GetLabel() => $"{code} (x{quantity})";

    public double GetTotalWeight() => quantity * unitWeight;

    public string GetPackagingType() => "Article";

    public void Display(int indent = 0)
    {
        Console.WriteLine(
            $"{new string(' ', indent)}|-- Article: {GetLabel()} - {GetTotalWeight():F2} kg"
        );
    }
}

// Composite : un carton
public class Carton(string reference) : IPackagingComponent
{
    private readonly List<IPackagingComponent> _contents = [];
    private const double CARTON_WEIGHT = 0.5; // Poids du carton vide

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
        string listStart = indent == 0 ? "[CARTON]" : "|-- ";
        Console.WriteLine(
            $"{new string(' ', indent)}{listStart} {GetLabel()} - {GetTotalWeight():F2} kg"
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
    private const double PALETTE_WEIGHT = 25.0; // Poids de la palette vide (EUR)

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
        // La palette est toujours un premier niveau (un carton ne contient pas de palette)
        // On n'a pas besoin de faire un affichage conditionnel
        Console.WriteLine(
            $"{new string(' ', indent)}[PALETTE] {GetLabel()} - {GetTotalWeight():F2} kg"
        );
        foreach (var item in _contents)
            item.Display(indent + 4);
    }

    public int GetItemCount() => _contents.Count;
}

// Expédition : contient tous les colis de premier niveau
// On ne récupère pas en récursion, c'est un affichage du poids total
// C'est l'information demandée par les transporteurs
public class Shipment(string shipmentId)
{
    private readonly List<IPackagingComponent> _packages = [];

    public void AddPackage(IPackagingComponent package) => _packages.Add(package);

    public double GetTotalWeight() => _packages.Sum(p => p.GetTotalWeight());

    public void DisplayPackingList()
    {
        Console.WriteLine("================================================================");
        Console.WriteLine($" BORDEREAU D'EXPEDITION - {shipmentId}");
        Console.WriteLine("================================================================\n");

        foreach (var package in _packages)
            package.Display();

        Console.WriteLine("\n----------------------------------------------------------------");
        Console.WriteLine($"POIDS TOTAL DE L'EXPEDITION : {GetTotalWeight():F2} kg");
        Console.WriteLine($"NOMBRE DE COLIS DE NIVEAU 1 : {_packages.Count}");
    }

    public string GenerateCarrierLabel()
    {
        var label = $"EXPEDITION: {shipmentId}\n";
        label += $"POIDS TOTAL: {GetTotalWeight():F2} kg\n";
        label += $"COLIS:\n";

        for (int i = 0; i < _packages.Count; i++)
        {
            var package = _packages[i];
            label +=
                $"  [{i + 1}] {package.GetPackagingType()} - {package.GetTotalWeight():F2} kg\n";
        }

        return label;
    }
}

public class CompositeDemo : IDemo
{
    public void Run()
    {
        // Création d'une expédition
        var shipment = new Shipment("EXP-2025-0342");

        Console.WriteLine("=== ETAPE DE COLISAGE ===\n");

        // --- CARTON 1 : petits articles ---
        var carton1 = new Carton("C001");
        carton1.Add(new Article("VIS-M6-INOX", 500, 0.002)); // 500 vis à 2g
        carton1.Add(new Article("ECROU-M6", 500, 0.003)); // 500 écrous à 3g
        carton1.Add(new Article("RONDELLE-M6", 300, 0.001)); // 300 rondelles à 1g
        shipment.AddPackage(carton1);
        Console.WriteLine(
            $"[OK] {carton1.GetLabel()} préparé ({carton1.GetItemCount()} types d'articles)"
        );

        // --- CARTON 2 : pièces moyennes ---
        var carton2 = new Carton("C002");
        carton2.Add(new Article("SUPPORT-METAL-A", 12, 0.85));
        carton2.Add(new Article("SUPPORT-METAL-B", 8, 1.2));
        shipment.AddPackage(carton2);
        Console.WriteLine(
            $"[OK] {carton2.GetLabel()} préparé ({carton2.GetItemCount()} types d'articles)"
        );

        // --- PALETTE 1 : articles lourds directs + carton imbriqué ---
        var palette1 = new Palette("P001");
        palette1.Add(new Article("PLAQUE-ACIER-100x50", 20, 8.5)); // Directement sur palette

        // Carton imbriqué dans la palette
        var carton3 = new Carton("C003");
        carton3.Add(new Article("BOULON-M12", 100, 0.05));
        carton3.Add(new Article("ECROU-M12", 100, 0.04));
        palette1.Add(carton3);

        shipment.AddPackage(palette1);
        Console.WriteLine(
            $"[OK] {palette1.GetLabel()} préparée (articles directs + carton imbriqué)"
        );

        // --- PALETTE 2 : plusieurs cartons imbriqués ---
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
        Console.WriteLine($"[OK] {palette2.GetLabel()} préparée (2 cartons imbriqués)");

        Console.WriteLine("\n" + new string('=', 64) + "\n");

        // Affichage du bordereau complet
        shipment.DisplayPackingList();

        // Génération de l'étiquette transporteur
        Console.WriteLine("\n" + new string('=', 64));
        Console.WriteLine("\nETIQUETTE TRANSPORTEUR :");
        Console.WriteLine(new string('-', 64));
        Console.WriteLine(shipment.GenerateCarrierLabel());
    }
}
