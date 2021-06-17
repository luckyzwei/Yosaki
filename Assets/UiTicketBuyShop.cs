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
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).AsObservable().Subscribe(WhenTicketBuyCountChanged).AddTo(this);
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).AsObservable().Subscribe(WhenAdRewardReceived).AddTo(this);
    }

    private void WhenAdRewardReceived(float received)
    {
        bool canReceive = received == 0f;

        if (canReceive)
        {
            adText.SetText("무료 획득!");
        }
        else
        {
            adText.SetText("내일 다시!");
        }
    }

    private void WhenTicketBuyCountChanged(float buyCount)
    {
        remainBuyCount.SetText($"오늘 구매({(int)buyCount}/{GameBalance.dailyTickBuyCountMax})");
    }

    public void OnClickBuyButton()
    {
        int currentBuyCount = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value;

#if !UNITY_EDITOR
        if (currentBuyCount >= GameBalance.dailyTickBuyCountMax)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 구매할 수 없습니다.");
            return;
        }

                int currentBlueStoneNum = (int)DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value;

        if (currentBlueStoneNum < GameBalance.ticketPrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BlueStone)}이 부족합니다.");
            return;
        }

#endif



        BuyProcess();

    }

    public void OnClickAdButton()
    {
        bool received = DatabaseManager.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value == 1;
        if (received) 
        {
            PopupManager.Instance.ShowAlarmMessage("내일 다시 획득 가능합니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(RewardAdFinished);
    }

    private void RewardAdFinished()
    {
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value++;
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value = 1f;

        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Ticket, DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.receivedTicketReward, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        DatabaseManager.SendTransaction(transactionList);
    }

    private void BuyProcess()
    {
        //로컬 갱신
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value++;
        DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value -= GameBalance.ticketPrice;
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value++;

        if (syncToServerRoutine != null)
        {
            StopCoroutine(syncToServerRoutine);
        }

        syncToServerRoutine =CoroutineExecuter.Instance.StartCoroutine(SyncToServer());
    }

    private IEnumerator SyncToServer()
    {
        yield return syncDelay;

        syncToServerRoutine = null;

        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BlueStone, DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value);
        goodsParam.Add(GoodsTable.Ticket, DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dailyTicketBuyCount, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        DatabaseManager.SendTransaction(transactionList);
    }
}
