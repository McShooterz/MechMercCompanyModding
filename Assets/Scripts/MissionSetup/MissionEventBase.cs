using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionEventBase : MonoBehaviour
{
    protected bool wasTriggered = false;

    public abstract void Trigger();
}
