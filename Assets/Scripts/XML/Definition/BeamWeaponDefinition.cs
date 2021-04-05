using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BeamWeaponDefinition : WeaponDefinition
{
    public DamageType DamageType = DamageType.Generic;

    public float Heat = 0.0f;

    public float DamageEffective = 0.0f;

    public float RangeMax = 0.0f;

    public float RangeEffective = 0.0f;

    public float CriticalChance = 0.25f;

    public float CriticalDamageMulti = 1.0f;

    public BeamFiringModeType BeamFiringModeType = BeamFiringModeType.Standard;

    public string BeamPrefab = "";
    public float Duration = 0.0f;
    public float DurationMin = 0.0f;
    public float Recharge = 0.0f;
    public float RechargeDelay = 0.0f;
    public float BeamWidth = 0.0f;
    public float MuzzleEffectScale = 1.0f;
    public float ImpactEffectScale = 1.0f;
    public float PulseFrequency = 0.0f;
    public float JamPerSecond = 0.0f;

    public int BeamCount = 1;

    public string FiringSound = "";

    public float FiringVolume = 1.0f;

    public float FiringPitch = 1.0f;

    public float EffectiveDamageDisplay
    { 
        get
        {
            if (Duration > 0)
            {
                return DamageEffective * Duration;
            }

            return DamageEffective;
        }
    }


    public float DPS
    {
        get
        {
            if (Duration > 0)
            {
                return EffectiveDamageDisplay / GetRecycleTime();
            }

            return EffectiveDamageDisplay;
        }
    }

    public float HPS
    {
        get
        {
            if (Duration > 0)
            {
                return GetWeaponHeat() / GetRecycleTime();
            }

            return Heat;
        }
    }

    public Vector3 MuzzleEffectScaleVector
    {
        get
        {
            return new Vector3(MuzzleEffectScale, MuzzleEffectScale, MuzzleEffectScale);
        }
    }

    public Vector3 ImpactEffectScaleVector
    {
        get
        {
            return new Vector3(ImpactEffectScale, ImpactEffectScale, ImpactEffectScale);
        }
    }

    public float GetDamage(float range)
    {
        if (range < RangeEffective)
        {
            return DamageEffective;
        }
        else
        {
            float rangeDifference = RangeMax - RangeEffective;
            float ratio = (rangeDifference - (range - RangeEffective)) / rangeDifference;
            return DamageEffective * ratio;
        }
    }

    public GameObject GetBeamPrefab() { return ResourceManager.Instance.GetBeamPrefab(BeamPrefab); }

    public AudioClip GetFiringSound() { return ResourceManager.Instance.GetAudioClip(FiringSound); }

    public bool InEffectiveRange(float range) { return range < RangeEffective; }

    public bool InMaxRange(float range) { return range < RangeMax; }

    public float GetRecycleTime()
    {
        if (Duration > 0)
        {
            return Duration + (Duration / Recharge);
        }

        return 0f;
    }

    public float GetWeaponHeat()
    {
        if (Duration > 0)
        {
            return Heat * Duration;
        }

        return Heat;
    }

    public bool GetCriticalRole() { return Random.Range(0.0f, 1.0f) < CriticalChance; }

    public override string GetWeaponDisplayInformation(float weight)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        float damageEffective = EffectiveDamageDisplay;
        float weaponHeat = GetWeaponHeat();
        float recycleTime = GetRecycleTime();
        float fireRate = 60 / recycleTime;
        float damagePerSecond = DPS;
        float damageHeat = 0.0f;
        float hps;

        if (recycleTime > 0)
        {
            hps = weaponHeat / recycleTime;
        }
        else
        {
            hps = Heat;
        }

        stringBuilder.AppendLine("Damage Effective: " + damageEffective.ToString("0.##"));

        if (damageHeat != 0.0f)
        {
            stringBuilder.AppendLine("Damage Heat: " + damageHeat.ToString("0.##") + "K");
        }

        stringBuilder.AppendLine("Range Effective: " + RangeEffective * 10.0f + "m");

        if (RangeEffective != RangeMax)
        {
            stringBuilder.AppendLine("Range Max: " + (RangeMax * 10.0f).ToString("0.#") + "m");
        }

        if (CriticalChance > 0)
        {
            stringBuilder.AppendLine("Critical Chance: " + (CriticalChance * 100f).ToString("0.#") + "%");

            if (CriticalDamageMulti != 1.0f)
            {
                stringBuilder.AppendLine("Critical Damage Multiplier: x" + CriticalDamageMulti.ToString("0.#"));
            }
        }

        if (Duration > 0)
        {
            stringBuilder.AppendLine("Duration: " + Duration.ToString("0.0#") + "s");
        }

        stringBuilder.AppendLine("Weapon Heat: " + weaponHeat.ToString("0.#") + "K");

        if (recycleTime > 0)
        {
            stringBuilder.AppendLine("Recycle Time: " + recycleTime.ToString("0.##") + "s");
        }

        if (fireRate != 0f)
        {
            stringBuilder.AppendLine("Fire Rate: " + fireRate.ToString("0.#") + "/m");
        }

        if (damagePerSecond > 0)
        {
            stringBuilder.AppendLine("Effective DPS: " + damagePerSecond.ToString("0.##"));

            stringBuilder.AppendLine("DPS Per Ton: " + (damagePerSecond / weight).ToString("0.##"));
        }

        if (hps > 0.0f)
        {
            stringBuilder.AppendLine("HPS: " + hps.ToString("0.##") + "K/s");
        }

        if (weaponHeat > 0)
        {
            stringBuilder.Append("Damage Per 100K Heat: " + (damageEffective / (weaponHeat / 100.0f)).ToString("0.##"));
        }

        return stringBuilder.ToString();
    }

    public override void BuildFromBase()
    {
        
    }
}
