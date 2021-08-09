using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentStageInfoPopup : SingletonMono<CurrentStageInfoPopup>
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject rootObject;

    public void ShowInfoPopup(bool show)
    {
        rootObject.SetActive(show);
    }

    private void Start()
    {
        ShowInfoPopup(false);

        SetDescription();
    }

    private void SetDescription()
    {
        var stageData = GameManager.Instance.CurrentStageData;
        var enemyTableData = TableManager.Instance.EnemyData[stageData.Monsterid1];

        string desc = string.Empty;

        desc += $"체력 : {Utils.ConvertBigNum(enemyTableData.Hp)}\n";
        desc += $"방어력 : {Utils.ConvertBigNum(enemyTableData.Defense)}\n";
        desc += $"공격력 : {Utils.ConvertBigNum(enemyTableData.Attackpower)}\n\n";
        desc += $"경험치 : {Utils.ConvertBigNum(enemyTableData.Exp)}\n";
        desc += $"{CommonString.GetItemName(Item_Type.Gold)} : {Utils.ConvertBigNum(enemyTableData.Attackpower * enemyTableData.Bossattackratio)}\n";
        desc += $"{CommonString.GetItemName(Item_Type.GrowthStone)} : {Utils.ConvertBigNum(stageData.Magicstoneamount)}\n";
        desc += $"{CommonString.GetItemName(Item_Type.Marble)} : {Utils.ConvertBigNum(stageData.Marbleamount)}\n\n";
        desc += $"보스체력 : {Utils.ConvertBigNum(enemyTableData.Hp * enemyTableData.Bosshpratio)}\n";
        desc += $"보스공격력 : {Utils.ConvertBigNum(enemyTableData.Attackpower * enemyTableData.Bossattackratio)}";

        description.SetText(desc);
    }
}
