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
            int attackType = Random.Range(0, 2);

#if UNITY_EDITOR
            Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                yield return StartCoroutine(AttackRoutine_2(2));
            }
            else if (attackType == 1)
            {
                yield return StartCoroutine(AttackRoutine_3(2));
            }

            StartCoroutine(PlayAttackAnim());

            yield return new WaitForSeconds(1f);
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
        horizontalHit.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));

        yield return new WaitForSeconds(delay);
    }

    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        verticalHit.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_4(float delay)
    {
        //alarmHitObject_4.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }
}
