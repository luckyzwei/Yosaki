using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : PoolItem
{
    private T prefab;
    private Transform parent;
    private Queue<T> pool = new Queue<T>();
    public Queue<T> Pool => pool;
    private Dictionary<int, T> outPool = new Dictionary<int, T>();
    public Dictionary<int, T> OutPool => outPool;
    
    public ObjectPool(T prefab, Transform parent, int initCount)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initCount; i++)
        {
            MakeItem();
        }
    }
    public T GetItem()
    {
        T item = null;
        if (pool.Count == 0)
        {
            item = MakeItem();
        }

        item = pool.Dequeue();

        item.gameObject.SetActive(true);

        outPool.Add(item.GetInstanceID(), item);

        return item;
    }

    private T MakeItem()
    {
        var item = UnityEngine.Object.Instantiate(prefab, parent);
        item.gameObject.SetActive(false);
        item.transform.localPosition = Vector3.zero;
        //  item.transform.localScale = Vector3.one;
        item.SetReturnEvent(ReturnToPool);
        pool.Enqueue(item);
        return item;
    }

    private void ReturnToPool(PoolItem obj)
    {
        pool.Enqueue(obj as T);
        outPool.Remove(obj.GetInstanceID());
    }

    public void DisableAllObject()
    {
        var keys = outPool.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
        {
            outPool[keys[i]].gameObject.SetActive(false);
        }

        outPool.Clear();
    }
}
