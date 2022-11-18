using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMonthPassPopup : MonoBehaviour
{

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_Free_Month_1;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupViewAd_Month_1;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_Free_Month_2;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupViewAd_Month_2;


    [SerializeField]
    private GameObject monthPass1;

    [SerializeField]
    private GameObject monthPass2;

    
    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        uiBuffPopupView_Free_Month_1.Initalize(tableDatas[21]);

        uiBuffPopupViewAd_Month_1.Initalize(tableDatas[22]);

        uiBuffPopupView_Free_Month_2.Initalize(tableDatas[17]);

        uiBuffPopupViewAd_Month_2.Initalize(tableDatas[18]);
    }

    private void OnEnable()
    {
        monthPass1.SetActive(ServerData.userInfoTable.IsMonthlyPass2() == false);
        monthPass2.SetActive(ServerData.userInfoTable.IsMonthlyPass2() == true);
    }

    private void Start()
    {
        Initialize();
    }

    private void CostumeReGetRoutine() 
    {
        
    }

}
