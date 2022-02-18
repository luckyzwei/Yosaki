using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSleepRewardView : SingletonMono<UiSleepRewardView>
{
    [SerializeField]
    private List<TextMeshProUGUI> rewards;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI timeDescription;

    public void CheckReward()
    {
        if (SleepRewardReceiver.Instance.sleepRewardInfo == null) return;

        StartCoroutine(SleepRewardReceiver.Instance.GetSleepReward(ShowReward));
    }

    private void ShowReward()
    {
        rootObject.SetActive(true);

        var reward = SleepRewardReceiver.Instance.sleepRewardInfo;

        TimeSpan ts = TimeSpan.FromSeconds(reward.elapsedSeconds);
        string maxTimeString = TimeSpan.FromSeconds(GameBalance.sleepRewardMaxValue).Hours.ToString();

        if (ts.Hours != 0)
        {
            timeDescription.SetText($"{ts.Hours}시간 {ts.Minutes}분\n(최대 :{maxTimeString}시간)");
        }
        else
        {
            timeDescription.SetText($"{ts.Minutes}분 {ts.Seconds}초\n(최대 :{maxTimeString}시간)");
        }

        //골드
        rewards[0].SetText(Utils.ConvertBigNum(reward.gold));
        //jade
        rewards[1].SetText(Utils.ConvertBigNum(reward.jade));
        //growthstone
        rewards[2].SetText(Utils.ConvertBigNum(reward.GrowthStone));

        //여우구슬
        rewards[3].SetText(Utils.ConvertBigNum(reward.marble));

        //exp
        rewards[4].SetText(Utils.ConvertBigNum(reward.exp));

        //요괴구슬
        rewards[5].SetText(Utils.ConvertBigNum(reward.yoguiMarble));
        //이벤트
        rewards[6].SetText(Utils.ConvertBigNum(reward.eventItem));

        //스테이지relic
        rewards[7].SetText(Utils.ConvertBigNum(reward.stageRelic));

        //설날
        rewards[8].SetText(Utils.ConvertBigNum(reward.sulItem));

        SleepRewardReceiver.Instance.GetRewardSuccess();
    }
}
