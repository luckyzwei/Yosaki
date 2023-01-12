using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBossEnemy : BossEnemyBase
{
    [SerializeField]
    private Animator animator;

    public static string poolName = "Enemy/Boss1EnemyMini";

    private float currentDamage;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        //  enemyHitObjects = GetComponentsInChildren<AlarmHitObject>().ToList();

        agentHpController.SetHp(float.MaxValue);

        var bossTableData = TableManager.Instance.BossTableData[GameManager.Instance.bossId];
        agentHpController.SetDefense(bossTableData.Defense);
        //enemyHitObjects.ForEach(e => e.SetDamage(1f));

        StartCoroutine(BossAttackPowerUpdateRoutine());

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {

        UpdateBossDamage();
        //선딜
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int attackType = Random.Range(0, 5);

#if UNITY_EDITOR
            //Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                animator.SetTrigger("Pattern1");
            }
            else if (attackType == 1)
            {
                animator.SetTrigger("Pattern2");
            }
            else if (attackType == 2)
            {
                animator.SetTrigger("Pattern3");
            }
            else if (attackType == 3)
            {
                animator.SetTrigger("Pattern4");
            }
            else if (attackType == 4)
            {
                animator.SetTrigger("Pattern5");
            }

            SoundManager.Instance.PlaySound("BeeBossMove");
            yield return new WaitForSeconds(4f);
        }
    }

    private IEnumerator BossAttackPowerUpdateRoutine()
    {
        var updateDelay = new WaitForSeconds(5.5f);

        while (true)
        {
            UpdateBossDamage();
            yield return updateDelay;
        }
    }

    private void UpdateBossDamage()
    {
        var bossTableData = TableManager.Instance.BossTableData[GameManager.Instance.bossId];

       // float ratio = SingleRaidManager.Instance.GetComponent<SingleRaidManager>().BossRemainHp / bossTableData.Hp;

        //float damage = Mathf.Lerp(bossTableData.Attackpowermin, bossTableData.Attackpowermax, 1f - ratio);

       // this.currentDamage = damage;

      //  hitObject.SetDamage(damage);
    }

    public void SpawnEnemy()
    {
//        SoundManager.Instance.PlaySound("MobSpawn");
//        var enemy = BattleObjectManager.Instance.GetItem(poolName).GetComponent<BeeBossEnemyMini>();

//        enemy.transform.position = this.transform.position;

//        int hp = (int)(SingleRaidManager.Instance.GetBossRemainHpRatio() * 100f) + 15;

//        enemy.Initialize(hp, 5f, currentDamage * 0.3f, Random.insideUnitCircle, null);

//#if UNITY_EDITOR
//        Debug.LogError($"Spawn enemy hp {hp}");
//#endif
    }
}
