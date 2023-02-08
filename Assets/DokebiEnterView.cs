using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class DokebiEnterView : MonoBehaviour
{
    [SerializeField]
    private GameObject enterButton;

    [SerializeField]
    private TextMeshProUGUI enterCountText;

    [SerializeField]
    private RectTransform popupBg;

    [SerializeField]
    private List<Button> dokebiEnterButtons;

    [SerializeField]
    private TextMeshProUGUI oldDokebiScore;


    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).AsObservable().Subscribe(e =>
        {
            enterCountText.SetText($"오늘 입장({(int)e}/{GameBalance.dokebiEnterCount})");
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.oldDokebi2LastClear).AsObservable().Subscribe(e => 
        {
            oldDokebiScore.SetText($"현재 클리어 층 : {e}");
        }).AddTo(this);

    }

    public void OnClickEnterButton(int idx)
    {
        int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value;

        if (currentEnterCount >= GameBalance.dokebiEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 입장할 수 없습니다.");
            return;
        }

        dokebiEnterButtons.ForEach(e => e.interactable = false);

        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value++;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList,
          successCallBack: () =>
          {
              GameManager.Instance.SetDokebiId(idx);
              GameManager.Instance.LoadContents(GameManager.ContentsType.Dokebi);
          },
          completeCallBack: () =>
          {
              dokebiEnterButtons.ForEach(e => e.interactable = true);
          });
    }
    public void OnClickEnterOldDokebi2Button(int idx)
    {
        dokebiEnterButtons.ForEach(e => e.interactable = false);

        GameManager.Instance.LoadContents(GameManager.ContentsType.OldDokebi2);
    }

    public void OnClickOldDokebi2RewardButton()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiBundle).Value > 0)
        {
            //이미 받음
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"오늘 {CommonString.GetItemName(Item_Type.DokebiBundle)}를 이미 수령하였습니다!", null);
            return;
        }

        ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiBundle).Value = 1;

        int score = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.oldDokebi2LastClear).Value;

        ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value += score;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.getDokebiBundle, ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiBundle).Value);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.DokebiBundle, ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


        ServerData.SendTransaction(transactionList,
          successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiBundle)} {score} 개 획득!!", null);
          });
    }
    private void OnEnable()
    {
        enterButton.SetActive(false);

    }


    public void OnClickInstantClearButton(int idx)
    {
        int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value;

        if (currentEnterCount >= GameBalance.dokebiEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            return;
        }

        int defeatCount = 0;

        int clearCount = GameBalance.dokebiEnterCount - currentEnterCount;

        if (idx == 0)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount0).Value;
        }
        else if (idx == 1)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount1).Value;
        }
        else if (idx == 2)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount2).Value;
        }
        else if (idx == 3)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount3).Value;
        }

        if (defeatCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("플레이 데이터가 없습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Dokebi)} <color=yellow>{defeatCount}</color>개로 <color=yellow>{clearCount}회</color> 소탕 합니까?", () =>
         {
             GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearOni);

             int rewardNum = defeatCount;

             ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value += rewardNum * clearCount;

             ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value += clearCount;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param goodsParam = new Param();

             goodsParam.Add(GoodsTable.DokebiKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value);

             Param userInfoParam = new Param();

             userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);

             transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
             transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
             EventMissionManager.UpdateEventMissionClear(EventMissionKey.ClearOni, clearCount);
             ServerData.SendTransaction(transactions, successCallBack: () =>
             {
                 PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Dokebi)} {rewardNum * clearCount}개 획득!");

                 //사운드
                 SoundManager.Instance.PlaySound("Reward");
                 //LogManager.Instance.SendLog("DokClear", $"{rewardNum}개 획득 {ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value}");
             });
         }, null);
    }

}
