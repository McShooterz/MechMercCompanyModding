using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenMissionSetupBattleSupport : RandomGenMissionSetupAbstract
{
    public GameObject[] spawnPointsEnemy = new GameObject[0];

    public GameObject[] spawnPointsAlly = new GameObject[0];

#if UNITY_EDITOR
    public override KeyValuePair<string, Color> GetMissionValidity()
    {
        if (spawnPointsEnemy.Length != 20)
        {
            return new KeyValuePair<string, Color>("Requires 20 enemy spawn points", Color.red);
        }
        if (spawnPointsAlly.Length != 20)
        {
            return new KeyValuePair<string, Color>("Requires 20 ally spawn points", Color.red);
        }
        else if (navigationPoints.Length != 2)
        {
            return new KeyValuePair<string, Color>("Requires 2 navigation points", Color.red);
        }
        else
        {
            foreach (GameObject spawnPoint in spawnPointsEnemy)
            {
                if (spawnPoint == null)
                {
                    return new KeyValuePair<string, Color>("Invalid enemy spawn points", Color.red);
                }
            }

            foreach (GameObject spawnPoint in spawnPointsAlly)
            {
                if (spawnPoint == null)
                {
                    return new KeyValuePair<string, Color>("Invalid ally spawn points", Color.red);
                }
            }

            foreach (Transform navPoint in navigationPoints)
            {
                if (navPoint == null)
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

        DrawSpawnPointsAllyGizmos(spawnPointsAlly);
    }

    public override void CreateSetupObjects(Transform landingZoneNavPoint)
    {
        navigationPoints = new Transform[2];

        GameObject navPoint1 = new GameObject();
        navPoint1.transform.parent = transform;
        navPoint1.gameObject.name = "NavPointBattlePosition";

        navigationPoints[0] = navPoint1.transform;
        navigationPoints[1] = landingZoneNavPoint;

        spawnPointsEnemy = CreateNamedGameObjects(20, "EnemySpawnPoint");

        spawnPointsAlly = CreateNamedGameObjects(20, "AllySpawnPoint");
    }
#endif
}