using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCameraController : MonoBehaviour
{
    [SerializeField]
    GameObject targetObject;

    [SerializeField]
    float rotationSpeed = 30.0f;

    [SerializeField]
    float moveSpeed = 50.0f;

    int collisionLayerMask;

    RaycastHit raycastHit;

    void Awake()
    {
        collisionLayerMask |= 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Unit") | 1 << LayerMask.NameToLayer("EnvironmentDestructible");
    }

    void Update()
    {
        transform.RotateAround(targetObject.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

        Vector3 targetPosition = targetObject.transform.position + Vector3.up;

        Vector3 desiredCameraPosition;

        Vector3 direction = new Vector3(transform.position.x - targetPosition.x, 1.0f, transform.position.z - targetPosition.z);

        if (Physics.Raycast(targetPosition, direction, out raycastHit, 2.0f, collisionLayerMask, QueryTriggerInteraction.Ignore))
        {
            desiredCameraPosition = raycastHit.point;
        }
        else
        {
            desiredCameraPosition = targetPosition + direction.normalized * 2.0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, desiredCameraPosition, moveSpeed * Time.deltaTime);

        transform.LookAt(targetPosition, Vector3.up);
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }
}
