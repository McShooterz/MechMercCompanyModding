using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class MechChassisDefinition : Definition
{
    public string DisplayName = "";
    public string Description = "";

    public int MarketValue = 0;
    public int MarketRoleMin = 1;
    public int MarketRoleMax = 3;
    public float MarketRoleChance = 0.5f;
    public int Maintenance = 0;

    public string DefaultDesign = "";

    public string MechPrefab = "";
    public string CockpitPrefab = "";

    public UnitClass UnitClass;
    public int Tonnage;

    public float DisplaySpeedForwardMulti;
    public float DisplaySpeedReverseMulti;

    public float SpeedForwardMulti;
    public float SpeedReverseMulti;
    public float AccelerationForwardMulti;
    public float AccelerationReverseMulti;
    public float DeaccelerationMulti;
    public float SpeedTurnMulti;
    public float SpeedTorsoTwistMulti;
    public float SpeedTorsoPitchMulti;

    public float TorsoTwistMax;
    public float TorsoPitchMax;
    public float TorsoPitchMin;

    public float CameraShakeModifier = 1.0f;
    public float RecoilModifier = 1.0f;

    public int HeadArmorLimit;
    public int TorsoCenterArmorLimit;
    public int TorsoLeftArmorLimit;
    public int TorsoRightArmorLimit;
    public int LegLeftArmorLimit;
    public int LegRightArmorLimit;
    public int ArmLeftArmorLimit;
    public int ArmRightArmorLimit;

    public float HeadInternal;
    public float TorsoCenterInternal;
    public float TorsoLeftInternal;
    public float TorsoRightInternal;
    public float LegLeftInternal;
    public float LegRightInternal;
    public float ArmLeftInternal;
    public float ArmRightInternal;

    public float SlopeThreshold = 10.0f;
    public float SlopeLimit = 30.0f;

    public SlotGroup[] HeadSlotGroups = new SlotGroup[0];
    public SlotGroup[] TorsoCenterSlotGroups = new SlotGroup[0];
    public SlotGroup[] TorsoLeftSlotGroups = new SlotGroup[0];
    public SlotGroup[] TorsoRightSlotGroups = new SlotGroup[0];
    public SlotGroup[] ArmLeftSlotGroups = new SlotGroup[0];
    public SlotGroup[] ArmRightSlotGroups = new SlotGroup[0];
    public SlotGroup[] LegLeftSlotGroups = new SlotGroup[0];
    public SlotGroup[] LegRightSlotGroups = new SlotGroup[0];

    public string[] FootStepClips = new string[0];

    public int MaxArmor { get => HeadArmorLimit + TorsoCenterArmorLimit + TorsoLeftArmorLimit + TorsoRightArmorLimit + LegLeftArmorLimit + LegRightArmorLimit + ArmLeftArmorLimit + ArmRightArmorLimit; }

    public float InternalsTotal { get => HeadInternal + TorsoCenterInternal + TorsoLeftInternal + TorsoRightInternal + LegLeftInternal + LegRightInternal + ArmLeftInternal + ArmRightInternal; }

    public float ValuePerInternal { get => MarketValue / InternalsTotal; }

    public SlotGroup[] AllSlotGroups
    {
        get
        {
            List<SlotGroup> allSlotGroups = new List<SlotGroup>();
            allSlotGroups.AddRange(HeadSlotGroups);
            allSlotGroups.AddRange(TorsoCenterSlotGroups);
            allSlotGroups.AddRange(TorsoLeftSlotGroups);
            allSlotGroups.AddRange(TorsoRightSlotGroups);
            allSlotGroups.AddRange(ArmLeftSlotGroups);
            allSlotGroups.AddRange(ArmRightSlotGroups);
            allSlotGroups.AddRange(LegLeftSlotGroups);
            allSlotGroups.AddRange(LegRightSlotGroups);

            return allSlotGroups.ToArray();
        }
    }

    public string UnitClassDisplay { get => StaticHelper.GetUnitClassName(UnitClass); }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }

    public GameObject GetMechPrefab()
    {
        return ResourceManager.Instance.GetMechPrefab(MechPrefab);
    }

    public GameObject GetCockpitPrefab()
    {
        return ResourceManager.Instance.GetCockpitPrefab(CockpitPrefab);
    }

    public MechDesign GetDefaultDesign()
    {
        MechDesign defaultMechDesign = ResourceManager.Instance.GetMechDesign(Key, DefaultDesign);

        if (defaultMechDesign != null)
        {
            return defaultMechDesign;
        }

        MechDesign[] designs = ResourceManager.Instance.GetMechDesignList(Key);

        if (designs.Length > 0)
        {
            return designs[0];
        }

        return null;
    }

    public float GetDisplaySpeedForward(float power)
    {
        return power / Tonnage * SpeedForwardMulti * DisplaySpeedForwardMulti;
    }

    public float GetDisplaySpeedReverse(float power)
    {
        return power / Tonnage * SpeedReverseMulti * DisplaySpeedReverseMulti;
    }

    public float GetSpeedTurn(float power)
    {
        return power / Tonnage * SpeedTurnMulti;
    }

    public float GetSpeedTorsoTwist(float power)
    {
        return power / Tonnage * SpeedTorsoTwistMulti;
    }

    public void SetPowerSettings(float power, ref float forwardSpeed, ref float reverseSpeed, ref float accelForward, ref float accelReverse, ref float deaccel, ref float turnSpeed, ref float torsoTwist, ref float torsoPitch)
    {
        float powerRatio = power / Tonnage;

        forwardSpeed = powerRatio * SpeedForwardMulti;
        reverseSpeed = powerRatio * SpeedReverseMulti;
        accelForward = powerRatio * AccelerationForwardMulti;
        accelReverse = powerRatio * AccelerationReverseMulti;
        deaccel = powerRatio * DeaccelerationMulti;
        turnSpeed = powerRatio * SpeedTurnMulti;
        torsoTwist = powerRatio * SpeedTorsoTwistMulti;
        torsoPitch = powerRatio * SpeedTorsoPitchMulti;
    }

    public AudioClip[] GetFootStepAudioClips()
    {
        List<AudioClip> audioClips = new List<AudioClip>();
        AudioClip audioClip;

        foreach (string audioClipName in FootStepClips)
        {
            audioClip = ResourceManager.Instance.GetAudioClip(audioClipName);

            if (audioClip != null)
            {
                audioClips.Add(audioClip);
            }
        }

        return audioClips.ToArray();
    }

    public List<Texture2D> GetSkins()
    {
        return ResourceManager.Instance.GetUniqueMechSkinList(Key);
    }

    public bool CanStepOn(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.GroundVehicleUltraLight:
                {
                    return true;
                }
            case UnitClass.GroundVehicleLight:
                {
                    return true;
                }
            case UnitClass.GroundVehicleMedium:
                {
                    if (UnitClass == UnitClass.MechLight || UnitClass == UnitClass.MechMedium || UnitClass == UnitClass.MechHeavy || UnitClass == UnitClass.MechAssault)
                        return true;
                         
                    break;
                }
            case UnitClass.GroundVehicleHeavy:
                {
                    if (UnitClass == UnitClass.MechMedium || UnitClass == UnitClass.MechHeavy || UnitClass == UnitClass.MechAssault)
                        return true;

                    break;
                }
            case UnitClass.GroundVehicleAssault:
                {
                    if (UnitClass == UnitClass.MechHeavy || UnitClass == UnitClass.MechAssault)
                        return true;

                    break;
                }
        }

        return false;
    }

    public bool IsJumpCapable()
    {
        foreach(SlotGroup slotGroup in HeadSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in TorsoCenterSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in TorsoLeftSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in TorsoRightSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in ArmLeftSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in ArmRightSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in LegLeftSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        foreach (SlotGroup slotGroup in LegRightSlotGroups)
        {
            if (slotGroup.SlotType == SlotType.JumpJet)
            {
                return true;
            }
        }

        return false;
    }

    public int GetRandomMarketCount()
    {
        int count = 0;
        int roles = Random.Range(MarketRoleMin, MarketRoleMax);

        for (int i = 0; i < roles; i++)
        {
            if (Random.Range(0f, 1f) < MarketRoleChance)
            {
                count++;
            }
        }

        return count;
    }

    public string GetDisplayInformation()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine(GetDisplayName().ToUpper() + " - " + UnitClassDisplay.ToUpper());
        stringBuilder.AppendLine("Tonnage: " + Tonnage.ToString("0.#"));
        stringBuilder.AppendLine("Max Armor: " + MaxArmor.ToString("0.#"));
        stringBuilder.AppendLine("Internals: " + InternalsTotal.ToString("0.#"));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Maintenance: " + StaticHelper.FormatMoney(Maintenance));
        stringBuilder.AppendLine();

        Dictionary<string, int> slots = new Dictionary<string, int>();
        string key;

        foreach (SlotGroup slotGroup in AllSlotGroups)
        {
            switch (slotGroup.SlotType)
            {
                case SlotType.Reactor:
                    {
                        stringBuilder.AppendLine("Reactor Slot Size " + slotGroup.SlotCount);

                        break;
                    }
                case SlotType.Electronics:
                    {
                        key = "Electronics Slot Size " + slotGroup.SlotCount;

                        if (slots.ContainsKey(key))
                        {
                            slots[key] += 1;
                        }
                        else
                        {
                            slots.Add(key, 1);
                        }

                        break;
                    }
                case SlotType.JumpJet:
                    {
                        key = "Jump Jet Size " + slotGroup.SlotCount;

                        if (slots.ContainsKey(key))
                        {
                            slots[key] += 1;
                        }
                        else
                        {
                            slots.Add(key, 1);
                        }

                        break;
                    }
                case SlotType.WeaponBallistic:
                    {
                        key = "Ballistic Weapon Slot Size " + slotGroup.SlotCount;

                        if (slots.ContainsKey(key))
                        {
                            slots[key] += 1;
                        }
                        else
                        {
                            slots.Add(key, 1);
                        }

                        break;
                    }
                case SlotType.WeaponEnergy:
                    {
                        key = "Energy Weapon Slot Size " + slotGroup.SlotCount;

                        if (slots.ContainsKey(key))
                        {
                            slots[key] += 1;
                        }
                        else
                        {
                            slots.Add(key, 1);
                        }

                        break;
                    }
                case SlotType.WeaponMissile:
                    {
                        key = "Missile Weapon Slot Size " + slotGroup.SlotCount;

                        if (slots.ContainsKey(key))
                        {
                            slots[key] += 1;
                        }
                        else
                        {
                            slots.Add(key, 1);
                        }

                        break;
                    }
                case SlotType.MissileDefense:
                    {
                        key = "Missile Defense Slot Size " + slotGroup.SlotCount;

                        if (slots.ContainsKey(key))
                        {
                            slots[key] += 1;
                        }
                        else
                        {
                            slots.Add(key, 1);
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        IOrderedEnumerable<KeyValuePair<string, int>> orderedSlots = from pair in slots orderby pair.Key ascending select pair;

        foreach (KeyValuePair<string, int> keyValuePair in orderedSlots)
        {
            if (keyValuePair.Value > 1)
            {
                stringBuilder.AppendLine(keyValuePair.Key + " - x" + keyValuePair.Value);
            }
            else
            {
                stringBuilder.AppendLine(keyValuePair.Key);
            }
        }

        return stringBuilder.ToString();
    }
}
