using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static PostManager;

public class UiPostView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Image rewardIcon;

    private PostInfo postInfo;

    [SerializeField]
    private Button receiveButton;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        PostManager.Instance.WhenPostRefreshed.Subscribe(WhenPostRefreshed).AddTo(this);
    }

    private void WhenPostRefreshed(Unit unit)
    {
        receiveButton.interactable = true;
    }

    public void Initilaize(PostInfo postInfo)
    {
        this.postInfo = postInfo;

        Item_Type type = (Item_Type)(int)postInfo.itemType;

        if (type.IsRankFrameItem())
        {
            switch (type)
            {
                case Item_Type.RankFrame1:
                    title.SetText("랭킹보상(1위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_1}개\n채팅 아이콘(1등급)");
                    break;
                case Item_Type.RankFrame2:
                    title.SetText("랭킹보상(2위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_2}개\n채팅 아이콘(2등급)");
                    break;
                case Item_Type.RankFrame3:
                    title.SetText("랭킹보상(3위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_3}개\n채팅 아이콘(3등급)");
                    break;
                case Item_Type.RankFrame4:
                    title.SetText("랭킹보상(4위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_4}개\n채팅 아이콘(4등급)");
                    break;
                case Item_Type.RankFrame5:
                    title.SetText("랭킹보상(5위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_5}개\n채팅 아이콘(5등급)");
                    break;
                case Item_Type.RankFrame6_20:
                    title.SetText("랭킹보상(6~20위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_6_20}개\n채팅 아이콘(6등급)");
                    break;
                case Item_Type.RankFrame21_100:
                    title.SetText("랭킹보상(21~100위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_21_100}개\n채팅 아이콘(7등급)");
                    break;
                case Item_Type.RankFrame101_1000:
                    title.SetText("랭킹보상(101~1000위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_101_1000}개\n채팅 아이콘(8등급)");
                    break;
                case Item_Type.RankFrame1001_10000:
                    title.SetText("랭킹보상(1001~10000위)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_1001_10000}개\n채팅 아이콘(9등급)");
                    break;
            }
        }
        else if (type.IsRelicRewardItem()) 
        {
            switch (type)
            {
                case Item_Type.RankFrame1_relic:
                    title.SetText("랭킹보상(1위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_1_relic}개");
                    break;
                case Item_Type.RankFrame2_relic:
                    title.SetText("랭킹보상(2위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_2_relic}개");
                    break;
                case Item_Type.RankFrame3_relic:
                    title.SetText("랭킹보상(3위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_3_relic}개");
                    break;
                case Item_Type.RankFrame4_relic:
                    title.SetText("랭킹보상(4위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_4_relic}개");
                    break;
                case Item_Type.RankFrame5_relic:
                    title.SetText("랭킹보상(5위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_5_relic}개");
                    break;
                case Item_Type.RankFrame6_20_relic:
                    title.SetText("랭킹보상(6~20위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_6_20_relic}개");
                    break;
                case Item_Type.RankFrame21_100_relic:
                    title.SetText("랭킹보상(21~100위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_21_100_relic}개");
                    break;
                case Item_Type.RankFrame101_1000_relic:
                    title.SetText("랭킹보상(101~1000위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_101_1000_relic}개");
                    break;
                case Item_Type.RankFrame1001_10000_relic:
                    title.SetText("랭킹보상(1001~10000위)(영혼의숲)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.RelicTicket)} {GameBalance.rankRewardTicket_1001_10000_relic}개");
                    break;
            }
        }
        else if (type.IsMiniGameRewardItem()) 
        {
            switch (type)
            {
                case Item_Type.RankFrame1_miniGame:
                    title.SetText("랭킹보상(1위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_1_MiniGame}개");
                    break;
                case Item_Type.RankFrame2_miniGame:
                    title.SetText("랭킹보상(2위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_2_MiniGame}개");
                    break;
                case Item_Type.RankFrame3_miniGame:
                    title.SetText("랭킹보상(3위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_3_MiniGame}개");
                    break;
                case Item_Type.RankFrame4_miniGame:
                    title.SetText("랭킹보상(4위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_4_MiniGame}개");
                    break;
                case Item_Type.RankFrame5_miniGame:
                    title.SetText("랭킹보상(5위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_5_MiniGame}개");
                    break;
                case Item_Type.RankFrame6_20_miniGame:
                    title.SetText("랭킹보상(6~20위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_6_20_MiniGame}개");
                    break;
                case Item_Type.RankFrame21_100_miniGame:
                    title.SetText("랭킹보상(21~100위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_21_100_MiniGame}개");
                    break;
                case Item_Type.RankFrame101_1000_miniGame:
                    title.SetText("랭킹보상(101~1000위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_101_1000_MiniGame}개");
                    break;
                case Item_Type.RankFrame1001_10000_miniGame:
                    title.SetText("랭킹보상(1001~10000위)(미니게임)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.MiniGameReward)} {GameBalance.rankReward_1001_10000_MiniGame}개");
                    break;
            }
        }
        else if (type.IsGuildRewardItem())
        {
            switch (type)
            {
                case Item_Type.RankFrame1_guild:
                    title.SetText("랭킹보상(1위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_1_guild}개");
                    break;
                case Item_Type.RankFrame2_guild:
                    title.SetText("랭킹보상(2위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_2_guild}개");
                    break;
                case Item_Type.RankFrame3_guild:
                    title.SetText("랭킹보상(3위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_3_guild}개");
                    break;
                case Item_Type.RankFrame4_guild:
                    title.SetText("랭킹보상(4위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_4_guild}개");
                    break;
                case Item_Type.RankFrame5_guild:
                    title.SetText("랭킹보상(5위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_5_guild}개");
                    break;
                case Item_Type.RankFrame6_20_guild:
                    title.SetText("랭킹보상(6~20위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_6_20_guild}개");
                    break;
                case Item_Type.RankFrame21_100_guild:
                    title.SetText("랭킹보상(21~100위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_21_100_guild}개");
                    break;
                case Item_Type.RankFrame101_1000_guild:
                    title.SetText("랭킹보상(101~1000위)(문파)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.GuildReward)} {GameBalance.rankReward_101_1000_guild}개");
                    break;
            }
        }
        else
        {
            title.SetText(postInfo.titleText);
            description.SetText(postInfo.contentText);
        }

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)postInfo.itemType);
    }

    public void OnClickReceiveButton()
    {
        receiveButton.interactable = false;
        PostManager.Instance.ReceivePost(postInfo);
    }
}
