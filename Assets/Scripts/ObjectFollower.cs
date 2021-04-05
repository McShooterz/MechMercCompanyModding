using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField]
    GameObject followTarget;

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.transform.position;
        }
    }

    public void SetFollowTarget(GameObject target)
    {
        followTarget = target;
    }
}
