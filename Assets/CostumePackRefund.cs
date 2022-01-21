using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumePackRefund : MonoBehaviour
{
    void Start()
    {
        Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.basicPackRefund).Value == 1) return;

        bool buyBeginner = ServerData.iapServerTable.TableDatas["newbiepack"].buyCount.Value == 1;
        bool buyMiddle = ServerData.iapServerTable.TableDatas["middlepack"].buyCount.Value == 1;
        bool buyHigh = ServerData.iapServerTable.TableDatas["highpack"].buyCount.Value == 1;

        if (buyBeginner == false && buyMiddle == false && buyHigh == false) 
        {
            List<TransactionValue> tr = new List<TransactionValue>();

            ServerData.userInfoTable.GetTableData(UserInfoTable.basicPackRefund).Value = 1;

            Param ur = new Param();

            ur.Add(UserInfoTable.basicPackRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.basicPackRefund).Value);

            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, ur));

            ServerData.SendTransaction(tr);
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.userInfoTable.GetTableData(UserInfoTable.basicPackRefund).Value = 1;

        Param userInfoParam = new Param();

        userInfoParam.Add(UserInfoTable.basicPackRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.basicPackRefund).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        //

        int beginnerPeachNum = 1000;
        int middlePeachNum = 3000;
        int highPeachNum = 7000;

        int peachNum = 0;


        Param goodsParam = new Param();

        if (buyBeginner)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += beginnerPeachNum;

            peachNum += beginnerPeachNum;
        }

        if (buyMiddle)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += middlePeachNum;

            peachNum += middlePeachNum;
        }

        if (buyHigh)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += highPeachNum;

            peachNum += highPeachNum;
        }

        goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            string desc = string.Empty;

            if (buyBeginner) 
            {
                desc += "초보자세트\n";
            }
            if (buyMiddle) 
            {
                desc += "중급자세트\n";
            }
            if (buyHigh) 
            {
                desc += "상급자세트\n";
            }

            PopupManager.Instance.ShowConfirmPopup($"알림", $"{desc}{CommonString.GetItemName(Item_Type.PeachReal)} 총 {peachNum}개 소급됨", null);
        });
    }
}
