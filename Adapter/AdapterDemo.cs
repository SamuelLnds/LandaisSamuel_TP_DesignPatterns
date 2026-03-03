namespace LandaisSamuel_TP_DesignPatterns.Adapter;

// Interface attendue
public interface IWarehouseScannerService
{
    ScanResult ScanItem();
}

// Format de la donnée attendue
public class ScanResult
{
    public required string ItemCode { get; set; }
    public required string LocationCode { get; set; }
    public int Quantity { get; set; }
}

// Scanner tiers existant (ne pas modifier)
public sealed class LegacyBarcodeScanner
{
    public string ReadRawData()
    {
        // Format brut
        return "ITEM:VIS-M6|LOC:A3|QTY:50";
    }
}

// Adapter
public class BarcodeScannerAdapter(LegacyBarcodeScanner legacyScanner) : IWarehouseScannerService
{
    public ScanResult ScanItem()
    {
        // Récupère les données brutes du scanner legacy
        string rawData = legacyScanner.ReadRawData();

        // Parse et transforme dans le format lisible par le système
        var parts = rawData.Split('|');
        return new ScanResult
        {
            ItemCode = parts[0].Split(':')[1],
            LocationCode = parts[1].Split(':')[1],
            Quantity = int.Parse(parts[2].Split(':')[1]),
        };
    }
}

// Système WMS
public class WarehouseSystem(IWarehouseScannerService scanner)
{
    public void ProcessScan()
    {
        var result = scanner.ScanItem();
        Console.WriteLine($"[WMS] Article scanné :");
        Console.WriteLine($"  Code article : {result.ItemCode}");
        Console.WriteLine($"  Emplacement  : {result.LocationCode}");
        Console.WriteLine($"  Quantité     : {result.Quantity}");
    }
}

public class AdapterDemo : IDemo
{
    public void Run()
    {
        var legacyScanner = new LegacyBarcodeScanner();
        var adapter = new BarcodeScannerAdapter(legacyScanner);
        var wms = new WarehouseSystem(adapter);

        wms.ProcessScan();
    }
}
