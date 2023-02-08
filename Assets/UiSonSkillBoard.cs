using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiSonSkillBoard : SingletonMono<UiSkillBoard>
{
    [SerializeField]
    private UiSonSkillCell uiSonSkillCell;

    [SerializeField]
    private Transform skillCellParent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.SkillTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].SKILLCASTTYPE != SkillCastType.Son) continue;

            var cell = Instantiate<UiSonSkillCell>(uiSonSkillCell, skillCellParent);

            cell.Initialize(tableData[i]);
        }
    }
}
