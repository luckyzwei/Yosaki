using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperty<T> where T : PoolItem
{
    private ObjectPool<T> pool;
    public ObjectPool<T> Pool => pool;

    public ObjectProperty(T prefab, Transform parent, int initCount)
    {
        pool = new ObjectPool<T>(prefab, parent, initCount);
    }

    public T GetItem()
    {
        return pool.GetItem();
    }
    public void DisableAllObjects()
    {
        pool.DisableAllObject();
    }
}
