using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTitleBoard : MonoBehaviour
{
    [SerializeField]
    private UiTitleCell titleCellPrefab;

    [SerializeField]
    private List<Transform> cellParents;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.TitleTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiTitleCell>(titleCellPrefab, cellParents[tableDatas[i].Displaygroup]);
            cell.Initialize(tableDatas[i]);
        }
    }
}
