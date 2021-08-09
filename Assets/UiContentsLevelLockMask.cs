using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiContentsLevelLockMask : MonoBehaviour
{
    [SerializeField]
    private int unlockLevel;

    [SerializeField]
    private TextMeshProUGUI levelDesc;

    void Start()
    {
        levelDesc.SetText($"{unlockLevel}레벨에 해금!");

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(currentLevel =>
        {
            this.gameObject.SetActive(currentLevel < unlockLevel);
        }).AddTo(this);
    }

}
