using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class FoxMaskResultPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject failObject;
    [SerializeField]
    private GameObject successObject;

    [SerializeField]
    private GameObject deadObject;

    public void Initialize(ContentsState state)
    {
        successObject.SetActive(state == ContentsState.Clear);
        failObject.SetActive(state != ContentsState.Clear);

        if (state == ContentsState.Dead)
        {
            PopupManager.Instance.ShowAlarmMessage("캐릭터가 사망했습니다..");
        }
        // deadObject.SetActive(state == ContentsState.Dead);
    }

    private string GetTitleText(ContentsState contentsState)
    {
        switch (contentsState)
        {
            case ContentsState.Dead:
                return "실패!";
                break;
            case ContentsState.TimerEnd:
                return "시간초과!";
                break;
            case ContentsState.Clear:
                return "클리어!!";
                break;
        }

        return "미등록";
    }
}
