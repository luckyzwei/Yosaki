using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyMoveController
{
    [SerializeField]
    protected Rigidbody2D rb;

    private Vector3[] movePointContainer;

    private int nextPointIndex = 0;

    private float moveSpeed = 2f;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        SetRandomMovePos();
    }

    private void Initialize()
    {
        rb.gravityScale = 0f;
    }

    private void SetRandomMovePos()
    {
        movePointContainer = new Vector3[10];
        for (int i = 0; i < movePointContainer.Length; i++)
        {
            movePointContainer[i] = MapInfo.Instance.GetRandomPos();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (ArriveNextPos())
        //{
        //    nextPointIndex++;
        //    if (nextPointIndex >= movePointContainer.Length)
        //    {
        //        nextPointIndex = 0;
        //    }
        //}

        //Vector3 moveDir = movePointContainer[nextPointIndex] - this.transform.position;

        //rb.velocity = moveDir.normalized * enemyData.tableData.Movespeed;
    }

    //private bool ArriveNextPos() 
    //{
    //    return Vector2.Distance(transform.position,movePointContainer[nextPointIndex]) <= 1f;
    //}
}
