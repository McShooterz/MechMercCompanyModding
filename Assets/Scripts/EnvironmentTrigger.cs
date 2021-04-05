using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class EnvironmentTrigger : MonoBehaviour
{
    protected int unitLayer;

    protected virtual void Awake()
    {
        unitLayer = LayerMask.NameToLayer("Unit");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == unitLayer)
        {
            MobileUnitController mobileUnitController = other.gameObject.GetComponent<MobileUnitController>();

            if (mobileUnitController != null)
            {
                ApplyState(mobileUnitController, true);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == unitLayer)
        {
            MobileUnitController mobileUnitController = other.gameObject.GetComponent<MobileUnitController>();

            if (mobileUnitController != null)
            {
                ApplyState(mobileUnitController, false);
            }
        }
    }

    protected abstract void ApplyState(MobileUnitController mobileUnitController, bool state);
}
