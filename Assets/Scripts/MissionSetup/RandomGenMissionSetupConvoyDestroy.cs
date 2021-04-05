using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenMissionSetupConvoyDestroy : RandomGenMissionSetupAbstract
{
    public GameObject[] spawnPointsEnemy = new GameObject[0];

    public GameObject[] spawnPointsConvoyEnemy = new GameObject[0];

    public Transform[] wayPointsConvoy = new Transform[0];

#if UNITY_EDITOR
    public override KeyValuePair<string, Color> GetMissionValidity()
    {
        if (spawnPointsConvoyEnemy.Length != 10)
        {
            return new KeyValuePair<string, Color>("Requires 10 convoy spawn points", Color.red);
        }
        else if (wayPointsConvoy.Length < 1)
        {
            return new KeyValuePair<string, Color>("Requires convoy ways points", Color.red);
        }
        else if (spawnPointsEnemy.Length != 20)
        {
            return new KeyValuePair<string, Color>("Requires 20 enemy spawn points", Color.red);
        }
        else if (navigationPoints.Length < 2)
        {
            return new KeyValuePair<string, Color>("Requires navigation points", Color.red);
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

            for (int i = 0; i < spawnPointsConvoyEnemy.Length; i++)
            {
                if (spawnPointsConvoyEnemy[i] == null)
                {
                    return new KeyValuePair<string, Color>("Invalid convoy spawn points", Color.red);
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

        DrawWayPointsEnemyGizmos(wayPointsConvoy);
    }

    public override void CreateSetupObjects(Transform landingZoneNavPoint)
    {
        navigationPoints = new Transform[5];

        for (int i = 0; i < 4; i++)
        {
            GameObject navPointObject = new GameObject();
            navPointObject.transform.parent = transform;
            navPointObject.gameObject.name = "NavPoint" + (i + 1).ToString();

            navigationPoints[i] = navPointObject.transform;
        }

        navigationPoints[4] = landingZoneNavPoint;

        spawnPointsEnemy = CreateNamedGameObjects(20, "EnemySpawnPoint");

        spawnPointsConvoyEnemy = CreateNamedGameObjects(10, "EnemyConvoySpawnPoint");

        GameObject[] wayPointObjects = CreateNamedGameObjects(5, "ConvoyWayPoint");
        wayPointsConvoy = new Transform[5];

        for (int i = 0; i < 5; i++)
        {
            wayPointsConvoy[i] = wayPointObjects[i].transform;
        }
    }
#endif
}