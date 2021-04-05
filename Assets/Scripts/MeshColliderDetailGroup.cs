using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LODGroup))]
public class MeshColliderDetailGroup : MonoBehaviour
{
    [SerializeField]
    Renderer[] renderers;

    [SerializeField]
    MeshCollider[] meshColliders;

    [SerializeField]
    float checkTimer;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > checkTimer)
        {
            checkTimer = Time.time + 1.0f;

            bool rendererVisibleFound = false;

            for (int i = 0; i < meshColliders.Length; i++)
            {
                if (i < meshColliders.Length)
                {
                    if (i < renderers.Length && renderers[i].isVisible)
                    {
                        rendererVisibleFound = true;
                        meshColliders[i].enabled = true;
                    }
                    else
                    {
                        meshColliders[i].enabled = false;
                    }
                }
            }

            if (!rendererVisibleFound)
            {
                for (int i = 0; i < meshColliders.Length; i++)
                {
                    meshColliders[i].enabled = (i == (meshColliders.Length - 1));
                }
            }
        }
    }
}
