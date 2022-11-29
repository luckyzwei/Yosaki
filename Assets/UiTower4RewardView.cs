using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiTower4RewardView : MonoBehaviour
{
    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;


    [SerializeField]
    private TextMeshProUGUI stageDescription;

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private GameObject rewardedIcon;

    private int currentId;

    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        stageDescription.SetText($"{currentId + 1}Ãþ º¸»ó");

        var towerTableData = TableManager.Instance.towerTableMulti.dataArray[idx];

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)towerTableData.Rewardtype);

        rewardDescription.SetText($"{Utils.ConvertBigNum(towerTableData.Rewardvalue)}°³");

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

        currentId = Mathf.Min(currentId, TableManager.Instance.towerTableMulti.dataArray.Length - 1);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != TableManager.Instance.towerTableMulti.dataArray.Length - 1;

        int myCurrentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value;

        rewardedIcon.SetActive(currentId < myCurrentFloor);
    }
}
