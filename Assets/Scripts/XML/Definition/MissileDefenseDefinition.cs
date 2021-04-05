using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MissileDefenseDefinition : EquipmentDefinition
{
    public string FiringSound = "";

    public bool FiringSoundLoops = false;

    public float Damage = 0.0f;

    public float Heat = 0.0f;

    public float Range = 0.0f;

    public float RecycleTime = 0.0f;

    public float FiringTime = 0.0f;

    public string BeamPrefab = "";

    public float BeamWidth = 0.0f;

    public override GameObject GetModelPrefab() { return ResourceManager.Instance.GetWeaponModelPrefab(ModelPrefab); }

    public AudioClip GetFiringSound() { return ResourceManager.Instance.GetAudioClip(FiringSound); }

    public GameObject GetBeamPrefab() { return ResourceManager.Instance.GetBeamPrefab(BeamPrefab); }

    public bool InRange(float range) { return range < Range; }

    public override string GetDisplayInformation(float weight)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        AmmoDefinition ammoDefinition = GetAmmoDefinition();
        float fireRate = 60 / RecycleTime;
        float damagePerSecond = Damage / RecycleTime;
        float hps = Heat / RecycleTime;

        stringBuilder.AppendLine("Damage : " + Damage.ToString("0.##"));

        stringBuilder.AppendLine("Range : " + (Range * 10).ToString("0.#") + "m");

        if (Heat > 0.0f)
        {
            stringBuilder.AppendLine("Heat: " + Heat.ToString("0.#") + "K");
        }

        stringBuilder.AppendLine("Recycle Time: " + RecycleTime.ToString("0.##") + "s");

        if (ammoDefinition != null)
        {
            stringBuilder.AppendLine("Ammo Type: " + ammoDefinition.GetDisplayName());
        }

        stringBuilder.AppendLine("Fire Rate: " + fireRate.ToString("0.#") + "/m");

        stringBuilder.AppendLine("DPS: " + damagePerSecond.ToString("0.##"));

        stringBuilder.AppendLine("DPS Per Ton: " + (damagePerSecond / weight).ToString("0.##"));

        if (hps > 0.0f)
        {
            stringBuilder.AppendLine("HPS: " + hps.ToString("0.##") + "K/s");
        }

        if (EquipmentModes.Length > 0)
        {
            stringBuilder.AppendLine();

            for (int i = 0; i < EquipmentModes.Length; i++)
            {
                stringBuilder.AppendLine(EquipmentModes[i].GetDisplayInformation());
                stringBuilder.AppendLine();
            }
        }

        return stringBuilder.ToString();
    }
}
