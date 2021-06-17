using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiPassiveSkillCell : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillName;

    [SerializeField]
    private TextMeshProUGUI skillDesc;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private PassiveSkillData passiveSkillData;
    private MagicBookData magicBookData;
    private MagicBookServerData magicBookServerData;

    private string lvTextFormat = "LV : {0}/{1}";

    private bool isSubscribed = false;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private WeaponView weaponView;

    private Coroutine syncRoutine;

    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private Sprite normalSprite;

    [SerializeField]
    private Sprite maxSprite;

    [SerializeField]
    private TextMeshProUGUI buttonDesc;


    public void Refresh(PassiveSkillData passiveSkillData)
    {
        this.passiveSkillData = passiveSkillData;

        skillIcon.sprite = CommonResourceContainer.GetPassiveSkillIconSprite(passiveSkillData);

        skillName.SetText(passiveSkillData.Skillname);

        int currentSkillLevel = DatabaseManager.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        var statusType = (StatusType)passiveSkillData.Abilitytype;

        if (statusType.IsPercentStat())
        {
            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {currentSkillLevel * passiveSkillData.Abilityvalue*100f}");
        }
        else
        {
            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {currentSkillLevel * passiveSkillData.Abilityvalue}");
        }

        levelDescription.SetText($"LV:{currentSkillLevel}/{passiveSkillData.Maxlevel}");

        if (currentSkillLevel >= passiveSkillData.Maxlevel)
        {
            buttonImage.sprite = maxSprite;
            buttonDesc.SetText("최고레벨");
        }
        else
        {
            buttonImage.sprite = normalSprite;
            buttonDesc.SetText("레벨업");
        }

        magicBookData = TableManager.Instance.MagicBoocDatas[passiveSkillData.Requiremagicbookidx];

        magicBookServerData = DatabaseManager.magicBookTable.TableDatas[magicBookData.Stringid];

        if (magicBookServerData.hasItem.Value == 0)
        {
            weaponView.Initialize(null, magicBookData);
            lockMask.SetActive(true);
        }
        else
        {
            lockMask.SetActive(false);
        }

        if (isSubscribed == false)
        {
            isSubscribed = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        magicBookServerData.hasItem.AsObservable().Subscribe(e =>
        {

            Refresh(this.passiveSkillData);

        }).AddTo(this);

        DatabaseManager.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.AsObservable().Subscribe(e =>
        {
            Refresh(this.passiveSkillData);
        }).AddTo(this);
    }

    public void OnClickUpgradeButton()
    {
        int currentLevel = DatabaseManager.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        if (currentLevel >= passiveSkillData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        if (magicBookServerData.hasItem.Value != 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마도서가 없습니다.");
            return;
        }

        //스킬포인트 체크
        var skillPoint = DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("스킬포인트가 부족합니다.");
            return;
        }
        //로컬
        DatabaseManager.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value++;
        skillPoint.Value--;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private IEnumerator SyncRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        var skillPoint = DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param passiveParam = new Param();
        passiveParam.Add(passiveSkillData.Stringid, DatabaseManager.passiveServerTable.TableDatas[passiveSkillData.Stringid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(PassiveServerTable.tableName, PassiveServerTable.Indate, passiveParam));

        Param skillPointParam = new Param();
        skillPointParam.Add(StatusTable.SkillPoint, skillPoint.Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, skillPointParam));

        DatabaseManager.SendTransaction(transactions);
    }
}
