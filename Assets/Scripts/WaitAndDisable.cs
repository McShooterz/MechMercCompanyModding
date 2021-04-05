using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndDisable : MonoBehaviour
{
    [SerializeField]
    float disableTime = 4f;

    [SerializeField]
    float disableTimer;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Time.time > disableTimer)
        {
            gameObject.SetActive(false);
        }
	}

    void OnEnable()
    {
        disableTimer = Time.time + disableTime;
    }
}
