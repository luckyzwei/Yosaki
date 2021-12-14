using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;

public class UiExpGauge : SingletonMono<UiExpGauge>
{
    [SerializeField]
    private Image gauge;
    [SerializeField]
    private TextMeshProUGUI gaugeText;

    private void Start()
    {
        WhenGrowthValueChanged();
    }

    public void WhenGrowthValueChanged()
    {
        Initialize(ServerData.growthTable.GetTableData(GrowthTable.Exp).Value, GrowthManager.Instance.maxExp.Value);
    }

    public void Initialize(float currentExp, float maxExp)
    {
        gauge.fillAmount = currentExp / maxExp;

        gaugeText.SetText($"{Utils.ConvertBigNum(currentExp)}/{Utils.ConvertBigNum(maxExp)}({(int)(currentExp / maxExp * 100f)}%)");
    }

}
