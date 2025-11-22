using System.Collections.Generic;
using UnityEngine;

public class FallingObjectPooler : ObjectPooler<Rigidbody>
{
    public static FallingObjectPooler Instance;
    void Awake()
    {
        Instance = this;
    }

    public List<ObjectData> fallingObject;

    private void Start()
    {
        ObjectsToPool = new List<IPoolData<Rigidbody>>(fallingObject);
        InitializePool();
    }

    public void StartFallingObjects(Utility.ObjectType objName, int count, Vector3 position, Quaternion rotation, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            ObjectData obj = fallingObject.Find(data => data.GetKey() == objName.ToString());
            Rigidbody rbObj = SetPostion_Rotation(obj, position, rotation, parent, true);
            rbObj.useGravity = true;
            rbObj.isKinematic = false;
        }
    }
    public override void ReturnToPool(Rigidbody obj)
    {
        obj.useGravity = false;
        obj.isKinematic = true;
        base.ReturnToPool(obj);
    }
}

[System.Serializable]
public class ObjectData : IPoolData<Rigidbody>
{
    public Utility.ObjectType objectType;
    public Rigidbody ObjectPrefab;
    public int PoolCount;

    public string GetKey()
    {
        return objectType.ToString();
    }

    public Rigidbody GetObject()
    {
        return ObjectPrefab;
    }

    public int GetPoolCount()
    {
        return PoolCount;
    }
}
