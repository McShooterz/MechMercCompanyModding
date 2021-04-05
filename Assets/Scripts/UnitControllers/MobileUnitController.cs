using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MobileUnitController : CombatUnitController
{
    [Header("Mobile Unit Variables")]

    [SerializeField]
    protected bool isGrounded = true;

    [SerializeField]
    protected bool isGroundedLast = true;

    [SerializeField]
    protected bool isInLava = false;

    //[SerializeField]
    //protected bool onNaveMesh;

    [SerializeField]
    protected NavMeshPath navMeshPath;

    //[SerializeField]
    //protected Vector3 targetLastDetectedPosition;

    [SerializeField]
    protected float repathTimer;

    [SerializeField]
    protected Transform currentNavigationPoint;

    protected int unitLayer;

    protected int environmentDestructibleLayer;

    [SerializeField]
    float checkFallThroughWorldTimer;

    [SerializeField]
    protected Queue<OrderBase> orders = new Queue<OrderBase>();

    public Queue<OrderBase> Orders { get => orders; }

    protected override void Awake()
    {
        base.Awake();

        navMeshPath = new NavMeshPath();

        unitLayer = LayerMask.NameToLayer("Unit");

        environmentDestructibleLayer = LayerMask.NameToLayer("EnvironmentDestructible");
    }

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

        CheckIfFallenThroughWorld();
    }

    protected abstract float GetAccelerationForward();

    protected abstract float GetAccelerationReverse();

    protected abstract float GetDeacceleration();

    //protected abstract void MoveTowardsTargetPosition(Vector3 position);

    //protected abstract void TurnTowardsDirection(Vector3 direction);

    public void SetIsInLava(bool state)
    {
        isInLava = state;
    }

    void CheckIfFallenThroughWorld()
    {
        if (Time.time > checkFallThroughWorldTimer)
        {
            checkFallThroughWorldTimer = Time.time + 10.0f;

            if (transform.position.y < -500.0f || transform.position.y > 200.0f)
            {
                Die();
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (navMeshPath != null)
        {
            Vector3[] corners = navMeshPath.corners;

            if (corners.Length > 1)
            {
                for (int i = 1; i < corners.Length; i++)
                {
                    Gizmos.DrawLine(corners[i - 1], corners[i]);
                }
            }
        }
    }
#endif
}
