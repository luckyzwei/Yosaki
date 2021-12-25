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
    private float popupOriginWidth;

    [SerializeField]
    private float popupDokebiWidth;

    [SerializeField]
    private List<Button> dokebiEnterButtons;

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


    private void OnEnable()
    {
        enterButton.SetActive(false);

        popupBg.sizeDelta = new Vector2(popupDokebiWidth, popupBg.sizeDelta.y);
    }

    private void OnDisable()
    {
        popupBg.sizeDelta = new Vector2(popupOriginWidth, popupBg.sizeDelta.y);
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

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Dokebi)} {defeatCount}개로 소탕 합니까?", () =>
         {
             int rewardNum = defeatCount * TableManager.Instance.DokebiTable.dataArray[idx].Rewardamount;
             ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value += rewardNum;

             ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value++;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param goodsParam = new Param();

             goodsParam.Add(GoodsTable.DokebiKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value);

             Param userInfoParam = new Param();

             userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);

             transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
             transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

             ServerData.SendTransaction(transactions, successCallBack: () =>
             {
                 PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Dokebi)} {rewardNum}개 획득!");

                //사운드
                SoundManager.Instance.PlaySound("Reward");
                 LogManager.Instance.SendLog("도꺠비 소탕", $"{rewardNum}개 획득 {ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value}");
             });
         }, null);
    }

}
