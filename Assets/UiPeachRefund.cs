using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPeachRefund : MonoBehaviour
{
    void Start()
    {
        Check();
    }

    private void Check()
    {
        //  if (ServerData.userInfoTable.GetTableData(UserInfoTable.peachRefund).Value == 1) return;

        double score = ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin;

        var tableDatas = TableManager.Instance.SonReward.dataArray;

        float accumPeach = 0;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (i < 15) continue;

            if (score > tableDatas[i].Score)
            {
                accumPeach += tableDatas[i].Rewardvalue * 2f;
            }
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += accumPeach;
       // ServerData.userInfoTable.GetTableData(UserInfoTable.peachRefund).Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();

        if (accumPeach != 0)
        {
            goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
        }

        Param userInfoParam = new Param();
     //   userInfoParam.Add(UserInfoTable.peachRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.peachRefund).Value);

        if (accumPeach != 0)
        {
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              if (accumPeach != 0)
              {
                  PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"복숭아 {accumPeach}개 소급됨", null);
              }
          });
    }


}
