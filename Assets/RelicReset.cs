using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicReset : MonoBehaviour
{
    void Start()
    {
        ResetAll();
    }

    private void ResetAll()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.relicReset].Value == 1) return;

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.hellRelicKillCount).Value < 4000)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset).Value = 1;

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.relicReset, ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset).Value);
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
            });
        }
        else
        {
            var tableDatas = TableManager.Instance.RelicTable.dataArray;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param relicParam = new Param();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].level.Value = 0;

                relicParam.Add(tableDatas[i].Stringid, ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
            }

            transactions.Add(TransactionValue.SetUpdate(RelicServerTable.tableName, RelicServerTable.Indate, relicParam));

            ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value = 0;

            int usedTicketNum = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value;

            ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += usedTicketNum;

            ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset).Value = 1;

            ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.hellRelicKillCount).Value = 0;

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.usedRelicTicketNum, ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value);
            userInfoParam.Add(UserInfoTable.relicReset, ServerData.userInfoTable.GetTableData(UserInfoTable.relicReset).Value);
            userInfoParam.Add(UserInfoTable.relicKillCount, ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value);
            userInfoParam.Add(UserInfoTable.hellRelicKillCount, ServerData.userInfoTable.GetTableData(UserInfoTable.hellRelicKillCount).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup("알림", $"영혼의숲,<color=red>지옥영혼의숲이</color> 초기화 됐습니다..\n영혼의숲 능력치를 다시 찍어주세요!", null);
            });
        }




    }
}
