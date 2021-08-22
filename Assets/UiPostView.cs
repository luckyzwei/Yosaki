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
