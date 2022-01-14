using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BackEnd;

public class YachaDogAwakeView : MonoBehaviour
{
    [SerializeField]
    private GameObject awakeButton;

    [SerializeField]
    private GameObject applyButton;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].AsObservable().Subscribe(e =>
        {
            awakeButton.SetActive(e == 0);
            applyButton.SetActive(e == 1);
        }).AddTo(this);
    }

    public void OnClickAwakeButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 각성 됐습니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.DogStone).Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"십이지신(술) 최종 보상 {CommonString.GetItemName(Item_Type.DogStone)}이 필요합니다.");
            return;
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(UserInfoTable.dogAwake, ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료 야차 기본 능력치가 강화 됩니다!", null);
        });
    }
}
