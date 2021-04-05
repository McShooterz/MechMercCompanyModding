using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour
{
    [SerializeField]
    Rigidbody attachedRigidbody;

    [SerializeField]
    Vector3 randomForceMin;

    [SerializeField]
    Vector3 randomForceMax;

    [SerializeField]
    Vector3 randomTorqueMin;

    [SerializeField]
    Vector3 randomTorqueMax;

    [SerializeField]
    float lifeTime;

    float lifeTimer = 0.0f;

    Vector3 RandomForce { get => new Vector3(Random.Range(randomForceMin.x, randomForceMax.x), Random.Range(randomForceMin.y, randomForceMax.y), Random.Range(randomForceMin.z, randomForceMax.z)); }

    Vector3 RandomRotation { get => new Vector3(Random.Range(randomTorqueMin.x, randomTorqueMax.x), Random.Range(randomTorqueMin.y, randomTorqueMax.y), Random.Range(randomTorqueMin.z, randomTorqueMax.z)); }

    void Start()
    {
        attachedRigidbody.velocity = RandomForce;

        attachedRigidbody.AddTorque(RandomRotation, ForceMode.VelocityChange);

        lifeTimer = Time.time + lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lifeTimer)
        {
            gameObject.SetActive(false);
        }
    }
}
