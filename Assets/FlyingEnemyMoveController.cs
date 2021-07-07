using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FlyingEnemyMoveController : MonoBehaviour
{
    private float moveSpeed = 0f;

    private Vector3 moveDir;

    [SerializeField]
    protected Rigidbody2D rb;

    private bool initialized = false;

    [SerializeField]
    private AgentHpController agentHpController;

    private bool isDamaged = false;

    private Transform playerTr;

    [SerializeField]
    private Transform viewTr;

    public void Initialize(Vector3 moveDir, float moveSpeed)
    {
        isDamaged = false;

        SetMoveDir(moveDir, moveSpeed);

        if (initialized == false)
        {
            initialized = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        playerTr = PlayerMoveController.Instance.transform;

        agentHpController.whenEnemyDamaged.AsObservable().Subscribe(e =>
        {
            //유저한테 맞아도 안쫒아오게
            return;
            isDamaged = true;
        }).AddTo(this);
    }

    private void Update()
    {
        if (isDamaged == false)
        {
            rb.velocity = moveDir.normalized * moveSpeed;

        }
        else
        {
            float playerDist = Vector3.Distance(playerTr.position, this.transform.position);

            if (playerDist >= 0.1f) 
            {
                Vector3 moveDir = playerTr.position - this.transform.position;
                rb.velocity = moveDir.normalized * moveSpeed * 1.5f;
            }

        }

        viewTr.transform.localScale = new Vector3(rb.velocity.x > 0 ? -1 : 1, 1, 1);
    }

    private void SetMoveDir(Vector3 moveDir, float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        this.moveDir = moveDir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDamaged == true) return;

        Vector3 refrectDir = this.transform.position - (Vector3)collision.contacts[0].point;

        SetMoveDir(refrectDir, this.moveSpeed);
    }
}
