using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundVehicleMovementControllerBase : MonoBehaviour
{
    [SerializeField]
    protected GroundVehicleMetaController groundVehicleMetaController;

    [SerializeField]
    protected Rigidbody attachedRigidbody;

    [SerializeField]
    protected float groundCheckTimer = 0.0f;

    [SerializeField]
    protected LayerMask groundCheckLayerMask;

    [SerializeField]
    protected bool isGrounded = false;

    protected GroundVehicleDefinition groundVehicleDefinition;

    protected float flippedTimer = 0.0f;

    protected bool lastIsFlipped = false;

    protected bool IsFlipped { get => Vector3.Dot(transform.up, Vector3.up) < 0.125f; }

    protected virtual void Update()
    {
        bool isFlipped = IsFlipped;

        if (isFlipped)
        {
            if (!lastIsFlipped)
            {
                flippedTimer = Time.time + 2.0f;
            }

            if (Time.time > flippedTimer)
            {
                FlipBackOver();
            }
        }
        else
        {
            flippedTimer = Mathf.Infinity;
        }

        lastIsFlipped = isFlipped;
    }    

    public virtual void Initialize(GroundVehicleMetaController metaController, GroundVehicleDefinition definition)
    {
        groundVehicleMetaController = metaController;
        groundVehicleDefinition = definition;

        AttachRigidbody();

        SetGroundCheckLayerMask();
    }

    protected virtual void AttachRigidbody()
    {
        attachedRigidbody = gameObject.AddComponent<Rigidbody>();
        attachedRigidbody.useGravity = true;
        attachedRigidbody.mass = groundVehicleDefinition.PhysicsMass;
        attachedRigidbody.drag = groundVehicleDefinition.PhysicsFriction;
        attachedRigidbody.angularDrag = 0.2f;
        attachedRigidbody.centerOfMass = Vector3.zero;
    }

    protected virtual void SetGroundCheckLayerMask()
    {
        groundCheckLayerMask |= (1 << LayerMask.NameToLayer("Terrain"));
    }

    protected virtual bool CheckGrounded()
    {
        for (int index = 0; index < groundVehicleMetaController.GroundCheckPoints.Length; index++)
        {
            Transform groundCheckPoint = groundVehicleMetaController.GroundCheckPoints[index];
            Ray ray = new Ray(groundCheckPoint.position + (transform.up * 0.025f), -transform.up);

            if (Physics.Raycast(ray, 0.05f, groundCheckLayerMask, QueryTriggerInteraction.Ignore))
            {
                groundCheckTimer = Time.time + 0.1f;
                return true;
            }
        }
        groundCheckTimer = Time.time + 0.25f;

        return false;
    }

    public virtual bool MoveTowardsTargetPosition(Vector3 position)
    {
        if (Time.time > groundCheckTimer)
        {
            isGrounded = CheckGrounded();
        }

        if (isGrounded)
        {
            Vector3 direction = position - transform.position;
            float distanceSqr = direction.sqrMagnitude;

            if (distanceSqr > 1.0f)
            {
                float angle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(direction.x, direction.z));

                if (angle > 1.0f)
                {
                    attachedRigidbody.AddTorque(transform.up * -groundVehicleDefinition.EngineTurnForce * Time.deltaTime);
                }
                else if (angle < -1.0f)
                {
                    attachedRigidbody.AddTorque(transform.up * groundVehicleDefinition.EngineTurnForce * Time.deltaTime);
                }

                if (Mathf.Abs(angle) < 30.0f)
                {
                    if (distanceSqr > 3.0f)
                    {
                        attachedRigidbody.AddForce(transform.forward * groundVehicleDefinition.EngineForce * Time.deltaTime);

                        if (attachedRigidbody.velocity.magnitude > groundVehicleDefinition.MaxSpeed)
                        {
                            attachedRigidbody.velocity = attachedRigidbody.velocity.normalized * groundVehicleDefinition.MaxSpeed;
                        }
                    }
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    protected void FlipBackOver()
    {
        float horizontalBalance = Vector3.Dot(transform.right, Vector3.up);
        float verticalBalance = Vector3.Dot(transform.forward, Vector3.up);

        if (horizontalBalance > 0)
        {
            attachedRigidbody.AddTorque(transform.forward * -1500 * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            attachedRigidbody.AddTorque(transform.forward * 1500 * Time.deltaTime, ForceMode.Acceleration);
        }

        if (verticalBalance > 0)
        {
            attachedRigidbody.AddTorque(transform.right * 1500 * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            attachedRigidbody.AddTorque(transform.right * -1500 * Time.deltaTime, ForceMode.Acceleration);
        }

        attachedRigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
    }

    public virtual void SetDestroyed()
    {
        enabled = false;
    }
}