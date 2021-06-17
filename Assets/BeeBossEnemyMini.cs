using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class BeeBossEnemyMini : PoolItem
{
    [SerializeField]
    private AgentHpController agentHpController;

    [SerializeField]
    private FlyingEnemyMoveController flyingEnemyMoveController;

    private CompositeDisposable disposable = new CompositeDisposable();

    public void Initialize(float hp, float moveSpeed,float damage ,Vector3 moveDir, Action enemyDeadCallBack)
    {
        EnemyTableData data = new EnemyTableData();
        data.Hp = hp;
        data.Useonedamage = true;
        data.Movespeed = moveSpeed;
        data.Attackpower = 0f;

        agentHpController.Initialize(data);
        flyingEnemyMoveController.Initialize(moveDir, data.Movespeed);

        //공겨력 0
        GetComponentInChildren<EnemyHitObject>().SetDamage(damage);

        disposable.Clear();
        agentHpController.whenEnemyDead.AsObservable().Subscribe(e =>
        {
            enemyDeadCallBack?.Invoke();
        }).AddTo(disposable);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //방향전환
    }
}
