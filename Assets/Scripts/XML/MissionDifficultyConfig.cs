using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionDifficultyConfig
{
    #region Variables
    public float BasePointsMechUltraLight = 10;
    public float BasePointsMechLight = 20;
    public float BasePointsMechMedium = 40;
    public float BasePointsMechHeavy = 60;
    public float BasePointsMechAssault = 80;

    public float BasePointsGroundVehicleUltraLight = 2.5f;
    public float BasePointsGroundVehicleLight = 5;
    public float BasePointsGroundVehicleMedium = 10;
    public float BasePointsGroundVehicleHeavy = 20;
    public float BasePointsGroundVehicleAssault = 30;

    public float SkillMultiplierRookie = 0.5f;
    public float SkillMultiplierRegular = 1.0f;
    public float SkillMultiplierVeteran = 1.25f;
    public float SkillMultiplierElite = 1.5f;
    public float SkillMultiplierLegendary = 2.0f;

    public int MaxGenerationTries = 40;

    // Mission Pay
    public MissionObjectivePay MissionObjectivePay = new MissionObjectivePay();

    public MissionDifficultyTier[] MissionDifficultyTiers = new MissionDifficultyTier[0];
    #endregion

    public List<UnitData> GetRandomForce(int tierIndex, MissionType missionType, FactionDefinition factionDefinition, bool secondary)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tierIndex);
        List<UnitData> randomForce;

        if (missionDifficultyTier != null)
        {
            randomForce = missionDifficultyTier.GetRandomForce(this, missionType, factionDefinition, secondary);

            if (randomForce.Count == 0)
            {
                Debug.LogError("Error: Failed to generate random units");
            }

            return randomForce;
        }

        return new List<UnitData>();
    }

    public List<UnitData> GetRandomTurrets(int tierIndex, FactionDefinition factionDefinition)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tierIndex);

        if (missionDifficultyTier != null)
        {
            return missionDifficultyTier.GetRandomTurrets(factionDefinition);
        }

        return new List<UnitData>();
    }

    public List<UnitData> GetRandomConvoy(int tierIndex, FactionDefinition factionDefinition)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tierIndex);

        if (missionDifficultyTier != null)
        {
            return missionDifficultyTier.GetRandomConvoyUnits(factionDefinition);
        }

        return new List<UnitData>();
    }

    public GameObject GetRandomBaseFoundation(int tierIndex, FactionDefinition factionDefinition)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tierIndex);

        if (missionDifficultyTier != null)
        {
            return factionDefinition.GetRandomBaseFoundation(missionDifficultyTier.BaseSize);
        }

        return null;
    }

    public float GetUnitStrength(UnitClass unitClass, BasicPilotSkillLevel basicPilotSkillLevel)
    {
        float skillMultiplier = 1f;

        switch (basicPilotSkillLevel)
        {
            case BasicPilotSkillLevel.Rookie:
                {
                    skillMultiplier = SkillMultiplierRookie;
                    break;
                }
            case BasicPilotSkillLevel.Regular:
                {
                    skillMultiplier = SkillMultiplierRegular;
                    break;
                }
            case BasicPilotSkillLevel.Veteran:
                {
                    skillMultiplier = SkillMultiplierVeteran;
                    break;
                }
            case BasicPilotSkillLevel.Elite:
                {
                    skillMultiplier = SkillMultiplierElite;
                    break;
                }
            case BasicPilotSkillLevel.Legendary:
                {
                    skillMultiplier = SkillMultiplierLegendary;
                    break;
                }
        }

        switch (unitClass)
        {
            case UnitClass.MechUltraLight:
                {
                    return BasePointsMechUltraLight * skillMultiplier;
                }
            case UnitClass.MechLight:
                {
                    return BasePointsMechLight * skillMultiplier;
                }
            case UnitClass.MechMedium:
                {
                    return BasePointsMechMedium * skillMultiplier;
                }
            case UnitClass.MechHeavy:
                {
                    return BasePointsMechHeavy * skillMultiplier;
                }
            case UnitClass.MechAssault:
                {
                    return BasePointsMechAssault * skillMultiplier;
                }
            case UnitClass.GroundVehicleUltraLight:
                {
                    return BasePointsGroundVehicleUltraLight * skillMultiplier;
                }
            case UnitClass.GroundVehicleLight:
                {
                    return BasePointsGroundVehicleLight * skillMultiplier;
                }
            case UnitClass.GroundVehicleMedium:
                {
                    return BasePointsGroundVehicleMedium * skillMultiplier;
                }
            case UnitClass.GroundVehicleHeavy:
                {
                    return BasePointsGroundVehicleHeavy * skillMultiplier;
                }
            case UnitClass.GroundVehicleAssault:
                {
                    return BasePointsGroundVehicleAssault * skillMultiplier;
                }
            default:
                {
                    return 0f;
                }
        }
    }

    MissionDifficultyTier GetMissionDifficultyTier(int tier)
    {
        if (MissionDifficultyTiers.Length > 0)
        {
            tier = Mathf.Clamp(tier, 0, MissionDifficultyTiers.Length - 1);

            return MissionDifficultyTiers[tier];
        }

        return null;
    }

    public int GetPay_Assassination_AssassinateTarget(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.AssassinationAssassinateTarget * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.AssassinationAssassinateTarget;
    }

    public int GetPay_Assassination_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.AssassinationDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.AssassinationDestroyAllEnemies;
    }

    public int GetPay_Battle_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BattleDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BattleDestroyAllEnemies;
    }

    public int GetPay_BattleSupport_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BattleSupportDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BattleSupportDestroyAllEnemies;
    }

    public int GetPay_BattleSupport_ProtectAllies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BattleSupportProtectAllies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BattleSupportProtectAllies;
    }

    public int GetPay_ConvoyDestroy_ConvoyDestroyed(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.ConvoyDestroyConvoyDestroyed * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.ConvoyDestroyConvoyDestroyed;
    }

    public int GetPay_ConvoyDestroy_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.ConvoyDestroyDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.ConvoyDestroyDestroyAllEnemies;
    }

    public int GetPay_SearchAndDestroy_DestroyEnemiesAtNavPoint(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.SearchAndDestroyDestroyEnemiesAtNavPoint * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.SearchAndDestroyDestroyEnemiesAtNavPoint;
    }

    public int GetPay_Recon_SurveyPosition(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.ReconSurveyPosition * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.ReconSurveyPosition;
    }

    public int GetPay_Recon_AvoidDetection(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.ReconAvoidDetection * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.ReconAvoidDetection;
    }

    public int GetPay_Recon_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.ReconDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.ReconDestroyAllEnemies;
    }

    public int GetPay_BaseDestroy_PrimaryBuilding(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDestroyPrimaryBuilding * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDestroyPrimaryBuilding;
    }

    public int GetPay_BaseDestroy_OptionalBuildings(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDestroyOptionalBuildings * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDestroyOptionalBuildings;
    }

    public int GetPay_BaseDestroy_Turrets(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDestroyTurrets * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDestroyTurrets;
    }

    public int GetPay_BaseDestroy_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDestroyDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDestroyDestroyAllEnemies;
    }

    public int GetPay_BaseCapture_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseCaptureDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseCaptureDestroyAllEnemies;
    }

    public int GetPay_BaseCapture_ProtectPrimaryBuilding(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseCapturePrimaryBuilding * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseCapturePrimaryBuilding;
    }

    public int GetPay_BaseCapture_ProtectAllBuildings(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseCaptureOptionalBuildings * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseCaptureOptionalBuildings;
    }

    public int GetPay_BaseDefend_DestroyAllEnemies(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDefendDestroyAllEnemies * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDefendDestroyAllEnemies;
    }

    public int GetPay_BaseDefend_ProtectPrimaryBuilding(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDefendPrimaryBuilding * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDefendPrimaryBuilding;
    }

    public int GetPay_BaseDefend_ProtectOptionalBuildings(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDefendOptionalBuildings * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDefendOptionalBuildings;
    }

    public int GetPay_BaseDefend_ProtectTurrets(int tier)
    {
        MissionDifficultyTier missionDifficultyTier = GetMissionDifficultyTier(tier);

        if (missionDifficultyTier != null)
        {
            return Mathf.CeilToInt(MissionObjectivePay.BaseDefendTurrets * missionDifficultyTier.MissionPayMultiplier);
        }

        return MissionObjectivePay.BaseDefendTurrets;
    }

    public int GetPayPotential(MissionType missionType, int tier)
    {
        switch (missionType)
        {
            case MissionType.Assassination:
                {
                    return GetPayPotential_Assassination(tier);
                }
            case MissionType.Battle:
                {
                    return GetPayPotential_Battle(tier);
                }
            case MissionType.BattleSupport:
                {
                    return GetPayPotential_BattleSupport(tier);
                }
            case MissionType.ConvoyDestroy:
                {
                    return GetPayPotential_ConvoyDestroy(tier);
                }
            case MissionType.SearchAndDestroy:
                {
                    return GetPayPotential_SearchAndDestroy(tier);
                }
            case MissionType.Recon:
                {
                    return GetPayPotential_Recon(tier);
                }
            case MissionType.BaseDestroy:
                {
                    return GetPayPotential_BaseDestroy(tier);
                }
            case MissionType.BaseCapture:
                {
                    return GetPayPotential_BaseCapture(tier);
                }
            case MissionType.BaseDefend:
                {
                    return GetPayPotential_BaseDefend(tier);
                }
            default:
                {
                    return GetPayPotential_Battle(tier);
                }
        }
    }

    public int GetPayPotential_Assassination(int tier)
    {
        int sum = 0;

        sum += GetPay_Assassination_AssassinateTarget(tier);
        sum += GetPay_Assassination_DestroyAllEnemies(tier);

        return sum;
    }

    public int GetPayPotential_Battle(int tier)
    {
        int sum = 0;

        sum += GetPay_Battle_DestroyAllEnemies(tier);

        return sum;
    }

    public int GetPayPotential_BattleSupport(int tier)
    {
        int sum = 0;

        sum += GetPay_BattleSupport_DestroyAllEnemies(tier);
        sum += GetPay_BattleSupport_ProtectAllies(tier);

        return sum;
    }

    public int GetPayPotential_ConvoyDestroy(int tier)
    {
        int sum = 0;

        sum += GetPay_ConvoyDestroy_ConvoyDestroyed(tier);
        sum += GetPay_ConvoyDestroy_DestroyAllEnemies(tier);

        return sum;
    }

    public int GetPayPotential_SearchAndDestroy(int tier)
    {
        int sum = 0;

        sum += GetPay_SearchAndDestroy_DestroyEnemiesAtNavPoint(tier);

        return sum;
    }

    public int GetPayPotential_Recon(int tier)
    {
        int sum = 0;

        sum += GetPay_Recon_SurveyPosition(tier);
        sum += GetPay_Recon_AvoidDetection(tier);
        sum += GetPay_Recon_DestroyAllEnemies(tier);

        return sum;
    }

    public int GetPayPotential_BaseDestroy(int tier)
    {
        int sum = 0;

        sum += GetPay_BaseDestroy_PrimaryBuilding(tier);
        sum += GetPay_BaseDestroy_OptionalBuildings(tier);
        sum += GetPay_BaseDestroy_Turrets(tier);
        sum += GetPay_BaseDestroy_DestroyAllEnemies(tier);

        return sum;
    }

    public int GetPayPotential_BaseCapture(int tier)
    {
        int sum = 0;

        sum += GetPay_BaseCapture_DestroyAllEnemies(tier);
        sum += GetPay_BaseCapture_ProtectPrimaryBuilding(tier);
        sum += GetPay_BaseCapture_ProtectAllBuildings(tier);

        return sum;
    }

    public int GetPayPotential_BaseDefend(int tier)
    {
        int sum = 0;

        sum += GetPay_BaseDefend_DestroyAllEnemies(tier);
        sum += GetPay_BaseDefend_ProtectPrimaryBuilding(tier);
        sum += GetPay_BaseDefend_ProtectOptionalBuildings(tier);
        sum += GetPay_BaseDefend_ProtectTurrets(tier);

        return sum;
    }
}