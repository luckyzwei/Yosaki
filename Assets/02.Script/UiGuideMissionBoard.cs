using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class UiGuideMissionBoard : MonoBehaviour
{
    [SerializeField]
    private UiGuideMissionCell missionCell;

    [SerializeField]
    private GameObject allClearMask;

    [SerializeField]
    private Transform cellParent;

    private Dictionary<int, UiGuideMissionCell> cellContainer = new Dictionary<int, UiGuideMissionCell>();

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.GuideMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiGuideMissionCell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }

        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.GuideMissionReward].AsObservable().Subscribe(e =>
        {
            var tabledata = TableManager.Instance.GuideMission.dataArray.Length;
            bool lastMissionRewarded = ServerData.etcServerTable.GuideMissionRewarded(tabledata - 1);

            allClearMask.SetActive(lastMissionRewarded);

        }).AddTo(this);
    }
}
