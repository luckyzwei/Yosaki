using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMagicBookCollectBoard : MonoBehaviour
{
    [SerializeField]
    private UiMagicBookCollectCell cellPrefab;
    [SerializeField]
    private Transform cellParent;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.SkillTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Issonskill == true) continue;
            if (tableData[i].Skilltype == 4|| tableData[i].Skilltype == 5 || tableData[i].Skilltype == 6 || tableData[i].Skilltype == 7) continue;
            var cell = Instantiate<UiMagicBookCollectCell>(cellPrefab, cellParent);
            cell.Initialize(tableData[i]);
        }
    }
}
