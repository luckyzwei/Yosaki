using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class RelicEnemyMoveController : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;

    private bool initialized = false;

    [SerializeField]
    private AgentHpController agentHpController;

    [SerializeField]
    private Transform viewTr;

    private Vector3 moveDir;
    private float moveSpeed;

    private Coroutine moveRoutine;

    public void Initialize(Vector3 moveDir, float moveSpeed)
    {
        SetMoveDir(moveDir, moveSpeed);

        if (initialized == false)
        {
            initialized = true;
        }
    }

    private void Update()
    {
        viewTr.transform.localScale = new Vector3(rb.velocity.x > 0 ? -1 : 1, 1, 1);
    }

    private void SetMoveDir(Vector3 moveDir, float moveSpeed)
    {
        this.moveDir = moveDir;
        this.moveSpeed = moveSpeed;

        rb.velocity = moveDir.normalized * moveSpeed;

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        WaitForSeconds moveDelay = new WaitForSeconds(1.0f);

        while (true)
        {
            yield return moveDelay;
            rb.velocity = moveDir.normalized * moveSpeed;
        }
    }

    private static string relicWallStr = "RelicWall";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(relicWallStr))
        {
            RelicDungeonManager.Instance.DiscountRelicDungeonHp();
            this.gameObject.SetActive(false);
        }

        // if (isDamaged == true) return;

        //Vector3 refrectDir = this.transform.position - (Vector3)collision.GetContact(0).point;

        // SetMoveDir(Quaternion.Euler(0f, 0f, Random.Range(200, 340)) * moveDir, this.moveSpeed);
        //SetMoveDir(refrectDir, this.moveSpeed);
    }
}
