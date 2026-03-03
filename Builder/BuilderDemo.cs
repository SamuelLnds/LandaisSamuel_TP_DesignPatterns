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
    CharioElevateur,
    Transpalette,
    ScannerPortable,
}

#endregion

// Product
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
        ConsoleHelper.WriteStep("================================================================");
        ConsoleHelper.WriteStep($" MISSION DE PRÉPARATION - {MissionId}");
        ConsoleHelper.WriteStep("================================================================");
        ConsoleHelper.WriteStep($"Commande              : {OrderReference}");
        ConsoleHelper.WriteStep($"Priorité              : {Priority}");
        ConsoleHelper.WriteStep($"Mode de picking       : {PickingMode}");
        ConsoleHelper.WriteStep($"Zones à parcourir     : {string.Join(", ", TargetZones)}");
        ConsoleHelper.WriteStep($"Équipements requis    : {string.Join(", ", RequiredEquipment)}");
        ConsoleHelper.WriteStep(
            $"Contrôle qualité      : {(RequiresQualityCheck ? "OUI" : "NON")}"
        );
        ConsoleHelper.WriteStep(
            $"Vérification poids    : {(RequiresWeightVerification ? "OUI" : "NON")}"
        );
        ConsoleHelper.WriteStep($"Opérateur assigné     : {AssignedOperator ?? "Non assigné"}");
        ConsoleHelper.WriteStep($"Créée le              : {CreatedAt:dd/MM/yyyy HH:mm}");
        ConsoleHelper.WriteStep(
            "================================================================\n"
        );
    }
}

// Builder Interface
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
    // Instance en cours de construction
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
        // Validation avant construction
        if (string.IsNullOrEmpty(_mission.MissionId))
            throw new InvalidOperationException("Mission ID est obligatoire");

        if (string.IsNullOrEmpty(_mission.OrderReference))
            throw new InvalidOperationException("Référence commande est obligatoire");

        if (_mission.TargetZones.Count == 0)
            throw new InvalidOperationException("Au moins une zone doit être spécifiée");

        var result = _mission;
        Reset(); // Prêt pour une nouvelle construction
        return result;
    }
}

// Director (optionnel)
public class PickingMissionDirector(IPickingMissionBuilder builder)
{
    // Configuration prédéfinie : mission standard
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

    // Configuration prédéfinie : mission urgente avec contrôles
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

    // Configuration prédéfinie : mission palette complète
    public PickingMission BuildPalletMission(string missionId, string orderRef)
    {
        return builder
            .SetMissionId(missionId)
            .SetOrderReference(orderRef)
            .SetPriority(Priority.Haute)
            .SetPickingMode(PickingMode.PaletteComplete)
            .AddZone("D")
            .AddEquipment(Equipment.CharioElevateur)
            .RequireWeightVerification()
            .Build();
    }
}

public class BuilderDemo : IDemo
{
    public void Run()
    {
        ConsoleHelper.WriteStep("=== CONSTRUCTION DE MISSIONS DE PICKING ===\n");

        ConsoleHelper.WriteStep(">>> SCENARIO 1 : Construction manuelle (fluent interface)\n");

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

        ConsoleHelper.WriteStep(">>> SCENARIO 2 : Utilisation du Director (mission standard)\n");

        var director = new PickingMissionDirector(new PickingMissionBuilder());
        var mission2 = director.BuildStandardMission("PICK-2025-002", "CMD-45679");

        mission2.Display();

        ConsoleHelper.WriteStep(">>> SCENARIO 3 : Utilisation du Director (mission urgente)\n");

        var mission3 = director.BuildUrgentMissionWithChecks(
            "PICK-2025-003",
            "CMD-45680",
            "Marie Martin"
        );

        mission3.Display();

        ConsoleHelper.WriteStep(">>> SCENARIO 4 : Mission palette complète\n");

        var mission4 = director.BuildPalletMission("PICK-2025-004", "CMD-45681");

        mission4.Display();
    }
}
