using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    public static Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    public static void ClearPool()
    {
        pool.Clear();
    }


    #region Spawn
    public static GameObject Spawn(string label, GameObject prefab)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab);
            return newObj;
        }
    }
    public static GameObject Spawn(string label, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab);
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj;
        }
    }
    public static GameObject Spawn(string label, GameObject prefab, Transform parent)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab, parent);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
                newObj.transform.parent = parent;
            }
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab, parent);
            return newObj;
        }
    }
    public static GameObject Spawn(string label, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab, parent);
            if(!newObj.activeSelf)
            {
                newObj.SetActive(true);
                newObj.transform.parent = parent;
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab, parent);
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj;
        }
    }



    public static T Spawn<T>(string label, T prefab) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab.gameObject);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab);
            return newObj.GetComponent<T>();
        }
    }
    public static T Spawn<T>(string label, T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab.gameObject);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab);
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj.GetComponent<T>();
        }
    }
    public static T Spawn<T>(string label, T prefab, Transform parent) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab.gameObject, parent);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
                newObj.transform.parent = parent;
            }
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab, parent);
            return newObj.GetComponent<T>();
        }
    }
    public static T Spawn<T>(string label, T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : GameObject.Instantiate(prefab.gameObject, parent);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
                newObj.transform.parent = parent;
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = GameObject.Instantiate(prefab, parent);
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj.GetComponent<T>();
        }
    }
    #endregion


    #region Despawn
    public static void Despawn(string label, GameObject prefab)
    {
        prefab.SetActive(false);
        if (pool.ContainsKey(label))
        {
            if (pool[label].Contains(prefab)) return;
            pool[label].Enqueue(prefab);
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            pool[label].Enqueue(prefab);
        }
    }
    #endregion


    #region Init
    public static void Init(string label, GameObject prefab)
    {
        var newObj = GameObject.Instantiate(prefab);
        Despawn(label, newObj);
    }
    public static void Init(string label, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var newObj = GameObject.Instantiate(prefab);
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        Despawn(label, newObj);
    }
    public static void Init(string label, GameObject prefab, Transform parent)
    {
        var newObj = GameObject.Instantiate(prefab, parent);
        Despawn(label, newObj);
    }
    public static void Init(string label, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        var newObj = GameObject.Instantiate(prefab, parent);
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        Despawn(label, newObj);
    }
    #endregion
}
