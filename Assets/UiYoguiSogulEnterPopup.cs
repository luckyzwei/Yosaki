using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiYoguiSogulEnterPopup : MonoBehaviour
{
    [SerializeField]
    private UiYoguiSogulRewardCell cellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI lastClearStageDesc;

    private List<UiYoguiSogulRewardCell> cellLists = new List<UiYoguiSogulRewardCell>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.YoguisogulTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            var cell = Instantiate<UiYoguiSogulRewardCell>(cellPrefab, cellParent);

            cell.Initialize(tableDatas[i]);

            cellLists.Add(cell);
        }

        lastClearStageDesc.SetText($"최고 단계 : {(int)(ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value)}");
    }

    public void OnClickEnterButton()
    {
        GameManager.Instance.LoadContents(GameManager.ContentsType.YoguiSoGul);
    }

    public void OnClickAllReceiveButton()
    {
        int rewardReceiveCount = 0;

        for (int i = 0; i < cellLists.Count; i++)
        {
            if (cellLists[i].CanGetReward())
            {
                rewardReceiveCount++;
                cellLists[i].OnClickGetButton();
            }
        }

        LogManager.Instance.SendLogType("Sogul", "Receive", rewardReceiveCount.ToString());
    }
}
