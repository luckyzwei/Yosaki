using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UniRx;

public class BonusDefenseManager : ContentsManagerBase
{
    private ObscuredFloat spawnDelay1 = 0.3f;
    //20초남았을때
    private ObscuredFloat spawnDelay2 = 0.2f;

    [SerializeField]
    private ObscuredFloat enemyHp = 0.3f;

    [SerializeField]
    private ObscuredFloat moveSpeed = 0f;

    [SerializeField]
    private BonusDefenseEnemy bonusDefenseEnemyPrefab;

    [SerializeField]
    private List<Transform> spawnPointsLeft;

    [SerializeField]
    private List<Transform> spawnPointsRight;

    private ReactiveProperty<ObscuredInt> enemyDeadCount = new ReactiveProperty<ObscuredInt>();

    [SerializeField]
    private TextMeshProUGUI enemyKillCount;

    [SerializeField]
    private UiBonusDefenseResultPopup resultPopup;

    private Coroutine spawnRoutine;

    public static string poolName = "Enemy/BonusDefenseMob";

    protected new void Start()
    {
        base.Start();

        spawnRoutine = StartCoroutine(EnemySpawnRoutine());

        Subscribe();
    }

    private void Subscribe()
    {
        enemyDeadCount.AsObservable().Subscribe(e =>
        {
            enemyKillCount.SetText($"{e}처치");
        }).AddTo(this);
    }

    protected override void TimerEnd()
    {
        base.TimerEnd();

        UiTutorialManager.Instance.SetClear(TutorialStep._12_ClearGoblin);

        StopCoroutine(spawnRoutine);

        resultPopup.Initialize(enemyDeadCount.Value);

        resultPopup.gameObject.SetActive(true);

        BattleObjectManager.Instance.PoolContainer[poolName].DisableAllObject();
    }

    private IEnumerator EnemySpawnRoutine()
    {
        WaitForSeconds delay1 = new WaitForSeconds(spawnDelay1);
        WaitForSeconds delay2 = new WaitForSeconds(spawnDelay2);

        while (true)
        {
            if (remainSec > 20)
            {
                yield return delay1;
            }
            else
            {
                yield return delay2;
            }
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int directionRand = Random.Range(0, 2);
        Vector3 moveDir = Vector3.zero;
        Vector3 spawnPos = Vector3.zero;

        if (directionRand == 0)
        {
            int randIdx = Random.Range(0, spawnPointsLeft.Count);
            moveDir = Vector3.right;
            spawnPos = spawnPointsLeft[randIdx].transform.position;
        }
        else if (directionRand == 1)
        {
            int randIdx = Random.Range(0, spawnPointsRight.Count);
            moveDir = Vector3.left;
            spawnPos = spawnPointsRight[randIdx].transform.position;
        }

        var enemy = BattleObjectManager.Instance.GetItem(poolName).GetComponent<BonusDefenseEnemy>();

        enemy.Initialize(enemyHp, moveSpeed, moveDir, WhenEnemyDead);

        enemy.transform.position = spawnPos;
    }



    private void WhenEnemyDead()
    {
        enemyDeadCount.Value++;
    }
}
