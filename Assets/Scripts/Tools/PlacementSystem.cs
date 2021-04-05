using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject[] prefabs;
    public LayerMask layerMask = 1;
    public Transform group;
    public string prefabTag;
    public bool randomizePrefab;
    public int minRange = 0, maxRange = 1;
    public int toInstantiate = 0;
   
    public float radius = 1;
    public float spread = 1;
    public int amount = 1;
    public float size = 0.1f;
    public float positionOffset = 2f;

    public bool canPlaceOver;
    public bool canAlign;
    public bool isRandomS;
    public bool isRandomR;
    public bool hideInHierarchy;
#endif
}
