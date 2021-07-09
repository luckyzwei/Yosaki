﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiSkillDescriptionPopup : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillName;

    [SerializeField]
    private TextMeshProUGUI skillDesc;

    [SerializeField]
    private TextMeshProUGUI skillAbility;

    private SkillTableData skillTableData;

    [SerializeField]
    private Button levelUpButton;

    [SerializeField]
    private Button awakeButton;

    [SerializeField]
    private TextMeshProUGUI awakeButtonDescription;

    [SerializeField]
    private WeaponView weaponView;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    [SerializeField]
    private TextMeshProUGUI awakeDescription;

    [SerializeField]
    private TextMeshProUGUI levelupButtonDescription;

    [SerializeField]
    private TextMeshProUGUI awakeText;

    private string lvTextFormat = "LV : {0}/{1}";

    private CompositeDisposable disposables = new CompositeDisposable();

    [SerializeField]
    private UiSkillSlotSettingBoard uiSkillSlotSettingBoard;

    [SerializeField]
    private Button equipButton;

    public void OnClickEquipButton()
    {
        int skillAwakeNum = DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].Value;

        if (skillAwakeNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("스킬을 배워야 등록할 수 있습니다.");
            return;
        }

        uiSkillSlotSettingBoard.gameObject.SetActive(true);
        uiSkillSlotSettingBoard.SetSkillIdx(skillTableData.Id);
    }

    public void Initialize(SkillTableData skillTableData)
    {
        this.gameObject.SetActive(true);

        this.skillTableData = skillTableData;

        skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(skillTableData.Id);

        skillName.SetText(skillTableData.Skillname);

        skillName.color = CommonUiContainer.Instance.itemGradeColor[skillTableData.Skillgrade];

        skillDesc.SetText(skillTableData.Skilldesc);

        UpdateSkillAwakeText();

        weaponView.Initialize(null, null, skillTableData);

        Subscribe();

        RefreshUpgradeButton();

        UpdateUi();
    }

    private void UpdateSkillAwakeText()
    {
        awakeText.SetText(DatabaseManager.skillServerTable.GetSkillMaxLevel(skillTableData.Id) == 0 ? "배우기" : "각성");
    }

    private void UpdateUi()
    {
        string desc = string.Empty;
        desc += $"데미지 : {(DatabaseManager.skillServerTable.GetSkillDamagePer(skillTableData.Id, applySkillDamAbility: false) * 100f).ToString("F1")}% -> {(DatabaseManager.skillServerTable.GetSkillDamagePer(skillTableData.Id, 1, applySkillDamAbility: false) * 100f).ToString("F1")}%\n";
        desc += $"쿨타임 : {skillTableData.Cooltime}\n";
        desc += $"타겟수 : {skillTableData.Targetcount}\n";
        desc += $"히트수 : {skillTableData.Hitcount}\n";
        desc += $"범위 : {skillTableData.Targetrange}\n";
        desc += $"마나 소모 : {skillTableData.Usecost}\n";
        desc += $"각성 최대 : {skillTableData.Awakemaxnum}\n";
        desc += $"스킬 타입 : {skillTableData.Skilltype}";
        skillAbility.SetText(desc);
    }

    private void Subscribe()
    {
        disposables.Clear();

        //스킬 각성시
        DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].AsObservable().Subscribe(WhenSkillAwake).AddTo(disposables);

        //스킬 레벨업시
        DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].AsObservable().Subscribe(WhenSkillUpgraded).AddTo(disposables);


        var weaponData = TableManager.Instance.WeaponData[skillTableData.Awakeweaponidx];

        // DatabaseManager.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenAwakeWeaponAmountChanged).AddTo(disposables);
        DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].AsObservable().Subscribe(WhenAwakeWeaponAmountChanged).AddTo(disposables);
    }

    private void WhenAwakeWeaponAmountChanged(int amount)
    {
        int skillAwakeNum = DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].Value;

        //최초에는 1개로 스킬 배울수있음.
        if (skillAwakeNum == 0)
        {
            awakeButton.interactable = amount >= GameBalance.firstSkillAwakeNum;
            awakeButtonDescription.SetText($"{amount}/{GameBalance.firstSkillAwakeNum}");
        }
        else
        {
            awakeButton.interactable = amount >= skillTableData.Awakeweaponreqcount;
            awakeButtonDescription.SetText($"{amount}/{skillTableData.Awakeweaponreqcount}");
        }
    }

    private void WhenSkillAwake(int awakeNum)
    {
        awakeDescription.SetText($"현재 각성 {awakeNum}회 스킬 최대레벨 {(awakeNum) * GameBalance.SkillAwakePlusNum}");

        var weaponData = TableManager.Instance.WeaponData[skillTableData.Awakeweaponidx];

        WhenAwakeWeaponAmountChanged(DatabaseManager.weaponTable.TableDatas[weaponData.Stringid].amount.Value);

        RefreshSkillLvText();

        equipButton.interactable = awakeNum != 0;
    }

    private void WhenSkillUpgraded(int skillLevel)
    {
        RefreshSkillLvText();
    }

    private void RefreshSkillLvText()
    {
        int skillLevel = DatabaseManager.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);
        int maxLevel = DatabaseManager.skillServerTable.GetSkillMaxLevel(skillTableData.Id);

        levelDescription.SetText(string.Format(lvTextFormat, skillLevel, maxLevel));
    }

    private void RefreshUpgradeButton()
    {
        int skillLevel = DatabaseManager.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);
        int maxLevel = DatabaseManager.skillServerTable.GetSkillMaxLevel(skillTableData.Id);
        var skillPoint = DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint);
        int awakeNum = DatabaseManager.skillServerTable.GetSkillAwakeNum(skillTableData.Id);

        levelUpButton.interactable = skillPoint.Value >= 0 && skillLevel < maxLevel;

        if (awakeNum == 0)
        {
            levelupButtonDescription.SetText("미습득");
        }
        else
        {
            if (levelUpButton.interactable)
            {
                levelupButtonDescription.SetText("레벨업");
            }
            else
            {
                levelupButtonDescription.SetText("최고레벨");
            }
        }
    }

    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);
    private void SyncServerRoutine()
    {
        if (SyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine);
        }

        SyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutine());
    }

    public void OnClickSkillUpgradeButton()
    {
        int maxLevel = DatabaseManager.skillServerTable.GetSkillMaxLevel(skillTableData.Id);
        int skillLevel = DatabaseManager.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);

        if (skillLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("스킬이 최대 레벨 입니다.");
            return;
        }

        var skillPoint = DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint);

