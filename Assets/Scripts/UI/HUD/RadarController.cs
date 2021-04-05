using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    [SerializeField]
    Transform radar;

    [SerializeField]
    Transform playerArrow;

    [SerializeField]
    Transform playerVision;

    [SerializeField]
    Transform elementContainer;

    [SerializeField]
    GameObject activeBlip;

    [SerializeField]
    List<GameObject> enemyBlips = new List<GameObject>();

    [SerializeField]
    List<GameObject> allyBlips = new List<GameObject>();

    [SerializeField]
    List<GameObject> neutralBlips = new List<GameObject>();

    [SerializeField]
    GameObject navPointBlip;

    [SerializeField]
    float radarRadius;

    //[SerializeField]
    //float radarRangeSqr = 10000;

    // Use this for initialization
    void Start ()
    {
        RectTransform rectTransform = radar.GetComponent<RectTransform>();
        radarRadius = rectTransform.rect.height / 2f * 0.95f;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateRadar(Transform origin, float visionAngle, UnitController activeBlipTarget, List<UnitController> enemyBlipReferences, List<UnitController> allyBlipReferences, List<UnitController> neutralBlipReferences, Transform navPoint)
    {
        playerVision.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, visionAngle);
        elementContainer.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, origin.eulerAngles.y);

        SetActiveBlip(origin, activeBlipTarget);

        SetEnemyBlips(origin, enemyBlipReferences);

        SetAllyBlips(origin, allyBlipReferences);

        SetNeutralBlips(origin, neutralBlipReferences);

        SetNavPointBlip(origin, navPoint);
    }

    void SetActiveBlip(Transform origin, UnitController target)
    {
        if (target != null)
        {
            activeBlip.SetActive(true);

            activeBlip.transform.localPosition = GetRadarPosition(origin, target.transform);
        }
        else
        {
            activeBlip.SetActive(false);
        }
    }

    void SetEnemyBlips(Transform origin, List<UnitController> references)
    {
        if (references.Count > enemyBlips.Count)
        {
            ResizeBlipList(references.Count, enemyBlips);
        }

        for (int i = 0; i < enemyBlips.Count; i++)
        {
            if (i < references.Count)
            {
                enemyBlips[i].SetActive(true);
                enemyBlips[i].transform.localPosition = GetRadarPosition(origin, references[i].transform);
                enemyBlips[i].transform.localRotation = Quaternion.identity;
            }
            else
            {
                enemyBlips[i].SetActive(false);
            }
        }
    }

    void SetAllyBlips(Transform origin, List<UnitController> references)
    {
        if (references.Count > allyBlips.Count)
        {
            ResizeBlipList(references.Count, allyBlips);
        }

        for (int i = 0; i < allyBlips.Count; i++)
        {
            if (i < references.Count)
            {
                allyBlips[i].SetActive(true);
                allyBlips[i].transform.localPosition = GetRadarPosition(origin, references[i].transform);
                allyBlips[i].transform.localRotation = Quaternion.identity;
            }
            else
            {
                allyBlips[i].SetActive(false);
            }
        }
    }

    void SetNeutralBlips(Transform origin, List<UnitController> references)
    {
        if (references.Count > allyBlips.Count)
        {
            ResizeBlipList(references.Count, neutralBlips);
        }

        for (int i = 0; i < neutralBlips.Count; i++)
        {
            if (i < references.Count)
            {
                neutralBlips[i].SetActive(true);
                neutralBlips[i].transform.localPosition = GetRadarPosition(origin, references[i].transform);
                neutralBlips[i].transform.localRotation = Quaternion.identity;
            }
            else
            {
                neutralBlips[i].SetActive(false);
            }
        }
    }

    void SetNavPointBlip(Transform origin, Transform target)
    {
        navPointBlip.transform.localPosition = GetRadarPosition(origin, target);
    }

    void ResizeBlipList(int count, List<GameObject> blips)
    {
        while (count > blips.Count)
        {
            GameObject blip = Instantiate(blips[0]);
            blip.transform.SetParent(blips[0].transform.parent);
            blip.transform.rotation = Quaternion.identity;
            blip.transform.localScale = Vector3.one;
            blips.Add(blip);
        }
    }

    Vector3 GetRadarPosition(Transform origin, Transform target)
    {
        Vector3 direction = target.position - origin.position;
        Vector3 position = new Vector3(direction.x, direction.z, 0f);
        position.Normalize();
        float distance = direction.magnitude;

        if (distance >= 100.0f)
        {
            position *= radarRadius;
        }
        else
        {
            position *= (distance / 100.0f) * radarRadius;
        }

        return position;
    }
}
