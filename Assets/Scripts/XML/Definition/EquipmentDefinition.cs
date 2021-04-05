using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDefinition : Definition
{
    public string DisplayName = "";

    public string ModelPrefab = "";

    public Vector3 ModelScale = Vector3.one;

    public EquipmentMode[] EquipmentModes = new EquipmentMode[0];

    public string AmmoType = "";

    public string GetDisplayName() { return ResourceManager.Instance.GetLocalization(DisplayName); }

    public virtual GameObject GetModelPrefab() { return ResourceManager.Instance.GetAccessoryModelPrefab(ModelPrefab); }

    public AmmoDefinition GetAmmoDefinition()
    {
        if (AmmoType != "")
        {
            return ResourceManager.Instance.GetAmmoDefinition(AmmoType);
        }

        return null;
    }

    public virtual string GetDisplayInformation(float weight)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

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
