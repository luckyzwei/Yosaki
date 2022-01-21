using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class UiSonRewardCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    //[SerializeField]
    //private TextMeshProUGUI itemDescription;

    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    [SerializeField]
    private GameObject rewardLockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;

    [SerializeField]
    private GameObject rewardedIcon;

    [SerializeField]
    private TextMeshProUGUI itemDescription;

    private SonRewardData tableData;

    private double score = 0;

    public void Initialize(SonRewardData tableData)
    {
        score = ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin;

        this.tableData = tableData;

        UpdateUi();

        Subscribe();
    }

    private void UpdateUi()
    {
        rewardLockMask.SetActive(score < tableData.Score);

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Rewardtype);

        itemDescription.SetText($"{CommonString.GetItemName((Item_Type)tableData.Rewardtype)}({Utils.ConvertBigNum(tableData.Score)})점");

        rewardAmount.SetText($"{Utils.ConvertBigNum(tableData.Rewardvalue)}개");

        lockDescription.SetText($"점수 {tableData.Scoredescription} 돌파시 해금");
    }

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].AsObservable().Subscribe(e =>
        {
            bool rewarded = ServerData.etcServerTable.SonRewarded(tableData.Id);

            rewardedIcon.SetActive(rewarded);

            UpdateUi();

        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        if (score < tableData.Score)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다.");
            return;
        }

        bool rewarded = ServerData.etcServerTable.SonRewarded(tableData.Id);

        if (rewarded)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        rewardButton.interactable = false;

        Item_Type type = (Item_Type)tableData.Rewardtype;

        float amount = tableData.Rewardvalue;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param rewardParam = new Param();

        ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

        rewardParam.Add(EtcServerTable.sonReward, ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            //LogManager.Instance.SendLogType("Son", "reward", tableData.Id.ToString());
            PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
            SoundManager.Instance.PlaySound("Reward");
            rewardButton.interactable = true;
        });
    }

    public void OnClickGetButtonByScript()
    {
        if (score < tableData.Score)
        {
            return;
        }

        bool rewarded = ServerData.etcServerTable.SonRewarded(tableData.Id);

        if (rewarded)
        {
            return;
        }

        rewardButton.interactable = false;

        Item_Type type = (Item_Type)tableData.Rewardtype;

        float amount = tableData.Rewardvalue;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param rewardParam = new Param();

        ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

        rewardParam.Add(EtcServerTable.sonReward, ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            //LogManager.Instance.SendLogType("Son", "reward", tableData.Id.ToString());
            PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
            SoundManager.Instance.PlaySound("Reward");
            rewardButton.interactable = true;
        });
    }
}
