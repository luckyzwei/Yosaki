using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class GameManager : SingletonMono<GameManager>
{
    public enum InitPlayerPortalPosit
    {
        Left, Right
    }
    public enum ContentsType
    {
        NormalField, BonusDefense, SingleRaid, InfiniteTower
    }
    public bool SpawnMagicStone => IsNormalField;
    public bool IsNormalField => contentsType == ContentsType.NormalField;

    public StageMapData CurrentStageData { get; private set; }
    public MapThemaInfo MapThemaInfo { get; private set; }

    private ReactiveProperty<int> currentMapIdx = new ReactiveProperty<int>();

    public ContentsType contentsType { get; private set; }

    public ReactiveCommand whenSceneChanged = new ReactiveCommand();

    public ObscuredInt bossId { get; private set; }
    public ObscuredInt currentTowerId { get; private set; }

    public void SetBossId(int bossId)
    {
        this.bossId = bossId;

        RandomizeKey();
    }

    private new void Awake()
    {
        base.Awake();
        SettingData.InitFirst();
    }

    private void RandomizeKey()
    {
        this.bossId.RandomizeCryptoKey();
    }

    private void Start()
    {
        Subscribe();
        InitGame();
    }

    private void InitGame()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

    private void SetFrameRate(int option)
    {
        Application.targetFrameRate = 30 + 15 * option;
#if UNITY_EDITOR
        Debug.LogError($"Frame changed {Application.targetFrameRate}");
#endif
    }

    private void Subscribe()
    {
        AutoManager.Instance.Subscribe();

        currentMapIdx.AsObservable().Subscribe(e =>
        {
            DatabaseManager.userInfoTable.UpData(UserInfoTable.LastMap, e, false);
        }).AddTo(this);

        SettingData.FrameRateOption.AsObservable().Subscribe(SetFrameRate).AddTo(this);
    }

    private void ClearStage()
    {
        currentMapIdx.Value = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.LastMap).Value;
        CurrentStageData = TableManager.Instance.StageMapData[currentMapIdx.Value];
        MapThemaInfo = Resources.Load<MapThemaInfo>($"MapThema/{CurrentStageData.Mapthema}");
    }

    public List<EnemyTableData> GetEnemyTableData()
    {
        List<EnemyTableData> enemyDatas = new List<EnemyTableData>();

        enemyDatas.Add(TableManager.Instance.EnemyData[CurrentStageData.Monsterid1]);
        enemyDatas.Add(TableManager.Instance.EnemyData[CurrentStageData.Monsterid2]);

        return enemyDatas;
    }

    public void LoadBackScene()
    {
        if (IsFirstScene() == false)
        {
            currentMapIdx.Value--;
            LoadNormalField();
        }
    }

    public void LoadNextScene()
    {
        if (IsLastScene() == false)
        {
            currentMapIdx.Value++;
            LoadNormalField();
        }
    }

    public void MoveMapByIdx(int idx)
    {
        currentMapIdx.Value = idx;
        LoadNormalField();
    }

    public void LoadNormalField()
    {
       contentsType = ContentsType.NormalField;

        ClearStage();

        ChangeScene();
    }

    public void LoadContents(ContentsType type)
    {
        if (type == ContentsType.BonusDefense)
        {
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);
        }

        contentsType = type;

        ChangeScene();
    }

    private void ChangeScene()
    {
        PopupManager.Instance.SetChatBoardPopupManager();

        PostManager.Instance.RefreshPost();

        if (contentsType == ContentsType.NormalField)
        {
            if (currentMapIdx.Value != 0 && UiTutorialManager.Instance != null)
            {
                UiTutorialManager.Instance.SetClear(TutorialStep._1_MoveField);
            }
        }

        SaveManager.Instance.SyncDatasInQueue();

        whenSceneChanged.Execute();

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);


    }
    public bool IsLastScene()
    {
        return currentMapIdx.Value == TableManager.Instance.StageMapData.Count - 1;
    }
    public bool IsFirstScene()
    {
        return currentMapIdx.Value == 0;
    }
}
