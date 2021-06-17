using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiFieldBossRewardView : SingletonMono<UiFieldBossRewardView>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI rewardAmount; 

    public void Initialize(int rewardAmount) 
    {
        SoundManager.Instance.PlaySound("BonusEnd");
        rootObject.gameObject.SetActive(true);
        this.rewardAmount.SetText(Utils.ConvertBigFloat(rewardAmount));
    }
}
