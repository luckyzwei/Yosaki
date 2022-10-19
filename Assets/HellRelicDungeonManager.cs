using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UniRx;


public class HellRelicDungeonManager : ContentsManagerBase
{
    private ObscuredFloat spawnDelay1 = 0.15f;

    [SerializeField]
    private ObscuredInt relicDungeonHp = 30;

    private ObscuredInt escapedEnemyNum = 0;

    private ObscuredInt spawnCount = 0;

    [SerializeField]
    private List<Transform> spawnPoints;

    private ReactiveProperty<ObscuredInt> enemyDeadCount = new ReactiveProperty<ObscuredInt>();

    [SerializeField]
    private TextMeshProUGUI enemyKillCount;

    [SerializeField]
    private TextMeshProUGUI relicDungeonHpText;

    [SerializeField]
    private UiRelicDungeonResultPopup resultPopup;

    private Coroutine spawnRoutine;

    [SerializeField]
    private List<string> spawnedEnemyList;

    private enum ModeState
    {
        Playing, End
    }

    private ModeState modeState = ModeState.Playing;

    protected new void Start()
    {
        SetPrefData();

        base.Start();

        spawnRoutine = StartCoroutine(EnemySpawnRoutine());

        Subscribe();

        UpdateRemainHp();
    }

    private void SetPrefData()
    {
        enemyDeadCount.Value = Mathf.Max(0, (int)ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value - 300);
        spawnCount = enemyDeadCount.Value;
    }

    private void UpdateRemainHp()
    {
        relicDungeonHpText.SetText($"탈출한 요괴 : {escapedEnemyNum}/{relicDungeonHp}");
    }

    private void Subscribe()
    {
        enemyDeadCount.AsObservable().Subscribe(e =>
        {
            enemyKillCount.SetText($"{Utils.ConvertBigNum(e)} 마리 처치!");
        }).AddTo(this);
    }

    protected override void TimerEnd()
    {
        base.TimerEnd();

        EndGame();
    }

    private void EndGame()
    {
        UpdateRank();

        modeState = ModeState.End;

        StopCoroutine(spawnRoutine);

        resultPopup.Initialize(enemyDeadCount.Value);

        resultPopup.gameObject.SetActive(true);

        for (int i = 0; i < spawnedEnemyList.Count; i++)
        {
            BattleObjectManager.Instance.PoolContainer[$"Enemy/{spawnedEnemyList[i]}"].DisableAllObject();
        }
    }

    private void UpdateRank()
    {
        int prefScore = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value;

        if (enemyDeadCount.Value > prefScore)
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value = enemyDeadCount.Value;

            ServerData.userInfoTable.UpData(UserInfoTable.hellRelicKillCount, false);

            RankManager.Instance.UpdateRelic_Score(enemyDeadCount.Value);

            //보상용
            var yeoraeData = ServerData.bossServerTable.TableDatas["b53"];

            if (string.IsNullOrEmpty(yeoraeData.score.Value) == false)
            {
                if (enemyDeadCount.Value < double.Parse(yeoraeData.score.Value))
                {
                    return;
                }
                else
                {
                    yeoraeData.score.Value = enemyDeadCount.Value.ToString();

                    ServerData.bossServerTable.UpdateData("b53");
                }
            }
            else
            {
                yeoraeData.score.Value = enemyDeadCount.Value.ToString();

                ServerData.bossServerTable.UpdateData("b53");
            }
        }
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (true)
        {
            float t = 0f;

            while (t < spawnDelay1)
            {
                t += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < 2; i++)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        int directionRand = Random.Range(0, 2);
        Vector3 moveDir = Vector3.zero;
        Vector3 spawnPos = Vector3.zero;

        int randIdx = Random.Range(0, spawnPoints.Count);
        spawnPos = spawnPoints[randIdx].transform.position;

        int randomEnemy = Random.Range(0, spawnedEnemyList.Count);

        var enemy = BattleObjectManager.Instance.GetItem($"Enemy/{spawnedEnemyList[randomEnemy]}").GetComponent<RelicEnemy>();

        EnemyTableData enemyTableData = new EnemyTableData();

        enemy.Initialize(GetEnemyHp(randomEnemy), GetMoveSpeed(), GetEnemyDefense(), WhenEnemyDead);

        enemy.transform.position = spawnPos;

        spawnCount++;
    }

    public float GetMoveSpeed()
    {
        int SpeedClamp = Mathf.Clamp(spawnCount, 0, 2500);

        return 3.3f + (((float)SpeedClamp / 10f) * 0.1f);
    }

    public double GetEnemyHp(int randIdx)
    {
        int enemyTableIdx = spawnCount * 2 + 4500 /*+ (randIdx * 200)*/;

        enemyTableIdx = Mathf.Clamp(enemyTableIdx, 100, 12400);

        bool isMax = enemyTableIdx == 12400;

        double addValue = 0d;

        if (isMax)
        {
            addValue = (spawnCount) - enemyTableIdx + 8450;
        }

        //최대층
        if (enemyTableIdx >= 12400)
        {
            return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Hp * (System.Math.Pow(1.002d, addValue));
        }
        else
        {
            return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Hp * 1000;
        }
        //
    }

    public int GetEnemyDefense()
    {
        int enemyTableIdx = spawnCount * 2 + 4500;

        enemyTableIdx = Mathf.Clamp(enemyTableIdx, 100, 12400);

        return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Defense;
    }

    private void WhenEnemyDead()
    {
        enemyDeadCount.Value++;
    }

    public override void DiscountRelicDungeonHp()
    {
        if (modeState == ModeState.End) return;

        escapedEnemyNum++;
        UpdateRemainHp();

        if (escapedEnemyNum >= relicDungeonHp)
        {
            EndGame();
        }
    }
}
