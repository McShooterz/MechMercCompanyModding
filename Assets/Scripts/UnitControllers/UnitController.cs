using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : MonoBehaviour
{
    #region Variables
    [Header("Base Unit Variables")]

    

    [SerializeField]
    protected bool aiControlled;

    [SerializeField]
    protected bool canBeDetectedByRadar = true;

    public abstract UnitData UnitData { get; }

    public TeamType Team { get; protected set; } = TeamType.Neutral;

    public virtual string TargetingDisplayName { get => UnitData.customName; }

    #endregion

    #region Properties
    public virtual bool IsDestroyed { get => UnitData.isDestroyed; }

    public virtual bool IsDisabled { get => UnitData.isDisabled; }

    public virtual Vector3 TargetablePosition { get => Bounds.center; }

    public virtual bool CanBeDetectedByRadar { get => canBeDetectedByRadar; private set => canBeDetectedByRadar = value; }
    
    public abstract  Bounds Bounds { get; }
    #endregion

    protected virtual void Awake()
    {

    }

    // Use this for initialization
    protected virtual void Start ()
    {
        
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {

    }

    public abstract void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController);

    public abstract void TakeDirectSplashDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController);

    public abstract void TakeIndirectSplashDamage(Vector2 direction, float damage, WeaponController weaponController);

    protected abstract void TakeHeatDamage(float damage);

    protected bool DirectionIsForward(Vector2 direction, Vector2 forward)
    {
        return Vector2.Dot(direction.normalized, forward.normalized) < 0f;
    }

    public abstract float GetRadarDetectionReduction();

    public abstract float GetRadarDetectionRange();

    public abstract void Die();

    public virtual void AddHeat(float value)
    {

    }

    public virtual void AddHeatDamage(float value)
    {

    }

    public virtual string GetWeaponsDisplay()
    {
        return "";
    }

    protected IEnumerator TurnOffRadarDetectable()
    {
        yield return new WaitForSeconds(2.0f);

        CanBeDetectedByRadar = false;
    }

    public void SetTeam(TeamType teamType)
    {
        Team = teamType;
        UnitData.teamType = teamType;
    }
}
