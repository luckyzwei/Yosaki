using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWinterPassPopup : MonoBehaviour
{

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_WinterPass_0;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_WinterPass_1;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        uiBuffPopupView_WinterPass_0.Initalize(tableDatas[21]);

        uiBuffPopupView_WinterPass_1.Initalize(tableDatas[22]); ;
    }

    private void OnEnable()
    {
       var serverTime = ServerData.userInfoTable.currentServerTime;
        
        if (serverTime.Year == 2023&&serverTime.Month >= 2  && serverTime.Day >= 1)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다.");
            return;
        }
    }

}
