namespace LandaisSamuel_TP_DesignPatterns.Builder;

#region Enums

public enum PickingMode
{
    Unitaire,
    CartonComplet,
    PaletteComplete,
}

public enum Priority
{
    Normale,
    Haute,
    Urgente,
}

public enum Equipment
{
    ChariotElevateur,
    Transpalette,
    ScannerPortable,
}

#endregion

// Produit construit par le Builder
public class PickingMission
{
    public string? MissionId { get; set; }
    public string? OrderReference { get; set; }
    public Priority Priority { get; set; }
    public PickingMode PickingMode { get; set; }
    public List<string> TargetZones { get; set; } = [];
    public List<Equipment> RequiredEquipment { get; set; } = [];
    public bool RequiresQualityCheck { get; set; }
    public bool RequiresWeightVerification { get; set; }
    public string? AssignedOperator { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Display()
    {
        ConsoleHelper.WriteStep($"[Mission] {MissionId}");
        ConsoleHelper.WriteStep($"[Mission] Commande              : {OrderReference}");
        ConsoleHelper.WriteStep($"[Mission] Priorité              : {Priority}");
        ConsoleHelper.WriteStep($"[Mission] Mode de picking       : {PickingMode}");
        ConsoleHelper.WriteStep(
            $"[Mission] Zones à parcourir     : {string.Join(", ", TargetZones)}"
        );
        ConsoleHelper.WriteStep(
            $"[Mission] Équipements requis    : {string.Join(", ", RequiredEquipment)}"
        );
        ConsoleHelper.WriteStep(
            $"[Mission] Contrôle qualité      : {(RequiresQualityCheck ? "OUI" : "NON")}"
        );
        ConsoleHelper.WriteStep(
            $"[Mission] Vérification poids    : {(RequiresWeightVerification ? "OUI" : "NON")}"
        );
        ConsoleHelper.WriteStep(
            $"[Mission] Opérateur assigné     : {AssignedOperator ?? "Non assigné"}"
        );
        ConsoleHelper.WriteStep($"[Mission] Créée le              : {CreatedAt:dd/MM/yyyy HH:mm}");
    }
}

// Interface Builder
public interface IPickingMissionBuilder
{
    IPickingMissionBuilder SetMissionId(string missionId);
    IPickingMissionBuilder SetOrderReference(string orderRef);
    IPickingMissionBuilder SetPriority(Priority priority);
    IPickingMissionBuilder SetPickingMode(PickingMode mode);
    IPickingMissionBuilder AddZone(string zone);
    IPickingMissionBuilder AddEquipment(Equipment equipment);
    IPickingMissionBuilder RequireQualityCheck();
    IPickingMissionBuilder RequireWeightVerification();
    IPickingMissionBuilder AssignOperator(string operatorName);
    PickingMission Build();
}

// Builder concret
public class PickingMissionBuilder : IPickingMissionBuilder
{
    private PickingMission _mission = new();

    public PickingMissionBuilder()
    {
        // On commence avec une instance vierge
        Reset();
    }

    public void Reset()
    {
        _mission = new PickingMission { CreatedAt = DateTime.Now, Priority = Priority.Normale };
    }

    public IPickingMissionBuilder SetMissionId(string missionId)
    {
        _mission.MissionId = missionId;
        return this;
    }

    public IPickingMissionBuilder SetOrderReference(string orderRef)
    {
        _mission.OrderReference = orderRef;
        return this;
    }

    public IPickingMissionBuilder SetPriority(Priority priority)
    {
        _mission.Priority = priority;
        return this;
    }

    public IPickingMissionBuilder SetPickingMode(PickingMode mode)
    {
        _mission.PickingMode = mode;
        return this;
    }

    public IPickingMissionBuilder AddZone(string zone)
    {
        _mission.TargetZones.Add(zone);
        return this;
    }

    public IPickingMissionBuilder AddEquipment(Equipment equipment)
    {
        if (!_mission.RequiredEquipment.Contains(equipment))
            _mission.RequiredEquipment.Add(equipment);
        return this;
    }

