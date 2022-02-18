using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiRelicRefund : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> buyCounts;

    [SerializeField]
    private List<TextMeshProUGUI> marblePlusCount;

    //[SerializeField]
    //private List<TextMeshProUGUI> ticketPlusCount;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI totalMarble;

    [SerializeField]
    private TextMeshProUGUI removeAd;

    //[SerializeField]
    //private TextMeshProUGUI totalTicket;

    private void Start()
    {
        Check();
    }

    private void Check()
    {
        rootObject.SetActive(false);

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset2).Value == 1) return;

        int marblePack1Count = ServerData.iAPServerTableTotal.TableDatas["relic1"].buyCount.Value;
        int marblePack2Count = ServerData.iAPServerTableTotal.TableDatas["relic2"].buyCount.Value;
        int marblePack3Count = ServerData.iAPServerTableTotal.TableDatas["relic3"].buyCount.Value;
        int marblePack4Count = ServerData.iAPServerTableTotal.TableDatas["relic4"].buyCount.Value;

        if (marblePack1Count == 0 && marblePack2Count == 0 && marblePack3Count == 0 && marblePack4Count == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset2).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.relicReset2, ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset2).Value);
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

        buyCounts[0].SetText(marblePack1Count.ToString() + "회");
        buyCounts[1].SetText(marblePack2Count.ToString() + "회");
        buyCounts[2].SetText(marblePack3Count.ToString() + "회");
        buyCounts[3].SetText(marblePack4Count.ToString() + "회");

        int _1DiffMarble = 6;
        //int _1DiffTicket = 2;

        int _2DiffMarble = 16;
        // int _2DiffTicket = 5;

        int _3DiffMarble = 27;

        int _4DiffMarble = 40;
        //int _3DiffTicket = 10;

        int marble1_MarbleAdd = marblePack1Count * _1DiffMarble;
        marblePlusCount[0].SetText(Utils.ConvertBigNum(marble1_MarbleAdd));

        //int marble1_TicketAdd = marblePack1Count * _1DiffTicket;
        //ticketPlusCount[0].SetText(marble1_TicketAdd.ToString());

        int marble2_MarbleAdd = marblePack2Count * _2DiffMarble;
        marblePlusCount[1].SetText(Utils.ConvertBigNum(marble2_MarbleAdd));

        //int marble2_TicketAdd = marblePack2Count * _2DiffTicket;
        // ticketPlusCount[1].SetText(marble2_TicketAdd.ToString());

        int marble3_MarbleAdd = marblePack3Count * _3DiffMarble;
        marblePlusCount[2].SetText(Utils.ConvertBigNum(marble3_MarbleAdd));

        int marble4_MarbleAdd = marblePack4Count * _4DiffMarble;
        marblePlusCount[3].SetText(Utils.ConvertBigNum(marble4_MarbleAdd));

        //int marble3_TicketAdd = marblePack3Count * _3DiffTicket;
        //ticketPlusCount[2].SetText(marble3_TicketAdd.ToString());


        int addMarbleTotal = marble1_MarbleAdd + marble2_MarbleAdd + marble3_MarbleAdd + marble4_MarbleAdd;
        //int addTicketTotal = marble1_TicketAdd + marble2_TicketAdd + marble3_TicketAdd;

        totalMarble.SetText($"총 {Utils.ConvertBigNum(addMarbleTotal)}");
        //totalTicket.SetText($"총 {Utils.ConvertBigNum(addTicketTotal)}");

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += addMarbleTotal;
        //ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += addTicketTotal;

        ServerData.userInfoTable.TableDatas[UserInfoTable.relicReset2].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
        //goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.relicReset2, ServerData.userInfoTable.TableDatas[UserInfoTable.relicReset2].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            //       LogManager.Instance.SendLogType("Marble3", "Get", $"m:{addMarbleTotal} t:{0}");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "영혼열쇠 세트 소급 완료!", null);
        });
    }
}
