using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionDifficultyTier
{
    #region Variables
    //public MissionObjectivePay MissionObjectivePay;

    public float MissionPayMultiplier = 1.0f;
    public int ContractPayMultiplier = 200000;
    public int KillBountyPay = 10000;
    public int TravelCoverage = 100000;
    public int Reputation = 2;
    public int ReputationGrowth = 1;
    public int Infamy = 2;
    public int InfamyGrowth = 1;

    public float EnemyStrengthMinBattle = 20;
    public float EnemyStrengthMaxBattle = 30;

    public float EnemyStrengthMinAssassination = 20;
    public float EnemyStrengthMaxAssassination = 30;

    public float EnemyStrengthMinSearchAndDestroy = 20;
    public float EnemyStrengthMaxSearchAndDestroy = 30;

    public float EnemyStrengthMinConvoyDestroy = 20;
    public float EnemyStrengthMaxConvoyDestroy = 30;

    public float EnemyStrengthMinBattleSupport = 20;
    public float EnemyStrengthMaxBattleSupport = 30;

    public float AllyStrengthMinBattleSupport = 20;
    public float AllyStrengthMaxBattleSupport = 30;

    public float EnemyStrengthMinRecon = 20;
    public float EnemyStrengthMaxRecon = 30;

    public float EnemyStrengthMinBaseDestroy = 20;
    public float EnemyStrengthMaxBaseDestroy = 30;

    public float EnemyStrengthMinBaseCapture = 20;
    public float EnemyStrengthMaxBaseCapture = 30;

    public float EnemyStrengthMinBaseDefend = 20;
    public float EnemyStrengthMaxBaseDefend = 30;

    public int ConvoyCountMin = 3;
    public int ConvoyCountMax = 4;

    public UnitClass BaseTurretClass = UnitClass.TurretLight;
    public int BaseTurretCountMin = 1;
    public int BaseTurretCountMax = 3;

    public float SkillRandomWeightRookie = 0.0f;
    public float SkillRandomWeightRegular = 0.0f;
    public float SkillRandomWeightVeteran = 0.0f;
    public float SkillRandomWeightElite = 0.0f;
    public float SkillRandomWeightLegendary = 0.0f;

    public float MechRandomWeightUltraLight = 0.0f;
    public float MechRandomWeightLight = 0.0f;
    public float MechRandomWeightMedium = 0.0f;
    public float MechRandomWeightHeavy = 0.0f;
    public float MechRandomWeightAssault = 0.0f;

    public float VehicleRandomWeightUltraLight = 0.0f;
    public float VehicleRandomWeightLight = 0.0f;
    public float VehicleRandomWeightMedium = 0.0f;
    public float VehicleRandomWeightHeavy = 0.0f;
    public float VehicleRandomWeightAssault = 0.0f;

    public BasicPilotSkillLevel SkillAssassinationTarget = BasicPilotSkillLevel.Veteran;
    public UnitWeightClass AssassinationMechClass = UnitWeightClass.Light;

    public BaseSize BaseSize;

    BasicPilotSkillLevel[] pilotSkillLevels = new BasicPilotSkillLevel[]
    {
        BasicPilotSkillLevel.Rookie,
        BasicPilotSkillLevel.Regular,
        BasicPilotSkillLevel.Veteran,
        BasicPilotSkillLevel.Elite,
        BasicPilotSkillLevel.Legendary,
    };
    #endregion

    public float[] PilotSkillLevelWeights
    {
        get
        {
            return new float[] 
            {
                SkillRandomWeightRookie,
                SkillRandomWeightRegular,
                SkillRandomWeightVeteran,
                SkillRandomWeightElite,
                SkillRandomWeightLegendary
            };
        }
    }

    public List<UnitData> GetRandomForce(MissionDifficultyConfig missionDifficultyConfig, MissionType missionType, FactionDefinition factionDefinition, bool secondary)
    {
        switch (missionType)
        {
            case MissionType.Battle:
                {
                    return GetRandomForceBattle(missionDifficultyConfig, factionDefinition);
                }
            case MissionType.Assassination:
                {
                    return GetRandomForceAssassination(missionDifficultyConfig, factionDefinition);
                }
            case MissionType.ConvoyDestroy:
                {
                    return GetRandomForceConvoyDestroy(missionDifficultyConfig, factionDefinition);
                }

            case MissionType.SearchAndDestroy:
                {
                    return GetRandomForceSearchAndDestroy(missionDifficultyConfig, factionDefinition);
                }
            case MissionType.BattleSupport:
                {
                    if (secondary)
                    {
                        return GetRandomAllyForceBattleSupport(missionDifficultyConfig, factionDefinition);
                    }
                    else
                    {
                        return GetRandomEnemyForceBattleSupport(missionDifficultyConfig, factionDefinition);
                    }
                }
            case MissionType.Recon:
                {
                    return GetRandomForceRecon(missionDifficultyConfig, factionDefinition);
                }
            case MissionType.BaseDestroy:
                {
                    return GetRandomForceBaseDestroy(missionDifficultyConfig, factionDefinition);
                }
            case MissionType.BaseCapture:
                {
                    return GetRandomForceBaseCapture(missionDifficultyConfig, factionDefinition);
                }
            case MissionType.BaseDefend:
                {
                    return GetRandomForceBaseDefend(missionDifficultyConfig, factionDefinition);
                }
            default:
                {
                    return GetRandomForceBattle(missionDifficultyConfig, factionDefinition);
                }
        }
    }

    public List<UnitData> GetRandomTurrets(FactionDefinition factionDefinition)
    {
        List<UnitData> turretUnitDatas = new List<UnitData>();

        int count = Random.Range(BaseTurretCountMin, BaseTurretCountMax + 1);

        for (int i = 0; i < count; i++)
        {
            SpawnDefinition spawnDefinition = factionDefinition.GetRandomSpawnDefinition(BaseTurretClass);

            if (spawnDefinition != null)
            {
                TurretDefinition turretDefinition = (spawnDefinition as TurretSpawnDefinition).GetTurretDefinition();

                if (turretDefinition != null)
                {
                    TurretUnitData turretUnitData = new TurretUnitData();
                    turretUnitData.BuildFromDefinition(turretDefinition);
                    turretUnitDatas.Add(turretUnitData);
                }
            }
        }

        if (turretUnitDatas.Count == 0)
        {
            Debug.LogError("Error: Failed to get random turrets");
        }

        return turretUnitDatas;
    }

    List<UnitData> GetRandomForceBattle(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, EnemyStrengthMinBattle, EnemyStrengthMaxBattle);
    }

    List<UnitData> GetRandomForceAssassination(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        List<UnitData> unitDatas = new List<UnitData>();

        MechSpawnDefinition mechSpawnDefinition = factionDefinition.GetRandomMechSpawnDefinition(AssassinationMechClass);

        if (mechSpawnDefinition != null)
        {
            MechData assassinationTarget = CreateMechData(mechSpawnDefinition, SkillAssassinationTarget, factionDefinition.RandomAssassinationTargetName);

            if (assassinationTarget != null)
            {
                unitDatas.Add(assassinationTarget);
            }
        }

        unitDatas.AddRange(GetUnitDatas(missionDifficultyConfig, 4, factionDefinition, EnemyStrengthMinAssassination, EnemyStrengthMaxAssassination));

        return unitDatas;
    }

    List<UnitData> GetRandomForceSearchAndDestroy(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 4, factionDefinition, EnemyStrengthMinSearchAndDestroy, EnemyStrengthMaxSearchAndDestroy);
    }

    List<UnitData> GetRandomForceConvoyDestroy(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 4, factionDefinition, EnemyStrengthMinConvoyDestroy, EnemyStrengthMaxConvoyDestroy);
    }

    List<UnitData> GetRandomEnemyForceBattleSupport(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, EnemyStrengthMinBattleSupport, EnemyStrengthMaxBattleSupport);
    }

    List<UnitData> GetRandomAllyForceBattleSupport(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, AllyStrengthMinBattleSupport, AllyStrengthMaxBattleSupport);
    }

    List<UnitData> GetRandomForceRecon(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, EnemyStrengthMinRecon, EnemyStrengthMaxRecon);
    }

    List<UnitData> GetRandomForceBaseDestroy(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, EnemyStrengthMinBaseDestroy, EnemyStrengthMaxBaseDestroy);
    }

    List<UnitData> GetRandomForceBaseCapture(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, EnemyStrengthMinBaseCapture, EnemyStrengthMaxBaseCapture);
    }

    List<UnitData> GetRandomForceBaseDefend(MissionDifficultyConfig missionDifficultyConfig, FactionDefinition factionDefinition)
    {
        return GetUnitDatas(missionDifficultyConfig, 2, factionDefinition, EnemyStrengthMinBaseDefend, EnemyStrengthMaxBaseDefend);
    }

    public List<UnitData> GetRandomConvoyUnits(FactionDefinition factionDefinition)
    {
        List<UnitData> unitDatas = new List<UnitData>();

        for (int i = 0; i < Random.Range(ConvoyCountMin, ConvoyCountMax + 1); i++)
        {
            GroundVehicleSpawnDefinition groundVehicleSpawnDefinition = factionDefinition.GetRandomConvoyVehicleSpawnDefinition();

            if (groundVehicleSpawnDefinition != null)
            {
                GroundVehicleDefinition groundVehicleDefinition = groundVehicleSpawnDefinition.GetGroundVehicleDefinition();

                if (groundVehicleDefinition != null)
                {
                    GroundVehicleData groundVehicleData = new GroundVehicleData();
                    groundVehicleData.BuildFromDefinition(groundVehicleSpawnDefinition.GetGroundVehicleDefinition());
                    unitDatas.Add(groundVehicleData);
                }
            }
        }

        return unitDatas;
    }

    float[] GetUnitWeights(FactionDefinition factionDefinition)
    {
        float[] unitWeights = new float[10];

        if (factionDefinition.HasMechsUltraLight)
            unitWeights[0] = MechRandomWeightUltraLight;

        if (factionDefinition.HasMechsLight)
            unitWeights[1] = MechRandomWeightLight;

        if (factionDefinition.HasMechsMedium)
            unitWeights[2] = MechRandomWeightMedium;

        if (factionDefinition.HasMechsHeavy)
            unitWeights[3] = MechRandomWeightHeavy;

        if (factionDefinition.HasMechsAssault)
            unitWeights[4] = MechRandomWeightAssault;

        if (factionDefinition.HasGroundVehiclesUltraLight)
            unitWeights[5] = VehicleRandomWeightUltraLight;

        if (factionDefinition.HasGroundVehiclesLight)
            unitWeights[6] = VehicleRandomWeightLight;

        if (factionDefinition.HasGroundVehiclesMedium)
            unitWeights[7] = VehicleRandomWeightMedium;

        if (factionDefinition.HasGroundVehiclesHeavy)
            unitWeights[8] = VehicleRandomWeightHeavy;

        if (factionDefinition.HasGroundVehiclesAssault)
            unitWeights[9] = VehicleRandomWeightAssault;

        return unitWeights;
    }

    List<UnitData> GetUnitDatas(MissionDifficultyConfig missionDifficultyConfig, int unitMinCount, FactionDefinition factionDefinition, float strengthMin, float strengthMax)
    {
        List<UnitData> unitDatas = new List<UnitData>();

        List <UnitClassSpawnEntry> unitClassSpawnEntries = GetUnitClassSpawnEntries(missionDifficultyConfig, GetUnitWeights(factionDefinition), unitMinCount, strengthMin, strengthMax);

        for (int i = 0; i < unitClassSpawnEntries.Count; i++)
        {
            UnitClassSpawnEntry unitClassSpawnEntry = unitClassSpawnEntries[i];

            SpawnDefinition spawnDefinition = factionDefinition.GetRandomSpawnDefinition(unitClassSpawnEntry.unitClass);

            if (spawnDefinition is MechSpawnDefinition)
            {
                MechSpawnDefinition mechSpawnDefinition = spawnDefinition as MechSpawnDefinition;
                MechData mechData = CreateMechData(mechSpawnDefinition, unitClassSpawnEntry.basicPilotSkillLevel, mechSpawnDefinition.MechDesign);

                if (mechData != null)
                {
                    unitDatas.Add(mechData);
                }
                else
                {
                    Debug.LogError("Error: Mech Spawn Definition Invalid: " + mechSpawnDefinition.MechChassis + " - " + mechSpawnDefinition.MechDesign);
                }
            }
            else if (spawnDefinition is GroundVehicleSpawnDefinition)
            {
                GroundVehicleData groundVehicleData = new GroundVehicleData();
                groundVehicleData.BuildFromDefinition((spawnDefinition as GroundVehicleSpawnDefinition).GetGroundVehicleDefinition());
                groundVehicleData.SetGunnerySkill(unitClassSpawnEntry.basicPilotSkillLevel);
                unitDatas.Add(groundVehicleData);
            }
        }

        return unitDatas;
    }

    MechData CreateMechData(MechSpawnDefinition mechSpawnDefinition, BasicPilotSkillLevel enemySkillLevel, string pilotName)
    {
        MechDesign mechDesign = mechSpawnDefinition.GetDesign();

        if (mechDesign != null)
        {
            MechData mechData = new MechData(mechDesign);
            mechData.mechPaintScheme = mechSpawnDefinition.GetMechPaintScheme();
            MechPilot mechPilot = new MechPilot(mechSpawnDefinition.MechDesign);
            mechPilot.SetBasicSkill(enemySkillLevel);
            mechPilot.displayName = pilotName;
            mechData.currentMechPilot = mechPilot;

            return mechData;
        }

        return null;
    }

    List<UnitClassSpawnEntry> GetUnitClassSpawnEntries(MissionDifficultyConfig missionDifficultyConfig, float[] unitRandomWeights, int minCount, float strengthMin, float strengthMax)
    {
        float[] pilotSkillLevelWeights = PilotSkillLevelWeights;
        List<UnitClassSpawnEntry> unitClassSpawnEntries = new List<UnitClassSpawnEntry>();
        float strength = 0;
        int maxTries = missionDifficultyConfig.MaxGenerationTries;

        while (unitClassSpawnEntries.Count < minCount || (maxTries > 0 && (strength < strengthMin || strength > strengthMax)))
        {
            maxTries--;

            if (unitClassSpawnEntries.Count < minCount || (unitClassSpawnEntries.Count < 20 && strength < strengthMax))
            {
                int unitClassIndex = StaticHelper.GetRandomIndexByWeight(unitRandomWeights);
                UnitClass unitClass = (UnitClass)unitClassIndex;
                BasicPilotSkillLevel pilotSkillLevel = pilotSkillLevels[StaticHelper.GetRandomIndexByWeight(pilotSkillLevelWeights)];
                strength += missionDifficultyConfig.GetUnitStrength(unitClass, pilotSkillLevel);
                unitClassSpawnEntries.Add(new UnitClassSpawnEntry(unitClass, pilotSkillLevel));
            }
            else
            {
                int index = Random.Range(0, unitClassSpawnEntries.Count);
                strength -= missionDifficultyConfig.GetUnitStrength(unitClassSpawnEntries[index].unitClass, unitClassSpawnEntries[index].basicPilotSkillLevel);
                unitClassSpawnEntries.RemoveAt(index);
            }
        }

        return unitClassSpawnEntries;
    }

    class UnitClassSpawnEntry
    {
        public UnitClass unitClass;

        public BasicPilotSkillLevel basicPilotSkillLevel;

        public UnitClassSpawnEntry(UnitClass unitClass, BasicPilotSkillLevel basicPilotSkillLevel)
        {
            this.unitClass = unitClass;
            this.basicPilotSkillLevel = basicPilotSkillLevel;
        }
    }
}
