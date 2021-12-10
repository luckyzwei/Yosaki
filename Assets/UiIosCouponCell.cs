using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiIosCouponCell : MonoBehaviour
{
    [SerializeField]
    private GameObject rewardButton;

    [SerializeField]
    private GameObject completeButton;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    private IosCouponData couponData;

    public void Initialize(IosCouponData couponData)
    {
        this.couponData = couponData;

        description.SetText(couponData.Description);

        Item_Type itemType = (Item_Type)couponData.Rewardtype;

        rewardAmount.SetText($"{CommonString.GetItemName(itemType)} {Utils.ConvertBigNum(couponData.Rewardamount)}개");

        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.iosCoupon].AsObservable().Subscribe(e =>
        {

            bool rewarded = ServerData.etcServerTable.IosCouponRewarded(couponData.Id);

            rewardButton.SetActive(!rewarded);

            completeButton.SetActive(rewarded);

        }).AddTo(this);
    }

    public void OnClickRewardButton()
    {
        bool rewarded = ServerData.etcServerTable.IosCouponRewarded(couponData.Id);

        if (rewarded)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Item_Type itemType = (Item_Type)couponData.Rewardtype;

        float rewardAmount = couponData.Rewardamount;

        ServerData.etcServerTable.TableDatas[EtcServerTable.iosCoupon].Value += $"{BossServerTable.rewardSplit}{couponData.Id}";

        Param rewardParam = new Param();

        rewardParam.Add(EtcServerTable.iosCoupon, ServerData.etcServerTable.TableDatas[EtcServerTable.iosCoupon].Value);


        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(itemType, (int)rewardAmount));
        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

        PopupManager.Instance.ShowAlarmMessage("보상 획득!");

        ServerData.SendTransaction(transactions);
    }
}
