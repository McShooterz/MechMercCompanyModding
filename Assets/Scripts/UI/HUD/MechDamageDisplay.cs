using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class MechDamageDisplay : BaseDamageDisplay
{

    [SerializeField]
    Image mechHeadArmor;

    [SerializeField]
    Image mechHeadInternal;

    [SerializeField]
    Image mechTorsoCenterArmor;

    [SerializeField]
    Image mechTorsoCenterInternal;

    [SerializeField]
    Image mechTorsoLeftArmor;

    [SerializeField]
    Image mechTorsoLeftInternal;

    [SerializeField]
    Image mechTorsoRightArmor;

    [SerializeField]
    Image mechTorsoRightInternal;

    [SerializeField]
    Image mechLegLeftArmor;

    [SerializeField]
    Image mechLegLeftInternal;

    [SerializeField]
    Image mechLegRightArmor;

    [SerializeField]
    Image mechLegRightInternal;

    [SerializeField]
    Image mechArmLeftArmor;

    [SerializeField]
    Image mechArmLeftInternal;

    [SerializeField]
    Image mechArmRightArmor;

    [SerializeField]
    Image mechArmRightInternal;

    [SerializeField]
    Image mechTorsoCenterRearArmor;

    [SerializeField]
    Image mechTorsoCenterRearInternal;

    [SerializeField]
    Image mechTorsoLeftRearArmor;

    [SerializeField]
    Image mechTorsoLeftRearInternal;

    [SerializeField]
    Image mechTorsoRightRearArmor;

    [SerializeField]
    Image mechTorsoRightRearInternal;

    public void SetDisplays(MechData mechData)
    {
        mechHeadArmor.color = GetArmorHealthColor(mechData.ArmorPercentHead);
        mechTorsoCenterArmor.color = GetArmorHealthColor(mechData.ArmorPercentTorsoCenter);
        mechTorsoCenterRearArmor.color = GetArmorHealthColor(mechData.ArmorPercentTorsoCenterRear);
        mechTorsoLeftArmor.color = GetArmorHealthColor(mechData.ArmorPercentTorsoLeft);
        mechTorsoLeftRearArmor.color = GetArmorHealthColor(mechData.ArmorPercentTorsoLeftRear);
        mechTorsoRightArmor.color = GetArmorHealthColor(mechData.ArmorPercentTorsoRight);
        mechTorsoRightRearArmor.color = GetArmorHealthColor(mechData.ArmorPercentTorsoRightRear);
        mechArmLeftArmor.color = GetArmorHealthColor(mechData.ArmorPercentArmLeft);
        mechArmRightArmor.color = GetArmorHealthColor(mechData.ArmorPercentArmRight);
        mechLegLeftArmor.color = GetArmorHealthColor(mechData.ArmorPercentLegLeft);
        mechLegRightArmor.color = GetArmorHealthColor(mechData.ArmorPercentLegRight);

        mechHeadInternal.color = GetInternalHealthColor(mechData.InternalPercentHead);
        mechTorsoCenterInternal.color = GetInternalHealthColor(mechData.InternalPercentTorsoCenter);
        mechTorsoCenterRearInternal.color = mechTorsoCenterInternal.color;
        mechTorsoLeftInternal.color = GetInternalHealthColor(mechData.InternalPercentTorsoLeft);
        mechTorsoLeftRearInternal.color = mechTorsoLeftInternal.color;
        mechTorsoRightInternal.color = GetInternalHealthColor(mechData.InternalPercentTorsoRight);
        mechTorsoRightRearInternal.color = mechTorsoRightInternal.color;
        mechArmLeftInternal.color = GetInternalHealthColor(mechData.InternalPercentArmLeft);
        mechArmRightInternal.color = GetInternalHealthColor(mechData.InternalPercentArmRight);
        mechLegLeftInternal.color = GetInternalHealthColor(mechData.InternalPercentLegLeft);
        mechLegRightInternal.color = GetInternalHealthColor(mechData.InternalPercentLegRight);
    }
}
