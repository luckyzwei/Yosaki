using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDailyMissionBoard : MonoBehaviour
{
    [SerializeField]
    private UiDailyMissionCell missionCell;

    [SerializeField]
    private Transform cellParent;

    private Dictionary<int, UiDailyMissionCell> cellContainer = new Dictionary<int, UiDailyMissionCell>();

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.DailyMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiDailyMissionCell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
    }
}
