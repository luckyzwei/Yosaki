using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWeaponCraftButton : MonoBehaviour
{
    public void OnClickCraftButton()
    {
        WeaponData yomulData = TableManager.Instance.WeaponTable.dataArray[20];

        if (ServerData.weaponTable.TableDatas[yomulData.Stringid].hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("요물은 한개만 제작 가능합니다.");
            return;
        }

        UiWeaponCraftBoard.Instance.ShowPopup();
    }
}
