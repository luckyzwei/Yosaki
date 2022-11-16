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

    [SerializeField]
    private float fixedPercendDam = 0f;

    private IEnumerator EnableRoutine()
    {
        collider.enabled = false;
        yield return enableDelay;
        collider.enabled = true;
        enableRoutine = null;
    }

    private void Start()
    {
        if (fixedPercendDam != 0)
        {
            this.percentDamage = fixedPercendDam;
        }
    }
    float percentDamage = 0f;

    private void SetPercentValueByBossId()
    {

        if (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && (GameManager.Instance.bossId == 57 ||
            GameManager.Instance.bossId == 72))
        {
            this.percentDamage = 1f;
        }
        
        else if (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && (
         //선녀들
         GameManager.Instance.bossId == 58 ||
         GameManager.Instance.bossId == 59 ||
         GameManager.Instance.bossId == 60 ||
         GameManager.Instance.bossId == 61 ||
         GameManager.Instance.bossId == 62 ||
         GameManager.Instance.bossId == 63 ||
         GameManager.Instance.bossId == 64 ||
         // 도깨비들
         GameManager.Instance.bossId == 75 || 
         GameManager.Instance.bossId == 76 ||
         GameManager.Instance.bossId == 77
         //
         ))
        {
            this.percentDamage = 0.6f;
        } 

    }

    public void SetDamage(double damage, float percentDamage = 0f)
    {
        this.damage = damage;
        this.percentDamage = percentDamage;

        SetPercentValueByBossId();
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

        PlayerStatusController.Instance.UpdateHp(-damage, percentDamage);
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
