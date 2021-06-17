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
    private BonusDefenseEnemyMoveController bonusDefenseEnemyMoveController;

    private CompositeDisposable disposable = new CompositeDisposable();

    public void Initialize(float hp, float moveSpeed, Vector3 moveDir, Action enemyDeadCallBack)
    {
        EnemyTableData data = new EnemyTableData();
        data.Hp = hp;
        data.Useonedamage = true;
        data.Movespeed = moveSpeed;
        data.Attackpower = 0f;

        agentHpController.Initialize(data);
        bonusDefenseEnemyMoveController.Initialize(moveDir, data.Movespeed);
        //공겨력 0
        GetComponentInChildren<EnemyHitObject>().SetDamage(0f);

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
}
