using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;

public class UiExpGauge : MonoBehaviour
{
    [SerializeField]
    private Image gauge;
    [SerializeField]
    private TextMeshProUGUI gaugeText;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        //GrowthManager.Instance.WhenPlayerLevelUp.AsObservable().Subscribe(e=> 
        //{
        //    WhenGrowthValueChanged();
        //}).AddTo(this);

        DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).AsObservable().Subscribe(e =>
        {
            WhenGrowthValueChanged();
        }).AddTo(this);
    }

    private void WhenGrowthValueChanged()
    {
        Initialize(DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value, GrowthManager.Instance.maxExp.Value);
    }

    public void Initialize(float currentExp, float maxExp)
    {
        gauge.fillAmount = currentExp / maxExp;

        gaugeText.SetText($"{(int)currentExp}/{(int)maxExp}({(int)(currentExp / maxExp *100f)}%)");
    }

}
