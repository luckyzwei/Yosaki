using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class YoguiSoGulManager : SingletonMono<YoguiSoGulManager>
{
    [SerializeField]
    private PolygonCollider2D cameracollider;

    [SerializeField]
    private List<GameObject> spawnPoints;

    [SerializeField]
    private ReactiveProperty<int> currentWave = new ReactiveProperty<int>(0);

    [SerializeField]
    private UiSogulResultPopup uiSogulResultPopup;

    [SerializeField]
    private Image timerGauge;

    private bool clearLastStage = false;
    private bool deadFlag = false;

    [SerializeField]
    private Animator currentWaveAnim;

    [SerializeField]
    private Animator remainTextAnim;

    public enum EnemyType
    {
        Normal, Fly_Damage, Tanker, End
    }

    public enum ModeState
    {
        Wait, Playing, End
    }

    private ReactiveProperty<ModeState> modeState = new ReactiveProperty<ModeState>(ModeState.Wait);

    private void Start()
    {
        Subscribe();

        SetCameraCollider();

        DisableUi();

        SetDefault();
    }

    private void SetDefault()
    {
        remainTimeObject.gameObject.SetActive(false);
    }

    #region ETC
    private void DisableUi()
    {
        UiSubMenues.Instance.gameObject.SetActive(false);
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }
    #endregion

    private void Subscribe()
    {
        PlayerStatusController.Instance.whenPlayerDead.AsObservable().Subscribe(e =>
        {
            deadFlag = true;
            modeState.Value = ModeState.End;
        }).AddTo(this);

        modeState.AsObservable().Subscribe(state =>
        {
            switch (state)
            {
                case ModeState.Wait:
                    {
                        StartCoroutine(StartTimer());
                    }
                    break;
                case ModeState.Playing:
                    {
                        mainGameRoutine = StartCoroutine(MainGameRoutine());
                    }
                    break;
                case ModeState.End:
                    {
                        EndGame();
                    }
                    break;
            }
        }).AddTo(this);
    }


    #region StartTimer
    [SerializeField]
    private GameObject startTimerObject;
    [SerializeField]
    private TextMeshProUGUI startTimerText;
    [SerializeField]
    private TextMeshProUGUI remainEnemyText;

    private IEnumerator StartTimer()
    {
        //초반 딜레이
        //초반 딜레이 알림 시작
        startTimerObject.gameObject.SetActive(true);

        float tick = 0f;

        float startWaitTime = 5f;

        while (tick < 5f)
        {
            tick += Time.deltaTime;

            startTimerText.SetText($"{(int)startWaitTime - (int)tick}초 뒤에 습격 시작");

            yield return null;
        }

        PopupManager.Instance.ShowAlarmMessage("습격이 시작됩니다.");

        startTimerObject.gameObject.SetActive(false);

        modeState.Value = ModeState.Playing;
    }
    #endregion

    private Coroutine mainGameRoutine;

    [SerializeField]
    private TextMeshProUGUI currentWaveText;

    private bool IsClear()
    {
        return currentWave.Value >= TableManager.Instance.YoguisogulTable.dataArray.Length - 1;
    }
    private IEnumerator MainGameRoutine()
    {
        while (true)
        {
            if (IsEnemyAllDead())
            {
                //웨이브 올리기
                if (IsClear())
                {
                    clearLastStage = true;
                    modeState.Value = ModeState.End;
                    break;
                }
                else
                {
                    currentWaveAnim.SetTrigger(PlayStr);

                    currentWaveText.SetText($"{currentWave.Value + 1} 단계");

                    SpawnEnemies();
                    StartWaveTimer();

                    currentWave.Value++;

                    SoundManager.Instance.PlaySound("Reward");
                }
            }
            //진행중
            else
            {
                //

            }

            yield return null;
        }
    }
    private int GetEnemySpawnCount()
    {
        return TableManager.Instance.YoguisogulTable.dataArray[currentWave.Value].Spawnnum;
    }

    public EnemyTableData GetEnemyTableData(EnemyType enemyType)
    {
        EnemyTableData enemyData = new EnemyTableData();

        var tableData = TableManager.Instance.YoguisogulTable.dataArray[currentWave.Value];

        switch (enemyType)
        {
            case EnemyType.Normal:
                {
                    enemyData.Hp = tableData.Hp;

                    enemyData.Attackpower = tableData.Attackpower;

                    enemyData.Defense = (int)tableData.Defense;

                    enemyData.Movespeed = tableData.Movespeed;
                }
                break;
            case EnemyType.Fly_Damage:
                {
                    enemyData.Hp = tableData.Hp;

                    enemyData.Attackpower = tableData.Attackpower * tableData.Attackerratio;

                    enemyData.Defense = (int)tableData.Defense;

                    enemyData.Movespeed = tableData.Movespeed;
                }
                break;
            case EnemyType.Tanker:
                {
                    enemyData.Hp = tableData.Hp * tableData.Tankerhpratio;

                    enemyData.Attackpower = tableData.Attackpower;

                    enemyData.Defense = (int)tableData.Defense;

                    enemyData.Movespeed = tableData.Movespeed;
                }
                break;
        }

        return enemyData;
    }

    private List<Enemy> spawnedEnemyList = new List<Enemy>();
    private void SpawnEnemies()
    {
        int spawnCount = GetEnemySpawnCount();

        for (int i = 0; i < spawnCount; i++)
        {
            int randValue = Random.Range(0, (int)EnemyType.End);

            EnemyType enemyType = (EnemyType)randValue;

            var enemyObject = BattleObjectManager.Instance.GetItem($"YoguiSogul/{enemyType.ToString()}") as Enemy;

            enemyObject.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;

            enemyObject.SetReturnCallBack(EnemyRemoveCallBack);

            EnemyTableData enemyData = GetEnemyTableData(enemyType);

            enemyObject.Initialize(enemyData, false, 0);

            spawnedEnemyList.Add(enemyObject);

            UpdateRemainEnemy();
        }
    }

    private void EnemyRemoveCallBack(Enemy enemy)
    {
        spawnedEnemyList.Remove(enemy);
        UpdateRemainEnemy();
    }

    private static string PlayStr = "Play";
    private void UpdateRemainEnemy()
    {
        remainEnemyText.SetText($"남은 요괴:{spawnedEnemyList.Count}");

        if (remainTextAnim != null)
        {
            remainTextAnim.SetTrigger(PlayStr);
        }
    }

    private void StartWaveTimer()
    {
        if (waveTimerRoutine != null)
        {
            StopCoroutine(waveTimerRoutine);
        }

        waveTimerRoutine = StartCoroutine(WaveTimerRoutine());
    }

    [SerializeField]
    private GameObject remainTimeObject;

    [SerializeField]
    private TextMeshProUGUI remainTimeText;

    private Coroutine waveTimerRoutine;

    private IEnumerator WaveTimerRoutine()
    {
        remainTimeObject.gameObject.SetActive(true);

        timerGauge.fillAmount = 1f;

        float tick = 0f;

        float waveTime = 10f;

        while (tick < waveTime)
        {
            tick += Time.deltaTime;

            remainTimeText.SetText($"남은시간 : {(int)waveTime - (int)tick}");

            timerGauge.fillAmount = (waveTime - tick) / waveTime;

            yield return null;
        }

        timerGauge.fillAmount = 0f;

        modeState.Value = ModeState.End;
    }

    private bool IsEnemyAllDead()
    {
        return spawnedEnemyList.Count == 0;
    }

    private void StopGameRoutines()
    {
        if (mainGameRoutine != null)
        {
            StopCoroutine(mainGameRoutine);
        }

        if (waveTimerRoutine != null)
        {
            StopCoroutine(waveTimerRoutine);
        }
    }
    private void EndGame()
    {
        SoundManager.Instance.PlaySound("Reward");

        StopGameRoutines();

        if (clearLastStage == false)
        {
            int pref = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value;

            int updateValue = currentWave.Value - 1;

            if (updateValue > pref)
            {
                ServerData.userInfoTable.UpData(UserInfoTable.yoguiSogulLastClear, updateValue, false);
            }

            uiSogulResultPopup.Initialize(currentWave.Value - 1, clearLastStage, deadFlag);

            RankManager.Instance.UpdateYoguiSogul_Score(currentWave.Value - 1);
        }
        else
        {
            ServerData.userInfoTable.UpData(UserInfoTable.yoguiSogulLastClear, currentWave.Value, false);

            uiSogulResultPopup.Initialize(currentWave.Value, clearLastStage, deadFlag);

            RankManager.Instance.UpdateYoguiSogul_Score(currentWave.Value);
        }


    }
}
