using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPassCellCreater : MonoBehaviour
{
    [SerializeField]
    private UiLevelPassCell levelPassCell;

    [SerializeField]
    private Transform cellParent;

    private float cellSize = 105.9f;


    private bool initialized = false;

    private int scrollId = 0;

    [SerializeField]
    private int GradeId = 0;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.LevelPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Passgrade != GradeId) continue;

            var prefab = Instantiate<UiLevelPassCell>(levelPassCell, cellParent);

            var passInfo = new PassInfo();

            passInfo.require = tableData[i].Unlocklevel;
            passInfo.id = tableData[i].Id;

            passInfo.rewardType_Free = tableData[i].Reward1_Free;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = NewLevelPass.freeReward;

            passInfo.rewardType_IAP = tableData[i].Reward2_Pass;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = NewLevelPass.premiumReward;
            passInfo.passGrade = tableData[i].Passgrade;

            prefab.Initialize(passInfo);
        }
    }
}
