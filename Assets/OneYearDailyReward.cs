using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;

public class OneYearDailyReward : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent].AsObservable().Subscribe(e =>
        {

            buttonDescription.SetText(e == 0 ? "보상받기" : "획득완료");


        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 보상을 받았습니다!");
            return;
        }


        ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent].Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += 1000;
        ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += 1000;
        ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += 1000;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
        goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
        goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.oneAttenEvent, ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent].Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup("알림", "보상 획득!", null);
          });
    }
}
