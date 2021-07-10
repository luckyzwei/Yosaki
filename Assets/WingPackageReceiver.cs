using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPackageReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ReceivePackageReward();
    }

    private void ReceivePackageReward()
    {
        int receivedReward = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingPackageRewardReceive).Value;


        //보상 안받음
        if (receivedReward == 0)
        {
            int packageBuyCount = DatabaseManager.iapServerTable.TableDatas["wingpackage1"].buyCount.Value;

            if (packageBuyCount == 0) return;

            int getTicketCount = packageBuyCount * 10;
            int getFeatherCount = packageBuyCount * 50000;

            List<TransactionValue> transactions = new List<TransactionValue>();

            DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value += getTicketCount;
            DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += getFeatherCount;

            DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingPackageRewardReceive).Value = 1;

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Ticket, DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value);
            goodsParam.Add(GoodsTable.MarbleKey, DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.wingPackageRewardReceive, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingPackageRewardReceive).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            DatabaseManager.SendTransaction(transactions, successCallBack: () =>
              {
                  PopupManager.Instance.ShowConfirmPopup("보석 패키지 소급 보상", $"티켓 {getTicketCount}개\n깃털 {getFeatherCount}개", null);

                  LogManager.Instance.SendLog("날개패키지 소급", $"티켓{getTicketCount} 깃털{getFeatherCount} ");
              });
        }
    }


}
