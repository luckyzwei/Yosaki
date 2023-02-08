using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiFourSkillBoard : SingletonMono<UiSkillBoard>
{
    [SerializeField]
    private UiFourSkillCell uiFourSkillCell;

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
            if (tableData[i].SKILLCASTTYPE != SkillCastType.Four) continue;

            var cell = Instantiate<UiFourSkillCell>(uiFourSkillCell, skillCellParent);

            cell.Initialize(tableData[i]);
        }
    }
}
