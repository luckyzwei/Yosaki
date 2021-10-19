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

        desc += $"경험치 +{GameBalance.HotTime_Exp * 100f}%\n";
        desc += $"골드 +{GameBalance.HotTime_Gold * 100f}%\n";
        desc += $"수련의돌 +{GameBalance.HotTime_GrowthStone * 100f}%";

        description.SetText(desc);

        string timeDesc = string.Empty;

        timeDesc += "핫타임 진행중!\n";
        timeDesc += $"({GameBalance.HotTime_Start - 12}시~{GameBalance.HotTime_End - 12}시)";


        timeDescription.SetText(timeDesc);
    }

}
