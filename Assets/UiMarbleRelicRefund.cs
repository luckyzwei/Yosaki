using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMarbleRelicRefund : MonoBehaviour
{



    private void Start()
    {
        Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value != 0)
        {
            return;
        }

        int marblePack1buyCount = ServerData.iAPServerTableTotal.TableDatas["marblepackage1"].buyCount.Value;
        int marblePack2buyCount = ServerData.iAPServerTableTotal.TableDatas["marblepackage2"].buyCount.Value;
        int marblePack3buyCount = ServerData.iAPServerTableTotal.TableDatas["marblepackage3"].buyCount.Value;

        int relicPack1buyCount = ServerData.iAPServerTableTotal.TableDatas["relic1"].buyCount.Value;
        int relicPack2buyCount = ServerData.iAPServerTableTotal.TableDatas["relic2"].buyCount.Value;
        int relicPack3buyCount = ServerData.iAPServerTableTotal.TableDatas["relic3"].buyCount.Value;
        int relicPack4buyCount = ServerData.iAPServerTableTotal.TableDatas["relic4"].buyCount.Value;

        if (marblePack1buyCount == 0 && marblePack2buyCount == 0 && marblePack3buyCount == 0 && 
            relicPack1buyCount == 0 && relicPack2buyCount == 0 && relicPack3buyCount == 0 && relicPack4buyCount == 0)
        {

            ServerData.userInfoTable.GetTableData(UserInfoTable.marRelicRefund).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.marRelicRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.marRelicRefund).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, marbleParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        //소급코드


        //

        int _1DiffMarble = 6;

        int _2DiffMarble = 16;

        int _3DiffMarble = 27;
        


        int marble1_MarbleAdd = marblePack1buyCount * _1DiffMarble;
        

        int marble2_MarbleAdd = marblePack2buyCount * _2DiffMarble;
        

        int marble3_MarbleAdd = marblePack3buyCount * _3DiffMarble;


        float addMarbleTotal = marble1_MarbleAdd + marble2_MarbleAdd + marble3_MarbleAdd;

        

        //

        int _1DiffRelic = 6;

        int _2DiffRelic = 16;

        int _3DiffRelic = 27;

        int _4DiffRelic = 40;

        int relic1_RelicAdd = relicPack1buyCount * _1DiffRelic;
        

        int relic2_RelicAdd = relicPack2buyCount * _2DiffRelic;
        int relic3_RelicAdd = relicPack3buyCount * _3DiffRelic;

        int relic4_RelicAdd = relicPack4buyCount * _4DiffRelic;

        float addRelicTotal = relic1_RelicAdd + relic2_RelicAdd + relic3_RelicAdd + relic4_RelicAdd;
        LogManager.Instance.SendLogType("MarRelicRefund", "Marble", $"{marblePack1buyCount},{marblePack2buyCount},{marblePack3buyCount}");
        LogManager.Instance.SendLogType("MarRelicRefund", "Relic", $"{relicPack1buyCount},{relicPack2buyCount},{relicPack3buyCount},{relicPack4buyCount}");
        
        //

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += addMarbleTotal;

        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += addRelicTotal;




        ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.marRelicRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
            $"여우구슬 {addMarbleTotal}개 소급됨\n"+$"영혼열쇠 {addRelicTotal}개 소급됨" ,null);
        });


    }

}
