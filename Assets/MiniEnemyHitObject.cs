using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemyHitObject : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void AttackStart()
    {
        this.gameObject.SetActive(true);
        animator.SetTrigger("Attack");
    }

}
