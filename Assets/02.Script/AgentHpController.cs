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

    public void Initialize(EnemyTableData enemyTableData, bool isFieldBossEnemy = false,bool updateSubHpBar = false)
    {
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

        if (GameManager.contentsType == GameManager.ContentsType.Boss)
        {
            value += value * PlayerStats.GetBossDamAddValue();
        }

        //보너스던전등 특수몹
        if (enemyTableData != null && enemyTableData.Useonedamage)
        {
            isCritical = false;
            value = -1f;
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

        Vector3 damTextspawnPos = spawnPos + Vector3.up * 1f + Vector3.right * UnityEngine.Random.Range(-0.4f, 0.4f) + Vector3.up * UnityEngine.Random.Range(-0.2f, 0.2f);


        if (Vector3.Distance(playerPos.position, this.transform.position) < GameBalance.effectActiveDistance)
        {
            BattleObjectManager.Instance.SpawnDamageText(value * -1f, damTextspawnPos, (isCritical ? DamTextType.Critical : DamTextType.Normal));
        }
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
        //방어력 적용
        ApplyDefense(ref value);
        //

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
    private void ApplyDefense(ref float value)
    {
        float ignoreDefense = PlayerStats.GetIgnoreDefenseValue();

        float enemyDefense = Mathf.Max(0f, defense - ignoreDefense);

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
}
