using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTitleRefund : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Refund();
    }

    private void Refund()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.titleRefund].Value == 1) return;

        var tableDatas = TableManager.Instance.TitleTable.dataArray;

        float rewardValue = 0;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype != (int)Item_Type.Jade) continue;
            if (tableDatas[i].Rewardvalue <= int.MaxValue) continue;

            //보상 둘다받은경우만
            if (ServerData.titleServerTable.TableDatas[tableDatas[i].Stringid].rewarded.Value == 1)
            {
                rewardValue += (tableDatas[i].Rewardvalue - (float)int.MaxValue);
            }
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.userInfoTable.TableDatas[UserInfoTable.titleRefund].Value = 1;
        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.titleRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.titleRefund].Value);

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardValue;
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"옥 {Utils.ConvertBigNum(rewardValue)}개 소급됨", null);
        });
    }

}
