using System.Collections.Generic;
using UnityEngine;


public class ObjectPooler<T> : MonoBehaviour where T : Component
{
    public List<IPoolData<T>> ObjectsToPool;
    private Dictionary<string,List<T>> pooledObjects = new();

    public void InitializePool()
    {
        foreach (IPoolData<T> objToPool in ObjectsToPool)
        {
            string key = objToPool.GetKey();
            List<T> pooledList = new();

            for (int i = 0; i < objToPool.GetPoolCount(); i++)
            {
                T objRec = objToPool.GetObject();
                T obj;
                if (objRec == null)
                {
                    var newObj = new GameObject();
                    obj = newObj.AddComponent<T>();
                }
                else
                    obj= Instantiate(objRec, transform);

                obj.transform.SetParent(transform);
                obj.gameObject.name = key;
                obj.gameObject.SetActive(false);
                pooledList.Add(obj);
            }

            pooledObjects[key] = pooledList;
        }
    }
    void ReInitializePool(IPoolData<T> fxName, int poolCount)
    {
        if (pooledObjects.ContainsKey(fxName.GetKey()))
        {
            for (int i = 0; i < poolCount; i++)
            {
                T objPooled = Instantiate(fxName.GetObject(), transform);
                objPooled.gameObject.name = fxName.ToString();
                objPooled.gameObject.SetActive(false);
                pooledObjects[fxName.GetKey()].Add(objPooled);
            }
        }
    }

    protected T GetFromPool(IPoolData<T> fxName)
    {
        if (pooledObjects.TryGetValue(fxName.GetKey(), out var pool))
        {
            foreach (T obj in pool)
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            ReInitializePool(fxName, 3);
            return GetFromPool(fxName);
        }
        return null;
    }

    protected T GetEnabledOneFromPool(string fxName)
    {
        if (pooledObjects.TryGetValue(fxName, out var pool))
        {
            foreach (T obj in pool)
                if (obj.gameObject.activeInHierarchy)
                    return obj;
        }
        return null;
    }
    public T SetPostion_Rotation(IPoolData<T> fxName,Vector3 position, Quaternion rotation, Transform parent = null, bool local = false)
    {
        T newObj = GetFromPool(fxName);
        if (parent != null)
            newObj.transform.SetParent(parent);

        if (local)
            newObj.transform.SetLocalPositionAndRotation(position, rotation);
        else
            newObj.transform.SetPositionAndRotation(position, rotation);
        return newObj;
    }

    public T SetPostion_Rotation(IPoolData<T> fxName,Vector3 position, Vector3 rotation, Transform parent = null, bool local = false)
    {
        T newObj = GetFromPool(fxName);
        if (parent != null)
            newObj.transform.SetParent(parent);

        if (local)
            newObj.transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
        else
            newObj.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        return newObj;
    }

    public virtual void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        obj.transform.position = Vector3.zero;
    }
}
