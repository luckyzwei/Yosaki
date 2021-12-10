using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMiniGameBullet : PoolItem
{
    [SerializeField]
    private Rigidbody2D rb;

    public void Initialize(Vector3 moveDir, float velocity)
    {
        rb.velocity = moveDir.normalized * velocity;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        rb.velocity = Vector3.zero;
    }
}
