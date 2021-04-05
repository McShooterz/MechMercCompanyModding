using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenMissionSetupBaseDefend : RandomGenMissionSetupAbstract
{
    [SerializeField]
    public GameObject[] spawnPointsEnemy = new GameObject[0];

    [SerializeField]
    public GameObject spawnPointBase;

    [SerializeField]
    public GameObject[] spawnPointsTurret = new GameObject[0];

#if UNITY_EDITOR
    public override KeyValuePair<string, Color> GetMissionValidity()
    {
        if (spawnPointsEnemy.Length != 20)
        {
            return new KeyValuePair<string, Color>("Requires 20 enemy spawn points", Color.red);
        }
        else if (navigationPoints.Length != 2)
        {
            return new KeyValuePair<string, Color>("Requires 2 navigation points", Color.red);
        }
        else if (spawnPointsTurret.Length != 8)
        {
            return new KeyValuePair<string, Color>("Requires 8 turret spawn points", Color.red);
        }
        else if (spawnPointBase == null)
        {
            return new KeyValuePair<string, Color>("Requires base spawn point", Color.red);
        }
        else
        {
            for (int i = 0; i < spawnPointsEnemy.Length; i++)
            {
                if (spawnPointsEnemy[i] == null)
                {
                    return new KeyValuePair<string, Color>("Invalid enemy spawn points", Color.red);
                }
            }

            for (int i = 0; i < navigationPoints.Length; i++)
            {
                if (navigationPoints[i] == null)
                {
                    return new KeyValuePair<string, Color>("Invalid navigation points", Color.red);
                }
            }

            for (int i = 0; i < spawnPointsTurret.Length; i++)
            {
                if (spawnPointsTurret[i] == null)
                {
                    return new KeyValuePair<string, Color>("Invalid turret spawn points", Color.red);
                }
            }

            return new KeyValuePair<string, Color>("Valid", Color.green);
        }
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        DrawSpawnPointsEnemyGizmos(spawnPointsEnemy);

        DrawTurretSpawnPointsAllyGizmos(spawnPointsTurret);

        DrawBaseSpawnPointsAllyGizmos(new GameObject[] { spawnPointBase });
    }

    public override void CreateSetupObjects(Transform landingZoneNavPoint)
    {
        navigationPoints = new Transform[2];
        navigationPoints[0] = new GameObject().transform;
        navigationPoints[0].parent = transform;
        navigationPoints[0].gameObject.name = "NavPointBase";

        if (landingZoneNavPoint != null)
        {
            navigationPoints[1] = landingZoneNavPoint;
        }

        spawnPointBase = new GameObject();
        spawnPointBase.transform.parent = transform;
        spawnPointBase.name = "SpawnPointBase";

        spawnPointsEnemy = CreateNamedGameObjects(20, "EnemySpawnPoint");

        spawnPointsTurret = CreateNamedGameObjects(8, "TurretSpawnPoint");
    }
#endif
}
