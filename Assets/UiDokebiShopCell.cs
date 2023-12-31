﻿using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDokebiShopCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemTitle;

    [SerializeField]
    private TextMeshProUGUI itemAmount;

    [SerializeField]
    private TextMeshProUGUI itemPrice;

    private DokebiRewardTableData tableData;

    public void Initialize(DokebiRewardTableData tableData)
    {
        this.tableData = tableData;
        UpdateUi();
    }

    private void UpdateUi()
    {
        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemTitle.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));

        itemAmount.SetText($"{Utils.ConvertBigNum(tableData.Rewardamount)}개");

        itemPrice.SetText(Utils.ConvertBigNum(tableData.Price));
    }

    private Coroutine exChangeRoutine;

    public void OnClickExChangeButton()
    {
        var currentDokebiHornNum = ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey);

        if (currentDokebiHornNum.Value < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Dokebi)}이 부족합니다.");
            return;
        }

        SoundManager.Instance.PlaySound("Reward");

        currentDokebiHornNum.Value -= tableData.Price;

        string goodsKey = ((Item_Type)tableData.Itemtype).ToString();

        ServerData.goodsTable.GetTableData(goodsKey).Value += tableData.Rewardamount;

        PopupManager.Instance.ShowAlarmMessage("교환 성공");

        if (exChangeRoutine != null) 
        {
            CoroutineExecuter.Instance.StopCoroutine(exChangeRoutine);
        }

        exChangeRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private WaitForSeconds delay = new WaitForSeconds(0.5f);
    private IEnumerator SyncRoutine() 
    {
        yield return delay;

        string goodsKey = ((Item_Type)tableData.Itemtype).ToString();

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.DokebiKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value);

        goodsParam.Add(goodsKey, ServerData.goodsTable.GetTableData(goodsKey).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
     

        //    LogManager.Instance.SendLog("도깨비", $"{goodsKey} {tableData.Rewardamount}");
        });
    }
}
