using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTerminal : InteractableBase
{
    [SerializeField]
    GameObject targetScreen;

    public GameObject TargetScreen { get => targetScreen; }

    protected override void Start()
    {
        base.Start();
    }
}
