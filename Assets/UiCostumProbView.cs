using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class UiCostumProbView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI probText;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        string desc = null;

        float total = TableManager.Instance.CostumeAbility.dataArray.Select(e => e.Prob).ToList().Sum();

        var tableDatas = TableManager.Instance.CostumeAbility.dataArray.ToList();

        //tableDatas.Sort((a, b) =>
        //{
        //    if (a.Grade < b.Grade) return -1;
        //    return 1;

        //});

        for (int i = 0; i < tableDatas.Count; i++)
        {
            if (tableDatas[i].Grade == 0) continue;
            desc += $"{GetGradeColor(tableDatas[i].Grade)}{tableDatas[i].Description} ({((tableDatas[i].Prob / total) * 100f).ToString("F2")}%)</color>\n";
        }

        probText.SetText(desc);
    }

    private string GetGradeColor(int grade)
    {
        switch (grade)
        {
            case 1:
            case 2:
            case 3:
                {
                    return "<color=#ffffffff>";
                }
                break;
            case 4:
                {
                    return "<color=yellow>";
                }
                break;
            case 5:
                {
                    return "<color=#ff0000ff>";
                }
                break;
        }
        return string.Empty;
    }
}
