using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Warning: rotation and pitch point must be different")]
    Transform rotationPoint;

    [SerializeField]
    [Tooltip("Warning: rotation and pitch point must be different")]
    Transform pitchPoint;

    [SerializeField]
    Transform aimPoint;

    [SerializeField]
    WeaponModelController[] weaponModelControllers;

    [SerializeField]
    LayerMask targetingLayerMask;

    [SerializeField]
    float currentHorizontalRotation;

    [SerializeField]
    float currentVerticalRotation;

    [SerializeField]
    Quaternion startingRotationPoint;

    [SerializeField]
    Quaternion startingPitchPoint;

    public float HorizontalRotationSpeed { get; private set; } = 0.0f;

    public float HorizontalRotationLimit { get; private set; } = 0.0f;

    public float VerticalRotationSpeed { get; private set; } = 0.0f;

    public float VerticalRotationLimitUp { get; private set; } = 0.0f;

    public float VerticalRotationLimitDown { get; private set; } = 0.0f;

    public WeaponModelController[] WeaponModelControllers { get => weaponModelControllers; }

    void Awake()
    {
        if (startingRotationPoint == startingPitchPoint)
        {
            Debug.LogError("Error: rotation and pitch point must be different");
        }

        startingRotationPoint = rotationPoint.localRotation;

        startingPitchPoint = pitchPoint.localRotation;

        targetingLayerMask |= (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("EnvironmentDestructible"));
    }

    public Vector3 TargetingPoint
    {
        get
        {
            Ray ray = new Ray(aimPoint.position, aimPoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetingLayerMask, QueryTriggerInteraction.Ignore))
            {
                return hit.point;
            }
            else
            {
                return aimPoint.position + aimPoint.forward * 100f;
            }
        }
    }

    public float AimAtPosition(Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - aimPoint.position;

        RotateHorizontal(new Vector2(targetDirection.x, targetDirection.z));

        RotateVertical(new Vector2(targetDirection.y, Mathf.Abs(targetDirection.z)));

        return Vector3.Angle(aimPoint.forward, targetDirection);
    }

    void RotateHorizontal(Vector2 direction)
    {
        float angleHorizontal = Vector2.SignedAngle(new Vector2(rotationPoint.forward.x, rotationPoint.forward.z), direction);

        float targetHorizontalRotation = currentHorizontalRotation;

        if (angleHorizontal > 0.0f)
        {
            float change = Mathf.Max(-angleHorizontal, -HorizontalRotationSpeed * Time.deltaTime);

            targetHorizontalRotation += change;
        }
        else if (angleHorizontal < 0.0f)
        {
            float change = Mathf.Min(-angleHorizontal, HorizontalRotationSpeed * Time.deltaTime);

            targetHorizontalRotation += change;
        }

        rotationPoint.localRotation = startingRotationPoint;

        if (HorizontalRotationLimit < 180)
        {
            currentHorizontalRotation = Mathf.Clamp(targetHorizontalRotation, -HorizontalRotationLimit, HorizontalRotationLimit);
        }
        else
        {
            currentHorizontalRotation = targetHorizontalRotation;
        }

        rotationPoint.RotateAround(rotationPoint.position, rotationPoint.up, currentHorizontalRotation);
    }

    void RotateVertical(Vector2 direction)
    {
        float angleVertical = Vector2.SignedAngle(new Vector2(aimPoint.forward.y, Mathf.Abs(aimPoint.forward.z)), direction);

        float targetVerticalRotation = currentVerticalRotation;

        if (angleVertical > 0)
        {
            targetVerticalRotation += VerticalRotationSpeed * Time.deltaTime;

            //float change = Mathf.Max(-angleVertical, -VerticalRotationSpeed * Time.deltaTime);

            //targetVerticalRotation += change;
        }
        else if (angleVertical < 0)
        {
            targetVerticalRotation -= VerticalRotationSpeed * Time.deltaTime;

            //float change = Mathf.Min(-angleVertical, VerticalRotationSpeed * Time.deltaTime);

            //targetVerticalRotation += change;
        }

        pitchPoint.localRotation = startingPitchPoint;

        currentVerticalRotation = Mathf.Clamp(targetVerticalRotation, VerticalRotationLimitUp, VerticalRotationLimitDown);

        pitchPoint.RotateAround(pitchPoint.position, pitchPoint.right, currentVerticalRotation);
    }

    public void SetTurretSetting(TurretSetting turretSetting)
    {
        HorizontalRotationSpeed = turretSetting.HorizontalSpeed;
        HorizontalRotationLimit = Mathf.Clamp(turretSetting.HorizontalLimit, 0, 180);
        VerticalRotationSpeed = turretSetting.VerticalSpeed;
        VerticalRotationLimitUp = Mathf.Clamp(-turretSetting.VerticalLimitUp, -90.0f, 0.0f);
        VerticalRotationLimitDown = Mathf.Clamp(turretSetting.VerticalLimitDown, 0.0f, 90.0f);
    }
}
