using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InteractableDoor : InteractableBase
{
    [SerializeField]
    string targetLocationName;

    [SerializeField]
    Transform targetLocationTransform;

    [SerializeField]
    DoorController[] doorControllers;

    public string TargetLocationName { get => targetLocationName; }

    public Transform TargetLocationTransform { get => targetLocationTransform; }

    //float doorCloseTimer = Mathf.Infinity;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
    }

    public override void StartHover()
    {
        for (int i = 0; i < doorControllers.Length; i++)
        {
            doorControllers[i].DoorAnimator.SetBool("Open", true);
        }
    }

    public override void EndHover()
    {
        for (int i = 0; i < doorControllers.Length; i++)
        {
            doorControllers[i].DoorAnimator.SetBool("Open", false);
        }
    }
}
