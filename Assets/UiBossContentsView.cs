using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;

public class UiBossContentsView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private Image bossIcon;

    [SerializeField]
    private List<Sprite> bossSprites;

    private BossTableData bossTableData;

    [SerializeField]
    private Button enterButton;

    [SerializeField]
    private Button clearButton;

    private ObscuredInt instantOpenAmount = 1;

    //private ContentsRewardManager

    public void OnClickToggle_1(bool isOn)
    {
        if (isOn)
            instantOpenAmount = 1;
    }

    public void OnClickToggle_2(bool isOn)
    {
        if (isOn)
            instantOpenAmount = 10;
    }

    public void OnClickToggle_3(bool isOn)
    {
        if (isOn)
            instantOpenAmount = 30;
    }

    public void Initialize(BossTableData bossTableData)
    {
        this.bossTableData = bossTableData;
        title.SetText(bossTableData.Name);
        description.SetText(bossTableData.Description);

        lockObject.SetActive(bossTableData.Islock);

        bossIcon.sprite = CommonUiContainer.Instance.bossIcon[bossTableData.Id];
    }

    public void OnClickEnterButton()
    {
        int price = 1;
        int currentTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value;

        if (currentTicketNum < price)
        {
            PopupManager.Instance.ShowAlarmMessage("티켓이 부족합니다.");
            return;
        }

        enterButton.interactable = false;

        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value--;

        ServerData.goodsTable.SyncToServerEach(GoodsTable.Ticket, () =>
        {
            //
            Param param = new Param();
            param.Add("티켓사용함", $"남은 갯수 : { ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value}");

            SendQueue.Enqueue(Backend.GameLog.InsertLog, "Ticket", param, (brk) => { });
            //로그

            GameManager.Instance.SetBossId(bossTableData.Id);
            GameManager.Instance.LoadContents(GameManager.ContentsType.Boss);
        },
        () =>
        {
            enterButton.interactable = true;

        });
    }

    public void OnClickInstantClearButton()
    {
        int price = 1;

        int currentTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value;

        if (currentTicketNum < price)
        {
            PopupManager.Instance.ShowAlarmMessage("티켓이 부족합니다.");
            return;
        }

        clearButton.interactable = false;

        int clearAmount = Mathf.Min(instantOpenAmount, currentTicketNum);

        var bossServerTable = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        float currentScore = float.Parse(bossServerTable.score.Value);

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{Utils.ConvertBigNum(currentScore)}점으로 {clearAmount}번\n소탕 하시겠습니까?", () =>
                    {
                        InstantClearReceive(currentScore, clearAmount);
                    },
                    () =>
                    {
                        clearButton.interactable = true;
                    });
    }

    private void InstantClearReceive(float score, int clearAmount)
    {
        LogManager.Instance.SendLog("보스소탕요청", $"{clearAmount}회");

        var rewardList = SingleRaidManager.GetRewawrdData(bossTableData, score, clearAmount);

        if (rewardList.Find(element => element.itemType == Item_Type.Ticket) == null)
        {
            //티켓 차감도 트랜젝션에 전송되도록
            rewardList.Add(new UiRewardView.RewardData(Item_Type.Ticket, 0));
        }

        //티켓차감
        ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value -= clearAmount;

        ServerData.SendTransaction(rewardList, addValue: null,
         completeCallBack: () =>
         {
             clearButton.interactable = true;
         }
        , successCallBack: () =>
        {
            clearButton.interactable = true;
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.RewardedBossContents, clearAmount);
            WhenClearSuccess(rewardList);
            LogManager.Instance.SendLog("보스소탕성공", $"{clearAmount}회");
        });
    }

    private void WhenClearSuccess(List<RewardData> rewardData)
    {
        var zeroData = rewardData.FindAll(e => e.amount == 0);
        for (int i = 0; i < zeroData.Count; i++)
        {
            rewardData.Remove(zeroData[i]);
        }

        UiBossContentsClearBoard.Instance.Initialize(rewardData);
    }
}
