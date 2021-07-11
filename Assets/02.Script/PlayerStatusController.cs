using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerStatusController : SingletonMono<PlayerStatusController>
{
    private bool canHit = true;

    private WaitForSeconds hitDelay = new WaitForSeconds(1f);
    public ReactiveProperty<float> maxHp { get; private set; } = new ReactiveProperty<float>(GameBalance.initHp);
    public ReactiveProperty<float> hp { get; private set; } = new ReactiveProperty<float>();

    public ReactiveProperty<float> maxMp { get; private set; } = new ReactiveProperty<float>(GameBalance.initMp);
    public ReactiveProperty<float> mp { get; private set; } = new ReactiveProperty<float>();

    public ReactiveCommand whenPlayerDead = new ReactiveCommand();

    private float damTextYOffect = 1f;

    private static bool isFirstGame = true;

    private void Start()
    {
        Initialize();
        Subscribe();
        StartCoroutine(RecoverRoutine());
    }

    public bool IsPlayerDead()
    {
        return hp.Value <= 0f;
    }

    private IEnumerator RecoverRoutine()
    {
        WaitForSeconds recoverDelay = new WaitForSeconds(5.0f);

        while (true)
        {
            yield return recoverDelay;

            if (IsPlayerDead()) continue;

            float hpRecoverPer = PlayerStats.GetHpRecover();
            float mpRecoverPer = PlayerStats.GetMpRecover();

            if (IsHpFull() == false && hpRecoverPer != 0f)
            {
                UpdateHp(maxHp.Value * hpRecoverPer);
            }

            if (IsMpFull() == false && mpRecoverPer != 0f)
            {
                UpdateMp(maxMp.Value * mpRecoverPer);
            }
        }
    }

    private void UpdateHpMax()
    {
        maxHp.Value = PlayerStats.GetMaxHp();
    }
    private void UpdateMpMax()
    {
        maxMp.Value = PlayerStats.GetMaxMp();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.HpLevel_Gold).Subscribe(e =>
        {
            UpdateHpMax();
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.HpPer_StatPoint).Subscribe(e =>
        {
            UpdateHpMax();
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.MpLevel_Gold).Subscribe(e =>
        {
            UpdateMpMax();
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.MpPer_StatPoint).Subscribe(e =>
        {
            UpdateMpMax();
        }).AddTo(this);

        hp.AsObservable().Subscribe(e =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.Hp).Value = e;
        }).AddTo(this);

        mp.AsObservable().Subscribe(e =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.Mp).Value = e;
        }).AddTo(this);

        //레벨업때
        ServerData.statusTable.GetTableData(StatusTable.Level).Subscribe(e =>
        {
            SetHpMpFull();
        }).AddTo(this);

        //코스튬 인덱스 바뀔때
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].AsObservable().Subscribe(e =>
        {
            UpdateHpMax();
            UpdateMpMax();
        }).AddTo(this);

        //코스튬 새로운 능력치 뽑기 했을때
        ServerData.costumeServerTable.WhenCostumeOptionChanged.AsObservable().Subscribe(e =>
        {
            UpdateHpMax();
            UpdateMpMax();
        }).AddTo(this);

        //펫 획득 했을때

        var iter = ServerData.petTable.TableDatas.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.hasItem.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);
        }

        //날개 렙업
        ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(e =>
        {
            UpdateHpMax();
            UpdateMpMax();
        }).AddTo(this);

        //패시브스킬
        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)StatusType.HpAddPer) continue;

            var serverData = ServerData.passiveServerTable.TableDatas[tableData[i].Stringid];

            serverData.level.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
            }).AddTo(this);
        }
    }

    private void Initialize()
    {
        UpdateHpMax();

        UpdateMpMax();

        if (isFirstGame)
        {
            isFirstGame = false;
            SetHpMpFull();
        }
        else
        {
            hp.Value = ServerData.userInfoTable.GetTableData(UserInfoTable.Hp).Value;
            mp.Value = ServerData.userInfoTable.GetTableData(UserInfoTable.Mp).Value;
        }
    }

    private void SetHpMpFull()
    {
        hp.Value = maxHp.Value;
        mp.Value = maxMp.Value;
    }

    private bool IsHpFull()
    {
        return hp.Value == maxHp.Value;
    }

    private bool IsMpFull()
    {
        return mp.Value == maxMp.Value;
    }

    public void UpdateHp(float value)
    {
        //데미지입음
        if (value < 0)
        {
            if (canHit == false || IsPlayerDead()) return;

            float damDecreaseValue = PlayerStats.GetDamDecreaseValue();

            value += value * damDecreaseValue;

            StartCoroutine(HitDelayRoutine());
        }
        //회복함
        else
        {
            if (IsPlayerDead()) return;
        }


#if UNITY_EDITOR
        Debug.Log($"Player damaged {value}");
#endif

        SpawnDamText(value);

        hp.Value += value;

        hp.Value = Mathf.Clamp(hp.Value, 0f, maxHp.Value);

        CheckDead();
    }

    public void UpdateMp(float value)
    {
        mp.Value += value;

        mp.Value = Mathf.Clamp(mp.Value, 0f, maxMp.Value);
    }

    private void CheckDead()
    {
#if UNITY_EDITOR
        //return;
#endif

        if (hp.Value <= 0)
        {
            whenPlayerDead.Execute();
        }
    }

    private void SpawnDamText(float value)
    {
        Vector2 position = Vector2.up * damTextYOffect + UnityEngine.Random.insideUnitCircle;

        if (value < 0)
        {
            BattleObjectManager.Instance.SpawnDamageText(value * -1f, false, this.transform.position, DamTextType.Red);
        }
        else
        {
            BattleObjectManager.Instance.SpawnDamageText(value, false, this.transform.position, DamTextType.Green);
        }
    }
    private IEnumerator HitDelayRoutine()
    {
        canHit = false;
        yield return hitDelay;
        canHit = true;
    }
}
