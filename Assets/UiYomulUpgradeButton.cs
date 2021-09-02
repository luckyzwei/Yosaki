using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYomulUpgradeButton : MonoBehaviour
{
    public void OnClickUpgradeButton()
    {
        UiYoumulUpgradeBoard.Instance.ShowUpgradePopup(true);
    }
}
