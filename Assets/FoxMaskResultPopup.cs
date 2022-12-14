using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class FoxMaskResultPopup : MonoBehaviour
{
    //[SerializeField]
    //private GameObject failObject;
    //[SerializeField]
    ////private GameObject successObject;

    //[SerializeField]
    //private GameObject deadObject;

    [SerializeField]
    private TextMeshProUGUI stageChangeText;
    [SerializeField]
    private GameObject stageChangeButton;

    [SerializeField]
    private TextMeshProUGUI resultText;

    public void Initialize(ContentsState state)
    {
        resultText.SetText(GetTitleText(state));
        NextStageButtonTextChange(state);
        //successObject.SetActive(state == ContentsState.Clear);
        //failObject.SetActive(state != ContentsState.Clear);

        if (state == ContentsState.Dead)
        {
            PopupManager.Instance.ShowAlarmMessage("캐릭터가 사망했습니다..");
        }
        // deadObject.SetActive(state == ContentsState.Dead);
    }
    private void NextStageButtonTextChange(ContentsState contentsState)
    {
        switch (contentsState)
        {
            case ContentsState.Dead:
                stageChangeText.SetText("재도전");
                break;
            case ContentsState.TimerEnd:
                stageChangeText.SetText("재도전");
                break;
            case ContentsState.Clear:
                stageChangeText.SetText("다음 스테이지");
                break;
        }
    }
    private string GetTitleText(ContentsState contentsState)
    {
        switch (contentsState)
        {
            case ContentsState.Dead:
                return "실패!";

            case ContentsState.TimerEnd:
                return "시간초과!";

            case ContentsState.Clear:
                if (GameManager.contentsType == GameManager.ContentsType.FoxMask)
                {
                    if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxMask).Value >= (TableManager.Instance.FoxMask.dataArray.Length))
                    {
                        if (stageChangeButton != null)
                        {
                            stageChangeButton.SetActive(false);
                        }
                    }
                }
                return "클리어!!";
        }

        return "미등록";
    }
}
