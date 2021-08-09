using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UniRx;

public class DokebiDungeonManager : ContentsManagerBase
{
    [SerializeField]
    private List<Transform> spawnPoints;

    private ReactiveProperty<ObscuredInt> enemyDeadCount = new ReactiveProperty<ObscuredInt>();

    [SerializeField]
    private TextMeshProUGUI enemyKillCount;

    [SerializeField]
    private UiDokebiResultPopup resultPopup;

    private Coroutine spawnRoutine;

    private DokebiData dokebiData;

    public static string poolName;

    private ObscuredInt currentSpawnedNum = 0;

    protected new void Start()
    {
        base.Start();

        dokebiData = TableManager.Instance.DokebiTable.dataArray[GameManager.Instance.dokebiIdx];

        poolName = $"Enemy/{dokebiData.Prefabname}";

        spawnRoutine = StartCoroutine(EnemySpawnRoutine());

        Subscribe();

        UiTutorialManager.Instance.SetClear(TutorialStep.PlayFireFly);
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

        StopCoroutine(spawnRoutine);

        resultPopup.Initialize(enemyDeadCount.Value);

        resultPopup.gameObject.SetActive(true);

        BattleObjectManager.Instance.PoolContainer[poolName].DisableAllObject();
    }

    private IEnumerator EnemySpawnRoutine()
    {
        WaitForSeconds delay1 = new WaitForSeconds(dokebiData.Spawndelay1);

        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            int enemySpawnNum = dokebiData.Maxenemynum - currentSpawnedNum;

            for (int i = 0; i < enemySpawnNum; i++)
            {
                SpawnEnemy();
            }

            yield return delay1;
        }
    }

    private void SpawnEnemy()
    {
        currentSpawnedNum++;

        int directionRand = Random.Range(0, 2);
        Vector3 moveDir = Vector3.zero;
        Vector3 spawnPos = Vector3.zero;

        int randIdx = Random.Range(0, spawnPoints.Count);
        spawnPos = spawnPoints[randIdx].transform.position;

        var enemyPrefab = BattleObjectManager.Instance.GetItem(poolName);

        var enemy = enemyPrefab.GetComponent<DokebiEnemy>();

        enemy.Initialize(dokebiData.Hp, dokebiData.Movespeed, WhenEnemyDead);

        enemy.transform.position = spawnPos;
    }



    private void WhenEnemyDead()
    {
        enemyDeadCount.Value++;
        currentSpawnedNum--;
    }
}
