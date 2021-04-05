using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMode
{
    public string DisplayName = "";

    public MissileDefenseType MissileDefenseType = MissileDefenseType.None;

    public float RadarSignatureReduction = 0.0f;

    public float RadarRangeBonus = 0.0f;

    public float TargetLockOnBonus = 0.0f;

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDisplayInformation()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine("Mode: " + GetDisplayName());

        switch (MissileDefenseType)
        {
            case MissileDefenseType.DirectFireAll:
                {
                    stringBuilder.AppendLine("Missile Defense: Direct Fire All");
                    break;
                }
            case MissileDefenseType.DirectFireHoming:
                {
                    stringBuilder.AppendLine("Missile Defense: Direct Fire Homing");
                    break;
                }
        }

        if (RadarSignatureReduction != 0)
        {
            stringBuilder.AppendLine("Radar Signature Reduction: " + (RadarSignatureReduction * 10f).ToString("0.") + "m");
        }

        if (RadarRangeBonus != 0)
        {
            stringBuilder.AppendLine("Radar Range Bonus: +" + (RadarRangeBonus * 10f).ToString("0.") + "m");
        }

        if (TargetLockOnBonus != 0)
        {
            stringBuilder.AppendLine("Lock-On Bonus: +" + (TargetLockOnBonus * 100f).ToString("0.") + "%");
        }

        return stringBuilder.ToString().TrimEnd(System.Environment.NewLine.ToCharArray());
    }
}
