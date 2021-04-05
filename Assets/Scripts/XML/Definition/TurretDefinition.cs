using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDefinition : Definition
{
    public string Prefab = "";

    public string DisplayName = "";

    public UnitClass UnitClass = UnitClass.TurretLight;

    public ArmorType ArmorType = ArmorType.standard;

    public float ArmorBase = 0.0f;

    public float ArmorCenter = 0.0f;

    public float ArmorPodLeft = 0.0f;

    public float ArmorPodRight = 0.0f;

    public float InternalBase = 0.0f;

    public float InternalCenter = 0.0f;

    public float InternalPodLeft = 0.0f;

    public float InternalPodRight = 0.0f;

    public TurretSetting TurretSetting;

    public string[] ComponentsBase = new string[0];

    public string[] ComponentsCenter = new string[0];

    public string[] ComponentsPodLeft = new string[0];

    public string[] ComponentsPodRight = new string[0];

    public GameObject GetPrefab()
    {
        return ResourceManager.Instance.GetTurretPrefab(Prefab);
    }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }
}
