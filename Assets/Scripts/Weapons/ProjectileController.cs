using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProjectileController : MonoBehaviour
{
    ProjectileDefinition projectileDefinition;

    ProjectileDefinition secondaryProjectileDefinition;

    [SerializeField]
    Rigidbody attachedRigidbody;

    [SerializeField]
    readonly Collider attachedCollider;

    [SerializeField]
    GameObject missileEngine;

    [SerializeField]
    WeaponController owner;

    [SerializeField]
    Vector3 startingPosition;

    [SerializeField]
    TrailController trailController;

    [SerializeField]
    UnitController homingTargetUnit;

    [SerializeField]
    Vector3 homingTargetPosition = Vector3.zero;

    [SerializeField]
    float flightHeight = -999f;

    [SerializeField]
    Vector3 homingSpread = Vector3.zero;

    [SerializeField]
    float health = 0.0f;

    [SerializeField]
    float lifeTimer = 0.0f;

    [SerializeField]
    float missileFuelTimer = 0.0f;

    [SerializeField]
    ProjectileHomingType projectileHomingType = ProjectileHomingType.None;

    int unitLayer = -1;

    int environmentDestructibleLayer = -1;

    int projectileMissileLayer = -1;

    public TeamType Team { get; private set; }

    void Awake()
    {
        unitLayer = LayerMask.NameToLayer("Unit");

        environmentDestructibleLayer = LayerMask.NameToLayer("EnvironmentDestructible");

        projectileMissileLayer = LayerMask.NameToLayer("ProjectileMissile");
    }

	void Update ()
    {
        if (Time.timeScale == 0.0f)
            return;

        if (Time.time > lifeTimer)
        {
            if (projectileDefinition.ExplodeOnLifeTime)
            {
                CreateImpactEffect(transform.position);

                if (projectileDefinition.SplashDamageRange > 0.0f)
                {
                    DealExplosionDamage();
                }
            }

            if (projectileDefinition.SecondaryProjectileFireAtEndLifeTimer)
            {
                FireSecondaryProjectiles();
            }

            DetachTrail();

            gameObject.SetActive(false);

            return;
        }

        if (missileFuelTimer > 0.0f && Time.time > missileFuelTimer)
        {
            missileFuelTimer = 0.0f;
            attachedRigidbody.useGravity = true;
            projectileHomingType = ProjectileHomingType.None;

            if (missileEngine != null)
                missileEngine.SetActive(false);

            if (projectileDefinition.Spread != 0f)
            {
                transform.Rotate(projectileDefinition.RandomSpread * 2.0f, projectileDefinition.RandomSpread * 2.0f, 0f);
            }

            attachedRigidbody.velocity = transform.forward * (projectileDefinition.Velocity / 4.0f);

            DetachTrail();
        }

        if (projectileHomingType != ProjectileHomingType.None)
        {
            Vector3 targetPosition = Vector3.zero;

            switch (projectileHomingType)
            {
                case ProjectileHomingType.LockOn:
                    {
                        if (homingTargetUnit != null)
                        {
                            targetPosition = homingTargetUnit.TargetablePosition;

                            if (homingTargetUnit is MechControllerPlayer)
                            {
                                (homingTargetUnit as MechControllerPlayer).AddMissileWarning();
                            }
                        }
                        else
                        {
                            targetPosition = homingTargetPosition;
                        }

                        break;
                    }
                case ProjectileHomingType.Auto:
                    {
                        UnitController target = owner.Owner.TargetUnit;

                        if (target != null && (!(owner.Owner is MechControllerPlayer) || target.Team == TeamType.Enemy))
                        {
                            targetPosition = target.TargetablePosition;

                            if (target is MechControllerPlayer)
                            {
                                (target as MechControllerPlayer).AddMissileWarning();
                            }
                        }

                        break;
                    }
                case ProjectileHomingType.Manual:
                    {
                        targetPosition = owner.Owner.GetTargetingPoint(0);
                        break;
                    }
            }

            if (targetPosition != Vector3.zero)
            {
                targetPosition += homingSpread;
                Vector3 direction = targetPosition - transform.position;
                float rangeSqr = (Mathf.Pow(targetPosition.x - transform.position.x, 2f) + Mathf.Pow(targetPosition.z - transform.position.z, 2f));

                if (secondaryProjectileDefinition != null && rangeSqr < projectileDefinition.SecondaryProjectileFireRangeSqr)
                {
                    FireSecondaryProjectiles();

                    DetachTrail();

                    CreateImpactEffect(transform.position);

                    gameObject.SetActive(false);
                }
                else if (rangeSqr > 1.0f)
                {
                    if (flightHeight != -999f && rangeSqr > projectileDefinition.HomingInterceptRangeSqr)
                    {
                        direction = new Vector3(targetPosition.x - transform.position.x, flightHeight - transform.position.y, targetPosition.z - transform.position.z);
                    }

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), projectileDefinition.HomingTurningRate * Time.deltaTime);
                    attachedRigidbody.velocity = transform.forward * projectileDefinition.Velocity;
                }
                else
                {
                    projectileHomingType = ProjectileHomingType.None;
                }
            }
        }
    }

    void OnEnable()
    {
        homingTargetUnit = null;
        flightHeight = -999f;

        projectileHomingType = ProjectileHomingType.None;

        if (missileEngine != null)
            missileEngine.SetActive(true);

        missileFuelTimer = 0.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        UnitController hitUnitController = null;

        if (collision.transform.root.gameObject.layer == unitLayer)
        {
            hitUnitController = collision.transform.root.gameObject.GetComponent<UnitController>();

            if (!hitUnitController.IsDestroyed)
            {
                float damage = projectileDefinition.GetDamage(GetRange()) * owner.DamageModifier;

                if (projectileDefinition.HasSplashDamage)
                {
                    if (projectileDefinition.HomingType == ProjectileHomingType.None)
                    {
                        hitUnitController.TakeDirectSplashDamage(collision.collider, new Vector2(transform.position.x - startingPosition.x, transform.position.z - startingPosition.z), damage, owner);
                    }
                    else
                    {
                        hitUnitController.TakeDirectSplashDamage(collision.collider, new Vector2(transform.forward.x, transform.forward.z), damage, owner);
                    }
                }
                else
                {
                    if (projectileDefinition.HomingType == ProjectileHomingType.None)
                    {
                        hitUnitController.TakeDamage(collision.collider, new Vector2(transform.position.x - startingPosition.x, transform.position.z - startingPosition.z), damage, owner);
                    }
                    else
                    {
                        hitUnitController.TakeDamage(collision.collider, new Vector2(transform.forward.x, transform.forward.z), damage, owner);
                    }
                }

                if (projectileDefinition.DamageHeat > 0.0f)
                {
                    hitUnitController.AddHeatDamage(projectileDefinition.DamageHeat);
                }

                if (hitUnitController is MechControllerPlayer)
                {
                    (hitUnitController as MechControllerPlayer).AddCameraShake(-transform.forward * damage);
                }
            }
        }
        else if (collision.gameObject.layer == environmentDestructibleLayer)
        {
            EnvironmentDestructible environmentDestructible = collision.collider.GetComponentInParent<EnvironmentDestructible>();

            environmentDestructible.TakeDamage(projectileDefinition.GetDamage(GetRange()) * owner.DamageModifier, transform.forward);
        }
        else if (collision.gameObject.layer == projectileMissileLayer)
        {
            return;
        }

        transform.position = collision.contacts[0].point;

        DetachTrail();

        CreateImpactEffect(collision.contacts[0].point);

        if (projectileDefinition.SplashDamageRange > 0.0f)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, projectileDefinition.SplashDamageRange, (1 << unitLayer) | (1 << environmentDestructibleLayer), QueryTriggerInteraction.Ignore);

            if (hitColliders.Length > 0)
            {
                float damageBase = projectileDefinition.GetDamage(GetRange()) * owner.DamageModifier;

                if (damageBase > 0.0f)
                {
                    foreach (Collider hitCollider in hitColliders)
                    {
                        Vector2 direction = new Vector2(hitCollider.bounds.center.x - transform.position.x, hitCollider.bounds.center.z - transform.position.z);

                        if (hitCollider.gameObject.layer == unitLayer)
                        {
                            UnitController unitController = hitCollider.GetComponent<UnitController>();

                            if (unitController.IsDestroyed || unitController == hitUnitController)
                            {
                                continue;
                            }

                            float splashDamage = damageBase * projectileDefinition.GetSplashRatio(direction.magnitude);

                            unitController.TakeIndirectSplashDamage(direction, splashDamage, owner);

                            if (unitController is MechControllerPlayer)
                            {
                                (unitController as MechControllerPlayer).AddCameraShake(-transform.forward * splashDamage);
                            }
                        }
                        else if (hitCollider.gameObject.layer == environmentDestructibleLayer)
                        {
                            EnvironmentDestructible environmentDestructible = hitCollider.GetComponent<EnvironmentDestructible>();

                            environmentDestructible.TakeDamage(damageBase * projectileDefinition.GetSplashRatio(direction.magnitude));
                        }
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }

    public void Initialize(WeaponController weaponController, ProjectileDefinition definition)
    {
        owner = weaponController;
        projectileDefinition = definition;

        if (projectileDefinition.SecondaryProjectileCount > 0)
        {
            secondaryProjectileDefinition = projectileDefinition.GetSecondaryProjectile();
        }
        else
        {
            secondaryProjectileDefinition = null;
        }

        Team = weaponController.Owner.Team;

        startingPosition = transform.position;

        lifeTimer = Time.time + projectileDefinition.ProjectileLifeTime;

        if (projectileDefinition.MissileFuelTime > 0.0f)
        {
            missileFuelTimer = Time.time + projectileDefinition.MissileFuelTime;
        }

        transform.localScale = projectileDefinition.ProjectileScaleVector3;

        if (projectileDefinition.Spread != 0f)
        {
            transform.Rotate(projectileDefinition.RandomSpread, projectileDefinition.RandomSpread, 0f);
        }

        if (attachedRigidbody == null)
        {
            attachedRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        attachedRigidbody.collisionDetectionMode = projectileDefinition.CollisionDetection;
        attachedRigidbody.interpolation = projectileDefinition.RigidbodyInterpolation;
        attachedRigidbody.velocity = transform.forward * projectileDefinition.Velocity;
        attachedRigidbody.angularVelocity = Vector3.zero;
        attachedRigidbody.useGravity = projectileDefinition.UseGravity;

        if (projectileDefinition.MissileDefenseTargetable)
        {
            attachedRigidbody.mass = 0.00001f;
        }
        else
        {
            attachedRigidbody.mass = 0.001f;
        }

        projectileHomingType = projectileDefinition.HomingType;

        if (projectileHomingType != ProjectileHomingType.None)
        {
            if (projectileDefinition.HomingSpread > 0)
            {
                homingSpread = Random.onUnitSphere * projectileDefinition.HomingSpread;
            }
            else
            {
                homingSpread = Vector3.zero;
            }
        }
    }

    public void SetTrailController(TrailController controller)
    {
        trailController = controller;
        trailController.transform.parent = transform;
        trailController.SetEmission(true);
        trailController.ClearTrailRenderers();
    }

    public void SetHomingTarget(UnitController unitController)
    {
        homingTargetUnit = unitController;
    }

    public void SetHomingTarget(Vector3 targetPosition)
    {
        homingTargetPosition = targetPosition;
    }

    public void SetFlightHeight(float height)
    {
        flightHeight = height;
    }

    public void SetHealth(float value)
    {
        health = value;
    }

    public void TakeDamage(float damage)
    {
        if (damage < health)
        {
            health -= damage;
        }
        else
        {
            if (projectileDefinition.SplashDamageRange > 0.0f)
            {
                DealExplosionDamage();
            }

            CreateImpactEffect(transform.position);

            DetachTrail();

            gameObject.SetActive(false);
        }
    }

    void CreateImpactEffect(Vector3 explosionPosition)
    {
        GameObject explosionPrefab = projectileDefinition.GetImpactEffectPrefab();

        if (explosionPrefab != null)
        {
            GameObject explosionObject = PoolingManager.Instance.Spawn(explosionPrefab, explosionPosition, transform.rotation);
            explosionObject.transform.localScale = projectileDefinition.ImpactEffectScaleVector3;

            EffectsController effectsController = explosionObject.GetComponent<EffectsController>();

            if (effectsController != null)
            {
                AudioClip impactAudioClip = projectileDefinition.GetImpactSound();

                if (impactAudioClip != null)
                {
                    effectsController.AudioSource.volume = projectileDefinition.ImpactSoundVolume;
                    AudioManager.Instance.PlayClip(effectsController.AudioSource, impactAudioClip, true, true);
                }
            }
        }
    }

    float GetRange()
    {
        return (transform.position - startingPosition).magnitude;
    }

    void DealExplosionDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, projectileDefinition.SplashDamageRange, (1 << unitLayer) | (1 << environmentDestructibleLayer), QueryTriggerInteraction.Ignore);

        if (hitColliders.Length > 0)
        {
            float damageBase = projectileDefinition.GetDamage(GetRange()) * owner.DamageModifier;

            if (damageBase > 0.0f)
            {
                foreach (Collider hitCollider in hitColliders)
                {
                    Vector2 direction = new Vector2(hitCollider.bounds.center.x - transform.position.x, hitCollider.bounds.center.z - transform.position.z);

                    if (hitCollider.gameObject.layer == unitLayer)
                    {
                        UnitController unitController = hitCollider.GetComponent<UnitController>();

                        float splashDamage = damageBase * projectileDefinition.GetSplashRatio(direction.magnitude);

                        unitController.TakeIndirectSplashDamage(direction, splashDamage * owner.DamageModifier, owner);

                        if (unitController is MechControllerPlayer)
                        {
                            (unitController as MechControllerPlayer).AddCameraShake(-transform.forward * splashDamage);
                        }
                    }
                    else if (hitCollider.gameObject.layer == environmentDestructibleLayer)
                    {
                        EnvironmentDestructible environmentDestructible = hitCollider.GetComponent<EnvironmentDestructible>();

                        environmentDestructible.TakeDamage(damageBase * projectileDefinition.GetSplashRatio(direction.magnitude));
                    }
                }
            }
        }
    }

    void DetachTrail()
    {
        if (trailController != null)
        {
            trailController.transform.parent = null;
            trailController.SetLifeTimer(projectileDefinition.TrailLifeTime);
            trailController.SetEmission(false);
        }
    }

    void FireSecondaryProjectiles()
    {
        if (secondaryProjectileDefinition == null || projectileDefinition.SecondaryProjectileCount < 1)
            return;

        GameObject projectilePrefab = secondaryProjectileDefinition.GetProjectilePrefab();

        if (projectilePrefab == null)
            return;

        GameObject trailPrefab = secondaryProjectileDefinition.GetTrailPrefab();

        for (int i = 0; i < projectileDefinition.SecondaryProjectileCount; i++)
        {
            GameObject projectileObject = PoolingManager.Instance.Spawn(projectilePrefab, transform.position, Quaternion.LookRotation(transform.forward));
            ProjectileController projectileController = projectileObject.GetComponent<ProjectileController>();

            if (projectileController != null)
            {
                projectileController.Initialize(owner, secondaryProjectileDefinition);
                projectileObject.layer = gameObject.layer;

                if (secondaryProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
                {
                    if (homingTargetUnit != null)
                    {
                        projectileController.SetHomingTarget(homingTargetUnit);
                    }
                    else
                    {
                        projectileController.SetHomingTarget(homingTargetPosition);
                    }
                }

                if ((object)trailPrefab != null)
                {
                    GameObject trailObject = PoolingManager.Instance.Spawn(trailPrefab, projectileObject.transform.position, projectileObject.transform.rotation);
                    trailObject.transform.localScale = secondaryProjectileDefinition.TrailScaleVector3;
                    TrailController trailController = trailObject.GetComponent<TrailController>();

                    if (trailController != null)
                    {
                        projectileController.SetTrailController(trailController);
                        trailController.SetScale(secondaryProjectileDefinition.TrailScale);
                    }
                }

                if (secondaryProjectileDefinition.MissileDefenseTargetable)
                {
                    projectileController.SetHealth(secondaryProjectileDefinition.MissileHealth);
                }
            }
        }
    }
}
