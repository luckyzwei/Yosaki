using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMiniGameBullet : PoolItem
{
    [SerializeField]
    private Rigidbody2D rb;

    private static WaitForSeconds disalbeWs = new WaitForSeconds(5f);

    public void Initialize(Vector3 moveDir, float velocity)
    {
        rb.velocity = moveDir.normalized * velocity;

        StartCoroutine(AutoDisableRoutine());
    }

    private IEnumerator AutoDisableRoutine()
    {
        yield return disalbeWs;
        this.gameObject.SetActive(false);
    }

    private new void OnDisable()
    {
        base.OnDisable();
        rb.velocity = Vector3.zero;
    }
}
