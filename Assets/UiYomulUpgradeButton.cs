using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYomulUpgradeButton : MonoBehaviour
{
    public void OnClickUpgradeButton()
    {
        PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다!");
    }
}
