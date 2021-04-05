using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField]
    int modeIndex = 0;

    protected AmmoPool ammoPool;

    [SerializeField]
    protected bool ammoEmpty = false;

    public EquipmentDefinition Definition { get; protected set; }

    public EquipmentMode CurrentMode { get; protected set; } = new EquipmentMode();

    public CombatUnitController Owner { get; protected set; }

    public bool RequiresAmmo { get => Definition.AmmoType != ""; }

    public bool IsDestroyed { get; protected set; } = false;

    public bool IsDisabled { get => IsDestroyed || ammoEmpty; }

    public virtual float BarValue { get => 0.0f; }

    public string CurrentAmmo
    {
        get
        {
            if (ammoPool != null)
            {
                return ammoPool.AmmoCount.ToString();
            }

            return "";
        }
    }

    public void Initialize(CombatUnitController ownerController, EquipmentDefinition definition)
    {
        Owner = ownerController;
        Definition = definition;

        if (Definition.EquipmentModes.Length > 0)
        {
            CurrentMode = Definition.EquipmentModes[0];
        }
    }

    public void SetAmmoPool(AmmoPool[] ammoPools)
    {
        bool foundAmmoPool = false;

        for (int i = 0; i < ammoPools.Length; i++)
        {
            AmmoPool potentialAmmoPool = ammoPools[i];

            if (Definition.AmmoType == potentialAmmoPool.AmmoType)
            {
                ammoPool = potentialAmmoPool;
                foundAmmoPool = true;
                break;
            }
        }

        if (!foundAmmoPool)
        {
            ammoPool = new AmmoPool(new ComponentData[0]);
            ammoEmpty = true;
        }
    }

    public void SetDestroyed()
    {
        IsDestroyed = true;

        Owner.UpdateMissileDefenseRange();
    }

    public bool CycleMode()
    {
        if (Definition.EquipmentModes.Length > 1)
        {
            modeIndex++;

            if (modeIndex > Definition.EquipmentModes.Length - 1)
            {
                modeIndex = 0;
            }

            CurrentMode = Definition.EquipmentModes[modeIndex];

            return true;
        }

        return false;
    }
}
