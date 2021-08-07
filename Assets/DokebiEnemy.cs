using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class DokebiEnemy : PoolItem
{
    [SerializeField]
    private AgentHpController agentHpController;

    [SerializeField]
    private DokebiMoveController dokebiMoveController;

    private CompositeDisposable disposable = new CompositeDisposable();

    public void Initialize(float hp, float moveSpeed, Action enemyDeadCallBack)
    {
        EnemyTableData data = new EnemyTableData();
        data.Hp = hp;
        data.Useonedamage = false;
        data.Movespeed = moveSpeed;
        data.Attackpower = 0f;

        agentHpController.Initialize(data);
        dokebiMoveController.Initialize(data.Movespeed);

        //공겨력
        GetComponentInChildren<EnemyHitObject>().SetDamage(data.Attackpower);

        disposable.Clear();

        agentHpController.whenEnemyDead.AsObservable().Subscribe(e =>
        {
            enemyDeadCallBack?.Invoke();
        }).AddTo(disposable);

        agentHpController.whenEnemyDamaged.AsObservable().Subscribe(e =>
        {
            dokebiMoveController.SetMoveState(DokebiMoveController.MoveState.FollowPlayer);
        }).AddTo(disposable);
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
