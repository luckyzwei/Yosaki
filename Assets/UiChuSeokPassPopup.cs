using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChuSeokPassPopup : MonoBehaviour
{
    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_OneYear;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_OneYear_2;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        uiBuffPopupView_OneYear.Initalize(tableDatas[17]);

        uiBuffPopupView_OneYear_2.Initalize(tableDatas[18]);
    }

    private void OnEnable()
    {
        var severTime = ServerData.userInfoTable.currentServerTime;

        if (severTime.Month >= 10 && severTime.Day > 3)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다.");
            return;
        }
    }
}
