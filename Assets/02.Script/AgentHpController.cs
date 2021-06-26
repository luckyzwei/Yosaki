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

    #region DamText
    private int attackCount = 0;
    private int attackCountMax = 8;
    private float attackResetCount = 0f;
    private float damTextStartOffectY = 0f;
    private float damTextOffsetY = 0.1f;
    private float damTextOffsetZ = 0.01f;
    private float damTextMergeTime = 0.5f;
    private const float damTextCountAddValue = 0.1f;
    private readonly WaitForSeconds DamTextDelay = new WaitForSeconds(damTextCountAddValue);
    private Coroutine damTextRoutine;
    #endregion
    private readonly WaitForSeconds waitEffectDelay = new WaitForSeconds(0.2f);

    [SerializeField]
    private EnemyHpBar enemyHpBar;

    private EnemyMoveController enemyMoveController;

    private bool isEnemyDead = false;

    public ReactiveCommand<float> WhenAgentDamaged = new ReactiveCommand<float>();

    private Transform playerPos;

    private void Awake()
    {
        GetRequireComponents();
        Initialize();
    }

    private void Initialize()
    {
        damTextStartOffectY = UnityEngine.Random.Range(0.5f, 1f);
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
        }).AddTo(this);
    }

    private void ResetEnemy()
    {
        currentHp.Value = maxHp;
        ResetDamTextValue();
        isEnemyDead = false;
    }

    private void ResetDamTextValue()
    {
        attackCount = 0;
        attackResetCount = 0f;
        damTextRoutine = null;
    }

    private IEnumerator DamTextRoutine()
    {
        while (attackResetCount < damTextMergeTime)
        {
            yield return DamTextDelay;
            attackResetCount += 0.1f;
        }

        ResetDamTextValue();
    }

    public void Initialize(EnemyTableData enemyTableData, bool isFieldBossEnemy = false)
    {
        this.enemyTableData = enemyTableData;

        SetHp(isFieldBossEnemy == false ? enemyTableData.Hp : enemyTableData.Hp * enemyTableData.Bosshpratio);

        SetHardIcon();
    }

    private void SetHardIcon()
    {
        if (enemyTableData.Ishardenemy)
        {
            enemyHpBar.SetHardIcon();
        }
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
    private Vector3 damTextspawnPos;
    private static string hitSfxName = "EnemyHitted";
    private void ApplyCriticalAndDamText(ref float value)
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

        if (damTextRoutine == null)
        {
            damTextRoutine = StartCoroutine(DamTextRoutine());
        }



        if (attackCount == 0)
        {
            damTextspawnPos = this.transform.position;
        }

        attackCount++;

        if (attackCount == attackCountMax)
        {
            attackCount = 0;
        }

        attackResetCount = 0f;


        //spawnPos += this.transform.position;
        damTextspawnPos += Vector3.up * ((attackCount * damTextOffsetY) + damTextStartOffectY);
        damTextspawnPos += Vector3.back * attackCount * damTextOffsetZ;
        //damTextspawnPos += Vector3.right * UnityEngine.Random.Range(-0.2f, 0.2f);

        if (Vector3.Distance(playerPos.position, this.transform.position) < GameBalance.effectActiveDistance)
        {
            BattleObjectManager.Instance.SpawnDamageText(value * -1f, isCritical, damTextspawnPos);
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

        ApplyCriticalAndDamText(ref value);

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

        float enemyDefense = enemyTableData.Defense - ignoreDefense;

        value -= value * enemyDefense * 0.01f;
    }

    private void EnemyDead()
    {
        whenEnemyDead.Execute();

        isEnemyDead = true;

        AddEnemyDeadCount();

        GetGoldByEnemy(enemyTableData.Gold);

        this.gameObject.SetActive(false);
    }

    private void AddEnemyDeadCount()
    {
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value++;
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void GetGoldByEnemy(float gold)
    {
        gold += gold * PlayerStats.GetGoldPlusValue();

        DatabaseManager.goodsTable.GetGold(gold);
    }
}
