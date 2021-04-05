using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefinition : Definition
{
    #region Variables
    public string BaseDefinition = "";

    public string AmmoType = "";

    public DamageType DamageType = DamageType.Generic;

    public float DamageEffective = 0.0f;

    public float DamageMin = 0.0f;

    public float DamageHeat = 0.0f;

    public bool MissileDefenseTargetable = false;

    public float MissileHealth = 0.0f;

    public bool HasSplashDamage = false;

    public float SplashDamageRange = 0.0f;

    public float AirCraftDamageMulti = 1.0f;

    public float Heat = 0.0f;

    public float RangeDamageMin = 0.0f;

    public float RangeEffective = 0.0f;

    public float CriticalChance = 0.25f;

    public float CriticalDamageMulti = 1.0f;

    public int ProjectileCount = 1;

    public float Spread = 0.0f;

    public float Velocity = 0.0f;

    public float ProjectileLifeTime = 0.0f;

    public float MissileFuelTime = 0.0f;

    public float TrailLifeTime = 0.0f;

    public bool ExplodeOnLifeTime = false;

    public bool UseGravity = false;

    public ProjectileHomingType HomingType = ProjectileHomingType.None;

    public float HomingTurningRate = 0.0f;

    public float HomingFlightHeightOffSet = 0.0f;

    public float HomingInterceptRangeSqr = 0.0f;

    public float HomingSpread = 0.0f;

    public float ArmorPiercing = 0.0f;

    public float Recoil = 0.0f;

    public float Jamming = 0.0f;

    public string ProjectilePrefab = "";

    public float ProjectileScale = 0.0f;

    public string TrailPrefab = "";

    public float TrailScale = 0.0f;

    public string ImpactEffectPrefab = "";

    public float ImpactEffectScale = 0.0f;

    public string FiringSound = "";

    public float FiringVolume = 1.0f;

    public float FiringPitchMin = 0.95f;

    public float FiringPitchMax = 1.05f;

    public string[] ImpactSounds = new string[0];

    public float ImpactSoundVolume = 0.0f;

    public string SecondaryProjectileDefinition = "";

    public int SecondaryProjectileCount = 0;

    public float SecondaryProjectileFireRangeSqr = 0.0f;

    public bool SecondaryProjectileFireAtEndLifeTimer = false;

    public CollisionDetectionMode CollisionDetection = CollisionDetectionMode.Discrete;

    public RigidbodyInterpolation RigidbodyInterpolation = RigidbodyInterpolation.None;

    #endregion

    public float RandomSpread { get => Random.Range(-Spread, Spread); }

    public Vector3 ProjectileScaleVector3 { get => new Vector3(ProjectileScale, ProjectileScale, ProjectileScale); }

    public Vector3 TrailScaleVector3 { get => new Vector3(TrailScale, TrailScale, TrailScale); }

    public Vector3 ImpactEffectScaleVector3 { get => new Vector3(ImpactEffectScale, ImpactEffectScale, ImpactEffectScale); }

    public bool InEffectiveRange(float range)
    {
        return range < RangeEffective;
    }

    public bool InMaxRange(float range)
    {
        return range < RangeDamageMin;
    }

    public float GetDamage(float range)
    {
        if (range < RangeEffective)
        {
            return DamageEffective;
        }
        else if (range < RangeDamageMin)
        {
            float rangeDifference = RangeDamageMin - RangeEffective;
            float ratio = (rangeDifference - (range - RangeEffective)) / rangeDifference;
            return DamageMin + ((DamageEffective - DamageMin) * ratio);
        }
        else
        {
            return DamageMin;
        }
    }

    public float GetSplashRatio(float range)
    {
        return range / SplashDamageRange;
    }

    public bool GetCriticalRole()
    {
        return Random.Range(0.0f, 1.0f) < CriticalChance;
    }

    public AudioClip GetFiringSound()
    {
        return ResourceManager.Instance.GetAudioClip(FiringSound);
    }

    public AmmoDefinition GetAmmoDefinition()
    {
        if (AmmoType != "")
        {
            return ResourceManager.Instance.GetAmmoDefinition(AmmoType);
        }

        return null;
    }

    public GameObject GetProjectilePrefab()
    {
        return ResourceManager.Instance.GetProjectilePrefab(ProjectilePrefab);
    }

    public GameObject GetTrailPrefab()
    {
        return ResourceManager.Instance.GetTrailPrefab(TrailPrefab);
    }

    public GameObject GetImpactEffectPrefab()
    {
        return ResourceManager.Instance.GetEffectPrefab(ImpactEffectPrefab);
    } 

    public AudioClip GetImpactSound()
    {
        if (ImpactSounds.Length > 0)
        {
            return ResourceManager.Instance.GetAudioClip(ImpactSounds[Random.Range(0, ImpactSounds.Length)]);
        }

        return null;
    }

    public ProjectileDefinition GetSecondaryProjectile()
    {
        return ResourceManager.Instance.GetProjectileDefinition(SecondaryProjectileDefinition);
    }

    public void BuildFromBase()
    {
        if (BaseDefinition == "")
            return;

        ProjectileDefinition targetDefinition = ResourceManager.Instance.GetProjectileDefinition(BaseDefinition);

        if (targetDefinition == null)
            return;

        if (AmmoType == "")
        {
            AmmoType = targetDefinition.AmmoType;
        }

        if (Heat == 0.0f)
        {
            Heat = targetDefinition.Heat;
        }

        if (DamageType == DamageType.Generic)
        {
            DamageType = targetDefinition.DamageType;
        }

        if (DamageEffective == 0.0f)
        {
            DamageEffective = targetDefinition.DamageEffective;
        }

        if (DamageMin == 0.0f)
        {
            DamageMin = targetDefinition.DamageMin;
        }

        if (RangeDamageMin == 0.0f)
        {
            RangeDamageMin = targetDefinition.RangeDamageMin;
        }

        if (RangeEffective == 0.0f)
        {
            RangeEffective = targetDefinition.RangeEffective;
        }

        if (CriticalChance == 0.25f)
        {
            CriticalChance = targetDefinition.CriticalChance;
        }

        if (CriticalDamageMulti == 1.0f)
        {
            CriticalDamageMulti = targetDefinition.CriticalDamageMulti;
        }

        if (MissileDefenseTargetable == false)
        {
            MissileDefenseTargetable = targetDefinition.MissileDefenseTargetable;
        }

        if (MissileHealth == 0.0f)
        {
            MissileHealth = targetDefinition.MissileHealth;
        }

        if (DamageHeat == 0.0f)
        {
            DamageHeat = targetDefinition.DamageHeat;
        }

        if (!HasSplashDamage)
        {
            HasSplashDamage = targetDefinition.HasSplashDamage;
        }

        if (SplashDamageRange == 0.0f)
        {
            SplashDamageRange = targetDefinition.SplashDamageRange;
        }

        if (AirCraftDamageMulti == 1.0f)
        {
            AirCraftDamageMulti = targetDefinition.AirCraftDamageMulti;
        }

        if (ProjectileCount == 1)
        {
            ProjectileCount = targetDefinition.ProjectileCount;
        }

        if (Spread == 0.0f)
        {
            Spread = targetDefinition.Spread;
        }

        if (Velocity == 0.0f)
        {
            Velocity = targetDefinition.Velocity;
        }

        if (ProjectileLifeTime == 0.0f)
        {
            ProjectileLifeTime = targetDefinition.ProjectileLifeTime;
        }

        if (MissileFuelTime == 0.0f)
        {
            MissileFuelTime = targetDefinition.MissileFuelTime;
        }

        if (TrailLifeTime == 0.0f)
        {
            TrailLifeTime = targetDefinition.TrailLifeTime;
        }

        if (ExplodeOnLifeTime == false)
        {
            ExplodeOnLifeTime = targetDefinition.ExplodeOnLifeTime;
        }

        if (UseGravity == false)
        {
            UseGravity = targetDefinition.UseGravity;
        }

        if (HomingType == ProjectileHomingType.None)
        {
            HomingType = targetDefinition.HomingType;
        }

        if (HomingTurningRate == 0.0f)
        {
            HomingTurningRate = targetDefinition.HomingTurningRate;
        }

        if (HomingFlightHeightOffSet == 0.0f)
        {
            HomingFlightHeightOffSet = targetDefinition.HomingFlightHeightOffSet;
        }

        if (HomingInterceptRangeSqr == 0.0f)
        {
            HomingInterceptRangeSqr = targetDefinition.HomingInterceptRangeSqr;
        }

        if (HomingSpread == 0.0f)
        {
            HomingSpread = targetDefinition.HomingSpread;
        }

        if (HomingSpread == 0.0f)
        {
            HomingSpread = targetDefinition.HomingSpread;
        }

        if (ArmorPiercing == 0.0f)
        {
            ArmorPiercing = targetDefinition.ArmorPiercing;
        }

        if (Recoil == 0.0f)
        {
            Recoil = targetDefinition.Recoil;
        }

        if (FiringSound == "")
        {
            FiringSound = targetDefinition.FiringSound;
        }

        if (ProjectilePrefab == "")
        {
            ProjectilePrefab = targetDefinition.ProjectilePrefab;
        }

        if (ProjectileScale == 0.0f)
        {
            ProjectileScale = targetDefinition.ProjectileScale;
        }

        if (TrailPrefab == "")
        {
            TrailPrefab = targetDefinition.TrailPrefab;
        }

        if (TrailScale == 0.0f)
        {
            TrailScale = targetDefinition.TrailScale;
        }

        if (ImpactEffectPrefab == "")
        {
            ImpactEffectPrefab = targetDefinition.ImpactEffectPrefab;
        }

        if (ImpactEffectScale == 0.0f)
        {
            ImpactEffectScale = targetDefinition.ImpactEffectScale;
        }

        if (ImpactSounds.Length == 0)
        {
            ImpactSounds = targetDefinition.ImpactSounds;
        }

        if (ImpactSoundVolume == 0.0f)
        {
            ImpactSoundVolume = targetDefinition.ImpactSoundVolume;
        }

        if (CollisionDetection == CollisionDetectionMode.Discrete)
        {
            CollisionDetection = targetDefinition.CollisionDetection;
        }

        if (RigidbodyInterpolation == RigidbodyInterpolation.None)
        {
            RigidbodyInterpolation = targetDefinition.RigidbodyInterpolation;
        }
    }

    public float GetDisplayDamage(int shotCount)
    {
        return DamageEffective * ProjectileCount * shotCount;
    }

    public float GetDPS(float recycleTime, int shotCount)
    {
        return DamageEffective * ProjectileCount * shotCount / recycleTime;
    }

    public float GetHPS(float recycleTime, int shotCount)
    {
        return Heat * shotCount / recycleTime;
    }

    public float GetRangeMax()
    {
        if (MissileFuelTime > 0)
        {
            return Mathf.RoundToInt(Velocity * MissileFuelTime);
        }

        return Mathf.RoundToInt(Velocity * ProjectileLifeTime);
    }

    public string GetDisplayInformation(float weight, float recycleTime, int shotCount)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        AmmoDefinition ammoDefinition = GetAmmoDefinition();

        ProjectileDefinition secondarProjectile = GetSecondaryProjectile();

        if (ammoDefinition != null)
        {
            stringBuilder.AppendLine("Ammo Type: " + ammoDefinition.GetDisplayName());
        }

        int totalProjectiles = ProjectileCount * shotCount;

        float damageEffective = DamageEffective * totalProjectiles;
        float damageMin = DamageMin * totalProjectiles;
        float totalHeat = Heat * shotCount;

        float damagePerSecond = 0.0f;
        float damagePerSecondSecondary = 0.0f;
        float damageHeat = DamageHeat * totalProjectiles;
        float hps = 0.0f;

        if (recycleTime > 0)
        {
            damagePerSecond = damageEffective / recycleTime;
            hps = totalHeat / recycleTime;
        }

        float lifeTime = ProjectileLifeTime;

        if (MissileFuelTime > 0)
        {
            lifeTime = Mathf.Min(MissileFuelTime, ProjectileLifeTime);
        }

        float rangeMax = Mathf.RoundToInt(Velocity * lifeTime);

        switch (HomingType)
        {
            case ProjectileHomingType.LockOn:
                {
                    stringBuilder.AppendLine("Lock-on Homing System");
                    break;
                }
            case ProjectileHomingType.Manual:
                {
                    stringBuilder.AppendLine("Manual Guidance System");
                    break;
                }
            case ProjectileHomingType.Auto:
                {
                    stringBuilder.AppendLine("Auto Homing System");
                    break;
                }
        }

        stringBuilder.AppendLine("Damage Effective: " + damageEffective.ToString("0.##"));

        if (damageEffective != damageMin)
        {
            stringBuilder.AppendLine("Damage Min: " + damageMin.ToString("0.###"));
        }

        if (secondarProjectile != null && SecondaryProjectileCount > 0)
        {
            float secondaryDamage = secondarProjectile.DamageEffective * SecondaryProjectileCount * totalProjectiles;

            damagePerSecondSecondary = secondaryDamage / recycleTime;

            stringBuilder.AppendLine("Secondary Damage: " + secondaryDamage.ToString("0.##"));
        }

        if (damageHeat != 0.0f)
        {
            stringBuilder.AppendLine("Damage Heat: " + damageHeat.ToString("0.##") + "K");
        }

        if (RangeEffective != rangeMax)
        {
            stringBuilder.AppendLine("Range Effective: " + (RangeEffective * 10.0f).ToString("0.") + "m");
        }

        if (RangeEffective != RangeDamageMin && rangeMax != RangeDamageMin)
        {
            stringBuilder.AppendLine("Range Damage Min: " + (RangeDamageMin * 10.0f).ToString("0.") + "m");
        }

        if (rangeMax != 0)
        {
            stringBuilder.AppendLine("Range Max: " + (rangeMax * 10f).ToString("0.") + "m");
        }

        if (CriticalChance > 0)
        {
            stringBuilder.AppendLine("Critical Chance: " + (CriticalChance * 100f).ToString("0.#") + "%");

            if (CriticalDamageMulti != 1.0f)
            {
                stringBuilder.AppendLine("Critical Damage Multiplier: x" + CriticalDamageMulti.ToString("0.#"));
            }
        }

        if (SplashDamageRange != 0.0f)
        {
            stringBuilder.AppendLine("Splash Damage Range: " + (SplashDamageRange * 10.0f).ToString("0.#") + "m");
        }

        if (AirCraftDamageMulti != 1.0f)
        {
            stringBuilder.AppendLine("Aircraft Damage Multiplier: x" + AirCraftDamageMulti.ToString("0.#"));
        }

        if (ArmorPiercing != 0.0f)
        {
            stringBuilder.AppendLine("Armor Piercing: " + (ArmorPiercing * 100f).ToString("0.") + "%");
        }

        stringBuilder.AppendLine("Weapon Heat: " + totalHeat.ToString("0.#") + "K");

        if (Velocity != 0)
        {
            stringBuilder.AppendLine("Velocity: " + (Velocity * 10f).ToString("0.") + "m/s");
        }

        if (damagePerSecond > 0)
        {
            stringBuilder.AppendLine("Effective DPS: " + damagePerSecond.ToString("0.##"));

            stringBuilder.AppendLine("DPS Per Ton: " + (damagePerSecond / weight).ToString("0.##"));
        }

        if (damagePerSecondSecondary > 0)
        {
            stringBuilder.AppendLine("Secondary DPS: " + damagePerSecondSecondary.ToString("0.##"));

            stringBuilder.AppendLine("Secondary DPS Per Ton: " + (damagePerSecondSecondary / weight).ToString("0.##"));
        }

        if (hps > 0.0f)
        {
            stringBuilder.AppendLine("HPS: " + hps.ToString("0.##") + "K/s");
        }

        if (totalHeat > 0)
        {
            stringBuilder.Append("Damage Per 100K Heat: " + (damageEffective / (totalHeat / 100.0f)).ToString("0.##"));
        }

        return stringBuilder.ToString();
    }
}
