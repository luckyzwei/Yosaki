using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class InfiniteTowerEnterView2 : MonoBehaviour
{
    [SerializeField]
    private DungeonRewardView dungeonRewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private GameObject normalRoot;

    [SerializeField]
    private GameObject allClearRoot;

    [SerializeField]
    private GameObject startButtonRoot;

    public void OnAutoToggleChanged(bool onOff)
    {
       // UiLastContentsFunc.AutoInfiniteTower2 = onOff;
    }

    void OnEnable()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value;

        return currentFloor >= TableManager.Instance.TowerTableData2.Count;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value;
            currentStageText.SetText($"{currentFloor + 1}층 도전");
        }
        else
        {
            currentStageText.SetText($"아직 발견되지 않은 구역 입니다.");
        }

    }

    private void SetReward()
    {
        bool isAllClear = IsAllClear();

        normalRoot.SetActive(isAllClear == false);
        allClearRoot.SetActive(isAllClear == true);
        startButtonRoot.SetActive(isAllClear == false);

        if (isAllClear == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value;

            if (TableManager.Instance.TowerTableData2.TryGetValue(currentFloor, out var TowerTableData2) == false)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {TowerTableData2}", null);
                return;
            }

            List<RewardData> rewardDatas = new List<RewardData>();

            var rewardData = new RewardData((Item_Type)TowerTableData2.Rewardtype, TowerTableData2.Rewardvalue);

            rewardDatas.Add(rewardData);

            dungeonRewardView.Initalize(rewardDatas);
        }


    }
}
