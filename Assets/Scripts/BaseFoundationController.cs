using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFoundationController : MonoBehaviour
{
    [SerializeField]
    Transform[] primaryBuildingSpots = new Transform[0];

    [SerializeField]
    Transform[] optionalBuildingSpots = new Transform[0];

    [SerializeField]
    Transform turretTowerSpot;

    [SerializeField]
    Transform powerGeneratorSpot;

    public Transform[] PrimaryBuildingSpots { get => primaryBuildingSpots; }

    public Transform[] OptionalBuildingSpots { get => optionalBuildingSpots; }

    public Transform TurretTowerSpot { get => turretTowerSpot; }

    public Transform PowerGeneratorSpot { get => powerGeneratorSpot; }
}
