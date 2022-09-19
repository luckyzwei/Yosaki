using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSkillCraftBoard : MonoBehaviour
{
    [SerializeField]
    private UiMagicbookCraftCell craftCell;

    [SerializeField]
    private Transform craftParent;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var skillTableDatas = TableManager.Instance.SkillTable.dataArray;

        for (int i = 0; i < skillTableDatas.Length; i++)
        {
            if (skillTableDatas[i].Issonskill == true) continue;
            if (skillTableDatas[i].Skilltype == 4 || skillTableDatas[i].Skilltype == 5 || skillTableDatas[i].Skilltype == 6) continue;

            var cell = Instantiate<UiMagicbookCraftCell>(craftCell, craftParent);

            cell.Initialize(skillTableDatas[i]);
        }

    }

}
