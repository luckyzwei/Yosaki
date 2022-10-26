using System.Collections;
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
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Yomul) continue;
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.OneYear) continue;
            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Chuseok) continue;


            var cell = Instantiate<UiBuffPopupView>(uiBuffPopupView, buffViewParent);
            cell.Initalize(tableDatas[i]);

            if (tableDatas[i].BUFFTYPEENUM == BuffTypeEnum.Month)
            {
                monthBuff.Add(cell);
            }
        }

        RefreshMonthBuff();
    }

    private void OnEnable()
    {
        RefreshMonthBuff();
    }

    private void RefreshMonthBuff()
    {
        for (int i = 0; i < monthBuff.Count; i++)
        {
            monthBuff[i].gameObject.SetActive(ServerData.userInfoTable.IsMonthlyPass2());
        }
    }
}
