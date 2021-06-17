using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDefenseEnemyMoveController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public void Initialize(Vector3 moveDir, float moveSpeed)
    {
        rb.velocity = moveDir * moveSpeed;

        if (moveDir == Vector3.right)
        {
            this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
    }

}
