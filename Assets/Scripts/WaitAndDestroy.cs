using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndDestroy : MonoBehaviour
{
    [SerializeField]
    float timer = Mathf.Infinity;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestroyTime(float value)
    {
        timer = Time.time + value;
    }
}
