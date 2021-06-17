using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmHitObject : MonoBehaviour
{
    private float damage = 10;

    [SerializeField]
    private Animator animator;

    public void AttackStart()
    {
        this.gameObject.SetActive(true);
        animator.SetTrigger("Attack");
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals(Tags.Player) == false) return;

        PlayerStatusController.Instance.UpdateHp(-damage);
    }

}
