using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class RelicEnemy : PoolItem
{
    [SerializeField]
    private AgentHpController agentHpController;

    [SerializeField]
    private RelicEnemyMoveController enemyMoveController;

    private CompositeDisposable disposable = new CompositeDisposable();

    public void Initialize(double hp, float moveSpeed, int defense, Action enemyDeadCallBack)
    {
        EnemyTableData data = new EnemyTableData();
        data.Hp = hp;
        data.Useonedamage = false;
        data.Movespeed = moveSpeed;
        data.Attackpower = 0f;
        data.Defense = defense;

        agentHpController.Initialize(data);
        enemyMoveController.Initialize(Vector3.left, data.Movespeed);
        //  enemyMoveController.Initialize(Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)) * Vector3.right, data.Movespeed);

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
