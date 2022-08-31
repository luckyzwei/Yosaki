using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOneYearEventBoard : MonoBehaviour
{
    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_OneYear;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.OneYear)
            {
                uiBuffPopupView_OneYear.Initalize(tableDatas[i]);
            }
        }
    }

    private void OnEnable()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;

        if (serverTime.Month == 9 && serverTime.Day > 8)
        {
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다.");
            this.gameObject.SetActive(false);
        }
    }
}
