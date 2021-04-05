using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupIntel
{
    [SerializeField]
    List<UnitController> enemyRadarTargets = new List<UnitController>();

    public Vector3 targetLastDetectedPosition;

    float updateTimer = 0.0f;

    public List<UnitController> EnemyRadarTargets
    {
        get
        {
            return enemyRadarTargets;
        }
    }

    public void ClearEnemyRadarTargets()
    {
        if (Time.time > updateTimer)
        {
            updateTimer = Time.time + 0.1f;
            enemyRadarTargets.Clear();
        }
    }
}
