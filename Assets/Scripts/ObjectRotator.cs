using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationAxis = Vector3.up;

    [SerializeField]
    float rotationRate = 100.0f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, rotationAxis, rotationRate * Time.unscaledDeltaTime);
    }

    public void SetRotationAxis(Vector3 axis)
    {
        rotationAxis = axis;
    }

    public void SetRotationRate(float rate)
    {
        rotationRate = rate;
    }
}
