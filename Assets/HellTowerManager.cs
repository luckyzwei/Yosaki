using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using static UiRewardView;
public class HellTowerManager : ContentsManagerBase
{
    [SerializeField]
    private Transform enemySpawnPos;

    private ReactiveProperty<ObscuredInt> contentsState = new ReactiveProperty<ObscuredInt>((int)ContentsState.Fight);

    private List<Enemy> spawnedEnemyList = new List<Enemy>();

    //null �϶� Ŭ���� ���Ѱ�
    private List<RewardData> rewardDatas;

    [SerializeField]
    private UiHellTowerResult uiInfinityTowerResult;

    public UnityEvent retryFunc;

    public static string poolName;

    private new void Start()
    {
        base.Start();

        Subscribe();

        StartCoroutine(ContentsRoutine());
    }

    #region Security
    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        var delay = new WaitForSeconds(1.0f);

        while (true)
        {
            RandomizeKey();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        RandomizeKey();
    }

    private void RandomizeKey()
    {
        contentsState.Value.RandomizeCryptoKey();
    }
    #endregion

    //��������
    #region EndConditions
    private void EnemyDeadCallBack(Enemy enemy)
    {
        spawnedEnemyList.Remove(enemy);

        //���� óġ��
        if (spawnedEnemyList.Count == 0)
        {
            contentsState.Value = (int)ContentsState.Clear;

        }
    }
    private void WhenPlayerDead()
    {
        //Ŭ���� ������ ���� �ʰ�
        if (contentsState.Value != (int)ContentsState.Fight) return;

        //  UiLastContentsFunc.AutoInfiniteTower2 = false;

        contentsState.Value = (int)ContentsState.Dead;
    }

    protected override void TimerEnd()
    {
        //  UiLastContentsFunc.AutoInfiniteTower2 = false;
        base.TimerEnd();
        contentsState.Value = (int)ContentsState.TimerEnd;
    }
    #endregion

    private void Subscribe()
    {
        contentsState.AsObservable().Subscribe(WhenTowerModeStateChanged).AddTo(this);

        PlayerStatusController.Instance.whenPlayerDead.Subscribe(e => { WhenPlayerDead(); }).AddTo(this);
    }


    private void WhenTowerModeStateChanged(ObscuredInt state)
    {
        if (state == (int)ContentsState.Clear)
        {
            //�ݵ�� �������۸���
            SendScore();

            SetClear();
        }

        if (state != (int)ContentsState.Fight)
        {
            EndInfiniteTower();
        }
    }

    private void SendScore()
    {
        //int currentScore = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value;
        //RankManager.Instance.UpdatInfinityTower_Score(currentScore);
    }

    private void EndInfiniteTower()
    {
        //�� ����
        spawnedEnemyList.ForEach(e => e.gameObject.SetActive(false));

        //Ÿ�̸� ����
        if (contentsState.Value != (int)ContentsState.TimerEnd)
        {
            StopTimer();
        }

        //�����˾�
        ShowResultPopup();
    }



    private void ShowResultPopup()
    {
        //Ŭ���� �˾����
        uiInfinityTowerResult.gameObject.SetActive(true);
        uiInfinityTowerResult.Initialize((ContentsState)(int)contentsState.Value, rewardDatas);
        //
    }

    private IEnumerator ContentsRoutine()
    {
        yield return null;

        SpawnEnemy();

        AutoManager.Instance.StartAutoWithDelay();

    }


    private void SpawnEnemy()
    {
        int stageId = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value;

        var TowerTableData2 = TableManager.Instance.towerTable3.dataArray[stageId];
        EnemyTableData spawnEnemyData = GetSpawnedEnemy(stageId);

        for (int i = 0; i < TowerTableData2.Spawnnum; i++)
        {
            poolName = $"Enemy/DokebiTower/{spawnEnemyData.Prefabname}";

            var enemyObject = BattleObjectManager.Instance.GetItem(poolName) as Enemy;

            enemyObject.transform.SetParent(enemySpawnPos.transform);

            enemyObject.transform.localPosition = Vector3.zero;

            enemyObject.transform.localScale = Vector3.one * 1.3f;

            enemyObject.SetEnemyDeadCallBack(EnemyDeadCallBack);

            enemyObject.Initialize(spawnEnemyData, updateSubHpBar: true);

            spawnedEnemyList.Add(enemyObject);
        }

    }
    private EnemyTableData GetSpawnedEnemy(int stageId)
    {
        stageId = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value;

        var tableData = TableManager.Instance.towerTable3.dataArray[stageId];

        EnemyTableData enemy = new EnemyTableData();

        enemy.Prefabname = tableData.Materialtype;
        enemy.Attackpower = tableData.Attackpower;
        enemy.Movespeed = tableData.Movespeed;
        enemy.Hp = tableData.Hp;
        enemy.Knockbackpower = tableData.Knockbackpower;
        enemy.Defense = (int)tableData.Defense;

        return enemy;
    }


    private void SetClear()
    {
        //��������
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value;

        var TowerTableData3 = TableManager.Instance.towerTable3.dataArray[currentFloor];

        rewardDatas = new List<RewardData>();

        var rewardData = new RewardData((Item_Type)TowerTableData3.Rewardtype, TowerTableData3.Rewardvalue);

        rewardDatas.Add(rewardData);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        HashSet<int> syncDataList = new HashSet<int>();

        //������ ����(����)
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            if (syncDataList.Contains((int)rewardDatas[i].itemType) == true)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"Duplicated tower itemType : {(Item_Type)(int)rewardDatas[i].itemType}", null);
                return;
            }
            else
            {
                syncDataList.Add((int)rewardDatas[i].itemType);
            }

            ServerData.AddLocalValue(rewardDatas[i].itemType, rewardDatas[i].amount);

            //���� Ʈ�����
            var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)rewardDatas[i].itemType);
            transactionList.Add(rewardTransactionValue);
        }

        //�ܰ���
        ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value++;

        Param floorParam = new Param();

        floorParam.Add(UserInfoTable.currentFloorIdx3, ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, floorParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            //  StartCoroutine(AutoPlayRoutine());
        });
    }

}