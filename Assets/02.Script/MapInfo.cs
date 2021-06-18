using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class MapInfo : SingletonMono<MapInfo>
{
    private List<EnemySpawnPlatform> spawnPlatforms;
    private List<EnemyTableData> spawnEnemyData;
    private List<Enemy> spawnedEnemyList = new List<Enemy>();
    public List<Enemy> SpawnedEnemyList => spawnedEnemyList;
    private int maxEnemy = 0;

    [SerializeField]
    private PolygonCollider2D cameracollider;

    public ReactiveCommand whenSpawnedEnemyCountChanged = new ReactiveCommand();
    public ReactiveProperty<float> spawnGaugeValue = new ReactiveProperty<float>();

    private new void Awake()
    {
        m_DontDestroy = false;
        Initialize();
        base.Awake();
        SetCameraCollider();
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }
    private void Initialize()
    {

        spawnPlatforms = GetComponentsInChildren<EnemySpawnPlatform>().ToList();

        SetEnemyData();


    }
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());

        Subscribe();
    }

    private void Subscribe()
    {
        //플레이어 사망시
        PlayerStatusController.Instance.whenPlayerDead.AsObservable().Subscribe(e =>
        {
            //자동사냥 종료
            AutoManager.Instance.SetAuto(false);

            //몹 스폰 중단
            StopAllCoroutines();

            //경험치 절반 감소
            GrowthManager.Instance.WhenPlayerDeadInNormalField();

            PopupManager.Instance.ShowDeadConfirmPopup(CommonString.Notice, "플레이어가 사망했습니다.\n경험치 50%를 잃습니다.", () =>
            {
                GameManager.Instance.LoadNormalField();
            });

        }).AddTo(this);

        UiStageNameIndicater.Instance.whenFieldBossTimerEnd.AsObservable().Subscribe(WhenFieldBossTimerEnd).AddTo(this);
    }

    private void WhenFieldBossTimerEnd(Unit unit)
    {
        UiStageNameIndicater.Instance.StopFieldBossTimer();
        DisableFieldBoss();
        PopupManager.Instance.ShowAlarmMessage("보스가 도망갔습니다!");
    }

    private void SetEnemyData()
    {
        spawnEnemyData = GameManager.Instance.GetEnemyTableData();

        maxEnemy = spawnPlatforms.Count * GameManager.Instance.CurrentStageData.Spawnamountperplatform;
    }
    [Header("init at script")]
    public Transform min;
    public Transform max;

    public Vector3 GetRandomPos()
    {
        return new Vector2(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y));
    }

    private bool IsEnemyMax()
    {
        return spawnedEnemyList.Count >= maxEnemy;
    }


    private IEnumerator EnemySpawnRoutine()
    {
        if (spawnPlatforms == null || spawnPlatforms.Count == 0) yield break;

        //초기딜레이
        yield return new WaitForSeconds(2.0f);

        if (UiAutoRevive.autoRevive)
        {
            UiAutoRevive.autoRevive = false;
            AutoManager.Instance.SetAuto(true);
        }

        WaitForSeconds spawnDelay = new WaitForSeconds(GameManager.Instance.CurrentStageData.Spawndelay);
        WaitForSeconds spawnInterval = new WaitForSeconds(3f);
        while (true)
        {
            int spawnNum = maxEnemy - spawnedEnemyList.Count;

            for (int i = 0; i < spawnNum; i++)
            {
                SpawnEnemy();

                whenSpawnedEnemyCountChanged.Execute();

                if (i == spawnNum / 2)
                {
                    yield return spawnInterval;
                }
            }

            if (gaugeRoutine != null)
            {
                StopCoroutine(gaugeRoutine);
            }

            gaugeRoutine = StartCoroutine(UpdateGauge(GameManager.Instance.CurrentStageData.Spawndelay));

            yield return spawnDelay;
        }
    }

    private void SpawnEnemy(bool isBossEnemy = false)
    {
        if (spawnEnemyData.Count == 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "데이터 없음", null);
            return;
        }

        var enemyObject = BattleObjectManager.Instance.GetItem($"Enemy/{spawnEnemyData[0].Prefabname}") as Enemy;

        if (isBossEnemy == false)
        {
            Vector3 spawnPos = spawnPlatforms[Random.Range(0, spawnPlatforms.Count)].GetRandomSpawnPos();
            enemyObject.transform.position = spawnPos;
        }
        else
        {
            UiStageNameIndicater.Instance.StartFieldBossTimer(15);

            PopupManager.Instance.ShowAlarmMessage("필드보스 출현!");

            enemyObject.transform.position = PlayerMoveController.Instance.transform.position;

            EffectManager.SpawnEffect("Circle1", enemyObject.transform.position);
            SoundManager.Instance.PlaySound("4-1");
        }


        enemyObject.SetReturnCallBack(EnemyRemoveCallBack);

        enemyObject.Initialize(spawnEnemyData[0], isBossEnemy);

        spawnedEnemyList.Add(enemyObject);
    }

    private Coroutine gaugeRoutine;

    private IEnumerator UpdateGauge(float spawnDelay)
    {
        spawnGaugeValue.Value = 0f;

        while (spawnGaugeValue.Value < spawnDelay)
        {
            spawnGaugeValue.Value += Time.deltaTime;

            yield return null;
        }

        spawnGaugeValue.Value = spawnDelay;
    }

#if UNITY_EDITOR
    IEnumerator SpawnTimerRoutine_Text(float delay)
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1);
        while (delay > 0)
        {
            Debug.LogError($"Enemy Spawn remain {delay}");
            yield return oneSecond;
            delay -= 1f;
        }
    }
#endif

    private void EnemyRemoveCallBack(Enemy enemy)
    {
        spawnedEnemyList.Remove(enemy);
        whenSpawnedEnemyCountChanged.Execute();
    }


    public void SpawnBossEnemy()
    {
        if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField)
        {
            PopupManager.Instance.ShowAlarmMessage("이곳에서는 소환할 수 없습니다.");
            return;
        }

        SpawnEnemy(true);
    }

    public bool HasSpawnedBossEnemy()
    {
        for (int i = 0; i < spawnedEnemyList.Count; i++)
        {
            if (spawnedEnemyList[i].isFieldBossEnemy == true)
            {
                return true;
            }
        }
        return false;
    }
    private void DisableFieldBoss() 
    {
        for (int i = 0; i < spawnedEnemyList.Count; i++)
        {
            if (spawnedEnemyList[i].isFieldBossEnemy == true)
            {
                spawnedEnemyList[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    public void RewardFieldBoss()
    {
        int rewardValue = GameManager.Instance.CurrentStageData.Bossrewardvalue;

        DatabaseManager.goodsTable.GetTableData(GoodsTable.MagicStone).Value += rewardValue;

        DatabaseManager.goodsTable.UpData(GoodsTable.MagicStone, false);

        UiFieldBossRewardView.Instance.Initialize(rewardValue);
    }

//#if UNITY_EDITOR
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.W)) 
//        {
//            DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingGrade).Value++;
//        }
//    }
//#endif
}
