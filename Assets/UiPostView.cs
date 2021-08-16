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
                case Item_Type.RankFrame1_10:
                    title.SetText("랭킹보상(1~10)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_1_10}개\n채팅 테두리(1등급)");
                    break;
                case Item_Type.RankFrame10_100:
                    title.SetText("랭킹보상(11~100)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_1_10}개\n채팅 테두리(2등급)");
                    break;
                case Item_Type.RankFrame100_1000:
                    title.SetText("랭킹보상(101~1000)");
                    description.SetText($"{CommonString.GetItemName(Item_Type.Ticket)} {GameBalance.rankRewardTicket_1_10}개\n채팅 테두리(3등급)");
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
