using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class TwelveRaidEnemy : BossEnemyBase
{
    [SerializeField]
    private List<AlarmHitObject> enemyHitObjects;

    [SerializeField]
    private AlarmHitObject horizontalHit;

    [SerializeField]
    private AlarmHitObject verticalHit;

    [SerializeField]
    private int attackTypeMax = 2;

    [SerializeField]
    private float attackInterval = 1f;

    [SerializeField]
    private List<AlarmHitObject> HitList_1;

    [SerializeField]
    private List<AlarmHitObject> HitList_2;

    [SerializeField]
    private List<AlarmHitObject> HitList_3;

    private void Start()
    {
        Initialize();
    }

    private void UpdateBossDamage()
    {
        var bossTableData = TableManager.Instance.TwelveBossTable.dataArray[GameManager.Instance.bossId];

        float ratio = TwelveDungeonManager.Instance.GetComponent<TwelveDungeonManager>().GetDamagedAmount() / bossTableData.Hp;

        float damage = Mathf.Lerp(bossTableData.Attackpowermin, bossTableData.Attackpowermax, Mathf.Min(1f, ratio));

        hitObject.SetDamage(damage);

        enemyHitObjects.ForEach(e => e.SetDamage(damage));
    }

    private IEnumerator BossAttackPowerUpdateRoutine()
    {
        var updateDelay = new WaitForSeconds(5.0f);

        while (true)
        {
            UpdateBossDamage();
            yield return updateDelay;
        }
    }

    private IEnumerator AttackRoutine()
    {

        UpdateBossDamage();
        //선딜
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int attackType = Random.Range(0, attackTypeMax);

#if UNITY_EDITOR
          //  Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                yield return StartCoroutine(AttackRoutine_2(2));
            }
            else if (attackType == 1)
            {
                yield return StartCoroutine(AttackRoutine_3(2));
            }
            else if (attackType == 2)
            {
                yield return StartCoroutine(AttackRoutine_4(2));
            }

            StartCoroutine(PlayAttackAnim());

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private IEnumerator PlayAttackAnim()
    {
        skeletonAnimation.AnimationName = "attack";
        yield return new WaitForSeconds(1.5f);
        skeletonAnimation.AnimationName = "walk";
    }

    private void Initialize()
    {
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>().ToList();

        agentHpController.SetHp(float.MaxValue);

        agentHpController.SetDefense(0);

        enemyHitObjects.ForEach(e => e.SetDamage(1f));

        StartCoroutine(BossAttackPowerUpdateRoutine());

        StartCoroutine(AttackRoutine());
    }


    private IEnumerator PlaySoundDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlaySound(name);
    }


    //?
    private IEnumerator AttackRoutine_2(float delay)
    {
        if (HitList_1.Count == 0)
        {
            horizontalHit.AttackStart();
        }

        for (int i = 0; i < HitList_1.Count; i++)
        {
            HitList_1[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));

        yield return new WaitForSeconds(delay);
    }

    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        if (HitList_2.Count == 0)
        {
            verticalHit.AttackStart();
        }

        for (int i = 0; i < HitList_2.Count; i++)
        {
            HitList_2[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_4(float delay)
    {
        //alarmHitObject_4.AttackStart();

        for (int i = 0; i < HitList_3.Count; i++)
        {
            HitList_3[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }
}
