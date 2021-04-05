using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDefinition : Definition
{
    public string DisplayName = "";

    public string ModelPrefab = "";

    public Vector3 ModelScale = Vector3.one;

    public WeaponGrouping DefaultWeaponGrouping = new WeaponGrouping();

    public WeaponClassification[] WeaponClassifications = new WeaponClassification[0];

    public float JamDecay = 0.0f;

    public float JamDecayDelay = 0.0f;

    public float JammedDecay = 0.0f;

    public string GetDisplayName() { return ResourceManager.Instance.GetLocalization(DisplayName); }

    public GameObject GetModelPrefab() { return ResourceManager.Instance.GetWeaponModelPrefab(ModelPrefab); }

    public abstract string GetWeaponDisplayInformation(float weight);

    public abstract void BuildFromBase();
}
