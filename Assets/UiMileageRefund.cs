using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiMileageRefund : MonoBehaviour
{
    [SerializeField]
    private UiText uiText;

    [SerializeField]
    private Transform parents;

    [SerializeField]
    private GameObject rootObject;


    void Start()
    {
        RefundRoutine();
    }

    private void RefundRoutine()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value != 0)
        {
            rootObject.SetActive(false);
            return;
        }

        int mileageTotalNum = 0;

        string description = string.Empty;

        var serverTable = ServerData.iAPServerTableTotal.TableDatas;

        var localTableData = TableManager.Instance.InAppPurchase.dataArray;

        for (int i = 0; i < localTableData.Length; i++)
        {
            for (int j = 0; j < localTableData[i].Rewardtypes.Length; j++)
            {
                if (localTableData[i].Rewardtypes[j] == 9000)
                {
                    int buyCount = serverTable[localTableData[i].Productid].buyCount.Value;

                    if (buyCount == 0) continue;

                    int mileageNum = localTableData[i].Rewardvalues[j] * buyCount;

                    mileageTotalNum += mileageNum;

                    var textObject = Instantiate<UiText>(uiText, parents);

                    textObject.Initialize($"{localTableData[i].Title} {buyCount}회 구매 마일리지 {mileageNum}개");
                }
            }
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value += mileageTotalNum;
        ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Mileage, ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.mileageRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        if (mileageTotalNum == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value = 1;
            ServerData.userInfoTable.UpData(UserInfoTable.mileageRefund, false);

            rootObject.SetActive(false);
        }
        else
        {
            rootObject.SetActive(true);

            var totalText = Instantiate<UiText>(uiText, parents);

            totalText.Initialize($"마일리지 총 {mileageTotalNum}개 소급됨");

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "마일리지 소급 완료!", null);
            });
        }
    }
}
