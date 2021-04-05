using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMetaController : MonoBehaviour
{
    [SerializeField]
    Collider mainCollider;

    [SerializeField]
    GameObject normalModelRoot;

    [SerializeField]
    GameObject destroyedModelRoot;

    [SerializeField]
    Transform[] fireLocations;

    [SerializeField]
    Animator[] animators;

    [SerializeField]
    DebrisController[] debrisControllers;

    public Collider MainCollider { get => mainCollider; }

    public void SwitchToDestroyedModel()
    {
        if (normalModelRoot != null && destroyedModelRoot != null)
        {
            normalModelRoot.SetActive(false);
            destroyedModelRoot.SetActive(true);
        }
    }

    public void AttachFires(GameObject firePrefab)
    {
        if (fireLocations.Length > 0)
        {
            for (int i = 0; i < fireLocations.Length; i++)
            {
                Transform fireLocation = fireLocations[i];

                if (fireLocation != null)
                {
                    Instantiate(firePrefab, fireLocation);
                }
            }
        }
        else
        {
            GameObject fireInstance = Instantiate(firePrefab);

            fireInstance.transform.position = mainCollider.bounds.center;
            fireInstance.transform.parent = transform;
        }
    }

    public void SetCollidersToDebris()
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        if (colliders.Length > 0)
        {
            int debrisLayer = LayerMask.NameToLayer("Debris");

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].gameObject.layer = debrisLayer;
            }
        }
    }

    public void SetAnimatorsState(bool state)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = state;
        }
    }
}
