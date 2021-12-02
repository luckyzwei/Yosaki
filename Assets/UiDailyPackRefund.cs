using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UiDailyPackRefund : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> buyCounts;

    [SerializeField]
    private List<TextMeshProUGUI> plusCount;

    //[SerializeField]
    //private List<TextMeshProUGUI> ticketPlusCount;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI totalMarble;

    //[SerializeField]
    //private TextMeshProUGUI totalTicket;

    private void Start()
    {
        Check();
    }

    private void Check()
    {
        rootObject.SetActive(false);

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dailyPackReset).Value == 1) return;

        int dailyPackCount = ServerData.iAPServerTableTotal.TableDatas["dailypackage1"].buyCount.Value;
        int weeklyPackCount = ServerData.iAPServerTableTotal.TableDatas["weeklypackage1"].buyCount.Value;
        int monthlyPackCount = ServerData.iAPServerTableTotal.TableDatas["monthlypackage1"].buyCount.Value;

        int bigOak1 = ServerData.iAPServerTableTotal.TableDatas["bigoak1"].buyCount.Value;
        int bigOak2 = ServerData.iAPServerTableTotal.TableDatas["bigoak2"].buyCount.Value;

        if (dailyPackCount == 0 && weeklyPackCount == 0 && monthlyPackCount == 0 && bigOak1 == 0 && bigOak2 == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.dailyPackReset).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.dailyPackReset, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyPackReset).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, marbleParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        rootObject.SetActive(true);

        buyCounts[0].SetText(dailyPackCount.ToString() + "회");
        buyCounts[1].SetText(weeklyPackCount.ToString() + "회");
        buyCounts[2].SetText(monthlyPackCount.ToString() + "회");
        buyCounts[3].SetText(bigOak1.ToString() + "회");
        buyCounts[4].SetText(bigOak2.ToString() + "회");

        //4
        int _1DiffTicket = 16;
        //13
        int _2DiffTicket = 52;
        //40
        int _3DiffTicket = 160;

        int _4DiffTicket = 80;

        int _5DiffTicket = 180;

        int marble1_MarbleAdd = dailyPackCount * _1DiffTicket;
        plusCount[0].SetText(Utils.ConvertBigNum(marble1_MarbleAdd));

        //int marble1_TicketAdd = marblePack1Count * _1DiffTicket;
        //ticketPlusCount[0].SetText(marble1_TicketAdd.ToString());

        int marble2_MarbleAdd = weeklyPackCount * _2DiffTicket;
        plusCount[1].SetText(Utils.ConvertBigNum(marble2_MarbleAdd));

        //int marble2_TicketAdd = marblePack2Count * _2DiffTicket;
        // ticketPlusCount[1].SetText(marble2_TicketAdd.ToString());

        int marble3_MarbleAdd = monthlyPackCount * _3DiffTicket;
        plusCount[2].SetText(Utils.ConvertBigNum(marble3_MarbleAdd));

        int big_oak_1_Add = bigOak1 * _4DiffTicket;
        plusCount[3].SetText(Utils.ConvertBigNum(big_oak_1_Add));

        int big_oak_2_Add = bigOak2 * _5DiffTicket;
        plusCount[4].SetText(Utils.ConvertBigNum(big_oak_2_Add));

        //int marble3_TicketAdd = marblePack3Count * _3DiffTicket;
        //ticketPlusCount[2].SetText(marble3_TicketAdd.ToString());

        int addTicketTotal = marble1_MarbleAdd + marble2_MarbleAdd + marble3_MarbleAdd + big_oak_1_Add + big_oak_2_Add;
        //int addTicketTotal = marble1_TicketAdd + marble2_TicketAdd + marble3_TicketAdd;

        totalMarble.SetText($"총 {Utils.ConvertBigNum(addTicketTotal)}");
        //totalTicket.SetText($"총 {Utils.ConvertBigNum(addTicketTotal)}");

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += addTicketTotal;
        //ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += addTicketTotal;

        ServerData.userInfoTable.TableDatas[UserInfoTable.dailyPackReset].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        //goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dailyPackReset, ServerData.userInfoTable.TableDatas[UserInfoTable.dailyPackReset].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            LogManager.Instance.SendLogType("TicketRefund", "Get", $"t:{addTicketTotal} t:{0}");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "세트 소급 완료!", null);
        });
    }
}
