using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiWingView : MonoBehaviour
{
    [SerializeField]
    private Image wingIcon;

    [SerializeField]
    private TextMeshProUGUI wingAbilityDesc;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    public void Initialize(int tableIdx)
    {
        if (tableIdx != -1 && tableIdx != TableManager.Instance.WingTable.dataArray.Length)
        {
            this.gameObject.SetActive(true);

            WingTableData tableData = TableManager.Instance.WingTable.dataArray[tableIdx];

            string abilDesc = null;

            gradeText.SetText($"{tableIdx + 1}단계 날개");

            for (int i = 0; i < tableData.Abilitytype.Length; i++)
            {
                StatusType statusType = (StatusType)tableData.Abilitytype[i];

                string abilValue = statusType.IsPercentStat() ? $"{(tableData.Abilityvalue[i] * 100f)}%" : $"{(tableData.Abilityvalue[i])}";

                abilDesc += $"{CommonString.GetStatusName(statusType)} {abilValue}";

                if (i != tableData.Abilitytype.Length - 1)
                {
                    abilDesc += "\n";
                }
            }

            wingAbilityDesc.SetText(abilDesc);
        }
        //없을때
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
