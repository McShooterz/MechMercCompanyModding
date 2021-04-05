using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnvironmentLava : EnvironmentTrigger
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    protected override void ApplyState(MobileUnitController mobileUnitController, bool state)
    {
        mobileUnitController.SetIsInLava(state);
    }
}
