using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechCustomizationStatsUI : MonoBehaviour
{
    [SerializeField]
    Text chassisText;

    [SerializeField]
    Image tonnageImage;

    [SerializeField]
    Text tonnageText;

    [SerializeField]
    Image heatLimitImage;

    [SerializeField]
    Text heatLimitText;

    [SerializeField]
    Image heatShutdownImage;

    [SerializeField]
    Text heatShutdownText;

    [SerializeField]
    Image coolingImage;

    [SerializeField]
    Text coolingText;

    [SerializeField]
    Image coolantImage;

    [SerializeField]
    Text coolantText;

    [SerializeField]
    Image speedImage;

    [SerializeField]
    Text speedText;

    [SerializeField]
    Image turningImage;

    [SerializeField]
    Text turningText;

    [SerializeField]
    Image torsoTwistImage;

    [SerializeField]
    Text torsoTwistText;

    [SerializeField]
    Image jumpJetThrustImage;

    [SerializeField]
    Text jumpJetThrustText;

    [SerializeField]
    Image armorPointsImage;

    [SerializeField]
    Text armorPointsText;

    [SerializeField]
    Image armorWeightImage;

    [SerializeField]
    Text armorWeightText;

    [SerializeField]
    Image weaponMaxRangeImage;

    [SerializeField]
    Text weaponMaxRangeText;

    [SerializeField]
    Image weaponDamageImage;

    [SerializeField]
    Text weaponDamageText;

    [SerializeField]
    Image weaponDPSImage;

    [SerializeField]
    Text weaponDPSText;

    [SerializeField]
    Image weaponHeatImage;

    [SerializeField]
    Text weaponHeatText;

    [SerializeField]
    Image weaponHPSImage;

    [SerializeField]
    Text weaponHPSText;

    MechChassisDefinition mechChassisDefinition;

    public void SetChassis(MechChassisDefinition definition)
    {
        mechChassisDefinition = definition;
        chassisText.text = mechChassisDefinition.GetDisplayName().ToUpper() + " - " + mechChassisDefinition.UnitClassDisplay.ToUpper();
    }

    public void SetTonnageCurrent(float currentTonnage)
    {
        if (currentTonnage == mechChassisDefinition.Tonnage)
        {
            tonnageImage.color = Color.white;
        }
        else if (currentTonnage > mechChassisDefinition.Tonnage)
        {
            tonnageImage.color = Color.red;
        }
        else
        {
            tonnageImage.color = Color.yellow;
        }

        tonnageText.text = currentTonnage.ToString("0.##") + "/" + mechChassisDefinition.Tonnage.ToString();
    }

    public void SetEnginePower(float enginePower)
    {
        if (enginePower > 0.0f)
        {
            speedImage.color = Color.white;
            turningImage.color = Color.white;
            torsoTwistImage.color = Color.white;
        }
        else
        {
            speedImage.color = Color.red;
            turningImage.color = Color.red;
            torsoTwistImage.color = Color.red;
        }

        speedText.text = mechChassisDefinition.GetDisplaySpeedForward(enginePower).ToString("0.0") + "/" + mechChassisDefinition.GetDisplaySpeedReverse(enginePower).ToString("0.0") + " KPH";
        turningText.text = mechChassisDefinition.GetSpeedTurn(enginePower).ToString("0.#");
        torsoTwistText.text = mechChassisDefinition.GetSpeedTorsoTwist(enginePower).ToString("0.#");
    }

    public void SetHeatLimit(float heatLimit)
    {
        if (heatLimit > 0.0f)
        {
            heatLimitImage.color = Color.white;
        }
        else
        {
            heatLimitImage.color = Color.red;
        }

        heatLimitText.text = heatLimit.ToString("0.#") + "K";
    }

    public void SetHeatShutdown(float heatShutdown)
    {
        if (heatShutdown > 0.0f)
        {
            heatShutdownImage.color = Color.white;
        }
        else
        {
            heatShutdownImage.color = Color.red;
        }

        heatShutdownText.text = heatShutdown.ToString("0.#") + "K";
    }

    public void SetCooling(float cooling)
    {
        if (cooling > 0.0f)
        {
            coolingImage.color = Color.white;
        }
        else
        {
            coolingImage.color = Color.red;
        }

        coolingText.text = cooling.ToString("0.#") + "K/s";
    }

    public void SetCoolant(float coolant)
    {
        if (coolant > 0.0f)
        {
            coolantImage.color = Color.white;
        }
        else
        {
            coolantImage.color = Color.yellow;
        }

        coolantText.text = coolant.ToString("0.#") + "kl";
    }

    public void SetJumpJetThrust(float jumpJetThrust)
    {
        jumpJetThrustText.text = jumpJetThrust.ToString("0.#");
    }

    public void SetArmorPoints(int armorPoints)
    {
        if (armorPoints > 0)
        {
            armorPointsImage.color = Color.white;
        }
        else
        {
            armorPointsImage.color = Color.yellow;
        }

        armorPointsText.text = armorPoints.ToString();
    }

    public void SetArmorWeight(float armorWeight)
    {
        armorWeightText.text = armorWeight.ToString("0.0##") + "T";
    }

    public void SetWeaponMaxRange(float range)
    {
        if (range > 0.0f)
        {
            weaponMaxRangeImage.color = Color.white;
        }
        else
        {
            weaponMaxRangeImage.color = Color.yellow;
        }

        weaponMaxRangeText.text = (range * 10).ToString("0.") + "m";
    }

    public void SetWeaponDamage(float damage)
    {
        if (damage > 0.0f)
        {
            weaponDamageImage.color = Color.white;
        }
        else
        {
            weaponDamageImage.color = Color.yellow;
        }

        weaponDamageText.text = damage.ToString("0.#");
    }

    public void SetWeaponDPS(float dps)
    {
        if (dps > 0.0f)
        {
            weaponDPSImage.color = Color.white;
        }
        else
        {
            weaponDPSImage.color = Color.yellow;
        }

        weaponDPSText.text = dps.ToString("0.##");
    }

    public void SetWeaponHeat(float weaponHeat, float heatLimit)
    {
        if (weaponHeat > 0.0f && weaponHeat < heatLimit)
        {
            weaponHeatImage.color = Color.white;
        }
        else
        {
            weaponHeatImage.color = Color.yellow;
        }

        weaponHeatText.text = weaponHeat.ToString("0.#") + "K";
    }

    public void SetWeaponHPS(float hps, float cooling)
    {
        if (hps < cooling)
        {
            weaponHPSImage.color = Color.white;
        }
        else
        {
            weaponHPSImage.color = Color.yellow;
        }

        weaponHPSText.text = hps.ToString("0.#") + "K/s";
    }
}