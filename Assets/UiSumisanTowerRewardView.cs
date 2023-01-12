using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSumisanTowerRewardView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI rewardDescription;

    [SerializeField]
    private TextMeshProUGUI stageDescription;

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    private int currentId;

    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        stageDescription.SetText($"{currentId + 1}층 보상");

        var towerTableData = TableManager.Instance.sumisanTowerTable.dataArray[idx];

        string rewardDesc = string.Empty;

        rewardDesc += $"{CommonString.GetStatusName((StatusType)towerTableData.Rewardtype)} {towerTableData.Rewardvalue * 100}% 증가\n";

        rewardDesc += "(누적 적용)";

        rewardDescription.SetText(rewardDesc);

        UpdateButtonState();

    }

    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId, TableManager.Instance.sumisanTowerTable.dataArray.Length - 1);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != TableManager.Instance.sumisanTowerTable.dataArray.Length - 1;
    }
}
