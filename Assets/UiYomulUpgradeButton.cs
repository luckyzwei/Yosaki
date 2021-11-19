using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYomulUpgradeButton : MonoBehaviour
{
    public void OnClickUpgradeButton()
    {
        UiYoumulUpgradeBoard.Instance.ShowUpgradePopup(true);
    }

    public void OnClickYachaUpgradeButton()
    {
        PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다.");
        return;
        UiYachaUpgradeBoard.Instance.ShowUpgradePopup(true);
    }
}
