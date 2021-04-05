using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFoundation : MonoBehaviour
{
    [SerializeField]
    GameObject turretLocation;

    public Transform TurretLocation { get => turretLocation.transform; }
}