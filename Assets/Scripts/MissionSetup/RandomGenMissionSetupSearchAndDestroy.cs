using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenMissionSetupSearchAndDestroy : RandomGenMissionSetupAbstract
{
    public GameObjectArray[] spawnPointGroupsEnemy = new GameObjectArray[0];

#if UNITY_EDITOR
    public override KeyValuePair<string, Color> GetMissionValidity()
    {
        if (spawnPointGroupsEnemy.Length != 4)
        {
            return new KeyValuePair<string, Color>("Requires 4 enemy groups", Color.red);
        }
        else if (navigationPoints.Length != 5)
        {
            return new KeyValuePair<string, Color>("Requires 5 navigation points", Color.red);
        }
        else
        {
            foreach (GameObjectArray spawnPointGroup in spawnPointGroupsEnemy)
            {
                if (spawnPointGroup.array.Length != 5)
                {
                    return new KeyValuePair<string, Color>("All enemy groups require 5 spawn points", Color.red);
                }

                foreach (GameObject spawnPoint in spawnPointGroup.array)
                {
                    if (spawnPoint == null)
                    {
                        return new KeyValuePair<string, Color>("Invalid enemy spawn points", Color.red);
                    }
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

        for (int i = 0; i < spawnPointGroupsEnemy.Length; i++)
        {
            DrawSpawnPointsEnemyGizmos(spawnPointGroupsEnemy[i].array);
        }
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

        spawnPointGroupsEnemy = new GameObjectArray[4];

        for (int i = 0; i < 4; i++)
        {
            spawnPointGroupsEnemy[i] = new GameObjectArray();
            spawnPointGroupsEnemy[i].array = CreateNamedGameObjects(5, "EnemySpawnPoint" + (i + 1) + "_");
        }
    }
#endif
}