using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitData
{
    public string customName = "";

    public bool isDestroyed = false;

    public bool isDisabled = false;

    public float radarDetectionRange = 100f;
    public float radarDetectionReduction = 0f;

    public TeamType teamType = TeamType.Neutral;

    protected abstract void Die();

    public virtual List<ComponentDefinition> ComponentsSalvagable { get => new List<ComponentDefinition>(); }

    protected float GetArmorDamageReduction(ArmorType armorType, DamageType damageType)
    {
        return ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorType, damageType);
    }

    protected float DamageArmor(ref float armor, ArmorType armorType, float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armor > 0)
        {
            float armorDamageReduction = GetArmorDamageReduction(armorType, weaponController.GetDamageType());

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorDamageReduction == 0)
            {
                if (armor > damage)
                {
                    armor -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armor;
                    armor = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armor > damageReduced)
                {
                    armor -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armor / damageReduced;
                    armor = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        return damage += armorPiercingDamage;
    }

    protected float DestroyComponents(ComponentData[] componentDatas)
    {
        float componentExplosionDamage = 0.0f;

        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            if (!componentData.isDestroyed)
            {
                componentData.SetDestroyed();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    componentExplosionDamage += componentData.ExplosionDamage;
                }

            }
        }

        return componentExplosionDamage;
    }

    protected float DestroyComponents(ComponentData[] componentDatas, ref bool ammoExplosion)
    {
        float componentExplosionDamage = 0.0f;

        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            if (!componentData.isDestroyed)
            {
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    componentExplosionDamage += componentData.ExplosionDamage;

                    if (componentData.AmmoCanExplode)
                    {
                        ammoExplosion = true;
                    }
                }

                componentData.SetDestroyed();
                componentData.SetAmmo(0);
            }
        }

        return componentExplosionDamage;
    }
}
