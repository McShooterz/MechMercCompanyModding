using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretUnitMetaController : MonoBehaviour
{
    [SerializeField]
    TurretController turretController;

    [SerializeField]
    GameObject CenterRoot;

    [SerializeField]
    GameObject PodLeftRoot;

    [SerializeField]
    GameObject PodRightRoot;

    [SerializeField]
    Collider mainCollider;

    [SerializeField]
    Transform[] hardpointsCenter;

    [SerializeField]
    Transform[] hardpointsPodLeft;

    [SerializeField]
    Transform[] hardpointsPodRight;

    [SerializeField]
    Collider[] baseColliders;

    [SerializeField]
    Collider[] centerColliders;

    [SerializeField]
    Collider[] podLeftColliders;

    [SerializeField]
    Collider[] podRightColliders;

    public TurretController TurretController { get => turretController; }

    public Collider MainCollider { get => mainCollider; }

    public Transform[] HardpointsCenter { get => hardpointsCenter; }

    public Transform[] HardpointsPodLeft { get => hardpointsPodLeft; }

    public Transform[] HardpointsPodRight { get => hardpointsPodRight; }

    public bool IsBaseCollider(Collider targetCollider)
    {
        return baseColliders.Contains(targetCollider);
    }

    public bool IsCenterCollider(Collider targetCollider)
    {
        return centerColliders.Contains(targetCollider);
    }

    public bool IsPodLeftCollider(Collider targetCollider)
    {
        return podLeftColliders.Contains(targetCollider);
    }

    public bool IsPodRightCollider(Collider targetCollider)
    {
        return podRightColliders.Contains(targetCollider);
    }

    public void SetCollidersToDebris()
    {
        int debrisLayer = LayerMask.NameToLayer("Debris");

        mainCollider.gameObject.layer = debrisLayer;

        for (int i = 0; i < baseColliders.Length; i++)
        {
            baseColliders[i].gameObject.layer = debrisLayer;
        }

        for (int i = 0; i < centerColliders.Length; i++)
        {
            centerColliders[i].gameObject.layer = debrisLayer;
        }

        for (int i = 0; i < podLeftColliders.Length; i++)
        {
            podLeftColliders[i].gameObject.layer = debrisLayer;
        }

        for (int i = 0; i < podRightColliders.Length; i++)
        {
            podRightColliders[i].gameObject.layer = debrisLayer;
        }
    }

    public void EjectPodLeft()
    {
        if (PodLeftRoot != null)
        {
            Rigidbody podLeftRigidbody = PodLeftRoot.AddComponent<Rigidbody>();
            podLeftRigidbody.AddForce((Random.onUnitSphere + Vector3.up) * 2, ForceMode.Acceleration);
            podLeftRigidbody.AddTorque(Random.onUnitSphere * 2);

            PodLeftRoot.transform.parent = null;

            PodLeftRoot = null;
        }
    }

    public void EjectPodRight()
    {
        if (PodRightRoot != null)
        {
            Rigidbody podRightRigidbody = PodRightRoot.AddComponent<Rigidbody>();
            podRightRigidbody.AddForce((Random.onUnitSphere + Vector3.up) * 2, ForceMode.Acceleration);
            podRightRigidbody.AddTorque(Random.onUnitSphere * 2);

            PodRightRoot.transform.parent = null;

            PodRightRoot = null;
        }
    }

    public void EjectAllParts()
    {
        EjectPodLeft();

        EjectPodRight();

        if (CenterRoot != null)
        {
            Rigidbody centerRigidbody = CenterRoot.AddComponent<Rigidbody>();
            centerRigidbody.AddForce((Random.onUnitSphere + Vector3.up) * 4, ForceMode.Acceleration);
            centerRigidbody.AddTorque(Random.onUnitSphere * 2);

            CenterRoot.transform.parent = null;
        }
    }
}
