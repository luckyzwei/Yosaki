using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class InfiniteTowerEnterView : MonoBehaviour
{
    [SerializeField]
    private DungeonRewardView dungeonRewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private GameObject normalRoot;

    [SerializeField]
    private GameObject allClearRoot;

    void Start()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value;

        return currentFloor >= TableManager.Instance.TowerTableData.Count;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value;
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

        if (isAllClear == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value;

            if (TableManager.Instance.TowerTableData.TryGetValue(currentFloor, out var towerTableData) == false)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {towerTableData}", null);
                return;
            }

            List<RewardData> rewardDatas = new List<RewardData>();

            var rewardData = new RewardData((Item_Type)towerTableData.Rewardtype, (int)towerTableData.Rewardvalue);

            rewardDatas.Add(rewardData);

            dungeonRewardView.Initalize(rewardDatas);
        }


    }
}
