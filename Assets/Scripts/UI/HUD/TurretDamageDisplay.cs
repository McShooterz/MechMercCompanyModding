using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretDamageDisplay : BaseDamageDisplay
{
    [SerializeField]
    Image armorBase;

    [SerializeField]
    Image armorCenter;

    [SerializeField]
    Image armorPodLeft;

    [SerializeField]
    Image armorPodRight;

    [SerializeField]
    Image interalBase;

    [SerializeField]
    Image interalCenter;

    [SerializeField]
    Image interalPodLeft;

    [SerializeField]
    Image interalPodRight;

    public void SetDisplays(TurretUnitData turretUnitData)
    {
        armorBase.color = GetArmorHealthColor(turretUnitData.ArmorPercentageBase);
        armorCenter.color = GetArmorHealthColor(turretUnitData.ArmorPercentageCenter);

        interalBase.color = GetInternalHealthColor(turretUnitData.InternalPercentageBase);
        interalCenter.color = GetInternalHealthColor(turretUnitData.InternalPercentageCenter);

        if (turretUnitData.HasLeftPod)
        {
            armorPodLeft.enabled = true;
            interalPodLeft.enabled = true;
            armorPodLeft.color = GetArmorHealthColor(turretUnitData.ArmorPercentagePodLeft);
            interalPodLeft.color = GetInternalHealthColor(turretUnitData.InternalPercentagePodLeft);
        }
        else
        {
            armorPodLeft.enabled = false;
            interalPodLeft.enabled = false;
        }

        if (turretUnitData.HasPodRight)
        {
            armorPodRight.enabled = true;
            interalPodRight.enabled = true;
            armorPodRight.color = GetArmorHealthColor(turretUnitData.ArmorPercentagePodRight);
            interalPodRight.color = GetInternalHealthColor(turretUnitData.InternalPercentagePodRight);
        }
        else
        {
            armorPodRight.enabled = false;
            interalPodRight.enabled = false;
        }
    }
}
