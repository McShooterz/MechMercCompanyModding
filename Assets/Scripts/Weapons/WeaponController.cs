using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected WeaponModelController weaponModelController;

    [SerializeField]
    protected CombatUnitController owner;

    public WeaponGrouping weaponGrouping;

    protected ComponentData weaponModifier;

    [SerializeField]
    protected bool isDestroyed;

    [SerializeField]
    protected float jamming = 0.0f;

    [SerializeField]
    protected bool jammed = false;

    [SerializeField]
    protected float jammingDecayTimer;

    public int targetingIndex = 0;

    public float DamageModifier { get; protected set; } = 1.0f;

    protected float HeatModified { get; set; }

    public abstract bool CanFire { get; }

    public bool IsDestroyed
    {
        get
        {
            return isDestroyed;
        }
    }

    public CombatUnitController Owner
    {
        get
        {
            return owner;
        }
    }

    public float Jamming { get => jamming; }

    public bool Jammed { get => jammed; }

    public virtual bool IsCharged { get => false; }

    public void SetOwner(CombatUnitController owner)
    {
        this.owner = owner;
    }

    public virtual void SetDestroyed()
    {
        isDestroyed = true;
        weaponModelController.gameObject.SetActive(false);
    }

    public abstract string GetDisplayName();

    public void SetWeaponModelController(WeaponModelController controller)
    {
        weaponModelController = controller;

        if (weaponModelController != null)
        {
            weaponModelController.InitializeAudioSource();
        }
        else
        {
            Debug.LogError("Error: weapon model controller is null");
        }
    }

    public abstract void Fire(bool firedThisFrame);

    public abstract void FireAI();

    public virtual void Stop()
    {
        weaponModelController.SetAnimatorsState(false);
    }

    public abstract float GetRefireBar();

    public abstract bool InMaxRange(float distanceSqr);

    public abstract bool InEffectiveRange(float distanceSqr);

    public abstract float GetRangeEffective();

    public abstract DamageType GetDamageType();

    public abstract bool GetCriticalRole();

    public abstract float GetCriticalDamageMulti();

    public abstract void SetWeaponModifier(ComponentData componentData);

    protected void UpdateJamming(float jammingDecay, float jammedDecay)
    {
        if (jamming > 0.0f)
        {
            if (jammed)
            {
                jamming -= jammedDecay * Time.deltaTime;

                if (jamming <= 0.0f)
                {
                    jamming = 0.0f;
                    jammed = false;
                }
            }
            else if (Time.time > jammingDecayTimer)
            {
                jamming -= jammingDecay * Time.deltaTime;

                if (jamming < 0.0f)
                {
                    jamming = 0.0f;
                }
            }
        }
    }

    public abstract void SetDefaultModifiers();
}
