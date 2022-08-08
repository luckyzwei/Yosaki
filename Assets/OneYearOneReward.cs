using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;

public class OneYearOneReward : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent_one].AsObservable().Subscribe(e =>
        {

            buttonDescription.SetText(e == 0 ? "보상받기" : "획득완료");


        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent_one].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 이미 받았습니다!");
            return;
        }


        ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent_one].Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += 50000;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.oneAttenEvent_one, ServerData.userInfoTable.TableDatas[UserInfoTable.oneAttenEvent_one].Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup("알림", "검기 50000개 획득!", null);
        });
    }
}
