using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmHitObject : MonoBehaviour
{
    private double damage = 10;

    [SerializeField]
    private Animator animator;

    public void AttackStart()
    {
        this.gameObject.SetActive(true);
        animator.SetTrigger("Attack");
    }

    float percentDamage = 0f;

    public void SetDamage(double damage, float percentDamage = 0f)
    {
        this.damage = damage;
        this.percentDamage = percentDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals(Tags.Player) == false) return;

        PlayerStatusController.Instance.UpdateHp(-damage, percentDamage);
    }

}
