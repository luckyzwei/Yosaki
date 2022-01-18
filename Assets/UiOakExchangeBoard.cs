using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiOakExchangeBoard : MonoBehaviour
{
    [SerializeField]
    private ObscuredInt requireOakAmount;

    [SerializeField]
    private ObscuredInt ticketGetAmount;

    [SerializeField]
    private TextMeshProUGUI description;

    private YomulAbilData yomulAbilData;

    private YomulServerData yomulServerData;

    [SerializeField]
    private GameObject exchangeButton;

    [SerializeField]
    private GameObject unlockButton;

    [SerializeField]
    private TextMeshProUGUI oakAmountText;

    [SerializeField]
    private TextMeshProUGUI ticketAmountText;

    void Start()
    {
        Initialize();
        Subscribe();
    }
    private void Subscribe()
    {
        yomulServerData.hasAbil.AsObservable().Subscribe(e =>
        {
            exchangeButton.SetActive(e == 1);
            unlockButton.SetActive(e == 0);
        }).AddTo(this);
    }

    private void Initialize()
    {
        this.yomulAbilData = TableManager.Instance.YomulAbilTable.dataArray[1];

        yomulServerData = ServerData.yomulServerTable.TableDatas[this.yomulAbilData.Stringid];

        oakAmountText.SetText($"{Utils.ConvertBigNum(requireOakAmount)}");
        ticketAmountText.SetText($"{Utils.ConvertBigNum(ticketGetAmount)}");

        description.SetText($"{CommonString.GetItemName(Item_Type.Jade)} {Utils.ConvertBigNum(requireOakAmount)}개를 바치면 소환서를 주지.");
    }
    public void OnClickUnlockButton()
    {
        if (yomulServerData.hasAbil.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 계약 됐습니다.");
            return;
        }

        //조건
        if (ServerData.goodsTable.GetTableData(GoodsTable.YomulExchangeStone).Value == 0f)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.YomulExchangeStone)}이 필요합니다.\n(12지신(축)최종 보상에서 획득 가능.)");
            return;
        }
        //
        //로컬
        yomulServerData.hasAbil.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param yomulParam = new Param();
        yomulParam.Add(yomulAbilData.Stringid, yomulServerData.ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(YomulServerTable.tableName, YomulServerTable.Indate, yomulParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
          //  LogManager.Instance.SendLogType("Yomul", "해제", yomulAbilData.Id.ToString());
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "계약 완료!", null);
        });

    }
    public void OnClickExchangeButton()
    {
        if (yomulServerData.hasAbil.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("먼저 계약을 해야 합니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value < requireOakAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= requireOakAmount;
        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += ticketGetAmount;

        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Ticket)} 획득!");

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
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
