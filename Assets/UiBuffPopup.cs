﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBuffPopup : MonoBehaviour
{
    [SerializeField]
    private UiBuffPopupView uiBuffPopupView;



    [SerializeField]
    private Transform buffViewParent;

    [SerializeField]
    private List<UiBuffPopupView> monthBuff = new List<UiBuffPopupView>();

    [SerializeField]
    private List<UiBuffPopupView> coldBuff = new List<UiBuffPopupView>();

    [SerializeField]
    private List<UiBuffPopupView> winterBuff = new List<UiBuffPopupView>();

    private List<UiBuffPopupView> allBuffList = new List<UiBuffPopupView>();



    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Buffseconds < 0) continue;
            if (tableDatas[i].Isactive == false) continue;
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Yomul) continue;
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.OneYear) continue;
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Chuseok) continue;

            var cell = Instantiate<UiBuffPopupView>(uiBuffPopupView, buffViewParent);

            cell.Initalize(tableDatas[i]);

            allBuffList.Add(cell);

            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Month)
            {
                monthBuff.Add(cell);
            }
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Cold)
            {
                coldBuff.Add(cell);
            }
            //if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Winter)
            //{
            //    winterBuff.Add(cell);
            //}


        }

        RefreshMonthBuff();
    }

    public void OnClickAllUseButton()
    {
        PopupManager.Instance.SetIgnoreAlarmMessage(true);

        allBuffList.ForEach(e => {
            if (e.gameObject.activeInHierarchy)
            {
                e.OnClickGetBuffButton();
            }
        });

        PopupManager.Instance.SetIgnoreAlarmMessage(false);

        PopupManager.Instance.ShowAlarmMessage("버프를 모두 사용했습니다.(광고 버프는 광고제거가 있어야 사용 됩니다.)");
   
    }

    private void OnEnable()
    {
        RefreshMonthBuff();
    }

    private void RefreshMonthBuff()
    {
        for (int i = 0; i < monthBuff.Count; i++)
        {
            // monthBuff[i].gameObject.SetActive(ServerData.userInfoTable.IsMonthlyPass2());
        }
    }
}
