using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenMissionSetupAssassination : RandomGenMissionSetupAbstract
{
    public GameObject[] spawnPointsEnemy = new GameObject[0];

    public GameObject[] spawnPointsAssassinationTarget = new GameObject[0];

#if UNITY_EDITOR
    public override KeyValuePair<string, Color> GetMissionValidity()
    {
        if (spawnPointsAssassinationTarget.Length != 4)
        {
            return new KeyValuePair<string, Color>("Requires 4 Assassination target spawn points", Color.red);
        }
        else if (spawnPointsEnemy.Length != 20)
        {
            return new KeyValuePair<string, Color>("Requires 20 enemy spawn points", Color.red);
        }
        else if (navigationPoints.Length != 5)
        {
            return new KeyValuePair<string, Color>("Requires 5 navigation points", Color.red);
        }
        else
        {
            foreach (GameObject spawnPoint in spawnPointsAssassinationTarget)
            {
                if (spawnPoint == null)
                {
                    return new KeyValuePair<string, Color>("Invalid assassination spawn points", Color.red);
                }
            }

            foreach (GameObject spawnPoint in spawnPointsEnemy)
            {
                if (spawnPoint == null)
                {
                    return new KeyValuePair<string, Color>("Invalid enemy spawn points", Color.red);
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

        DrawSpawnPointsEnemyGizmos(spawnPointsAssassinationTarget);
    }

    public override void CreateSetupObjects(Transform landingZoneNavPoint)
    {
        navigationPoints = new Transform[5];

        for (int i = 0; i < 4; i++)
        {
            GameObject navPointGameObject = new GameObject();
            navPointGameObject.transform.parent = transform;
            navPointGameObject.name = "NavPoint" + (i + 1).ToString();

            navigationPoints[i] = navPointGameObject.transform;
        }

        navigationPoints[4] = landingZoneNavPoint;


        spawnPointsEnemy = CreateNamedGameObjects(20, "EnemySpawnPoint");

        spawnPointsAssassinationTarget = CreateNamedGameObjects(4, "AssassinationTargetSpawnPoint");
    }
#endif
}
