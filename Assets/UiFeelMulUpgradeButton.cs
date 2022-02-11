using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFeelMulUpgradeButton : MonoBehaviour
{
    public void OnClickYachaUpgradeButton()
    {
        UiFeelMulUpgradeBoard.Instance.ShowUpgradePopup(true);
    }
}
