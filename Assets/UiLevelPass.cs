using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UiLevelPass : MonoBehaviour
{
    [SerializeField]
    private UiLevelPassCell levelPassCell;

    [SerializeField]
    private Transform cellParent;

    private List<UiLevelPassCell> uiPassCellContainer = new List<UiLevelPassCell>();
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.LevelPass.dataArray;

        int interval = tableData.Length - uiPassCellContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var prefab = Instantiate<UiLevelPassCell>(levelPassCell, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();

                passInfo.require = tableData[i].Unlocklevel;
                passInfo.id = tableData[i].Id;

                passInfo.rewardType_Free = tableData[i].Reward1_Free;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = NewLevelPass.freeReward;

                passInfo.rewardType_IAP = tableData[i].Reward2_Pass;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = NewLevelPass.premiumReward;

                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }
    }
}
