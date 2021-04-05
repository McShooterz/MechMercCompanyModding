using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSetupControllerBase : MonoBehaviour
{
    [SerializeField]
    protected string displayName = "";

    [SerializeField]
    protected TeamType team = TeamType.Neutral;

    public abstract void BuildUnit();
}
