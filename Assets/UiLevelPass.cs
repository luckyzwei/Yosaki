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

    private float cellSize = 105.9f;

    private List<UiLevelPassCell> uiPassCellContainer = new List<UiLevelPassCell>();

    private bool initialized = false;

    private int scrollId = 0;

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
                passInfo.passGrade = tableData[i].Passgrade;

                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }

        initialized = true;

        //StartCoroutine(scrollRoutine());
    }

    //private void OnEnable()
    //{
    //    if (initialized)
    //    {
    //        StartCoroutine(scrollRoutine());
    //    }
    //}

    //private IEnumerator scrollRoutine()
    //{

    //    yield return null;
    //    yield return null;
    //    yield return null;

    //    var tableData = TableManager.Instance.LevelPass.dataArray;

    //    int currentLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

    //    for (int i = 0; i < tableData.Length; i++)
    //    {
    //        if (currentLevel >= tableData[i].Unlocklevel)
    //        {
    //            scrollId = i;
    //        }
    //    }

    //    float scrollPosY = scrollId * cellSize;

    //    var rc = cellParent.GetComponent<RectTransform>();

    //    rc.anchoredPosition = new Vector2(rc.anchoredPosition.x, scrollPosY);
    //}
}
