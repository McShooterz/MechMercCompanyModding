using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorUI : MonoBehaviour
{
    [SerializeField]
    float rotationRate = 60f;

    [SerializeField]
    Vector3 rotation = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        rotation += Vector3.forward * rotationRate * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);
	}
}
