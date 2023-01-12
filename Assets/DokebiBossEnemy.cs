using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class DokebiBossEnemy : BossEnemyBase
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
    private float attackInterval_Real = 2f;

    [SerializeField]
    private List<AlarmHitObject> AnimHitList_1;

    [SerializeField]
    private List<AlarmHitObject> HitList_1;

    [SerializeField]
    private List<AlarmHitObject> HitList_2;

    [SerializeField]
    private List<AlarmHitObject> HitList_3;

    [SerializeField]
    private List<AlarmHitObject> HitList_4;

    [SerializeField]
    private List<AlarmHitObject> HitList_5;

    [SerializeField]
    private List<AlarmHitObject> HitList_6;

    [SerializeField]
    private List<AlarmHitObject> RandomHit;

    [SerializeField]
    private List<AlarmHitObject> RandomHit2;

    [SerializeField]
    private List<AlarmHitObject> RandomHit3;

    [SerializeField]
    private List<AlarmHitObject> RandomHit3_Guild;

    [SerializeField]
    private List<AlarmHitObject> RandomHit3_Guild2;

    [SerializeField]
    private bool isGuildBoss = false;

    [SerializeField]
    private bool playAnim = true;

    private void Start()
    {
        Initialize();
    }
    [SerializeField]
    private float percentDamageValue = 0f;
    private void UpdateBossDamage()
    {
        float damage = 10f;
        hitObject.SetDamage(damage, percentDamageValue);
        enemyHitObjects.ForEach(e => e.SetDamage((float)damage, percentDamageValue));
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
                if (isGuildBoss == false)
                {
                    yield return StartCoroutine(AttackRoutine_2(attackInterval_Real));
                }
                else
                {
                    yield return StartCoroutine(GuildBossHit(attackInterval_Real));

                }
            }
            else if (attackType == 1)
            {
                if (isGuildBoss == false)
                {
                    yield return StartCoroutine(AttackRoutine_3(attackInterval_Real));
                }
                else
                {
                    yield return StartCoroutine(GuildBossHit(attackInterval_Real));
                }
            }
            else if (attackType == 2)
            {
                if (isGuildBoss == false)
                {
                    yield return StartCoroutine(AttackRoutine_4(attackInterval_Real));
                }
                else
                {
                    yield return StartCoroutine(GuildBossHit(attackInterval_Real));
                }
            }

            if (playAnim)
            {
                //나타

                StartCoroutine(PlayAttackAnim());
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private IEnumerator AttackRoutine_PartyRaid()
    {
        UpdateBossDamage();
        //선딜
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int attackType = Random.Range(0, 6);

#if UNITY_EDITOR
              Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                    yield return StartCoroutine(AttackRoutine_2(attackInterval_Real));
            }
            else if (attackType == 1)
            {
                    yield return StartCoroutine(AttackRoutine_3(attackInterval_Real));
            }
            else if (attackType == 2)
            {
                    yield return StartCoroutine(AttackRoutine_4(attackInterval_Real));
            }
            else if (attackType == 3)
            {
                yield return StartCoroutine(AttackRoutine_5(attackInterval_Real));
            }
            else if (attackType == 4)
            {
                yield return StartCoroutine(AttackRoutine_6(attackInterval_Real));
            }
            else if (attackType == 5)
            {
                yield return StartCoroutine(AttackRoutine_7(attackInterval_Real));
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private IEnumerator PlayAttackAnim()
    {

        skeletonAnimation.AnimationName = "attack";
        yield return new WaitForSeconds(1.5f);
        skeletonAnimation.AnimationName = "walk";
    }

    private IEnumerator PlayAttackAnim_Nata()
    {

        skeletonAnimation.AnimationName = "attack2";
        yield return new WaitForSeconds(1.5f);
        skeletonAnimation.AnimationName = "idle";
    }

    private void Initialize()
    {
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>(true).ToList();

        agentHpController.SetHp(float.MaxValue);

        agentHpController.SetDefense(0);

        //enemyHitObjects.ForEach(e => e.SetDamage(1f));

        StartAttackRoutine();
    }

    public void StartAttackRoutine()
    {
        StartCoroutine(AttackRoutine());
    }

    public void StartAttackRoutine_PartyRaid()
    {
        StartCoroutine(AttackRoutine_PartyRaid());
    }


    private IEnumerator PlaySoundDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlaySound(name);
    }


    //?
    private IEnumerator AttackRoutine_2(float delay)
    {
        if (HitList_1.Count == 0 && horizontalHit != null)
        {

            horizontalHit.AttackStart();
        }

        for (int i = 0; i < HitList_1.Count; i++)
        {
            HitList_1[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }


    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        if (HitList_2.Count == 0 && verticalHit != null)
        {
            verticalHit.AttackStart();
        }

        for (int i = 0; i < HitList_2.Count; i++)
        {
            HitList_2[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits();

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

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_5(float delay)
    {
        //alarmHitObject_4.AttackStart();

        for (int i = 0; i < HitList_4.Count; i++)
        {
            HitList_4[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }
    private IEnumerator AttackRoutine_6(float delay)
    {
        //alarmHitObject_4.AttackStart();

        for (int i = 0; i < HitList_5.Count; i++)
        {
            HitList_5[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }
    private IEnumerator AttackRoutine_7(float delay)
    {
        //alarmHitObject_4.AttackStart();

        for (int i = 0; i < HitList_6.Count; i++)
        {
            HitList_6[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }

    private void PlayRandomHits()
    {
        int rand = Random.Range(0, 3);

        if (rand == 0)
        {
            if (RandomHit.Count != 0)
            {
                int rankIdx = Random.Range(0, RandomHit.Count);

                for (int i = 0; i < RandomHit.Count; i++)
                {
                    if (i == rankIdx)
                    {
                        RandomHit[i].AttackStart();
                    }
                }
            }
        }
        else if (rand == 1)
        {
            if (RandomHit2.Count != 0)
            {
                int rankIdx = Random.Range(0, RandomHit2.Count);

                for (int i = 0; i < RandomHit2.Count; i++)
                {
                    if (i == rankIdx)
                    {
                        RandomHit2[i].AttackStart();
                    }
                }
            }
        }
        else
        {
            if (RandomHit3.Count != 0)
            {
                int rankIdx = Random.Range(0, RandomHit3.Count);

                for (int i = 0; i < RandomHit3.Count; i++)
                {
                    if (i == rankIdx)
                    {
                        RandomHit3[i].AttackStart();
                    }
                }
            }
        }



    }

    private IEnumerator GuildBossHit(float delay)
    {

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits_Guild();

        yield return new WaitForSeconds(delay);
    }

    private int idx1 = 0;

    private int idx2 = 0;
    private int idx3 = 0;

    public void AnimAttackPattern(int index)
    {
        AnimHitList_1[index].AttackStart();
    }

    private void PlayRandomHits_Guild()
    {
        int rankIdx = Random.Range(0, RandomHit.Count);

        if (RandomHit.Count != 0)
        {
            if (idx1 >= RandomHit.Count) idx1 = 0;

            for (int i = 0; i < RandomHit.Count; i++)
            {
                if (i == rankIdx)
                {
                    RandomHit[i].AttackStart();
                }
            }

            idx1++;
        }

        if (RandomHit2.Count != 0)
        {
            if (idx2 >= RandomHit.Count) idx2 = 0;

            for (int i = 0; i < RandomHit2.Count; i++)
            {
                if (i == rankIdx)
                {
                    RandomHit2[i].AttackStart();
                }
            }

            idx2++;
        }

        if (RandomHit3_Guild.Count != 0)
        {
            if (idx3 >= RandomHit3_Guild.Count) idx3 = 0;

            for (int i = 0; i < RandomHit3_Guild.Count; i++)
            {
                if (i == idx3)
                {
                    RandomHit3_Guild[i].AttackStart();
                }
            }

            idx3++;
        }

        if (RandomHit3_Guild2 != null && RandomHit3_Guild2.Count != 0)
        {
            RandomHit3_Guild2[Random.Range(0, RandomHit3_Guild2.Count)].AttackStart();
        }
    }
}
