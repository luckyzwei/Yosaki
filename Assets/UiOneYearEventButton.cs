using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOneYearEventButton : MonoBehaviour
{
    private void OnEnable()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;

        if (serverTime.Month == 9 && serverTime.Day > 8)
        {
            this.gameObject.SetActive(false);
        }
    }
}
