using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMarbleRelicRefund : MonoBehaviour
{



    private void Start()
    {
      //  Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value != 0)
        {
            return;
        }

        int monthlypackage1buyCount = ServerData.iAPServerTableTotal.TableDatas["monthlypackage1"].buyCount.Value;
        //int marblePack1buyCount = ServerData.iAPServerTableTotal.TableDatas["monthlypackage1"].buyCount.Value;
        //int marblePack2buyCount = ServerData.iAPServerTableTotal.TableDatas["marblepackage2"].buyCount.Value;
        //int marblePack3buyCount = ServerData.iAPServerTableTotal.TableDatas["marblepackage3"].buyCount.Value;

        //int relicPack1buyCount = ServerData.iAPServerTableTotal.TableDatas["relic1"].buyCount.Value;
        //int relicPack2buyCount = ServerData.iAPServerTableTotal.TableDatas["relic2"].buyCount.Value;
        //int relicPack3buyCount = ServerData.iAPServerTableTotal.TableDatas["relic3"].buyCount.Value;
        //int relicPack4buyCount = ServerData.iAPServerTableTotal.TableDatas["relic4"].buyCount.Value;

        if (monthlypackage1buyCount == 0)
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

        float _1DiffMileage = 5;

        //float _1DiffMarble = 24000000;

        //float _2DiffMarble = 48000000;

        //float _3DiffMarble = 110000000;
        


        float monthly1_MileageAdd = monthlypackage1buyCount * _1DiffMileage;

        //float marble1_MarbleAdd = marblePack1buyCount * _1DiffMarble;


        //float marble2_MarbleAdd = marblePack2buyCount * _2DiffMarble;


        //float marble3_MarbleAdd = marblePack3buyCount * _3DiffMarble;


        //float addMarbleTotal = marble1_MarbleAdd + marble2_MarbleAdd + marble3_MarbleAdd;



        //

        //float _1DiffRelic = 70;

        //float _2DiffRelic = 200;

        //float _3DiffRelic = 400;

        //float _4DiffRelic = 1500;

        //float relic1_RelicAdd = relicPack1buyCount * _1DiffRelic;


        //float relic2_RelicAdd = relicPack2buyCount * _2DiffRelic;
        //float relic3_RelicAdd = relicPack3buyCount * _3DiffRelic;

        //float relic4_RelicAdd = relicPack4buyCount * _4DiffRelic;

        //float addRelicTotal = relic1_RelicAdd + relic2_RelicAdd + relic3_RelicAdd + relic4_RelicAdd;
        //LogManager.Instance.SendLogType("MarRelicRefund", "Marble", $"{marblePack1buyCount},{marblePack2buyCount},{marblePack3buyCount}");
        //LogManager.Instance.SendLogType("MarRelicRefund", "Relic", $"{relicPack1buyCount},{relicPack2buyCount},{relicPack3buyCount},{relicPack4buyCount}");
        // LogManager.Instance.SendLogType("MarRelicRefund", "Milegage", $"{monthlypackage1buyCount}");
        
        //

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value += monthly1_MileageAdd;

        //ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += addRelicTotal;




        ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Mileage, ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value);
        //goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.marRelicRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        PopupManager.Instance.ShowConfirmPopup("월간 세트 마일리지 추가 소급",
            $"마일리지 {Utils.ConvertBigNum(monthly1_MileageAdd)}개 소급됨" ,null);
        });


    }

}
