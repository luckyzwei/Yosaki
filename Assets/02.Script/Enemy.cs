using CodeStage.AntiCheat.ObscuredTypes;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(EnemyMoveBase))]
[RequireComponent(typeof(AgentHpController))]
public class Enemy : PoolItem
{
    private AgentHpController agentHpController;

    private EnemyMoveController enemyMoveController;
    private FlyMove_Normal flyMove_normal;

    private Action<Enemy> returnCallBack;

    private Action<Enemy> enemyDeadCallBack;

    public EnemyTableData tableData { get; private set; }

    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    private EnemyHitObject enemyHitObject;

    private Vector3 originScale;

    private float bossSize = 2f;

    public ObscuredBool isFieldBossEnemy { get; private set; } = false;

    public int spawnedPlatformIdx = 0;

    [SerializeField]
    private bool isFlyingEnemy = false;

    private void Awake()
    {
        SetOriginScale();
        SetRequireComponents();
    }

    private void SetOriginScale()
    {
        this.originScale = this.transform.localScale;
    }
    private bool initialized = false;
    private void SetRequireComponents()
    {
        if (initialized == true)
        {
            return;
        }

        initialized = true;
        agentHpController = GetComponent<AgentHpController>();

        if (isFlyingEnemy == false)
        {
            enemyMoveController = GetComponent<EnemyMoveController>();
        }
        else
        {
            flyMove_normal = GetComponent<FlyMove_Normal>();
        }

        enemyHitObject = GetComponentInChildren<EnemyHitObject>();
    }

    public void SetReturnCallBack(Action<Enemy> returnCallBack)
    {
        this.returnCallBack = returnCallBack;
    }

    public void SetEnemyDeadCallBack(Action<Enemy> enemyDeadCallBack)
    {
        this.enemyDeadCallBack = enemyDeadCallBack;
    }

    private void Start()
    {
        Subscribe();
    }

    public void Initialize(EnemyTableData enemyTableData, bool isFieldBossEnemy = false, int spawnedPlatformIdx = 0, bool updateSubHpBar = false)
    {
        SetRequireComponents();

        this.spawnedPlatformIdx = spawnedPlatformIdx;

        this.isFieldBossEnemy = isFieldBossEnemy;

        this.tableData = enemyTableData;

        agentHpController.Initialize(enemyTableData, isFieldBossEnemy, updateSubHpBar);

        this.transform.localScale = isFieldBossEnemy == false ? originScale : originScale * bossSize;

        if (enemyHitObject != null)
        {
            if (isFieldBossEnemy == false)
            {
                enemyHitObject.SetDamage(tableData.Attackpower);
            }
            else
            {
                enemyHitObject.SetDamage(tableData.Attackpower * tableData.Bossattackratio);
            }
        }

        if (isFlyingEnemy) 
        {
            flyMove_normal.Initialize(Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)) * Vector3.right, tableData.Movespeed);
        }
    }

    private void SetSkeletonColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString(tableData.Color, out color);
        skeletonAnimation.skeleton.SetColor(color);
    }

    private void Subscribe()
    {
        agentHpController.whenEnemyDead.AsObservable().Subscribe(WhenEnemyDead).AddTo(this);
        agentHpController.WhenAgentDamaged.AsObservable().Subscribe(WhenAgentDamaged).AddTo(this);
    }

    private void WhenAgentDamaged(float damage)
    {
        if (isFlyingEnemy == false)
        {
            enemyMoveController.SetMoveState(EnemyMoveController.MoveState.FollowPlayer);
        }
        else
        {
            //flyMove_normal
        }
    }
    private static string DeadEfxName = "Dead";
    private void WhenEnemyDead(Unit unit)
    {
        GrowthManager.Instance.GetExp(tableData.Exp);

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.KillEnemy, 1);

        EffectManager.SpawnEffectAllTime(DeadEfxName, this.transform.position + Vector3.up * 1f);

        SpawnDropItem();

        GetPetUpgradeGem();

        GetEventItem();

        GetStageRelicItem();

        enemyDeadCallBack?.Invoke(this);

        if (this.isFieldBossEnemy)
        {
            WhenFieldBossEnemyDead();
        }
    }

    private void GetPetUpgradeGem()
    {
        ServerData.goodsTable.GetPetUpgradeSoul(1);
    }

    private void GetEventItem()
    {
        if (ServerData.userInfoTable.CanSpawnEventItem() == false) return;

        ServerData.goodsTable.GetEventItem(1);
    }

    private void GetStageRelicItem() 
    {
        if (GameManager.Instance.IsNormalField == false) return;

        ServerData.goodsTable.GetStageRelic(GameManager.Instance.CurrentStageData.Relicspawnamount);
    }

    private void WhenFieldBossEnemyDead()
    {
        //보상처리
        MapInfo.Instance.SetFieldClear();
        //

        UiStageNameIndicater.Instance.StopFieldBossTimer();
    }

    //private void UpdateCollection()
    //{
    //    if (tableData.Usecollection == false) return;
    //    var collectionData = CollectionManager.Instance.GetCollectionData(tableData.Collectionkey, true);
    //    collectionData.amount.Value++;
    //}

    private void SpawnDropItem()
    {
        if (GameManager.Instance.SpawnMagicStone == false) return;

        //GrowthStone
        float magicStoneSpawnAmount = GameManager.Instance.CurrentStageData.Magicstoneamount;
        float marbleSpawnAmount = GameManager.Instance.CurrentStageData.Marbleamount;

        if (ServerData.userInfoTable.TableDatas[UserInfoTable.marbleAwake].Value != 1)
        {
            if (magicStoneSpawnAmount != 0)
            {
                var growthStone = BattleObjectManager.Instance.dropItemProperty.GetItem();
                growthStone.Initialize(Item_Type.GrowthStone, magicStoneSpawnAmount);
                growthStone.transform.position = this.transform.position + UnityEngine.Random.Range(-0.3f, 0.3f) * Vector3.right;
            }

            //여우구슬
            if (marbleSpawnAmount != 0)
            {
                var marble = BattleObjectManager.Instance.dropItemProperty.GetItem();
                marble.Initialize(Item_Type.Marble, marbleSpawnAmount);
                marble.transform.position = this.transform.position + UnityEngine.Random.Range(-0.3f, 0.3f) * Vector3.right;
            }
        }
        else 
        {
            ServerData.goodsTable.GetMagicStone(magicStoneSpawnAmount);
            ServerData.goodsTable.GetMarble(marbleSpawnAmount);
        }


    }



    private new void OnDisable()
    {
        base.OnDisable();
        this.returnCallBack?.Invoke(this);

        if (isFieldBossEnemy && MapInfo.Instance != null)
        {
            MapInfo.Instance.SetCanSpawnEnemy(true);
        }
    }
}
