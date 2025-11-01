using System.Collections.Generic;
using UnityEngine;

public class SceneCacheManager : MonoBehaviour
{
    public static SceneCacheManager Instance { get; private set; }

    [System.Serializable]
    public class CacheGroup
    {
        public string groupName;
        public List<GameObject> objects = new List<GameObject>();
    }

    [Header("Caching settings")]
    public List<CacheGroup> groups = new List<CacheGroup>();

    private Dictionary<string, CacheGroup> groupLookup = new Dictionary<string, CacheGroup>();

    private Dictionary<System.Type, List<Component>> typeCache = new Dictionary<System.Type, List<Component>>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGroups();
    }

    private void InitializeGroups()
    {
        groupLookup.Clear();

        foreach (var group in groups)
        {
            if (!groupLookup.ContainsKey(group.groupName))
            {
                groupLookup.Add(group.groupName, group);
            }
        }

    }

    public void AddToGroup(string groupName, GameObject obj)
    {
        if (string.IsNullOrEmpty(groupName) || obj == null) return;

        if (!groupLookup.TryGetValue(groupName, out var group))
        {
            group = new CacheGroup { groupName = groupName };
            groups.Add(group);
            groupLookup[groupName] = group;
        }

        if (!group.objects.Contains(obj))
            group.objects.Add(obj);
    }

    public void RemoveFromGroup(string groupName, GameObject obj)
    {
        if (groupLookup.TryGetValue(groupName, out var group))
        {
            group.objects.Remove(obj);
        }
    }

    public IReadOnlyList<GameObject> GetGroup(string groupName)
    {
        return groupLookup.TryGetValue(groupName, out var group)
            ? group.objects
            : new List<GameObject>();
    }

    public void ClearGroups()
    {
        foreach (var group in groups)
        {
            group.objects.Clear();
        }

        Debug.Log("[SceneCacheManager] All cache clearing groups.");
    }

    public List<T> GetCachedOfType<T>() where T : Component
    {
        var type = typeof(T);

        if (!typeCache.TryGetValue(type, out var list))
        {
            list = new List<Component>(FindObjectsOfType<T>());
            typeCache[type] = list;
        }

        List<T> result = new List<T>();
        foreach (var comp in list)
        {
            if (comp != null) result.Add(comp as T);
        }

        return result;
    }

    public void RefreshCacheOfType<T>() where T : Component
    {
        var type = typeof(T);
        typeCache[type] = new List<Component>(FindObjectsOfType<T>());
    }

    public void ClearTypeCache()
    {
        typeCache.Clear();
        Debug.Log("[SceneCacheManager] Typical cache cleared.");
    }
}

