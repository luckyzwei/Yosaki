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

    [SerializeField]
    private TextMeshProUGUI yoguiFireAmount;

    //[SerializeField]
    //private TextMeshProUGUI totalTicket;

    private void Start()
    {
        Check();
    }

    private void Check()
    {
        rootObject.SetActive(false);

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.relicPackReset).Value == 1) return;

        int relic1 = ServerData.iAPServerTableTotal.TableDatas["relic1"].buyCount.Value;
        int relic2 = ServerData.iAPServerTableTotal.TableDatas["relic2"].buyCount.Value;
        int relic3 = ServerData.iAPServerTableTotal.TableDatas["relic3"].buyCount.Value;
        int relic4 = ServerData.iAPServerTableTotal.TableDatas["relic4"].buyCount.Value;

        if (relic1 == 0 && relic2 == 0 && relic3 == 0 && relic4 == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.relicPackReset).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.relicPackReset, ServerData.userInfoTable.GetTableData(UserInfoTable.relicPackReset).Value);
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

        buyCounts[0].SetText(relic1.ToString() + "회");
        buyCounts[1].SetText(relic2.ToString() + "회");
        buyCounts[2].SetText(relic3.ToString() + "회");
        buyCounts[3].SetText(relic4.ToString() + "회");

        //4
        int _1DiffTicket = 18;
        //13
        int _2DiffTicket = 64;
        //40
        int _3DiffTicket = 128;
        int _4DiffTicket = 340;

        int marble1_MarbleAdd = relic1 * _1DiffTicket;
        plusCount[0].SetText(Utils.ConvertBigNum(marble1_MarbleAdd));


        int marble2_MarbleAdd = relic2 * _2DiffTicket;
        plusCount[1].SetText(Utils.ConvertBigNum(marble2_MarbleAdd));


        int marble3_MarbleAdd = relic3 * _3DiffTicket;
        plusCount[2].SetText(Utils.ConvertBigNum(marble3_MarbleAdd));


        int marble4_MarbleAdd = relic4 * _4DiffTicket;
        plusCount[3].SetText(Utils.ConvertBigNum(marble4_MarbleAdd));

        yoguiFireAmount.gameObject.SetActive(relic4 != 0);

        if (relic4 != 0)
        {
            yoguiFireAmount.SetText($"40000");
        }

        int addTicketTotal = marble1_MarbleAdd + marble2_MarbleAdd + marble3_MarbleAdd + marble4_MarbleAdd;

        totalMarble.SetText($"총 {Utils.ConvertBigNum(addTicketTotal)}");

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += addTicketTotal;

        if (relic4 != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += 40000;
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.relicPackReset].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

        if (relic4 != 0)
        {
            goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
        }

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.relicPackReset, ServerData.userInfoTable.TableDatas[UserInfoTable.relicPackReset].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            //  LogManager.Instance.SendLogType("TicketRefund", "Get", $"t:{addTicketTotal} t:{0}");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "세트 소급 완료!", null);
        });
    }
}
