using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionDefinition : Definition
{
    public string DisplayName = "";

    public string Description = "";

    public float StartingInfluence = 0.0f;

    public FactionLogo FactionLogo;

    public MechSpawnDefinition[] MechsUltraLight = new MechSpawnDefinition[0];
    public MechSpawnDefinition[] MechsLight = new MechSpawnDefinition[0];
    public MechSpawnDefinition[] MechsMedium = new MechSpawnDefinition[0];
    public MechSpawnDefinition[] MechsHeavy = new MechSpawnDefinition[0];
    public MechSpawnDefinition[] MechsAssault = new MechSpawnDefinition[0];

    public GroundVehicleSpawnDefinition[] GroundVehiclesUltraLight = new GroundVehicleSpawnDefinition[0];
    public GroundVehicleSpawnDefinition[] GroundVehiclesLight = new GroundVehicleSpawnDefinition[0];
    public GroundVehicleSpawnDefinition[] GroundVehiclesMedium = new GroundVehicleSpawnDefinition[0];
    public GroundVehicleSpawnDefinition[] GroundVehiclesHeavy = new GroundVehicleSpawnDefinition[0];
    public GroundVehicleSpawnDefinition[] GroundVehiclesAssault = new GroundVehicleSpawnDefinition[0];

    public TurretSpawnDefinition[] TurretsLight = new TurretSpawnDefinition[0];
    public TurretSpawnDefinition[] TurretsMedium = new TurretSpawnDefinition[0];
    public TurretSpawnDefinition[] TurretsHeavy = new TurretSpawnDefinition[0];
    public TurretSpawnDefinition[] TurretsAssault = new TurretSpawnDefinition[0];

    public GroundVehicleSpawnDefinition[] ConvoyVehicles = new GroundVehicleSpawnDefinition[0];

    public string[] BaseFoundationsSmall = new string[0];
    public string[] BaseFoundationsMedium = new string[0];
    public string[] BaseFoundationsLarge = new string[0];
    public string[] BaseFoundationsHuge = new string[0];

    public BuildingSpawnDefinition[] BasePrimaryBuildings = new BuildingSpawnDefinition[0];
    public BuildingSpawnDefinition[] BaseOptionalBuildings = new BuildingSpawnDefinition[0];

    public BuildingSpawnDefinition TurretControlTower = new BuildingSpawnDefinition();
    public BuildingSpawnDefinition TurretGenerator = new BuildingSpawnDefinition();

    public string[] AssassinationTargetNames = new string[0];

    public bool HasMechsUltraLight { get => MechsUltraLight.Length > 0; }

    public bool HasMechsLight { get => MechsLight.Length > 0; }

    public bool HasMechsMedium { get => MechsMedium.Length > 0; }

    public bool HasMechsHeavy { get => MechsHeavy.Length > 0; }

    public bool HasMechsAssault { get => MechsAssault.Length > 0; }

    public bool HasGroundVehiclesUltraLight { get => GroundVehiclesUltraLight.Length > 0; }

    public bool HasGroundVehiclesLight { get => GroundVehiclesLight.Length > 0; }

    public bool HasGroundVehiclesMedium { get => GroundVehiclesMedium.Length > 0; }

    public bool HasGroundVehiclesHeavy { get => GroundVehiclesHeavy.Length > 0; }

    public bool HasGroundVehiclesAssault { get => GroundVehiclesAssault.Length > 0; }

    public bool HasTurretsLight { get => TurretsLight.Length > 0; }

    public bool HasTurretsMedium { get => TurretsMedium.Length > 0; }

    public bool HasTurretsHeavy { get => TurretsHeavy.Length > 0; }

    public bool HasTurretsAssault { get => TurretsAssault.Length > 0; }

    public GameObject RandomBaseFoundationSmall
    {
        get
        {
            if (BaseFoundationsSmall.Length > 0)
            {
                return ResourceManager.Instance.GetBaseFoundationPrefab(BaseFoundationsSmall[Random.Range(0, BaseFoundationsSmall.Length)]);
            }
            else
            {
                Debug.LogError("Error: No base foundation found");
            }

            return null;
        }
    }

    public GameObject RandomBaseFoundationMedium
    {
        get
        {
            if (BaseFoundationsMedium.Length > 0)
            {
                return ResourceManager.Instance.GetBaseFoundationPrefab(BaseFoundationsMedium[Random.Range(0, BaseFoundationsMedium.Length)]);
            }

            return RandomBaseFoundationSmall;
        }
    }

    public GameObject RandomBaseFoundationLarge
    {
        get
        {
            if (BaseFoundationsLarge.Length > 0)
            {
                return ResourceManager.Instance.GetBaseFoundationPrefab(BaseFoundationsLarge[Random.Range(0, BaseFoundationsLarge.Length)]);
            }

            return RandomBaseFoundationMedium;
        }
    }

    public GameObject RandomBaseFoundationHuge
    {
        get
        {
            if (BaseFoundationsMedium.Length > 0)
            {
                return ResourceManager.Instance.GetBaseFoundationPrefab(BaseFoundationsMedium[Random.Range(0, BaseFoundationsMedium.Length)]);
            }

            return RandomBaseFoundationLarge;
        }
    }

    public string RandomAssassinationTargetName
    {
        get
        {
            if (AssassinationTargetNames.Length > 0)
            {
                return AssassinationTargetNames[Random.Range(0, AssassinationTargetNames.Length)];
            }

            return "Enemy Leader";
        }
    }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }

    public SpawnDefinition GetRandomSpawnDefinition(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.MechUltraLight:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsUltraLight);
                }
            case UnitClass.MechLight:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsLight);
                }
            case UnitClass.MechMedium:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsMedium);
                }
            case UnitClass.MechHeavy:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsHeavy);
                }
            case UnitClass.MechAssault:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsAssault);
                }
            case UnitClass.GroundVehicleUltraLight:
                {
                    return GetRandomSpawnDefinitionByWeight<GroundVehicleSpawnDefinition>(GroundVehiclesUltraLight);
                }
            case UnitClass.GroundVehicleLight:
                {
                    return GetRandomSpawnDefinitionByWeight<GroundVehicleSpawnDefinition>(GroundVehiclesLight);
                }
            case UnitClass.GroundVehicleMedium:
                {
                    return GetRandomSpawnDefinitionByWeight<GroundVehicleSpawnDefinition>(GroundVehiclesMedium);
                }
            case UnitClass.GroundVehicleHeavy:
                {
                    return GetRandomSpawnDefinitionByWeight<GroundVehicleSpawnDefinition>(GroundVehiclesHeavy);
                }
            case UnitClass.GroundVehicleAssault:
                {
                    return GetRandomSpawnDefinitionByWeight<GroundVehicleSpawnDefinition>(GroundVehiclesAssault);
                }
            case UnitClass.TurretLight:
                {
                    return GetRandomSpawnDefinitionByWeight<TurretSpawnDefinition>(TurretsLight);
                }
            case UnitClass.TurretMedium:
                {
                    if (HasTurretsMedium)
                    {
                        return GetRandomSpawnDefinitionByWeight<TurretSpawnDefinition>(TurretsMedium);
                    }

                    return GetRandomSpawnDefinition(UnitClass.TurretLight);
                }
            case UnitClass.TurretHeavy:
                {
                    if (HasTurretsHeavy)
                    {
                        return GetRandomSpawnDefinitionByWeight<TurretSpawnDefinition>(TurretsHeavy);
                    }

                    return GetRandomSpawnDefinition(UnitClass.TurretMedium);
                }
            case UnitClass.TurretAssault:
                {
                    if (HasTurretsAssault)
                    {
                        return GetRandomSpawnDefinitionByWeight<TurretSpawnDefinition>(TurretsAssault);
                    }

                    return GetRandomSpawnDefinition(UnitClass.TurretHeavy);
                }
            default:
                {
                    return null;
                }
        }
    }

    public MechSpawnDefinition GetRandomMechSpawnDefinition(UnitWeightClass weightClass)
    {
        switch (weightClass)
        {
            case UnitWeightClass.UltraLight:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsUltraLight);
                }
            case UnitWeightClass.Light:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsLight);
                }
            case UnitWeightClass.Medium:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsMedium);
                }
            case UnitWeightClass.Heavy:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsHeavy);
                }
            case UnitWeightClass.Assault:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsAssault);
                }
            default:
                {
                    return GetRandomSpawnDefinitionByWeight<MechSpawnDefinition>(MechsLight);
                }
        }
    }

    public GroundVehicleSpawnDefinition GetRandomConvoyVehicleSpawnDefinition()
    {
        return GetRandomSpawnDefinitionByWeight<GroundVehicleSpawnDefinition>(ConvoyVehicles);
    }

    public GameObject GetRandomBaseFoundation(BaseSize baseSize)
    {
        switch (baseSize)
        {
            case BaseSize.Small:
                {
                    return RandomBaseFoundationSmall;
                }
            case BaseSize.Medium:
                {
                    return RandomBaseFoundationMedium;
                }
            case BaseSize.Large:
                {
                    return RandomBaseFoundationLarge;
                }
            case BaseSize.Huge:
                {
                    return RandomBaseFoundationHuge;
                }
            default:
                {
                    return RandomBaseFoundationSmall;
                }
        }
    }

    public BuildingData GetRandomPrimaryBuilding()
    {
        BuildingSpawnDefinition buildingSpawnDefinition = GetRandomSpawnDefinitionByWeight<BuildingSpawnDefinition>(BasePrimaryBuildings);

        if (buildingSpawnDefinition != null)
        {
            return BuildingData.CreateBuildingData(buildingSpawnDefinition);
        }
        else
        {
            Debug.LogError("Error: No primary building spawn definition");
        }

        return null;
    }

    public List<UnitData> GetRandomOptionalBuildings(int count)
    {
        List<UnitData> buildingDatas = new List<UnitData>();

        if (BaseOptionalBuildings.Length > 0)
        {
            for (int i = 0; i < count; i++)
            {
                BuildingSpawnDefinition buildingSpawnDefinition = GetRandomSpawnDefinitionByWeight<BuildingSpawnDefinition>(BaseOptionalBuildings);

                if (buildingSpawnDefinition != null)
                {
                    BuildingData buildingData = BuildingData.CreateBuildingData(BaseOptionalBuildings[Random.Range(0, BaseOptionalBuildings.Length)]);

                    if (buildingData != null)
                    {
                        buildingDatas.Add(buildingData);
                    }
                }
            }
        }

        return buildingDatas;
    }

    public BuildingData GetTurretControlTower()
    {
        return BuildingData.CreateBuildingData(TurretControlTower);
    }

    public BuildingData GetTurretGenerator()
    {
        return BuildingData.CreateBuildingData(TurretGenerator);
    }

    T GetRandomSpawnDefinitionByWeight<T>(SpawnDefinition[] spawnDefinitions) where T : SpawnDefinition
    {
        if (spawnDefinitions.Length > 0)
        {
            float[] weights = new float[spawnDefinitions.Length];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = spawnDefinitions[i].RandomWeight;
            }

            return spawnDefinitions[StaticHelper.GetRandomIndexByWeight(weights)] as T;
        }

        return null;
    }
}