using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using BackEnd;
using Spine.Unity;

public class UiCollectionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI gaugeDescription;
    [SerializeField]
    private TextMeshProUGUI levelText;

    private EnemyTableData tableData;

    [SerializeField]
    private GameObject upgradeButton;
    [SerializeField]
    private TextMeshProUGUI updateButtonText;


    [SerializeField]
    private GameObject upgradeGemButton;
    [SerializeField]
    private TextMeshProUGUI updateGemButtonText;

    [SerializeField]
    private TextMeshProUGUI abilityText;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    private void SetCollectionSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.enemySpineAssets[idx];
      //  skeletonGraphic.startingAnimation = "Walk";
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

    public void Initialize(EnemyTableData tableData,int materialIdx)
    {
        this.tableData = tableData;

        title.SetText($"{tableData.Name}");

        Subscribe();

        SetCollectionSpine(materialIdx);
    }

    private void Subscribe()
    {
        DatabaseManager.collectionTable.TableDatas[tableData.Collectionkey].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(this);
        DatabaseManager.collectionTable.TableDatas[tableData.Collectionkey].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(this);
    }

    private void WhenAmountChanged(int amount)
    {
        gaugeDescription.SetText($"{amount}/{tableData.Collectionneedamount}");
    }
    private void WhenLevelChanged(int level)
    {
        bool isMaxLevel = level >= tableData.Collectionmaxlevel;

        upgradeButton.SetActive(isMaxLevel == false);
        upgradeGemButton.SetActive(isMaxLevel == false);

        if (tableData.Collectionmaxlevel == level)
        {
            levelText.SetText($"최고레벨");
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.white;
            levelText.SetText($"Lv:{level}");
            updateButtonText.SetText("흡수");
            updateGemButtonText.SetText($"{tableData.Stoneprice}");
        }

        abilityText.SetText($"{CommonString.GetStatusName((StatusType)tableData.Collectionabiltype)} + {DatabaseManager.collectionTable.GetCollectionAbilValue(tableData)}");
    }

    public void OnClickCollectButton()
    {
        if (DatabaseManager.collectionTable.TableDatas[tableData.Collectionkey].amount.Value < tableData.Collectionneedamount)
        {
            PopupManager.Instance.ShowAlarmMessage("영혼이 부족 합니다.");
            return;
        }

        if (DatabaseManager.collectionTable.TableDatas[tableData.Collectionkey].level.Value >= tableData.Collectionmaxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 최고레벨 입니다.");
            return;
        }

        var collectionData = CollectionManager.Instance.GetCollectionData(tableData.Collectionkey, true);
        collectionData.amount.Value -= tableData.Collectionneedamount;
        collectionData.level.Value++;

        CollectionManager.Instance.SyncToServer();

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.Collection, 1);
    }

    public void OnClickCollectionByGemButton()
    {
        if (DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value < tableData.Stoneprice)
        {
            PopupManager.Instance.ShowAlarmMessage("재료가 부족합니다.");
            return;
        }

        if (DatabaseManager.collectionTable.TableDatas[tableData.Collectionkey].level.Value >= tableData.Collectionmaxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 최고레벨 입니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup("알림", $"보석 {tableData.Stoneprice}개를 사용해 영혼을 흡수 합니까?", CollectByGem, null);
    }

    private void CollectByGem()
    {
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value -= tableData.Stoneprice;

        CollectionManager.Instance.GetCollectionData(tableData.Collectionkey, true).level.Value++;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value);

        //컬렉션 레벨
        Param collectionParam = new Param();
        var collectionServerData = DatabaseManager.collectionTable.TableDatas[tableData.Collectionkey];

        collectionParam.Add(tableData.Collectionkey, $"{collectionServerData.idx},{collectionServerData.level.Value},{collectionServerData.amount.Value}");

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(CollectionTable.tableName, CollectionTable.Indate, collectionParam));

        DatabaseManager.SendTransaction(transactionList);

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.Collection, 1);
    }
}
