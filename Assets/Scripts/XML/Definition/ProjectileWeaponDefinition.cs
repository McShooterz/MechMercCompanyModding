using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ProjectileWeaponDefinition : WeaponDefinition
{
    public string BaseDefinition = "";

    public ProjectileFiringModeType ProjectileFiringModeType = ProjectileFiringModeType.Standard;

    public float ProjectSpawnForwardMulti = 0.2f;

    public float RecycleTime = 0.0f;

    public int ShotsPerSalvo = 1;

    public int SalvoCount = 1; 

    public float SalvoDelay = 0.0f;  

    public bool RequiresAmmo = false;

    public string MuzzleEffectPrefab = "";

    public float JammingRapidRefire = 0.0f;

    public float ChargingRate = 0.0f;

    public bool FiringSoundLoops = false;

    public string[] ProjectileDefinitions = new string[0];

    public float GetDamage()
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            return projectileDefinitions[0].GetDisplayDamage(ShotsPerSalvo * SalvoCount);
        }

        return 0.0f;
    }

    public float GetMaxDamage(List<string> ammoTypes)
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            float damageMax = 0.0f;
            float damageCurrent;

            for (int i = 0; i < projectileDefinitions.Count; i++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                if (ammoTypes.Contains(projectileDefinition.AmmoType))
                {
                    damageCurrent = projectileDefinition.GetDisplayDamage(ShotsPerSalvo * SalvoCount);

                    if (damageCurrent > damageMax)
                        damageMax = damageCurrent;
                }
            }

            return damageMax;
        }

        return 0.0f;
    }

    public float GetDPS()
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            return projectileDefinitions[0].GetDPS(RecycleTime, ShotsPerSalvo * SalvoCount);
        }

        return 0.0f;
    }

    public float GetMaxDPS(List<string> ammoTypes)
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            float dpsMax = 0.0f;
            float dps;

            for (int i = 0; i < projectileDefinitions.Count; i++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                if (ammoTypes.Contains(projectileDefinition.AmmoType))
                {
                    dps = projectileDefinition.GetDPS(RecycleTime, ShotsPerSalvo * SalvoCount);

                    if (dps > dpsMax)
                        dpsMax = dps;
                }
            }

            return dpsMax;
        }

        return 0.0f;
    }

    public float GetMaxRange()
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            return projectileDefinitions[0].GetRangeMax();
        }

        return 0.0f;
    }

    public float GetMaxRange(List<string> ammoTypes)
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            float rangeMax = 0.0f;
            float range;

            for (int i = 0; i < projectileDefinitions.Count; i++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                if (ammoTypes.Contains(projectileDefinition.AmmoType))
                {
                    range = projectileDefinition.GetRangeMax();

                    if (range > rangeMax)
                        rangeMax = range;
                }
            }

            return rangeMax;
        }

        return 0.0f;
    }

    public float GetHeat()
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            return projectileDefinitions[0].Heat * ShotsPerSalvo * SalvoCount;
        }

        return 0.0f;
    }

    public float GetMaxHeat(List<string> ammoTypes)
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            float heatMax = 0.0f;
            float currentHeat;

            for (int i = 0; i < projectileDefinitions.Count; i++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                if (ammoTypes.Contains(projectileDefinition.AmmoType))
                {
                    currentHeat = projectileDefinition.Heat * ShotsPerSalvo * SalvoCount;

                    if (currentHeat > heatMax)
                        heatMax = currentHeat;
                }
            }

            return heatMax;
        }

        return 0.0f;
    }

    public float GetHPS()
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            return projectileDefinitions[0].GetHPS(RecycleTime, ShotsPerSalvo * SalvoCount);
        }

        return 0.0f;
    }

    public float GetMaxHPS(List<string> ammoTypes)
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            float hpsMax = 0.0f;
            float hps;

            for (int i = 0; i < projectileDefinitions.Count; i++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                if (ammoTypes.Contains(projectileDefinition.AmmoType))
                {
                    hps = projectileDefinition.GetHPS(RecycleTime, ShotsPerSalvo * SalvoCount);

                    if (hps > hpsMax)
                        hpsMax = hps;
                }
            }

            return hpsMax;
        }

        return 0.0f;
    }

    public List<string> GetAmmoTypes()
    {
        List<string> ammoTypes = new List<string>();

        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        for (int i = 0; i < projectileDefinitions.Count; i++)
        {
            ProjectileDefinition projectileDefinition = projectileDefinitions[i];

            if (projectileDefinition.AmmoType != "")
            {
                ammoTypes.Add(projectileDefinition.AmmoType);
            }
        }

        return ammoTypes;
    }

    public List<ProjectileDefinition> GetProjectileDefinitions()
    {
        List<ProjectileDefinition> projectileDefinitions = new List<ProjectileDefinition>();

        for (int i = 0; i < ProjectileDefinitions.Length; i++)
        {
            ProjectileDefinition projectileDefinition = ResourceManager.Instance.GetProjectileDefinition(ProjectileDefinitions[i]);

            if (projectileDefinition != null)
            {
                projectileDefinitions.Add(projectileDefinition);
            }
        }

        return projectileDefinitions;
    }

    public int GetShotCount()
    {
        return ShotsPerSalvo * SalvoCount;
    }

    public GameObject GetMuzzleEffectPrefab()
    {
        return ResourceManager.Instance.GetEffectPrefab(MuzzleEffectPrefab);
    }

    public override string GetWeaponDisplayInformation(float weight)
    {
        List<ProjectileDefinition> projectileDefinitions = GetProjectileDefinitions();

        if (projectileDefinitions.Count > 0)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            int shotCount = GetShotCount();

            float modifiedRecycleTime;

            switch (ProjectileFiringModeType)
            {
                case ProjectileFiringModeType.Charging:
                    {
                        modifiedRecycleTime = RecycleTime + (1.0f / ChargingRate);
                        break;
                    }
                case ProjectileFiringModeType.SingleShot:
                    {
                        modifiedRecycleTime = 0.0f;
                        break;
                    }
                default:
                    {
                        modifiedRecycleTime = RecycleTime;
                        break;
                    }
            }

            if (modifiedRecycleTime > 0)
            {
                stringBuilder.AppendLine("Recycle Time: " + modifiedRecycleTime.ToString("0.##") + "s");
                stringBuilder.AppendLine("Fire Rate: " + (60.0f / modifiedRecycleTime * shotCount).ToString("0.") + "/m");
            }

            if (RequiresAmmo)
            {
                for (int i = 0; i < projectileDefinitions.Count; i++)
                {
                    ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                    stringBuilder.AppendLine("");

                    stringBuilder.AppendLine(projectileDefinition.GetDisplayInformation(weight, modifiedRecycleTime, shotCount));
                }
            }
            else
            {
                stringBuilder.AppendLine(projectileDefinitions[0].GetDisplayInformation(weight, modifiedRecycleTime, shotCount));
            }

            return stringBuilder.ToString();
        }

        return "";
    }

    public override void BuildFromBase()
    {
        if (BaseDefinition == "")
            return;

        WeaponDefinition weaponDefinition = ResourceManager.Instance.GetWeaponDefinition(BaseDefinition);

        if (weaponDefinition == null || !(weaponDefinition is ProjectileWeaponDefinition))
            return;

        ProjectileWeaponDefinition targetDefinition = weaponDefinition as ProjectileWeaponDefinition;

        if (DisplayName == "")
        {
            DisplayName = targetDefinition.DisplayName;
        }

        if (ModelPrefab == "")
        {
            ModelPrefab = targetDefinition.ModelPrefab;
        }

        if (ModelScale == Vector3.one)
        {
            ModelScale = targetDefinition.ModelScale;
        }

        if (DefaultWeaponGrouping.WeaponGroup1 == false)
        {
            DefaultWeaponGrouping.WeaponGroup1 = targetDefinition.DefaultWeaponGrouping.WeaponGroup1;
        }

        if (DefaultWeaponGrouping.WeaponGroup2 == false)
        {
            DefaultWeaponGrouping.WeaponGroup2 = targetDefinition.DefaultWeaponGrouping.WeaponGroup2;
        }

        if (DefaultWeaponGrouping.WeaponGroup3 == false)
        {
            DefaultWeaponGrouping.WeaponGroup3 = targetDefinition.DefaultWeaponGrouping.WeaponGroup3;
        }

        if (DefaultWeaponGrouping.WeaponGroup4 == false)
        {
            DefaultWeaponGrouping.WeaponGroup4 = targetDefinition.DefaultWeaponGrouping.WeaponGroup4;
        }

        if (DefaultWeaponGrouping.WeaponGroup5 == false)
        {
            DefaultWeaponGrouping.WeaponGroup5 = targetDefinition.DefaultWeaponGrouping.WeaponGroup5;
        }

        if (DefaultWeaponGrouping.WeaponGroup6 == false)
        {
            DefaultWeaponGrouping.WeaponGroup6 = targetDefinition.DefaultWeaponGrouping.WeaponGroup6;
        }

        if (WeaponClassifications.Length == 0)
        {
            WeaponClassifications = targetDefinition.WeaponClassifications;
        }

        if (RecycleTime == 0.0f)
        {
            RecycleTime = targetDefinition.RecycleTime;
        }

        if (FiringSoundLoops == false)
        {
            FiringSoundLoops = targetDefinition.FiringSoundLoops;
        }

        if (ProjectileFiringModeType == ProjectileFiringModeType.Standard)
        {
            ProjectileFiringModeType = targetDefinition.ProjectileFiringModeType;
        }

        if (ProjectSpawnForwardMulti == 0.2f)
        {
            ProjectSpawnForwardMulti = targetDefinition.ProjectSpawnForwardMulti;
        }

        if (ShotsPerSalvo == 1)
        {
            ShotsPerSalvo = targetDefinition.ShotsPerSalvo;
        }

        if (SalvoCount == 1)
        {
            SalvoCount = targetDefinition.SalvoCount;
        }

        if (SalvoDelay == 0.0f)
        {
            SalvoDelay = targetDefinition.SalvoDelay;
        }

        if (!RequiresAmmo)
        {
            RequiresAmmo = targetDefinition.RequiresAmmo;
        }

        if (MuzzleEffectPrefab == "")
        {
            MuzzleEffectPrefab = targetDefinition.MuzzleEffectPrefab;
        }

        if (JamDecay == 0.0f)
        {
            JamDecay = targetDefinition.JamDecay;
        }

        if (JamDecayDelay == 0.0f)
        {
            JamDecayDelay = targetDefinition.JamDecayDelay;
        }

        if (JammedDecay == 0.0f)
        {
            JammedDecay = targetDefinition.JammedDecay;
        }

        if (ProjectileDefinitions.Length == 0)
        {
            ProjectileDefinitions = targetDefinition.ProjectileDefinitions;
        }
    }
}
