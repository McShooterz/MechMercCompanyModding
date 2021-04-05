using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GroundVehicleDamageDisplay : BaseDamageDisplay
{
    [SerializeField]
    Image armorFront;

    [SerializeField]
    Image armorRear;

    [SerializeField]
    Image armorRight;

    [SerializeField]
    Image armorLeft;

    [SerializeField]
    Image interalHealth;

    public void SetDisplays(GroundVehicleData groundVehicleData)
    {
        armorFront.color = GetArmorHealthColor(groundVehicleData.ArmorFrontPercentage);
        armorRear.color = GetArmorHealthColor(groundVehicleData.ArmorRearPercentage);
        armorLeft.color = GetArmorHealthColor(groundVehicleData.ArmorLeftPercentage);
        armorRight.color = GetArmorHealthColor(groundVehicleData.ArmorRightPercentage);

        interalHealth.color = GetInternalHealthColor(groundVehicleData.InternalHealthPercentage);
    }
}
