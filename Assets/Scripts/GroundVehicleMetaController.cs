using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVehicleMetaController : MonoBehaviour
{
    [SerializeField]
    Collider mainCollider;

    [SerializeField]
    TurretController[] turretcontrollers;

    [SerializeField]
    MeshRenderer[] tracksLeft;

    [SerializeField]
    MeshRenderer[] tracksRight;

    [SerializeField]
    Transform[] groundCheckPoints;

    [SerializeField]
    ParticleSystem[] hoverEffects;

    public Collider MainCollider { get => mainCollider; }

    public TurretController[] TurretControllers { get => turretcontrollers; }

    public MeshRenderer[] TracksLeft { get => tracksLeft; }

    public MeshRenderer[] TracksRight { get => tracksRight; }

    public Transform[] GroundCheckPoints { get => groundCheckPoints; }

    public void SetHoverEffects(bool state)
    {
        for (int i = 0; i < hoverEffects.Length; i++)
        {
            ParticleSystem.EmissionModule emissionModule = hoverEffects[i].emission;

            emissionModule.enabled = state;
        }
    }
}
