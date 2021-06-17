using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class SingleRaidEnemy : BossEnemyBase
{
    private List<AlarmHitObject> enemyHitObjects;

    [SerializeField]
    private List<AlarmHitObject> alarmHitObject_1;

    [SerializeField]
    private AlarmHitObject alarmHitObject_2;

    [SerializeField]
    private List<Transform> attack2SpawnPos;

    [SerializeField]
    private AlarmHitObject alarmHitObject_3;

    [SerializeField]
    private AlarmHitObject alarmHitObject_4;

    [SerializeField]
    private Transform atk_minPos;

    [SerializeField]
    private Transform atk_maxPos;

    [SerializeField]
    private List<Transform> alarmHitObjectSpawnPos;


    private void Start()
    {
        Initialize();
    }

    private void UpdateBossDamage()
    {
        var bossTableData = TableManager.Instance.BossTableData[GameManager.Instance.bossId];

        float ratio = SingleRaidManager.Instance.GetComponent<SingleRaidManager>().BossRemainHp / bossTableData.Hp;

        float damage = Mathf.Lerp(bossTableData.Attackpowermin, bossTableData.Attackpowermax, 1f - ratio);

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
            int attackType = Random.Range(0, 4);

#if UNITY_EDITOR
            Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                yield return StartCoroutine(AttackRoutine_1(1f, Random.Range(5, 10)));
            }
            else if (attackType == 1)
            {
                yield return StartCoroutine(AttackRoutine_2(1f));
            }
            else if (attackType == 2)
            {
                yield return StartCoroutine(AttackRoutine_3(1.5f));
            }
            else if (attackType == 3)
            {
                yield return StartCoroutine(AttackRoutine_4(1.5f));
            }

            StartCoroutine(PlayAttackAnim());

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator PlayAttackAnim()
    {
        skeletonAnimation.AnimationName = "attack";
        yield return new WaitForSeconds(1.0f);
        skeletonAnimation.AnimationName = "Idle";
    }

    private void Initialize()
    {
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>().ToList();

        agentHpController.SetHp(float.MaxValue);
        enemyHitObjects.ForEach(e => e.SetDamage(1f));

        StartCoroutine(BossAttackPowerUpdateRoutine());

        StartCoroutine(AttackRoutine());
    }

    //하늘에서 비쏟아냄
    private IEnumerator AttackRoutine_1(float delay, int count)
    {
        alarmHitObject_1.Shuffle();

        for (int i = 0; i < count; i++)
        {
            alarmHitObject_1[i].transform.position = new Vector3(Random.Range(atk_minPos.position.x, atk_maxPos.position.x), alarmHitObject_1[i].transform.position.y, alarmHitObject_1[i].transform.position.z);

            alarmHitObject_1[i].AttackStart();

            StartCoroutine(PlaySoundDelay(1f, "BossSkill1"));

            yield return new WaitForSeconds(delay / (float)count);
        }
    }

    private IEnumerator PlaySoundDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlaySound(name);
    }


    //?
    int attack2Count = 0;
    private IEnumerator AttackRoutine_2(float delay)
    {
        attack2Count++;
        if (attack2Count % 3 == 0)
        {
            //맨위스폰
            alarmHitObject_2.transform.position = attack2SpawnPos[attack2SpawnPos.Count - 1].position;
        }
        else
        {
            alarmHitObject_2.transform.position = attack2SpawnPos[Random.Range(0, attack2SpawnPos.Count)].position;
        }

        alarmHitObject_2.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));

        yield return new WaitForSeconds(delay);
    }

    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        alarmHitObject_3.transform.position = alarmHitObjectSpawnPos[Random.Range(0, alarmHitObjectSpawnPos.Count)].position;

        alarmHitObject_3.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }

    //전영역 (맨 윗층 점프로만 피할수있음)
    private IEnumerator AttackRoutine_4(float delay)
    {
        alarmHitObject_4.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }
}