#if UNITY_EDITOR
        SkillLevelUp(skillPoint);
        return;
#endif

        int currentSkillPoint = skillPoint.Value;
        if (currentSkillPoint <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("스킬포인트가 부족합니다.");
            return;
        }

        SkillLevelUp(skillPoint);
    }

    private void SkillLevelUp(ReactiveProperty<int> skillPoint)
    {
        //스킬포인트 감소
        skillPoint.Value--;

        //스킬 레벨 증가
        DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].Value++;

        //서버 업데이트 요청
        SyncServerRoutine();

        //Ui갱신
        Initialize(skillTableData);

     //   UiTutorialManager.Instance.SetClear(TutorialStep._5_SkillLevelUp);
    }

    private IEnumerator SyncDataRoutine()
    {

        yield return syncWaitTime;

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        Param skillParam = new Param();

        //스킬포인트
        statusParam.Add(StatusTable.SkillPoint, DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).Value);
        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        //스킬레벨
        skillParam.Add(SkillServerTable.SkillLevel, DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillLevel].Select(e => e.Value).ToList());
        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));

        DatabaseManager.SendTransaction(transactionList);

        SyncRoutine = null;
    }

    public void OnClickAwakeButton()
    {
       // UiTutorialManager.Instance.SetClear(TutorialStep._10_GetSkill);

        int currentAwakeNum = DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].Value;
        int maxAwakeNum = skillTableData.Awakemaxnum;

        if (currentAwakeNum >= maxAwakeNum)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 각성 입니다.");
            return;
        }

        //로컬 데이터 갱신


        //스킬북 차감 맨처음에는 1개만 차감
        if (currentAwakeNum == 0)
        {
            DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value -= 1;
        }
        else
        {
            DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value -= skillTableData.Awakeweaponreqcount;
        }

        //각성 +1
        DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].Value++;

        Initialize(skillTableData);

        //서버 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //스킬 각성 횟수 증가
        Param skillParam = new Param();
        List<int> skillAWakeData = new List<int>();
        var currentAwakeData = DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum];
        skillParam.Add(SkillServerTable.SkillAwakeNum, currentAwakeData.Select(e => e.Value).ToList());

        //스킬북 차감
        List<int> skillAmountSyncData = new List<int>();
        for (int i = 0; i < DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
        }
        skillParam.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);

        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));


        DatabaseManager.SendTransaction(transactionList);

        UpdateSkillAwakeText();

        //일일미션
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.SkillAwake, 1);
    }

    private void OnDisable()
    {
        disposables.Dispose();
    }
}
