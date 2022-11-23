using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEventMissionBoard : MonoBehaviour
{
    [SerializeField]
    private UiEventMissionCell missionCell;

    [SerializeField]
    private Transform cellParent;

    private Dictionary<int, UiEventMissionCell> cellContainer = new Dictionary<int, UiEventMissionCell>();

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_XMas).Value += 1000;
        }
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiEventMissionCell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
    }
}
