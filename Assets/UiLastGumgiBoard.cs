using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiLastGumgiBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI abil0Description;
    
    [SerializeField]
    private TextMeshProUGUI abil1Description;

    [SerializeField]
    private TextMeshProUGUI currentAwakeLevel;

    [SerializeField]
    private Image gumgiIcon;


    private void OnEnable()
    {
        UpdateUi();
    }
    private void UpdateUi()
    {
        float currentGumgi = ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value;

        if (currentGumgi < TableManager.Instance.gumGiTable.dataArray[200].Require) return;


        var tableData = TableManager.Instance.gumGiTable.dataArray[200];

        float over200 = Mathf.Max(0, ((int)ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value - TableManager.Instance.gumGiTable.dataArray[200].Require) / 50000);
        float gumgiDamage = over200 * GameBalance.gumgiAttackValue200;
        float gumgiDefense = over200 * GameBalance.gumgiDefenseValue200;

        currentAwakeLevel.SetText($"현재 검기 : {over200+200}단계");
        //gumgiIcon.sprite = CommonResourceContainer.GetDragonBallSprite(currentIdx);

        abil0Description.SetText($"{CommonString.GetStatusName((StatusType)tableData.Abiltype)} {Utils.ConvertBigNum(TableManager.Instance.gumGiTable.dataArray[200].Abilvalue+ gumgiDamage)}");

        abil1Description.SetText($"{CommonString.GetStatusName((StatusType)tableData.Abiltype2)} {Utils.ConvertBigNum(TableManager.Instance.gumGiTable.dataArray[200].Abilvalue2+gumgiDefense)}");

        //gradeText.SetText($"{currentIdx + 1}단계");
    }
}
