using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiDokebiPackRefund : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> buyCounts;

    [SerializeField]
    private List<TextMeshProUGUI> ticketPlusCount;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI totalTicket;

    private void Start()
    {
        Check();
    }

    private void Check()
    {
        rootObject.SetActive(false);

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiPackRefund).Value == 1) return;

        int dokevi_1_BuyCount = ServerData.iAPServerTableTotal.TableDatas["bigoak1"].buyCount.Value;
        int dokevi_2_BuyCount = ServerData.iAPServerTableTotal.TableDatas["bigoak2"].buyCount.Value;

        if (dokevi_1_BuyCount == 0 && dokevi_2_BuyCount == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiPackRefund).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param dokebiParam = new Param();

            dokebiParam.Add(UserInfoTable.dokebiPackRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiPackRefund).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, dokebiParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        rootObject.SetActive(true);

        buyCounts[0].SetText(dokevi_1_BuyCount.ToString() + "회");
        buyCounts[1].SetText(dokevi_2_BuyCount.ToString() + "회");

        int _1DiffTicket = 10;

        int _2DiffTicket = 10;

        int marble1_TicketAdd = dokevi_1_BuyCount * _1DiffTicket;
        ticketPlusCount[0].SetText(marble1_TicketAdd.ToString());

        int marble2_TicketAdd = dokevi_2_BuyCount * _2DiffTicket;
        ticketPlusCount[1].SetText(marble2_TicketAdd.ToString());

        int addTicketTotal = marble1_TicketAdd + marble2_TicketAdd;

        totalTicket.SetText($"총 {Utils.ConvertBigNum(addTicketTotal)}");

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += addTicketTotal;

        ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiPackRefund].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dokebiPackRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiPackRefund].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
          //  LogManager.Instance.SendLogType("TicketRefund", "Get", $"t:{addTicketTotal}");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 세트 소급 완료!", null);
        });
    }
}
