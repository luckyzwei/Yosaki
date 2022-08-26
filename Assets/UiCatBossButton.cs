using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiCatBossButton : MonoBehaviour
{
    [SerializeField]
    private ObscuredInt requireOakAmount;

    [SerializeField]
    private ObscuredInt ticketGetAmount;

    private YomulAbilData yomulAbilData;
    private YomulServerData yomulServerData;

    void Start()
    {
        this.yomulAbilData = TableManager.Instance.YomulAbilTable.dataArray[1];

        yomulServerData = ServerData.yomulServerTable.TableDatas[this.yomulAbilData.Stringid];
    }

    public void OnClickExchangeAllButton() 
    {
        if (yomulServerData.hasAbil.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("먼저 계약을 해야 합니다.(요물무기->제물)");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "모든 옥을 소환서로 교환 할까요?", () =>
        {
            float oakAmount = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

            if (oakAmount < requireOakAmount)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
                return;
            }

            int exchangeNum = (int)(oakAmount / (float)requireOakAmount);

            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= requireOakAmount * (float)exchangeNum;
            ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += ticketGetAmount * (float)exchangeNum;

            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Ticket)} 획득!");

            if (syncRoutine != null)
            {
                CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
            }

            syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
        }, null);
    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(1.0f);
    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }


}
