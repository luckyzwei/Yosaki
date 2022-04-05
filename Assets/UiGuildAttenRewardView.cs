using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildAttenRewardView : MonoBehaviour
{
    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private GameObject rewardedObject;

    [SerializeField]
    private TextMeshProUGUI rewardCut;

    private ObscuredInt idx;
    private ObscuredInt rewardType;
    private ObscuredFloat rewardValue;
    private ObscuredInt requireAtten;

    public void Initialize(int requireAtten, int idx, int rewardType, float rewardValue)
    {
        this.requireAtten = requireAtten;
        this.idx = idx;
        this.rewardType = rewardType;
        this.rewardValue = rewardValue;

        rewardAmount.SetText(Utils.ConvertBigNum(rewardValue));

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)rewardType);

        rewardCut.SetText($"{requireAtten}명");

        Subscribe();

        RefreshReward();
    }

    private void RefreshReward()
    {
        bool rewarded = ServerData.etcServerTable.GuildAttenRewarded(idx);
        rewardedObject.gameObject.SetActive(rewarded);

        lockObject.gameObject.SetActive(UiGuildMemberList.Instance.attenUserNum.Value < requireAtten);
    }

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].AsObservable().Subscribe(e =>
        {
            RefreshReward();
        }).AddTo(this);

        UiGuildMemberList.Instance.attenUserNum.AsObservable().Subscribe(e =>
        {
            RefreshReward();
        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        GetReward(true);
    }

    public bool GetReward(bool showAlarmMessage)
    {
        if (UiGuildMemberList.Instance.attenUserNum.Value < requireAtten)
        {
            if (showAlarmMessage)
            {
                PopupManager.Instance.ShowAlarmMessage("점수 등록 인원이 부족합니다.");
            }

            return false;
        }

        bool rewarded = ServerData.etcServerTable.GuildAttenRewarded(idx);

        if (rewarded)
        {
            if (showAlarmMessage)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보상을 받으셨습니다!");
            }

            return false;
        }

        Item_Type type = (Item_Type)(int)rewardType;
        float amount = rewardValue;
        //
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param rewardParam = new Param();

        ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].Value += $"{BossServerTable.rewardSplit}{idx}";

        rewardParam.Add(EtcServerTable.guildAttenReward, ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
            SoundManager.Instance.PlaySound("Reward");
        });

        return true;
    }
}
