using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static UiTwelveRewardPopup;

public class UiTwelveBossRewardView : MonoBehaviour
{
    private TwelveBossRewardInfo rewardInfo;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemDescription;

    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private TextMeshProUGUI rewardButtonDescription;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    [SerializeField]
    private GameObject rewardLockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;

    private BossServerData bossServerData;

    [SerializeField]
    private GameObject rewardedIcon;

    private CompositeDisposable disposable = new CompositeDisposable();

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void Initialize(TwelveBossRewardInfo rewardInfo, BossServerData bossServerData)
    {
        this.rewardInfo = rewardInfo;

        this.bossServerData = bossServerData;

        rewardLockMask.SetActive(rewardInfo.currentDamage < rewardInfo.damageCut);

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)rewardInfo.rewardType);

        itemDescription.SetText($"{CommonString.GetItemName((Item_Type)rewardInfo.rewardType)}");

        rewardAmount.SetText($"{Utils.ConvertBigNum(rewardInfo.rewardAmount)}개");

        lockDescription.SetText($"최고 피해량 {Utils.ConvertBigNum(rewardInfo.damageCut)}에 해금");

        Subscribe();
    }

    private void Subscribe()
    {
        disposable.Clear();

        bossServerData.rewardedId.AsObservable().Subscribe(e =>
        {
            var rewards = e.Split(BossServerTable.rewardSplit).ToList();

            bool rewarded = rewards.Contains(rewardInfo.idx.ToString());

            rewardButtonDescription.SetText(rewarded ? "완료" : "받기");

            rewardedIcon.SetActive(rewarded);

        }).AddTo(disposable);

    }

    public void OnClickGetButton()
    {
        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 피해량이 부족 합니다.");
            return;
        }

        var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit).ToList();

        if (rewards.Contains(rewardInfo.idx.ToString()))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        rewardButton.interactable = false;

        Item_Type type = (Item_Type)rewardInfo.rewardType;

        float amount = rewardInfo.rewardAmount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param bossParam = new Param();

        bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

        var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

        bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
              SoundManager.Instance.PlaySound("Reward");
              rewardButton.interactable = true;
          });
    }
}
