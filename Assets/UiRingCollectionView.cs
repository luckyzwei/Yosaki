using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRingCollectionView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI ringName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private NewGachaTableData newGachaData;

    private NewGachaServerData newGachaServerData;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private Image newGachaIcon;

    [SerializeField]
    private Image reward0Icon;
    [SerializeField]
    private Image reward1Icon;

    [SerializeField]
    private TextMeshProUGUI reward0Value;
    [SerializeField]
    private TextMeshProUGUI reward1Value;

    [SerializeField]
    private Button reward0Button;
    [SerializeField]
    private Button reward1Button;

    [SerializeField]
    private TextMeshProUGUI reward0Description;
    [SerializeField]
    private TextMeshProUGUI reward1Description;


    public void Initialize(NewGachaTableData ringData)
    {

        this.newGachaData = ringData;

        this.newGachaServerData = ServerData.newGachaServerTable.TableDatas[ringData.Stringid];

        ringName.SetText($"{ringData.Skillname}");
        
        newGachaIcon.sprite = CommonResourceContainer.GetRingSprite(ringData.Id);        

        reward0Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)ringData.Rewardtype0);
        reward1Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)ringData.Rewardtype1);

        reward0Value.SetText(Utils.ConvertBigNum(ringData.Rewardvalue0));
        reward1Value.SetText(Utils.ConvertBigNum(ringData.Rewardvalue1));
        

        Subscribe();
    }

    private void Subscribe()
    {
        newGachaServerData.hasItem.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);
        newGachaServerData.getReward0.AsObservable().Subscribe(e =>
        {
            bool hasReward = e == 1;

            reward0Button.interactable = !hasReward;

            reward0Description.SetText(!hasReward ? "보상수령" : "수령완료");
        }).AddTo(this);
        newGachaServerData.getReward1.AsObservable().Subscribe(e =>
        {
            bool hasReward = e == 1;

            reward1Button.interactable = !hasReward;

            reward1Description.SetText(!hasReward ? "보상수령" : "수령완료");
        }).AddTo(this);


    }

    public void OnClickGetRewardFreeButton()
    {
        if (newGachaServerData.getReward0.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        newGachaServerData.getReward0.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //재화 획득
        Item_Type rewardType = (Item_Type)newGachaData.Rewardtype0;

        float rewardValue = newGachaData.Rewardvalue0;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));


        // 보상 획득 
        Param ringParam = new Param();        
        string updateValue = ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].ConvertToString();
        ringParam.Add(newGachaData.Stringid, updateValue);

        transactions.Add(TransactionValue.SetUpdate(NewGachaServerTable.tableName, NewGachaServerTable.Indate, ringParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
    public void OnClickGetRewardAdButton()
    {
        if (newGachaServerData.getReward1.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (ServerData.iapServerTable.TableDatas[UiEquipmentCollectionPassBuyButton.collectionPassKey].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("도감 패스권이 필요합니다.");
            return;
        }

        newGachaServerData.getReward1.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //재화 획득
        Item_Type rewardType = (Item_Type)newGachaData.Rewardtype1;

        float rewardValue = newGachaData.Rewardvalue1;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));


        // 보상 획득 
        Param ringParam = new Param();        
        string updateValue = ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].ConvertToString();
        ringParam.Add(newGachaData.Stringid, updateValue);

        transactions.Add(TransactionValue.SetUpdate(NewGachaServerTable.tableName, NewGachaServerTable.Indate, ringParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
}
