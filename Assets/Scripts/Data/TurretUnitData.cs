using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUnitData : CombatUnitData
{
    public TurretDefinition turretDefinition;

    public TurretUnitController turretUnitController;

    public ComponentData[] componentDatasBase = new ComponentData[0];

    public ComponentData[] componentDatasCenter = new ComponentData[0];

    public ComponentData[] componentDatasPodLeft = new ComponentData[0];

    public ComponentData[] componentDatasPodRight = new ComponentData[0];

    public AmmoPool[] ammoPools = new AmmoPool[0];

    public float gunnerySkill = 0.0f;

    ArmorType armorType;

    float armorBase;

    float armorCenter;

    float armorPodLeft;

    float armorPodRight;

    float armorBaseMax;

    float armorCenterMax;

    float armorPodLeftMax;

    float armorPodRightMax;

    float internalBase;

    float internalCenter;

    float internalPodLeft;

    float internalPodRight;

    float internalBaseMax;

    float internalCenterMax;

    float internalPodLeftMax;

    float internalPodRightMax;

    public float ArmorPercentageBase { get => armorBase / armorBaseMax; }

    public float ArmorPercentageCenter { get => armorCenter / armorCenterMax; }

    public float ArmorPercentagePodLeft { get => armorPodLeft / armorPodLeftMax; }

    public float ArmorPercentagePodRight { get => armorPodRight / armorPodRightMax; }

    public float InternalPercentageBase { get => internalBase / internalBaseMax; }

    public float InternalPercentageCenter { get => internalCenter / internalCenterMax; }

    public float InternalPercentagePodLeft { get => internalPodLeft / internalPodLeftMax; }

    public float InternalPercentagePodRight { get => internalPodRight / internalPodRightMax; }

    public bool HasLeftPod { get => internalPodLeftMax > 0.0f; }

    public bool HasPodRight { get => internalPodRightMax > 0.0f; }

    public List<ComponentData> ComponentDatas
    {
        get
        {
            List<ComponentData> componentDatas = new List<ComponentData>();

            componentDatas.AddRange(componentDatasBase);
            componentDatas.AddRange(componentDatasCenter);
            componentDatas.AddRange(componentDatasPodLeft);
            componentDatas.AddRange(componentDatasPodRight);

            return componentDatas;
        }
    }

    public void BuildFromDefinition(TurretDefinition definition)
    {
        turretDefinition = definition;

        customName = turretDefinition.GetDisplayName();

        armorType = turretDefinition.ArmorType;

        armorBase = turretDefinition.ArmorBase;
        armorCenter = turretDefinition.ArmorCenter;
        armorPodLeft = turretDefinition.ArmorPodLeft;
        armorPodRight = turretDefinition.ArmorPodRight;

        armorBaseMax = armorBase;
        armorCenterMax = armorCenter;
        armorPodLeftMax = armorPodLeft;
        armorPodRightMax = armorPodRight;

        internalBase = turretDefinition.InternalBase;
        internalCenter = turretDefinition.InternalCenter;
        internalPodLeft = turretDefinition.InternalPodLeft;
        internalPodRight = turretDefinition.InternalPodRight;

        internalBaseMax = internalBase;
        internalCenterMax = internalCenter;
        internalPodLeftMax = internalPodLeft;
        internalPodRightMax = internalPodRight;

        componentDatasBase = BuildComponentDatas(turretDefinition.ComponentsBase);

        componentDatasCenter = BuildComponentDatas(turretDefinition.ComponentsCenter);

        componentDatasPodLeft = BuildComponentDatas(turretDefinition.ComponentsPodLeft);

        componentDatasPodRight = BuildComponentDatas(turretDefinition.ComponentsPodRight);

        List<ComponentData> componentDatasAll = new List<ComponentData>();

        componentDatasAll.AddRange(componentDatasBase);
        componentDatasAll.AddRange(componentDatasCenter);
        componentDatasAll.AddRange(componentDatasPodLeft);
        componentDatasAll.AddRange(componentDatasPodRight);

        ammoPools = BuildAmmoPools(componentDatasAll.ToArray());

        if (turretDefinition.ArmorType == ArmorType.stealth)
        {
            radarDetectionReduction = 50.0f;
        }

        for (int i = 0; i < ammoPools.Length; i++)
        {
            ammoPools[i].FullReload();
        }

        for (int i = 0; i < componentDatasAll.Count; i++)
        {
            ComponentData componentData = componentDatasAll[i];

            radarDetectionRange += componentData.ComponentDefinition.RadarRangeBonus;
            radarDetectionReduction += componentData.ComponentDefinition.RadarSignatureReduction;
            lockOnBonus += componentData.ComponentDefinition.TargetLockOnBonus;
        }
    }

    public void TakeDamageBase(float damage, WeaponController weaponController)
    {
        float damageRemainder = DamageArmor(ref armorBase, armorType, damage, weaponController);

        if (damageRemainder > 0)
        {
            TakeDamageInternalBase(damageRemainder);
        }
    }

    public void TakeDamageCenter(float damage, WeaponController weaponController)
    {
        float damageRemainder = DamageArmor(ref armorCenter, armorType, damage, weaponController);

        if (damageRemainder > 0)
        {
            TakeDamageInternalCenter(damageRemainder);
        }
    }

    public void TakeDamagePodLeft(float damage, WeaponController weaponController)
    {
        float damageRemainder = DamageArmor(ref armorPodLeft, armorType, damage, weaponController);

        if (damageRemainder > 0)
        {
            TakeDamageInternalPodLeft(damageRemainder);
        }
    }

    public void TakeDamagePodRight(float damage, WeaponController weaponController)
    {
        float damageRemainder = DamageArmor(ref armorPodRight, armorType, damage, weaponController);

        if (damageRemainder > 0)
        {
            TakeDamageInternalPodRight(damageRemainder);
        }
    }

    public void TakeDamageInternalBase(float damage)
    {
        if (damage >= internalBase)
        {
            internalBase = 0;
            DestroyComponents(componentDatasBase);
            Die();
        }
        else
        {
            internalBase -= damage;
        }
    }

    public void TakeDamageInternalCenter(float damage)
    {
        if (damage >= internalCenter)
        {
            internalCenter = 0;
            DestroyComponents(componentDatasCenter);
            Die();
        }
        else
        {
            internalCenter -= damage;
        }
    }

    public void TakeDamageInternalPodLeft(float damage)
    {
        if (damage >= internalPodLeft)
        {
            internalPodLeft = 0;
            turretUnitController.EjectPodLeft();
            DestroyComponents(componentDatasPodLeft);
        }
        else
        {
            internalPodLeft -= damage;
        }
    }

    public void TakeDamageInternalPodRight(float damage)
    {
        if (damage >= internalPodRight)
        {
            internalPodRight = 0;
            turretUnitController.EjectPodRight();
            DestroyComponents(componentDatasPodRight);
        }
        else
        {
            internalPodRight -= damage;
        }
    }

    ComponentData[] BuildComponentDatas(string[] componentKeys)
    {
        List<ComponentData> componentDatasList = new List<ComponentData>();

        for (int i = 0; i < componentKeys.Length; i++)
        {
            string componentKey = componentKeys[i];

            ComponentDefinition componentDefinition = ResourceManager.Instance.GetComponentDefinition(componentKey);

            if (componentDefinition != null)
            {
                componentDatasList.Add(new ComponentData(componentDefinition, new WeaponGrouping(), 0));
            }
        }

        return componentDatasList.ToArray();
    }

    protected override void Die()
    {
        turretUnitController.CreateDeathExplosion();
        turretUnitController.Die();
    }
}