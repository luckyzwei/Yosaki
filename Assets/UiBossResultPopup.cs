using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiBossResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI remainHpText;

    [SerializeField]
    private Transform rewardParent;

    public void Initialize(float damagedAmount, float bossRemainHpPer, List<RewardData> rewardDatas)
    {
        scoreText.SetText(Utils.ConvertBigFloat(damagedAmount));
        remainHpText.SetText(((bossRemainHpPer * 100f).ToString("F7")).ToString());

        MakeRewardView(rewardDatas);
    }

    private void MakeRewardView(List<RewardData> rewardDatas)
    {
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            var rewardView = Instantiate<UiRewardView>(CommonPrefabContainer.Instance.uiRewardViewPrefab,rewardParent);
            rewardView.Initialize(rewardDatas[i]);
        }
    }
}
