using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    Dictionary<GameObject, PoolingGroup> poolingGroups = new Dictionary<GameObject, PoolingGroup>();

    void Awake()
    {
        //Make a Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Clear()
    {
        poolingGroups.Clear();
    }

    PoolingGroup GetPoolingGroup(GameObject prefab)
    {
        PoolingGroup poolingGroup;

        if (!poolingGroups.TryGetValue(prefab, out poolingGroup))
        {
            poolingGroup = new PoolingGroup(prefab);
            poolingGroups.Add(prefab, poolingGroup);
        }

        return poolingGroup;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if ((object)prefab != null)
        {
            GameObject poolMember = GetPoolingGroup(prefab).GetMember();

            poolMember.transform.parent = null;
            poolMember.transform.position = position;
            poolMember.transform.rotation = rotation;
            poolMember.SetActive(true);
            return poolMember;
        }

        return null;
    }

    public void CreateMembers(GameObject prefab, int count)
    {
        GetPoolingGroup(prefab).CreateMembers(count);
    }

    [System.Serializable]
    public class StartingPrefab
    {
        [SerializeField]
        public GameObject prefab;

        [SerializeField]
        public int count;
    }

    public class PoolingGroup
    {
        GameObject prefab;

        List<GameObject> members = new List<GameObject>();

        public PoolingGroup(GameObject basePrefab)
        {
            prefab = basePrefab;
        }

        public void CreateMembers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject createdMember = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                createdMember.SetActive(false);
                members.Add(createdMember);
            }
        }

        public GameObject GetMember()
        {
            for (int i = 0; i < members.Count; i++)
            {
                GameObject member = members[i];
                if ((object)member != null && !member.activeInHierarchy)
                {
                    return member;
                }
            }

            GameObject createdMember = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            members.Add(createdMember);
            return createdMember;
        }
    }
}
