using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class GameManager : SingletonMono<GameManager>
{
    public enum InitPlayerPortalPosit
    {
        Left, Right
    }
    public enum ContentsType
    {
        NormalField, FireFly, Boss, InfiniteTower, Dokebi, TwelveDungeon
    }
    public bool SpawnMagicStone => IsNormalField;
    public bool IsNormalField => contentsType == ContentsType.NormalField;

    public StageMapData CurrentStageData { get; private set; }
    public MapThemaInfo MapThemaInfo { get; private set; }

    private ReactiveProperty<int> currentMapIdx = new ReactiveProperty<int>();

    public static ContentsType contentsType { get; private set; }

    public ReactiveCommand whenSceneChanged = new ReactiveCommand();

    public ObscuredInt bossId { get; private set; }
    public ObscuredInt currentTowerId { get; private set; }

    public ObscuredInt dokebiIdx { get; private set; }

    public ContentsType lastContentsType { get; private set; } = ContentsType.NormalField;

    public void ResetLastContents()
    {
        lastContentsType = ContentsType.NormalField;
    }
    public void SetBossId(int bossId)
    {
        this.bossId = bossId;

        RandomizeKey();
    }

    public void SetDokebiId(int dokebiId)
    {
        this.dokebiIdx = dokebiId;
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
            ServerData.userInfoTable.UpData(UserInfoTable.LastMap, e, false);
        }).AddTo(this);

        SettingData.FrameRateOption.AsObservable().Subscribe(SetFrameRate).AddTo(this);
    }

    private void ClearStage()
    {
        int lastIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.LastMap).Value;

        currentMapIdx.Value = Mathf.Max(lastIdx, 0);

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

            if (currentMapIdx.Value < 0)
            {
                currentMapIdx.Value = 0;
            }

            LoadNormalField();
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("첫번째 스테이지 입니다.");
        }
    }

    private bool CanMoveStage = true;
    public void LoadNextScene()
    {
        UiTutorialManager.Instance.SetClear(TutorialStep.GoNextStage);

        if (IsLastScene() == false && CanMoveStage)
        {
            CanMoveStage = false;

            currentMapIdx.Value++;

            LoadNormalField();
        }
    }

    public void MoveMapByIdx(int idx)
    {
        currentMapIdx.Value = idx;
        LoadNormalField();
    }

    private Coroutine internetConnectCheckRoutine;

    public void LoadNormalField()
    {
        if (internetConnectCheckRoutine != null)
        {
            StopCoroutine(internetConnectCheckRoutine);
        }

        internetConnectCheckRoutine = StartCoroutine(checkInternetConnection((isConnected) =>
        {
            if (isConnected)
            {
                contentsType = ContentsType.NormalField;

                ClearStage();

                ChangeScene();
            }
            else
            {
                currentMapIdx.Value--;

                if (currentMapIdx.Value < 0)
                {
                    currentMapIdx.Value = 0;
                }

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크가 불안정 합니다.\n잠시 후에 다시 시도해주세요.", null);
            }

            CanMoveStage = true;
        }));
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }


    public void LoadContents(ContentsType type)
    {
        if (type != ContentsType.NormalField)
        {
            lastContentsType = type;
        }

        if (type == ContentsType.FireFly)
        {
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);
        }

        contentsType = type;

        ChangeScene();
    }

    private void ChangeScene()
    {
        if (UiDeadConfirmPopup.Instance != null)
        {
            Destroy(UiDeadConfirmPopup.Instance.gameObject);
        }

        PopupManager.Instance.SetChatBoardPopupManager();

        PostManager.Instance.RefreshPost();

        if (contentsType == ContentsType.NormalField)
        {
            if (currentMapIdx.Value != 0 && UiTutorialManager.Instance != null)
            {
                //   UiTutorialManager.Instance.SetClear(TutorialStep._1_MoveField);
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
