using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiSmithTreeLockMask : MonoBehaviour
{
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        var level = ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        

        if(level>300000)
        {
            this.gameObject.SetActive(false);
        }
    }

}
