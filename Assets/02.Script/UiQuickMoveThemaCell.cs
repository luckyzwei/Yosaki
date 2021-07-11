using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiQuickMoveThemaCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stageName;

    private int mapIdx;

    [SerializeField]
    private GameObject notClearMask;

    public void Initialize(int mapIdx)
    {
        if (mapIdx == 0)
        {
            stageName.SetText($"{CommonString.ThemaName[0]}");
        }
        else
        {
            int pref = (mapIdx - 1) / 6 + 1;
            int def = (mapIdx - 1) % 6 + 1;

            stageName.SetText($"{pref}-{def}");
        }

        this.mapIdx = mapIdx;

        int lastClearStageId = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;
        notClearMask.SetActive(this.mapIdx > lastClearStageId + 1); 
    }

    public void OnClickButton()
    {
        int lastClearStageId = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (this.mapIdx > lastClearStageId + 1)
        {
            PopupManager.Instance.ShowAlarmMessage("현재 스테이지를 클리어 하지 못했습니다.");
            return;
        }

        GameManager.Instance.MoveMapByIdx(this.mapIdx);
    }
}
