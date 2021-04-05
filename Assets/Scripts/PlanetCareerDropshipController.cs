using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCareerDropshipController : MonoBehaviour
{
    [SerializeField]
    GameObject planetObject;

    [SerializeField]
    float planetScale = 1.0f;

    [SerializeField]
    Vector3 randomRotationAxis;

    void Start()
    {
        if (planetObject != null)
            Destroy(planetObject);

        GameObject planetPrefab = GlobalDataManager.Instance.currentCareer.currentContract.PlanetDefinition.GetPrefab();

        if ((object)planetPrefab != null)
        {
            planetObject = Instantiate(planetPrefab, transform);

            planetObject.transform.localScale = new Vector3(planetScale, planetScale, planetScale);
        }

        randomRotationAxis = Random.onUnitSphere;
    }

    void Update()
    {
        if ((object)planetObject != null)
        {
            planetObject.transform.RotateAround(transform.position, randomRotationAxis, Time.unscaledDeltaTime);
        }
    }
}