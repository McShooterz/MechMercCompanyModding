using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponModification
{
    public WeaponModificationType WeaponModificationType;

    public WeaponClassification WeaponClassification;

    public float Value;

    public string GetDisplay()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append(StaticHelper.GetWeaponClassificationDisplay(WeaponClassification) + " " + StaticHelper.GetWeaponModificationTypeDisplay(WeaponModificationType) + ": ");

        if (Value < 0.0f)
        {
            stringBuilder.Append((Value * 100).ToString("0.#") + "%");
        }
        else
        {
            stringBuilder.Append("+" + (Value * 100).ToString("0.#") + "%");
        }

        return stringBuilder.ToString();
    }
}
