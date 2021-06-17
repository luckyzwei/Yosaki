using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class EnemyMoveBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Enemy enemyData;
    protected ReactiveProperty<EnemyTableData> enemyInfo = new ReactiveProperty<EnemyTableData>();
    protected void Awake()
    {
        GetRequireComponents();
    }

    protected void Start()
    {
        GetMyInfo();
    }

    private void GetMyInfo()
    {
        enemyData = GetComponent<Enemy>();
        if (enemyData != null)
        {
            enemyInfo.Value = enemyData.tableData;
        }
    }

    private void GetRequireComponents()
    {
        rb = GetComponent<Rigidbody2D>();
    }

}
