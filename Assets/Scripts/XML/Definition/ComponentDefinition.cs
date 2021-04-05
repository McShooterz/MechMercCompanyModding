using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ComponentDefinition : Definition
{
    public string DisplayName = "";

    public string Description = "";

    public ComponentType ComponentType = ComponentType.Equipment;

    public SlotType RequiredSlotType = SlotType.General;

    public Color Color = Color.black;

    public Color TextColor = Color.white;

    public int SlotSize = 1;

    public float Weight = 1.0f;

    public float Health = 10.0f;

    public float ExplosionChance = 0.0f;

    public float ExplosionDamage = 0.0f;

    public float AmmoExplosionDamage = 0.0f;

    public int MountingCost = 0;

    public int RemovalCost = 0;

    public int MarketValue = 0;

    public int MarketRoleMin = 3;

    public int MarketRoleMax = 10;

    public float MarketRoleChance = 0.5f;

    public float Cooling = 0.0f;

    public float Coolant = 0.0f;

    public string AmmoType = "";

    public int AmmoCount = 0;

    public bool AmmoInternal = false;

    public string Weapon = "";

    public string Equipment = "";

    public float EnginePower = 0.0f;

    public float HeatLimit = 0.0f;

    public float HeatLimitBonus = 0.0f;

    public float JumpJetThrust = 0.0f;

    public float JumpJetCapacity = 0.0f;

    public float JumpJetRecharge = 0.0f;

    public float RadarSignatureReduction = 0.0f;

    public float RadarRangeBonus = 0.0f;

    public float TargetLockOnBonus = 0.0f;

    public float InternalBonus = 0.0f;

    public float RecoilModifier = 0.0f;

    public WeaponModification[] WeaponModifications = new WeaponModification[0];

    public bool HasWeaponModification { get => WeaponModifications.Length > 0; }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }

    public WeaponDefinition GetWeaponDefinition()
    {
        return ResourceManager.Instance.GetWeaponDefinition(Weapon);
    }

    public EquipmentDefinition GetEquipmentDefinition()
    {
        return ResourceManager.Instance.GetEquipmentDefinition(Equipment);
    }

    public bool HasWeapon()
    {
        if (Weapon != "")
        {
            return GetWeaponDefinition() != null;
        }

        return false;
    }

    public bool HasEquipment()
    {
        if (Equipment != "")
        {
            return GetEquipmentDefinition() != null;
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

        stringBuilder.AppendLine(GetDisplayName());
        stringBuilder.AppendLine(System.Text.RegularExpressions.Regex.Unescape(StaticHelper.SplitLineToMultiline(GetDescription(), 35)));
        stringBuilder.AppendLine("Slots: " + SlotSize);
        stringBuilder.AppendLine("Tons: " + Weight.ToString("0.#"));
        stringBuilder.AppendLine("Health: " + Health.ToString("0.#"));

        if (ExplosionChance > 0 && (ExplosionDamage > 0 || AmmoExplosionDamage > 0))
        {
            stringBuilder.AppendLine("Explosion Chance: " + (ExplosionChance * 100f).ToString("0.#") + "%");

            if (ExplosionDamage > 0)
            {
                stringBuilder.AppendLine("Explosion Damage: " + ExplosionDamage.ToString("0.#"));
            }

            if (AmmoExplosionDamage > 0)
            {
                stringBuilder.AppendLine("Ammo Explosion Damage: " + AmmoExplosionDamage.ToString("0.###") + "/ammo");
            }
        }

        if (EnginePower > 0.0f)
        {
            stringBuilder.AppendLine("Power: " + EnginePower);
        }

        if (Cooling != 0.0f)
        {
            stringBuilder.AppendLine("Cooling: " + Cooling + "K/s");
        }

        if (Coolant != 0.0f)
        {
            stringBuilder.AppendLine("Coolant: " + Coolant.ToString("0.#") + "kl");
        }

        if (HeatLimit != 0.0f)
        {
            stringBuilder.AppendLine("Heat Limit: " + HeatLimit + "K");
        }

        if (HeatLimitBonus != 0.0f)
        {
            stringBuilder.AppendLine("Heat Limit Bonus: " + HeatLimitBonus + "K");
        }

        if (AmmoType != "")
        {
            AmmoDefinition ammoDefinition = ResourceManager.Instance.GetAmmoDefinition(AmmoType);

            if (ammoDefinition != null)
            {
                if (AmmoInternal)
                {
                    stringBuilder.AppendLine("Internal Ammo: " + ammoDefinition.GetDisplayName() + " x " + AmmoCount.ToString());
                }
                else
                {
                    stringBuilder.AppendLine("Ammo: " + ammoDefinition.GetDisplayName() + " x " + AmmoCount.ToString());
                }
            }
            else
            {
                if (AmmoInternal)
                {
                    stringBuilder.AppendLine("Internal Ammo: " + AmmoType + " x " + AmmoCount.ToString());
                }
                else
                {
                    stringBuilder.AppendLine("Ammo: " + AmmoType + " x " + AmmoCount.ToString());
                }
            }
        }

        if (JumpJetThrust != 0)
        {
            stringBuilder.AppendLine("Jump Jet Thrust: " + JumpJetThrust);
        }

        if (JumpJetCapacity != 0)
        {
            stringBuilder.AppendLine("Jump Jet Capacity: " + JumpJetCapacity + "s");
        }

        if (JumpJetRecharge != 0)
        {
            stringBuilder.AppendLine("Jump Jet Recharge: " + JumpJetRecharge);
        }

        if (RadarRangeBonus != 0)
        {
            stringBuilder.AppendLine("Radar Range Bonus: +" + (RadarRangeBonus * 10f).ToString("0.") + "m");
        }

        if (RadarSignatureReduction != 0)
        {
            stringBuilder.AppendLine("Radar Signature Reduction: " + (RadarSignatureReduction * 10f).ToString("0.") + "m");
        }

        if (TargetLockOnBonus != 0)
        {
            stringBuilder.AppendLine("Lock-On Bonus: +" + (TargetLockOnBonus * 100f).ToString("0.") + "%");
        }

        if (InternalBonus != 0)
        {
            stringBuilder.AppendLine("Internal Bonus: +" + (InternalBonus).ToString("0.#"));
        }

        if (RecoilModifier != 0.0f)
        {
            stringBuilder.AppendLine("Recoil Reduction: +" + (RecoilModifier * 100f).ToString("0.") + "%");
        }

        foreach (WeaponModification weaponModification in WeaponModifications)
        {
            stringBuilder.AppendLine(weaponModification.GetDisplay());
        }

        WeaponDefinition weaponDefinition = GetWeaponDefinition();

        if (weaponDefinition != null)
        {
            stringBuilder.Append(weaponDefinition.GetWeaponDisplayInformation(Weight));
        }

        EquipmentDefinition equipmentDefinition = GetEquipmentDefinition();

        if (equipmentDefinition != null)
        {
            stringBuilder.Append(equipmentDefinition.GetDisplayInformation(Weight));
        }

        return stringBuilder.ToString().TrimEnd('\r', '\n');
    }

    public bool GetExplosionRole()
    {
        if (ExplosionChance > 0.0f)
        {
            return Random.Range(0.0f, 1.0f) < ExplosionChance;
        }

        return false;
    }
}
