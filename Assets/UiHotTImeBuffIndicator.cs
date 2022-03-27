using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiHotTImeBuffIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI timeDescription;

    void Start()
    {
        SetDescription();
    }

    private void SetDescription()
    {
        string desc = string.Empty;

        desc += $"<color=yellow>평일 19~22시\n";
        desc += $"주말 18~22시</color>\n";
        desc += $"경험치 +{GameBalance.HotTime_Exp * 100f}%\n";
        desc += $"경험치 +{GameBalance.HotTime_Exp * 100f}%\n";
        desc += $"골드 +{GameBalance.HotTime_Gold * 100f}%\n";
        desc += $"수련의돌 +{GameBalance.HotTime_GrowthStone * 100f}%\n";
        desc += $"여우구슬 +{GameBalance.HotTime_Marble * 100f}%";

        description.SetText(desc);

        //string timeDesc = string.Empty;

        //timeDesc += "핫타임 진행중!";

        //timeDescription.SetText(timeDesc);
    }

}
