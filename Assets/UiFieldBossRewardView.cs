using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;

public class UiFieldBossRewardView : SingletonMono<UiFieldBossRewardView>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    [SerializeField]
    private UiStageCell stageCell;

    [SerializeField]
    private GameObject moreRewardPopup;


    public void Initialize(int rewardAmount)
    {
        var stageMapData = GameManager.Instance.CurrentStageData;
        SoundManager.Instance.PlaySound("Reward");
        rootObject.gameObject.SetActive(true);

        this.rewardAmount.SetText(Utils.ConvertBigNum(rewardAmount));

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)stageMapData.Bossrewardtype);
    }

    public void OnClickMoreRewardButton()
    {
        moreRewardPopup.SetActive(true);
        stageCell.Initialize(GameManager.Instance.CurrentStageData);
    }

    bool buttonClicked = false;

    public void OnClickNextStage()
    {
        if (buttonClicked == true)
        {
            return;
        }

        buttonClicked = true;

        GameManager.Instance.LoadNextScene();
    }
}
