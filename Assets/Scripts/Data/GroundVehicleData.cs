using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVehicleData : VehicleData
{
    public GroundVehicleDefinition groundVehicleDefinition;

    public GroundVehicleController groundVehicleController;

    public ComponentData[] componentDatas = new ComponentData[0];

    public AmmoPool[] ammoPools = new AmmoPool[0];

    float armorFront;

    float armorFrontMax;

    float armorRear;

    float armorRearMax;

    float armorLeft;

    float armorLeftMax;

    float armorRight;

    float armorRightMax;

    ArmorType armorType;

    public float internalHealth;

    public float internalHealthMax;

    public float ArmorFrontPercentage { get => armorFront / armorFrontMax; }

    public float ArmorRearPercentage { get => armorRear / armorRearMax; }

    public float ArmorLeftPercentage { get => armorLeft / armorLeftMax; }

    public float ArmorRightPercentage { get => armorRight / armorRightMax; }

    public float InternalHealthPercentage { get => internalHealth / internalHealthMax; }

    public override List<ComponentDefinition> ComponentsSalvagable
    {
        get
        {
            List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

            foreach (ComponentData componentData in componentDatas)
            {
                if (Random.Range(0, 2) == 0)
                {
                    componentDefinitions.Add(componentData.ComponentDefinition);
                }
            }

            return componentDefinitions;
        }
    }

    public void BuildFromDefinition(GroundVehicleDefinition definition)
    {
        groundVehicleDefinition = definition;

        customName = groundVehicleDefinition.GetDisplayName();

        armorFront = groundVehicleDefinition.FrontArmor;
        armorFrontMax = armorFront;

        armorRear = groundVehicleDefinition.RearArmor;
        armorRearMax = armorRear;

        armorLeft = groundVehicleDefinition.LeftArmor;
        armorLeftMax = armorLeft;

        armorRight = groundVehicleDefinition.RightArmor;
        armorRightMax = armorRight;

        internalHealth = groundVehicleDefinition.InternalHealth;

        armorType = groundVehicleDefinition.ArmorTypeAllSides;

        componentDatas = ComponentData.BuildComponentDatas(groundVehicleDefinition.Components);

        ammoPools = BuildAmmoPools(componentDatas);

        if (groundVehicleDefinition.ArmorTypeAllSides == ArmorType.stealth)
        {
            radarDetectionReduction = 50.0f;
        }

        for (int i = 0; i < ammoPools.Length; i++)
        {
            ammoPools[i].FullReload();
        }

        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            internalHealth += componentData.ComponentDefinition.InternalBonus;
            radarDetectionRange += componentData.ComponentDefinition.RadarRangeBonus;
            radarDetectionReduction += componentData.ComponentDefinition.RadarSignatureReduction;
            lockOnBonus += componentData.ComponentDefinition.TargetLockOnBonus;
        }

        internalHealthMax = internalHealth;
    }

    public void TakeDamageFront(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorFront > 0)
        {
            float armorDamageReduction = GetArmorDamageReduction(weaponController.GetDamageType());

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorDamageReduction == 0)
            {
                if (armorFront > damage)
                {
                    armorFront -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorFront;
                    armorFront = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorFront > damageReduced)
                {
                    armorFront -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorFront / damageReduced;
                    armorFront = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternal(damage);
        }
    }

    public void TakeDamageRear(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorRear > 0)
        {
            float armorDamageReduction = GetArmorDamageReduction(weaponController.GetDamageType());

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorDamageReduction == 0)
            {
                if (armorRear > damage)
                {
                    armorRear -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorRear;
                    armorRear = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorRear > damageReduced)
                {
                    armorRear -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorRear / damageReduced;
                    armorRear = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternal(damage);
        }
    }

    public void TakeDamageRight(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorRight > 0)
        {
            float armorDamageReduction = GetArmorDamageReduction(weaponController.GetDamageType());

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorDamageReduction == 0)
            {
                if (armorRight > damage)
                {
                    armorRight -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorRight;
                    armorRight = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorRight > damageReduced)
                {
                    armorRight -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorRight / damageReduced;
                    armorRight = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternal(damage);
        }
    }

    public void TakeDamageLeft(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorLeft > 0)
        {
            float armorDamageReduction = GetArmorDamageReduction(weaponController.GetDamageType());

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorDamageReduction == 0)
            {
                if (armorLeft > damage)
                {
                    armorLeft -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorLeft;
                    armorLeft = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorLeft > damageReduced)
                {
                    armorLeft -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorLeft / damageReduced;
                    armorLeft = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternal(damage);
        }
    }

    public void TakeDamageInternal(float damage)
    {
        if (damage >= internalHealth)
        {
            internalHealth = 0;
            Die();
        }
        else
        {
            internalHealth -= damage;
        }
    }

    protected override void Die()
    {
        groundVehicleController.CreateDeathExplosion();
        groundVehicleController.Die();
    }

    float GetArmorDamageReduction(DamageType damageType)
    {
        return ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorType, damageType);
    }
}
