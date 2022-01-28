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

    public void OnClickYachaCraftButton()
    {
        WeaponData yomulData = TableManager.Instance.WeaponTable.dataArray[21];

        if (ServerData.weaponTable.TableDatas[yomulData.Stringid].hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다.");
            return;
        }

        UiYachaCraftBoard.Instance.ShowPopup();
    }

    public void OnClickFeelMulCraftButton()
    {
        WeaponData yomulData = TableManager.Instance.WeaponTable.dataArray[22];

        if (ServerData.weaponTable.TableDatas[yomulData.Stringid].hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다.");
            return;
        }

        UiFeelMulCraftBoard.Instance.ShowPopup();
    }
}
