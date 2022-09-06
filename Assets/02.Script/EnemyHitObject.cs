using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitObject : MonoBehaviour
{
    [SerializeField]
    private Collider2D collider;

    private WaitForSeconds enableDelay = new WaitForSeconds(1.5f);

    private double damage = 10;

    private Coroutine enableRoutine;

    [SerializeField]
    private AgentHpController agentHpController;

    [SerializeField]
    private bool deadWhenTriggered = false;

    private IEnumerator EnableRoutine()
    {
        collider.enabled = false;
        yield return enableDelay;
        collider.enabled = true;
        enableRoutine = null;
    }

    public void SetDamage(double damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals(Tags.Player) == false) return;

        //검기 영혼 지옥
        if (deadWhenTriggered)
        {
            if (agentHpController != null)
            {
                agentHpController.UpdateHp(double.MinValue);
                PopupManager.Instance.ShowAlarmMessage("몸으로 흡수");
            }
        }

        SetTriggerRoutine();

        PlayerStatusController.Instance.UpdateHp(-damage);
    }

    public void SetTriggerRoutine()
    {
        if (enableRoutine == null && this.gameObject.activeInHierarchy)
        {
            enableRoutine = StartCoroutine(EnableRoutine());
        }
    }

    private void OnEnable()
    {
        collider.enabled = true;
    }

    private void OnDisable()
    {
        enableRoutine = null;
    }


}
