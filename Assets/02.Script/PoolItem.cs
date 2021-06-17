using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItem : MonoBehaviour
{
    private Action<PoolItem> returnToPool;

    public void SetReturnEvent(Action<PoolItem> returnToPool)
    {
        this.returnToPool = returnToPool;
    }

    private void ReturnToPool()
    {
        returnToPool?.Invoke(this);
    }

    public void OnDisable()
    {
        ReturnToPool();
    }
}
