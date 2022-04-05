using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiGuilaAttenBoard : MonoBehaviour
{
    [SerializeField]
    private List<int> rewardCut;
    [SerializeField]
    private List<int> rewardType;
    [SerializeField]
    private List<float> rewardValue;

    [SerializeField]
    private TextMeshProUGUI attenUserNumText;

    [SerializeField]
    private List<UiGuildAttenRewardView> rewardViews;

    private int todayDonatedUserNum;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Initialize()
    {
        for (int i = 0; i < rewardViews.Count; i++)
        {
            rewardViews[i].Initialize(rewardCut[i], i, rewardType[i], rewardValue[i]);
        }
    }

    private void Subscribe()
    {
        UiGuildMemberList.Instance.attenUserNum.AsObservable().Subscribe(e =>
        {
            attenUserNumText.SetText($"점수 등록한 문파원 수 : {e}");


        }).AddTo(this);
    }

    public void OnClickRefreshButton()
    {
        UiGuildMemberList.Instance.RefreshMemberList();
    }

    public void OnClickAllReceiveButton()
    {
        bool rewarded = false;

        for (int i = 0; i < rewardViews.Count; i++)
        {
            rewarded |= rewardViews[i].GetReward(false);
        }

        if (rewarded)
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다.");
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 전부 받았습니다.");
        }
    }
}
