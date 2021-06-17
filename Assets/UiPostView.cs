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

        title.SetText(postInfo.titleText);
        description.SetText(postInfo.contentText);

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)postInfo.itemType);
    }

    public void OnClickReceiveButton()
    {
        receiveButton.interactable = false;
        PostManager.Instance.ReceivePost(postInfo);
    }
}
