using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiRewardView;

public class UiBossContentsClearBoard : SingletonMono<UiBossContentsClearBoard>
{
    [SerializeField]
    private DungeonRewardView dungeonRewardView;
    [SerializeField]
    private GameObject rootObject;

    public void Initialize(List<RewardData> rewardData)
    {
        rootObject.SetActive(true);

        dungeonRewardView.Initalize(rewardData);
    }
}
