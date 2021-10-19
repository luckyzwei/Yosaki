using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AgentHpController : MonoBehaviour
{
    private ReactiveProperty<float> currentHp = new ReactiveProperty<float>();
    public float CurrentHp => currentHp.Value;
    public ReactiveCommand whenEnemyDead { get; private set; } = new ReactiveCommand();
    public ReactiveCommand<float> whenEnemyDamaged { get; private set; } = new ReactiveCommand<float>();
    public float maxHp { get; private set; }

    private EnemyTableData enemyTableData;

    [SerializeField]
    private EnemyHpBar enemyHpBar;

    private EnemyMoveController enemyMoveController;

    private bool isEnemyDead = false;

    public ReactiveCommand<float> WhenAgentDamaged = new ReactiveCommand<float>();

    private Transform playerPos;

    private ObscuredFloat defense;

    [SerializeField]
    private Transform damTextSpawnPos;

    private bool updateSubHpBar = false;

    private bool isFieldBossEnemy = false;
    private bool fieldBossTimerStarted = false;

    private void Awake()
    {
        GetRequireComponents();
    }
    private void GetRequireComponents()
    {
        enemyMoveController = GetComponent<EnemyMoveController>();
    }

    private void Start()
    {
        Subscribe();

        playerPos = PlayerMoveController.Instance.transform;
    }

    private void Subscribe()
    {
        currentHp.AsObservable().Subscribe(e =>
        {
            enemyHpBar.UpdateGauge(e, maxHp);

            if (updateSubHpBar)
            {
                UiSubHpBar.Instance.UpdateGauge(e, maxHp);
            }
        }).AddTo(this);
    }

    private void ResetEnemy()
    {
        currentHp.Value = maxHp;
        isEnemyDead = false;
    }

    public void SetDefense(float defense)
    {
        this.defense = defense;
    }

    public void Initialize(EnemyTableData enemyTableData, bool isFieldBossEnemy = false, bool updateSubHpBar = false)
    {
        fieldBossTimerStarted = false;

        this.isFieldBossEnemy = isFieldBossEnemy;

        this.enemyTableData = enemyTableData;

        this.updateSubHpBar = isFieldBossEnemy || updateSubHpBar;

        SetDefense(enemyTableData.Defense);

        SetHp(isFieldBossEnemy == false ? enemyTableData.Hp : enemyTableData.Hp * enemyTableData.Bosshpratio);
    }

    public void SetHp(float hp)
    {
        this.maxHp = hp;
        currentHp.Value = maxHp;
    }

    public void SetKnockBack()
    {
        if (enemyMoveController == null) return;
        enemyMoveController.SetKnockBack();
    }
    private static string hitSfxName = "EnemyHitted";
    private static string deadSfxName = "EnemyDead";
    private void ApplyPlusDamage(ref float value)
    {
        bool isCritical = PlayerStats.ActiveCritical();

        SoundManager.Instance.PlaySound(hitSfxName);

        //크리티컬
        if (isCritical)
        {
            value += value * PlayerStats.CriticalDam();
        }

        //보너스던전등 특수몹
        if (enemyTableData != null && enemyTableData.Useonedamage)
        {
            isCritical = false;
            value = -1f;
        }

        if (GameManager.contentsType != GameManager.ContentsType.NormalField || isFieldBossEnemy)
        {
            value += value * PlayerStats.GetBossDamAddValue();
        }

        //방어력 초과데미지
        //방어력 차이
        float gapDefense = defense - ignoreDefenseValue;
        if (gapDefense < 0)
        {
            float penetrateValue = PlayerStats.GetPenetrateDefense();
            value += Mathf.Abs(gapDefense) * value * penetrateValue;
        }

        //슈퍼크리티컬

        bool isSuperCritical = PlayerStats.ActiveSuperCritical();

        if (isSuperCritical)
        {
            //30% 고정
            value += value * GameBalance.SuperCriticalDamPer;
        }

        Vector3 spawnPos = Vector3.zero;

        if (damTextSpawnPos != null)
        {
            spawnPos = damTextSpawnPos.position;
        }
        else
        {
            spawnPos = this.transform.position;
        }

        if (damTextRoutine == null)
        {
            damTextRoutine = StartCoroutine(DamTextRoutine());
        }

        damTextspawnPos = this.transform.position + Vector3.up * attackCount * 1f + Vector3.right * rightValue + Vector3.up * upValue;

        attackCount++;

        if (attackCount == attackCountMax)
        {
            attackCount = 0;
            rightValue = UnityEngine.Random.Range(-3f, 3f);
            upValue = UnityEngine.Random.Range(2f, 4.5f);
        }

        attackResetCount = 0f;

        if (Vector3.Distance(playerPos.position, this.transform.position) < GameBalance.effectActiveDistance)
        {
            DamTextType damType = DamTextType.Normal;

            if (isCritical) 
            {
                 damType = DamTextType.Critical;
            }

            if (isSuperCritical) 
            {
                 damType = DamTextType.SuperCritical;
            }

            BattleObjectManagerAllTime.Instance.SpawnDamageText(value * -1f, damTextspawnPos, damType);
        }
    }
    private Vector3 damTextspawnPos;
    private Coroutine damTextRoutine;
    private int attackCount = 0;
    private int attackCountMax = 16;
    private float attackResetCount = 0f;
    private float damTextMergeTime = 0.5f;
    private const float damTextCountAddValue = 0.1f;
    private readonly WaitForSeconds DamTextDelay = new WaitForSeconds(damTextCountAddValue);
    private float rightValue = 0f;
    private float upValue = 2f;

    private IEnumerator DamTextRoutine()
    {
        while (attackResetCount < damTextMergeTime)
        {
            yield return DamTextDelay;
            attackResetCount += 0.1f;
        }

        ResetDamTextValue();
    }
    private void ResetDamTextValue()
    {
        attackCount = 0;
        attackResetCount = 0f;
        damTextRoutine = null;
    }

    //안씀, 골드 드랍으로 바꿈
    private void GetHitGold(float value)
    {
        //데미지는 -값임
        if (value < 0)
        {
            if (currentHp.Value + value > 0)
            {
                GetGoldByEnemy(-value);
            }
            else
            {
                GetGoldByEnemy(-(value - currentHp.Value));
            }
        }

    }

    public void UpdateHp(float value)
    {
        if (value < 0 && isFieldBossEnemy && fieldBossTimerStarted == false)
        {
            fieldBossTimerStarted = true;
            UiStageNameIndicater.Instance.StartFieldBossTimer(15);
        }

        //방어력 적용
        ApplyDefense(ref value);
        //1

        value *= DamageBalance.GetRandomDamageRange();

        ApplyPlusDamage(ref value);

        if (isEnemyDead == true) return;

        if (value < 0f)
        {
            WhenAgentDamaged.Execute(-value);
        }

        //GetHitGold(value);

        currentHp.Value += value;

        whenEnemyDamaged.Execute(value);

        if (currentHp.Value <= 0)
        {
            EnemyDead();

            return;
        }
    }
    private float ignoreDefenseValue;

    private void ApplyDefense(ref float value)
    {
        ignoreDefenseValue = PlayerStats.GetIgnoreDefenseValue();

        float enemyDefense = Mathf.Max(0f, defense - ignoreDefenseValue);

        value -= value * enemyDefense * 0.01f;
    }

    private void EnemyDead()
    {
        whenEnemyDead.Execute();

        isEnemyDead = true;

        AddEnemyDeadCount();

        GetGoldByEnemy(enemyTableData.Gold);

        this.gameObject.SetActive(false);

        SoundManager.Instance.PlaySound(deadSfxName);
    }

    private void AddEnemyDeadCount()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value++;
        ServerData.userInfoTable.GetKillCountTotal();
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void GetGoldByEnemy(float gold)
    {
        gold += gold * PlayerStats.GetGoldPlusValue();

        ServerData.goodsTable.GetGold(gold);
    }

    private void OnDisable()
    {
        updateSubHpBar = false;
    }
}
