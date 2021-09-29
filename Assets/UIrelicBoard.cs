using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIrelicBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UiRelicCell uiRelicCell;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.RelicTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiRelicCell>(uiRelicCell, cellParents);

            cell.Initialize(tableDatas[i]);
        }
    }
}
