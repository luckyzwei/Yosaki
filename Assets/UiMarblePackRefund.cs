using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMarblePackRefund : MonoBehaviour
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

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.marbleReset3).Value == 1) return;

        int marblePack1Count = ServerData.iAPServerTableTotal.TableDatas["marblepackage1"].buyCount.Value;
        int marblePack2Count = ServerData.iAPServerTableTotal.TableDatas["marblepackage2"].buyCount.Value;
        int marblePack3Count = ServerData.iAPServerTableTotal.TableDatas["marblepackage3"].buyCount.Value;

#if UNITY_ANDROID
        int removeAdCount = ServerData.iAPServerTableTotal.TableDatas["removead"].buyCount.Value;
#else 
        int removeAdCount = ServerData.iAPServerTableTotal.TableDatas["removeadios"].buyCount.Value;
#endif

        if (removeAdCount >= 1)
        {
            removeAdCount = 1;
        }

        if (marblePack1Count == 0 && marblePack2Count == 0 && marblePack3Count == 0 && removeAdCount == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.marbleReset3).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.marbleReset3, ServerData.userInfoTable.GetTableData(UserInfoTable.marbleReset3).Value);
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

        removeAd.SetText(removeAdCount.ToString() + "회(1000만개)");

        int _1DiffMarble = 4000000;
        //int _1DiffTicket = 2;

        int _2DiffMarble = 8000000;
        // int _2DiffTicket = 5;

        int _3DiffMarble = 18000000;
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

        //int marble3_TicketAdd = marblePack3Count * _3DiffTicket;
        //ticketPlusCount[2].SetText(marble3_TicketAdd.ToString());

        int removeAdMarble = removeAdCount * 10000000;

        int addMarbleTotal = marble1_MarbleAdd + marble2_MarbleAdd + marble3_MarbleAdd + removeAdMarble;
        //int addTicketTotal = marble1_TicketAdd + marble2_TicketAdd + marble3_TicketAdd;

        totalMarble.SetText($"총 {Utils.ConvertBigNum(addMarbleTotal)}");
        //totalTicket.SetText($"총 {Utils.ConvertBigNum(addTicketTotal)}");

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += addMarbleTotal;
        //ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += addTicketTotal;

        ServerData.userInfoTable.TableDatas[UserInfoTable.marbleReset3].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        //goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.marbleReset3, ServerData.userInfoTable.TableDatas[UserInfoTable.marbleReset3].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              //       LogManager.Instance.SendLogType("Marble3", "Get", $"m:{addMarbleTotal} t:{0}");
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우구슬 세트,광고제거 소급 완료!", null);
          });
    }
}
