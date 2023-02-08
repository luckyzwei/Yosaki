using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiColdSeasonPassPopup : MonoBehaviour
{
    [SerializeField]
    private List<UiBuffPopupView> uiBuffPopupView_OneYear;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_OneYear_2;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;
        int count = uiBuffPopupView_OneYear.Count;
        for (int i = 0; i < count ; i++)
        {
            uiBuffPopupView_OneYear[i].Initalize(tableDatas[23 + i]);

        }


    }

    private void OnEnable()
    {
        var severTime = ServerData.userInfoTable.currentServerTime;
        
        if (severTime.Month >= 4)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다.");
            return;
        }
    }
}
