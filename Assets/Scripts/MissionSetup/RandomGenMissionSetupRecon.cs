using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenMissionSetupRecon : RandomGenMissionSetupAbstract
{
    public GameObject[] spawnPointsEnemy = new GameObject[0];

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

            return new KeyValuePair<string, Color>("Valid", Color.green);
        }
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        DrawSpawnPointsEnemyGizmos(spawnPointsEnemy);
    }

    public override void CreateSetupObjects(Transform landingZoneNavPoint)
    {
        navigationPoints = new Transform[2];

        GameObject navPoint1 = new GameObject();
        navPoint1.transform.parent = transform;
        navPoint1.gameObject.name = "NavPointReconPosition";

        navigationPoints[0] = navPoint1.transform;
        navigationPoints[1] = landingZoneNavPoint;

        spawnPointsEnemy = CreateNamedGameObjects(20, "EnemySpawnPoint");
    }
#endif
}
