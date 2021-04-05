using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GroundVehicleMovementControllerHovered : GroundVehicleMovementControllerBase
{
    protected override void Update()
    {
        bool isFlipped = IsFlipped;

        if (isFlipped)
        {
            isGrounded = false;

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
            isGrounded = false;
            float hoverHeight = groundVehicleDefinition.HoverHeight;
            float doubleHoverHeight = hoverHeight + hoverHeight;

            for (int i = 0; i < groundVehicleMetaController.GroundCheckPoints.Length; i++)
            {
                Transform groundCheckPoint = groundVehicleMetaController.GroundCheckPoints[i];

                if (Physics.Raycast(groundCheckPoint.position, -transform.up, out RaycastHit raycastHit, doubleHoverHeight, groundCheckLayerMask))
                {
                    float heightRatio = hoverHeight - raycastHit.distance / hoverHeight;

                    if (heightRatio < 0)
                        heightRatio *= Mathf.Abs(heightRatio);

                    attachedRigidbody.AddForceAtPosition(transform.up * heightRatio * groundVehicleDefinition.HoverForce * Time.deltaTime, groundCheckPoint.position, ForceMode.Acceleration);
                    isGrounded = true;
                }
            }
        }

        lastIsFlipped = isFlipped;
    }

    public override void Initialize(GroundVehicleMetaController metaController, GroundVehicleDefinition definition)
    {
        base.Initialize(metaController, definition);
    }

    protected override void AttachRigidbody()
    {
        attachedRigidbody = gameObject.AddComponent<Rigidbody>();
        attachedRigidbody.useGravity = true;
        attachedRigidbody.mass = groundVehicleDefinition.PhysicsMass;
        attachedRigidbody.drag = groundVehicleDefinition.PhysicsFriction;
        attachedRigidbody.angularDrag = 0.5f;
        attachedRigidbody.centerOfMass = Vector3.zero;
    }

    protected override void SetGroundCheckLayerMask()
    {
        groundCheckLayerMask |= (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("EnvironmentLava"));
    }

    public override bool MoveTowardsTargetPosition(Vector3 position)
    {
        if (isGrounded)
        {
            Vector3 direction = position - transform.position;
            float distanceSqr = direction.sqrMagnitude;

            if (distanceSqr > 1.0f)
            {
                float angle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(direction.x, direction.z));

                if (angle > 1.0f)
                {
                    attachedRigidbody.AddTorque(transform.up * -groundVehicleDefinition.EngineTurnForce * Time.deltaTime, ForceMode.Acceleration);
                }
                else if (angle < -1.0f)
                {
                    attachedRigidbody.AddTorque(transform.up * groundVehicleDefinition.EngineTurnForce * Time.deltaTime, ForceMode.Acceleration);
                }

                direction = new Vector3(direction.x, transform.position.y, direction.z);

                attachedRigidbody.AddForce(direction * groundVehicleDefinition.EngineForce * Time.deltaTime, ForceMode.Acceleration);

                //attachedRigidbody.AddTorque(Vector3.Cross(direction, transform.up) * 10 * Time.deltaTime);

                if (attachedRigidbody.velocity.magnitude > groundVehicleDefinition.MaxSpeed)
                {
                    attachedRigidbody.velocity = attachedRigidbody.velocity.normalized * groundVehicleDefinition.MaxSpeed;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public override void SetDestroyed()
    {
        base.SetDestroyed();

        attachedRigidbody.angularDrag = 0.1f;
    }
}
