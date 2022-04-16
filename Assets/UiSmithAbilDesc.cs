using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSmithAbilDesc : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {

        return;

        var tableData = TableManager.Instance.smithTable.dataArray;

        string desc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].STATUSTYPE == StatusType.growthStoneUp)
            {
                desc += $"<color=#ff00ffff>LV:{tableData[i].Require} {CommonString.GetStatusName(tableData[i].STATUSTYPE)} {tableData[i].Value}개\n";
            }
            else if (tableData[i].STATUSTYPE == StatusType.WeaponHasUp)
            {
                desc += $"<color=red>LV:{tableData[i].Require} {CommonString.GetStatusName(tableData[i].STATUSTYPE)} {tableData[i].Value}배\n";
            }
            else if (tableData[i].STATUSTYPE == StatusType.NorigaeHasUp)
            {
                desc += $"<color=blue>LV:{tableData[i].Require} {CommonString.GetStatusName(tableData[i].STATUSTYPE)} {tableData[i].Value}배\n";
            }
            else if (tableData[i].STATUSTYPE == StatusType.PetEquipHasUp)
            {
                desc += $"<color=yellow>LV:{tableData[i].Require} {CommonString.GetStatusName(tableData[i].STATUSTYPE)} {tableData[i].Value}배\n";
            }
            else if (tableData[i].STATUSTYPE == StatusType.PetEquipProbUp)
            {
                desc += $"<color=#00ffffff>LV:{tableData[i].Require} {CommonString.GetStatusName(tableData[i].STATUSTYPE)} {tableData[i].Value}%\n";
            }
        }

        description.SetText(desc);
    }

}
