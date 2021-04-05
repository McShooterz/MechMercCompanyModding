using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameConstants
{
    public int StartingCareerFunds = 3000000;

    public int StartingDay = 1;
    public int StartingMonth = 4;
    public int StartingYear = 2203;

    public string[] CareerStartingComponents;

    public float ShuttingDownTime = 2.0f;
    public float ShutDownMinTime = 1.5f;
    public float StartingUpTime = 2.0f;

    public float ArmorStandardWeight = 0.02f;
    public float ArmorCompositeWeight = 0.03f;
    public float ArmorThermalWeight = 0.03f;
    public float ArmorReactiveWeight = 0.03f;
    public float ArmorStealthWeight = 0.04f;

    public float ArmorDamageReductionComposite = 0.5f;
    public float ArmorDamageReductionReactive = 0.5f;
    public float ArmorDamageReductionThermal = 0.5f;

    public float HeatShutDownPercent = 0.8f;
    public float ShutDownCoolingFactor = 1.5f;
    public float HeatWarningPercent = 0.6f;
    public float HeatDamageMinorPercent = 0.7f;
    public float HeatDamageMajorPercent = 0.9f;
    public float HeatDamageMinor = 0.25f;
    public float HeatDamageMajor = 1.5f;
    public float HeatDamageExtreme = 20.0f;
    public float CoolantHeatReduction = 4000f;
    public float JumpJetHeatMultiplier = 180f;

    public float StealthArmorSignatureReductionHead = 20.0f;
    public float StealthArmorSignatureReductionTorsoCenter = 90.0f;
    public float StealthArmorSignatureReductionTorsoSide = 50.0f;
    public float StealthArmorSignatureReductionArm = 40.0f;
    public float StealthArmorSignatureReductionLeg = 55.0f;

    public Color SlotColorGeneral = Color.clear;
    public Color SlotColorWeaponBallistic = Color.clear;
    public Color SlotColorWeaponEnergy = Color.clear;  
    public Color SlotColorWeaponMissile = Color.clear;
    public Color SlotColorReactor = Color.clear;
    public Color SlotColorElectronics = Color.clear;
    public Color SlotColorMissileDefense = Color.clear;
    public Color SlotColorJumpJet = Color.clear;

    public Color ArmorColorHealthFull = Color.clear;
    public Color ArmorColorHealthMedium = Color.clear;
    public Color ArmorColorHealthLow = Color.clear;
    public Color ArmorColorHealthDestroyed = Color.clear;

    public Color InternalColorHealthFull = Color.clear;
    public Color InternalColorHealthMedium = Color.clear;
    public Color InternalColorHealthLow = Color.clear;
    public Color InternalColorHealthDestroyed = Color.clear;

    public Color WeaponGroupColorDefault = Color.clear;
    public Color WeaponGroupColorActive = Color.clear;

    public Color WeaponColorDefault = Color.clear;
    public Color WeaponColorOutOfRange = Color.clear;
    public Color WeaponColorDestroyed = Color.clear;

    public string ObjectiveColorActive = "ffffffff";
    public string ObjectiveColorCompleted = "00ff00ff";
    public string ObjectiveColorFailed = "ff0000ff";
    public string ObjectiveColorDisabled = "808080ff";

    public float KillExperienceMechUltraLight = 0.001f;
    public float KillExperienceMechLight = 0.0025f;
    public float KillExperienceMechMedium = 0.005f;
    public float KillExperienceMechHeavy = 0.009f;
    public float KillExperienceMechAssault = 0.014f;

    public float KillExperienceVehicleUltraLight = 0.0005f;
    public float KillExperienceVehicleLight = 0.00125f;
    public float KillExperienceVehicleMedium = 0.0025f;
    public float KillExperienceVehicleHeavy = 0.0045f;
    public float KillExperienceVehicleAssault = 0.007f;

    public float LockingOnValueMin = 0.25f;

    public string[] Hints = new string[0];

    public GameDate CareerStartingDate { get => new GameDate(StartingDay, StartingMonth, StartingYear); }

    public float GetArmorWeight(ArmorType armorTypes)
    {
        switch (armorTypes)
        {
            case ArmorType.standard:
                {
                    return ArmorStandardWeight;
                }
            case ArmorType.composite:
                {
                    return ArmorCompositeWeight;
                }
            case ArmorType.thermal:
                {
                    return ArmorThermalWeight;
                }
            case ArmorType.reactive:
                {
                    return ArmorReactiveWeight;
                }
            case ArmorType.stealth:
                {
                    return ArmorStealthWeight;
                }
            default:
                {
                    return ArmorStandardWeight;
                }
        }
    }

    public Color GetSlotColor(SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.General:
                {
                    return SlotColorGeneral;
                }
            case SlotType.WeaponBallistic:
                {
                    return SlotColorWeaponBallistic;
                }
            case SlotType.WeaponEnergy:
                {
                    return SlotColorWeaponEnergy;
                }
            case SlotType.WeaponMissile:
                {
                    return SlotColorWeaponMissile;
                }
            case SlotType.Reactor:
                {
                    return SlotColorReactor;
                }
            case SlotType.Electronics:
                {
                    return SlotColorElectronics;
                }
            case SlotType.MissileDefense:
                {
                    return SlotColorMissileDefense;
                }
            case SlotType.JumpJet:
                {
                    return SlotColorJumpJet;
                }
            default:
                {
                    return Color.clear;
                }
        }
    }

    public float GetArmorDamageReduction(ArmorType armorType, DamageType damageType)
    {
        switch (armorType)
        {
            case ArmorType.composite:
                {
                    if (damageType == DamageType.Ballistic)
                    {
                        return ArmorDamageReductionComposite;
                    }

                    return 0f;
                }
            case ArmorType.reactive:
                {
                    if (damageType == DamageType.Missile)
                    {
                        return ArmorDamageReductionReactive;
                    }

                    return 0f;
                }
            case ArmorType.thermal:
                {
                    if (damageType == DamageType.Energy)
                    {
                        return ArmorDamageReductionThermal;
                    }

                    return 0f;
                }
            default:
                {
                    return 0f;
                }
        }
    }

    public Color GetArmorColorHealthFullToMedium(float ratio)
    {
        return Color.Lerp(ArmorColorHealthMedium, ArmorColorHealthFull, ratio);
    }

    public Color GetArmorColorHealthMediumToLow(float ratio)
    {
        return Color.Lerp(ArmorColorHealthLow, ArmorColorHealthMedium, ratio);
    }

    public Color GetInternalColorHealthFullToMedium(float ratio)
    {
        return Color.Lerp(InternalColorHealthMedium, InternalColorHealthFull, ratio);
    }

    public Color GetInternalColorHealthMediumToLow(float ratio)
    {
        return Color.Lerp(InternalColorHealthLow, InternalColorHealthMedium, ratio);
    }

    public string GetRandomHint()
    {
        if (Hints.Length > 0)
        {
            return ResourceManager.Instance.GetLocalization(Hints[Random.Range(0, Hints.Length)]);
        }

        return "Not hints yet.";
    }

    public string GetObjectiveColorValue(ObjectiveState objectiveState)
    {
        switch (objectiveState)
        {
            case ObjectiveState.Active:
                {
                    return ResourceManager.Instance.GameConstants.ObjectiveColorActive;
                }
            case ObjectiveState.Completed:
                {
                    return ResourceManager.Instance.GameConstants.ObjectiveColorCompleted;
                }
            case ObjectiveState.Failed:
                {
                    return ResourceManager.Instance.GameConstants.ObjectiveColorFailed;
                }
            case ObjectiveState.Disabled:
                {
                    return ResourceManager.Instance.GameConstants.ObjectiveColorDisabled;
                }
            default:
                {
                    return "ffffffff";
                }
        }
    }

    public float GetKillExperience(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.MechUltraLight:
                {
                    return KillExperienceMechUltraLight;
                }
            case UnitClass.MechLight:
                {
                    return KillExperienceMechLight;
                }
            case UnitClass.MechMedium:
                {
                    return KillExperienceMechMedium;
                }
            case UnitClass.MechHeavy:
                {
                    return KillExperienceMechHeavy;
                }
            case UnitClass.MechAssault:
                {
                    return KillExperienceMechAssault;
                }
            case UnitClass.GroundVehicleUltraLight:
                {
                    return KillExperienceVehicleUltraLight;
                }
            case UnitClass.GroundVehicleLight:
                {
                    return KillExperienceVehicleLight;
                }
            case UnitClass.GroundVehicleMedium:
                {
                    return KillExperienceVehicleMedium;
                }
            case UnitClass.GroundVehicleHeavy:
                {
                    return KillExperienceVehicleHeavy;
                }
            case UnitClass.GroundVehicleAssault:
                {
                    return KillExperienceVehicleAssault;
                }
        }

        return 0.0f;
    }
}
