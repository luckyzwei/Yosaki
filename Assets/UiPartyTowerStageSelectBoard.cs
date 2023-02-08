using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPartyTowerStageSelectBoard : SingletonMono<UiPartyTowerStageSelectBoard>
{
    [SerializeField]
    private PartyTowerStageView partyTowerStageView;

    [SerializeField]
    private Transform cellParents;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.twoCave.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<PartyTowerStageView>(partyTowerStageView, cellParents);
            cell.Initialize(tableData[i]);
        }
    }
}
