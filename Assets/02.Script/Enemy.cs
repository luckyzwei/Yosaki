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

    private Action<Enemy> returnCallBack;

    private Action<Enemy> enemyDeadCallBack;

    public EnemyTableData tableData { get; private set; }

    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    private EnemyHitObject enemyHitObject;

    private Vector3 originScale;

    private float bossSize = 2f;

    public ObscuredBool isFieldBossEnemy { get; private set; } = false;

    private void Awake()
    {
        SetOriginScale();
        SetRequireComponents();
    }

    private void SetOriginScale()
    {
        this.originScale = this.transform.localScale;
    }

    private void SetRequireComponents()
    {
        agentHpController = GetComponent<AgentHpController>();
        enemyMoveController = GetComponent<EnemyMoveController>();
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
        SetBaseInfo();
        Subscribe();
    }

    public void Initialize(EnemyTableData enemyTableData, bool isFieldBossEnemy = false)
    {
        this.isFieldBossEnemy = isFieldBossEnemy;

        this.tableData = enemyTableData;

        agentHpController.Initialize(enemyTableData, isFieldBossEnemy);

        this.transform.localScale = isFieldBossEnemy == false ? originScale : originScale * bossSize;
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
        enemyMoveController.SetMoveState(EnemyMoveController.MoveState.FollowPlayer);
    }

    private void WhenEnemyDead(Unit unit)
    {
        GrowthManager.Instance.GetExp(tableData.Exp);

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.KillEnemy, 1);

        EffectManager.SpawnEffect("GenericDeath", this.transform.position + Vector3.up * 1f);

        SpawnMagicStone();

        UpdateCollection();

        UpdateFieldKillCount();

        enemyDeadCallBack?.Invoke(this);

        if (this.isFieldBossEnemy)
        {
            WhenFieldBossEnemyDead();
        }
    }

    private void WhenFieldBossEnemyDead()
    {
        //보상처리
        MapInfo.Instance.RewardFieldBoss();
        //

        UiStageNameIndicater.Instance.StopFieldBossTimer();
    }

    private void UpdateFieldKillCount()
    {
        if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField) return;

        string stageKey = GameManager.Instance.CurrentStageData.Stagestringkey;

        DatabaseManager.fieldBossTable.TableDatas[stageKey].killCount.Value++;
    }

    private void UpdateCollection()
    {
        if (tableData.Usecollection == false) return;
        var collectionData = CollectionManager.Instance.GetCollectionData(tableData.Collectionkey, true);
        collectionData.amount.Value++;
    }

    private void SpawnMagicStone()
    {
        if (GameManager.Instance.SpawnMagicStone == false) return;

        var dropItem = BattleObjectManager.Instance.dropItemProperty.GetItem();

        float magicStoneSpawnAmount = GameManager.Instance.CurrentStageData.Magicstoneamount;

        dropItem.Initialize(Item_Type.MagicStone, magicStoneSpawnAmount);
        dropItem.transform.position = this.transform.position;
    }

    private void SetBaseInfo()
    {
        if (agentHpController != null)
        {
            agentHpController.Initialize(tableData, isFieldBossEnemy);
        }

        if (enemyHitObject != null)
        {
            enemyHitObject.SetDamage(tableData.Attackpower);
        }
    }

    private new void OnDisable()
    {
        base.OnDisable();
        this.returnCallBack?.Invoke(this);
    }
}
