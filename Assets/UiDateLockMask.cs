using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiDateLockMask : MonoBehaviour
{
    [SerializeField]
    private int year;
    [SerializeField]
    private int month;
    [SerializeField]
    private int day;
    private void Start()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;
        
        if (serverTime.Year> year|| serverTime.Month>month||(serverTime.Month == month && serverTime.Day > day))
        {
            this.gameObject.SetActive(true);
            return;
        }
        
    }

}