    public IPickingMissionBuilder RequireQualityCheck()
    {
        _mission.RequiresQualityCheck = true;
        return this;
    }

    public IPickingMissionBuilder RequireWeightVerification()
    {
        _mission.RequiresWeightVerification = true;
        return this;
    }

    public IPickingMissionBuilder AssignOperator(string operatorName)
    {
        _mission.AssignedOperator = operatorName;
        return this;
    }

    public PickingMission Build()
    {
        if (string.IsNullOrEmpty(_mission.MissionId))
            throw new InvalidOperationException("Mission ID est obligatoire");

        if (string.IsNullOrEmpty(_mission.OrderReference))
            throw new InvalidOperationException("Référence commande est obligatoire");

        if (_mission.TargetZones.Count == 0)
            throw new InvalidOperationException("Au moins une zone doit être spécifiée");

        var result = _mission;
        Reset();
        return result;
    }
}

// Director : configurations prédéfinies de missions
public class PickingMissionDirector(IPickingMissionBuilder builder)
{
    public PickingMission BuildStandardMission(string missionId, string orderRef)
    {
        return builder
            .SetMissionId(missionId)
            .SetOrderReference(orderRef)
            .SetPriority(Priority.Normale)
            .SetPickingMode(PickingMode.Unitaire)
            .AddZone("A")
            .AddZone("B")
            .AddEquipment(Equipment.ScannerPortable)
            .Build();
    }

    public PickingMission BuildUrgentMissionWithChecks(
        string missionId,
        string orderRef,
        string operatorName
    )
    {
        return builder
            .SetMissionId(missionId)
            .SetOrderReference(orderRef)
            .SetPriority(Priority.Urgente)
            .SetPickingMode(PickingMode.CartonComplet)
            .AddZone("A")
            .AddZone("C")
            .AddEquipment(Equipment.Transpalette)
            .AddEquipment(Equipment.ScannerPortable)
            .RequireQualityCheck()
            .RequireWeightVerification()
            .AssignOperator(operatorName)
            .Build();
    }

    public PickingMission BuildPalletMission(string missionId, string orderRef)
    {
        return builder
            .SetMissionId(missionId)
            .SetOrderReference(orderRef)
            .SetPriority(Priority.Haute)
            .SetPickingMode(PickingMode.PaletteComplete)
            .AddZone("D")
            .AddEquipment(Equipment.ChariotElevateur)
            .RequireWeightVerification()
            .Build();
    }
}

public class BuilderDemo : IDemo
{
    public void Run()
    {
        ConsoleHelper.WriteStep("[Builder] Construction manuelle via fluent interface");

        var builder = new PickingMissionBuilder();
        var mission1 = builder
            .SetMissionId("PICK-2025-001")
            .SetOrderReference("CMD-45678")
            .SetPriority(Priority.Haute)
            .SetPickingMode(PickingMode.Unitaire)
            .AddZone("A")
            .AddZone("B")
            .AddZone("C")
            .AddEquipment(Equipment.ScannerPortable)
            .RequireQualityCheck()
            .AssignOperator("Jean Dupont")
            .Build();

        mission1.Display();

        ConsoleHelper.WriteStep("");
        ConsoleHelper.WriteStep("[Director] Mission standard");

        var director = new PickingMissionDirector(new PickingMissionBuilder());
        var mission2 = director.BuildStandardMission("PICK-2025-002", "CMD-45679");

        mission2.Display();

        ConsoleHelper.WriteStep("");
        ConsoleHelper.WriteStep("[Director] Mission urgente avec contrôles");

        var mission3 = director.BuildUrgentMissionWithChecks(
            "PICK-2025-003",
            "CMD-45680",
            "Marie Martin"
        );

        mission3.Display();

        ConsoleHelper.WriteStep("");
        ConsoleHelper.WriteStep("[Director] Mission palette complète");

        var mission4 = director.BuildPalletMission("PICK-2025-004", "CMD-45681");

        mission4.Display();
    }
}
