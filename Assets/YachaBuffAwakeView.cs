using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BackEnd;

public class YachaBuffAwakeView : MonoBehaviour
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
        ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].AsObservable().Subscribe(e =>
        {
            awakeButton.SetActive(e == 0);
            applyButton.SetActive(e == 1);
        }).AddTo(this);
    }

    public void OnClickAwakeButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 각성 됐습니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.SheepStone).Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"십이지신(미) 최종 보상 {CommonString.GetItemName(Item_Type.SheepStone)}이 필요합니다.");
            return;
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(UserInfoTable.buffAwake, ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "시동 각성 완료!\n일부 시동 능력치가 강화 되고,\n모든 시동 능력치가 항상 적용 됩니다.", null);
          });
    }
}
