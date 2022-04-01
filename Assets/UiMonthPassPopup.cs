using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMonthPassPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject monthPass1;

    [SerializeField]
    private GameObject monthPass2;

    private void OnEnable()
    {
        monthPass1.SetActive(ServerData.userInfoTable.IsMonthlyPass2() == false);
        monthPass2.SetActive(ServerData.userInfoTable.IsMonthlyPass2() == true);
    }

    private void Start()
    {
        
    }

    private void CostumeReGetRoutine() 
    {
        
    }

}
