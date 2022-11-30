using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;

public class UiTicketBuyShop : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI remainBuyCount;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI adText;


    private Coroutine syncToServerRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(1.0f);

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Initialize()
    {
        priceText.SetText($"{GameBalance.ticketPrice}");
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).AsObservable().Subscribe(WhenTicketBuyCountChanged).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).AsObservable().Subscribe(WhenAdRewardReceived).AddTo(this);
    }

    private void WhenAdRewardReceived(double received)
    {
        bool canReceive = received == 0f;

        if (canReceive)
        {
            adText.SetText("무료 획득!");
        }
        else
        {
            adText.SetText("오늘 획득함");
        }

    }

    private void WhenTicketBuyCountChanged(double buyCount)
    {
        remainBuyCount.SetText($"오늘 구매({(int)buyCount}/{GameBalance.dailyTickBuyCountMax})");
    }

    public void OnClickBuyButton()
    {
        int currentBuyCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value;

        if (currentBuyCount >= GameBalance.dailyTickBuyCountMax)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 구매할 수 없습니다.");
            return;
        }

        float currentBlueStoneNum = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

        if (currentBlueStoneNum < GameBalance.ticketPrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        UiTutorialManager.Instance.SetClear(TutorialStep.BuyTicket);

        BuyProcess();
    }

    public void OnClickAdButton()
    {
        bool received = ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value == 1;
        if (received)
        {
            PopupManager.Instance.ShowAlarmMessage("내일 다시 획득 가능합니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(RewardAdFinished);
    }

    private void RewardAdFinished()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value == 1f) return;

        UiTutorialManager.Instance.SetClear(TutorialStep.BuyTicket);

        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value++;
        ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value = 1f;

        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.receivedTicketReward, ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList);
    }

    private void BuyProcess()
    {
        UiTutorialManager.Instance.SetClear(TutorialStep.BuyTicket);

        //로컬 갱신
        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value++;
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.ticketPrice;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value++;

        if (syncToServerRoutine != null)
        {
            StopCoroutine(syncToServerRoutine);
        }

        syncToServerRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncToServer());
    }

    private IEnumerator SyncToServer()
    {
        yield return syncDelay;

        syncToServerRoutine = null;

        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dailyTicketBuyCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList);
    }
}
