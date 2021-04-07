using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class RandomGenMissionSetupAbstract : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    bool showGizmosOnSelected = false;
#endif

    public Transform[] navigationPoints = new Transform[0];

    //Color navPointColor = new Color(0.0f, 1.0f, 1.0f, 0.25f);

    Color enemyColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);

    Color alliedColor = new Color(0.0f, 0.0f, 1.0f, 0.5f);

    Color neutralColor = new Color(1.0f, 0.92f, 0.016f, 0.5f);

    Vector3 spawnPointScaleUnit = new Vector3(1.5f, 2.25f, 1.5f);

    protected Vector3 spawnPointScaleTurret = new Vector3(2.5f, 1.5f, 2.5f);

    protected Vector3 spawnPointScaleBase = new Vector3(12.0f, 4.0f, 12.0f);

#if UNITY_EDITOR

    public abstract KeyValuePair<string, Color> GetMissionValidity();

    public abstract void CreateSetupObjects(Transform landingZoneNavPoint);

    public virtual void DrawGizmos()
    {
        DrawNavPointGizmos(navigationPoints);
    }

    protected void OnDrawGizmosSelected()
    {
        if (showGizmosOnSelected)
            DrawGizmos();
    }

    protected GameObject[] CreateNamedGameObjects(int count, string nameStart)
    {
        GameObject[] spawnPoints = new GameObject[count];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = new GameObject();
            spawnPoints[i].transform.parent = transform;
            spawnPoints[i].gameObject.name = nameStart + (i + 1).ToString();
        }

        return spawnPoints;
    }

    void DrawNavPointGizmos(Transform[] navPoints)
    {
        Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.5f);

        for (int i = 0; i < navPoints.Length; i++)
        {
            Transform navPoint = navPoints[i];

            if (navPoint != null)
            {
                Gizmos.DrawSphere(navPoint.transform.position, 3f);
                Gizmos.DrawSphere(navPoint.transform.position, 100f);
            }
        }

        if (navPoints.Length > 2)
        {
            if (navPoints[navPoints.Length - 1] != null && navPoints[0] != null)
            {
                Gizmos.DrawLine(navPoints[navPoints.Length - 1].position, navPoints[0].position);
            }

            for (int i = 0; i < navPoints.Length - 1; i++)
            {
                if (navPoints[i] != null && navPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(navPoints[i].position, navPoints[i + 1].position);
                }
            }
        }
        else if (navPoints.Length > 1)
        {
            if (navPoints[0] != null && navPoints[1] != null)
            {
                Gizmos.DrawLine(navPoints[0].position, navPoints[1].position);
            }
        }
    }

    void DrawSpawnPointGizmos(GameObject[] spawnPoints, Color color, Vector3 scale)
    {
        Gizmos.color = color;

        Vector3 offset = new Vector3(0.0f, scale.y / 2.0f, 0.0f);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject point = spawnPoints[i];

            if (point != null)
            {
                Gizmos.matrix = point.transform.localToWorldMatrix;

                Gizmos.DrawCube(Vector3.zero + offset, scale);
            }
        }

        Gizmos.matrix = Matrix4x4.identity;
    }

    protected void DrawWayPointGizmos(Transform[] wayPoints, float scale, Color color)
    {
        Gizmos.color = color;

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Transform wayPoint = wayPoints[i];

            if (wayPoint != null)
            {
                Gizmos.DrawSphere(wayPoint.transform.position, scale);
            }
        }

        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            if (wayPoints[i] != null && wayPoints[i + 1] != null)
            {
                Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
            }
        }
    }

    protected void DrawSpawnPointsEnemyGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, enemyColor, spawnPointScaleUnit);
    }

    protected void DrawSpawnPointsAllyGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, alliedColor, spawnPointScaleUnit);
    }

    protected void DrawSpawnPointsNeutralGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, neutralColor, spawnPointScaleUnit);
    }

    protected void DrawTurretSpawnPointsEnemyGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, enemyColor, spawnPointScaleTurret);
    }

    protected void DrawTurretSpawnPointsAllyGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, alliedColor, spawnPointScaleTurret);
    }

    protected void DrawBaseSpawnPointsEnemyGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, enemyColor, spawnPointScaleBase);
    }

    protected void DrawBaseSpawnPointsAllyGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, alliedColor, spawnPointScaleBase);
    }

    protected void DrawBaseSpawnPointsNeutralGizmos(GameObject[] spawnPoints)
    {
        DrawSpawnPointGizmos(spawnPoints, neutralColor, spawnPointScaleBase);
    }

    protected void DrawWayPointsEnemyGizmos(Transform[] wayPoints)
    {
        DrawWayPointGizmos(wayPoints, 1.2f, enemyColor);
    }

    protected void AttachNavMeshObstacles(GameObject[] targetObjects, Vector3 scale)
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            GameObject targetObject = targetObjects[i];

            NavMeshObstacle navMeshObstacle = targetObject.AddComponent<NavMeshObstacle>();

            navMeshObstacle.size = new Vector3(scale.x, 100, scale.z);
        }
    }
#endif
}
