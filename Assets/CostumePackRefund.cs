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
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.petCostumePackRefund).Value == 1) return;


        bool buyPet0 = ServerData.iapServerTable.TableDatas["petpackage1"].buyCount.Value == 1;
        bool buyPet1 = ServerData.iapServerTable.TableDatas["petpackage2"].buyCount.Value == 1;

        bool buyCostume0 = ServerData.iapServerTable.TableDatas["costumepackage1"].buyCount.Value == 1;
        bool buyCostume1 = ServerData.iapServerTable.TableDatas["costumepackage2"].buyCount.Value == 1;


        List<TransactionValue> transactions = new List<TransactionValue>();
        //

        ServerData.userInfoTable.GetTableData(UserInfoTable.petCostumePackRefund).Value = 1;

        Param userInfoParam = new Param();

        userInfoParam.Add(UserInfoTable.petCostumePackRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.petCostumePackRefund).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        //

        int pet0RelicTicketNum = 15;
        int pet1RelicTicketNum = 25;

        int costume0PeachNum = 1500;
        int costume1PeachNum = 3000;


        Param goodsParam = new Param();

        if (buyPet0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += pet0RelicTicketNum;
        }

        if (buyPet1)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += pet1RelicTicketNum;
        }

        if (buyCostume0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += costume0PeachNum;
        }

        if (buyCostume1)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += costume1PeachNum;
        }

        if (buyPet0 || buyPet1)
        {
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
        }

        if (buyCostume0 || buyCostume1)
        {
            goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
        }

        if (buyPet0 || buyPet1 || buyCostume0 || buyCostume1)
        {
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            if (buyPet0 || buyPet1 || buyCostume0 || buyCostume1)
            {
                PopupManager.Instance.ShowConfirmPopup("환수,외형 세트상품 소급", $"영혼열쇠 {pet0RelicTicketNum + pet1RelicTicketNum}개\n천도복숭아 {costume0PeachNum + costume1PeachNum}개 소급됨", null);
            }
        });
    }
}
