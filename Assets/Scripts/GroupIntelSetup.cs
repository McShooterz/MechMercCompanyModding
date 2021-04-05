using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupIntelSetup : MonoBehaviour
{
    public GroupIntel groupIntel;

    void LateUpdate()
    {
        Destroy(this);
    }
}
