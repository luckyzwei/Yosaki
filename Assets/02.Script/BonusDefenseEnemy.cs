using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class BonusDefenseEnemy : PoolItem
{
    [SerializeField]
    private AgentHpController agentHpController;

    [SerializeField]
    private FlyingEnemyMoveController bonusDefenseEnemyMoveController;

    private CompositeDisposable disposable = new CompositeDisposable();

    public void Initialize(float hp, float moveSpeed, Action enemyDeadCallBack)
    {
        EnemyTableData data = new EnemyTableData();
        data.Hp = hp;
        data.Useonedamage = true;
        data.Movespeed = moveSpeed;
        data.Attackpower = 0f;

        agentHpController.Initialize(data);
        bonusDefenseEnemyMoveController.Initialize(Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)) * Vector3.right, data.Movespeed);
        //공겨력 0
        //   GetComponentInChildren<EnemyHitObject>().SetDamage(0f);

        disposable.Clear();
        agentHpController.whenEnemyDead.AsObservable().Subscribe(e =>
        {
            enemyDeadCallBack?.Invoke();
        }).AddTo(disposable);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(EnemyMoveController.DefenseWall_str))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
