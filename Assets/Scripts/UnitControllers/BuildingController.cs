using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : UnitController
{
    [Header("Building Controller")]

    [SerializeField]
    BuildingData buildingData;

    [SerializeField]
    BuildingMetaController buildingMetaController;

    [SerializeField]
    TurretUnitController[] turretsToDisableOnDie = new TurretUnitController[0];

    public override UnitData UnitData { get => buildingData; }

    public BuildingData BuildingData { get => buildingData; }

    public override bool IsDestroyed { get => buildingData.isDestroyed; }

    public override Bounds Bounds { get => buildingMetaController.MainCollider.bounds; }

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }   

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public void SetData(BuildingData data)
    {
        buildingData = data;

        buildingData.buildingController = this;

        buildingMetaController = gameObject.GetComponent<BuildingMetaController>();

        gameObject.layer = LayerMask.NameToLayer("Unit");
    }

    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        buildingData.TakeDamage(damage);
    }

    public override void TakeDirectSplashDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        buildingData.TakeDamage(damage);
    }

    public override void TakeIndirectSplashDamage(Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        buildingData.TakeDamage(damage);
    }

    protected override void TakeHeatDamage(float damage)
    {

    }

    public override void Die()
    {
        buildingData.isDestroyed = true;
        buildingData.health = 0.0f;

        StartCoroutine(TurnOffRadarDetectable());

        buildingMetaController.SwitchToDestroyedModel();

        GameObject smokePrefab = ResourceManager.Instance.GetEffectPrefab("SmokeLoop");

        if ((object)smokePrefab != null)
        {
            buildingMetaController.AttachFires(smokePrefab);
        }

        buildingMetaController.SetCollidersToDebris();

        buildingMetaController.SetAnimatorsState(false);

        for (int i = 0; i < turretsToDisableOnDie.Length; i++)
        {
            turretsToDisableOnDie[i].DisableTurret();
        }

        MissionManager.Instance.UpdateObjectives();
    }

    public void CreateDeathExplosion()
    {
        GameObject explosion = PoolingManager.Instance.Spawn(ResourceManager.Instance.GetEffectPrefab("ExplosionMech"), Bounds.center, transform.rotation);

        EffectsController effectsController = explosion.GetComponent<EffectsController>();

        effectsController.AudioSource.PlayOneShot(ResourceManager.Instance.GetAudioClip("Explosion0"));
    }

    

    public override float GetRadarDetectionRange()
    {
        return buildingData.radarDetectionRange;
    }

    public override float GetRadarDetectionReduction()
    {
        return buildingData.radarDetectionReduction;
    }

    public void SetTurretsToDisableOnDie(TurretUnitController[] turrets)
    {
        turretsToDisableOnDie = turrets;
    }
}
