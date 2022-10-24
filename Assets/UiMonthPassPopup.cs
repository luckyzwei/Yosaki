using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMonthPassPopup : MonoBehaviour
{

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_Month;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_Month_2;


    [SerializeField]
    private GameObject monthPass1;

    [SerializeField]
    private GameObject monthPass2;

    
    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        uiBuffPopupView_Month.Initalize(tableDatas[17]);

        uiBuffPopupView_Month_2.Initalize(tableDatas[18]);
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
