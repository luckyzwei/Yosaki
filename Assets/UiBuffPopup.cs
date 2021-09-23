using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBuffPopup : MonoBehaviour
{
    [SerializeField]
    private UiBuffPopupView uiBuffPopupView;

    [SerializeField]
    private Transform buffViewParent;

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
            if (tableDatas[i].Isyomulabil == true) continue;

            var cell = Instantiate<UiBuffPopupView>(uiBuffPopupView, buffViewParent);

            cell.Initalize(tableDatas[i]);
        }
    }
}
