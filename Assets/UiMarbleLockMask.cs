using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiMarbleLockMask : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lockMaskDesc;

    private void Start()
    {
        lockMaskDesc.SetText($"{GameBalance.marbleUnlockLevel}레벨에 해방");

        Subscribe();
    }

    private void Subscribe() 
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(level=> 
        {
            this.gameObject.SetActive(level < GameBalance.marbleUnlockLevel);
        }).AddTo(this);
    }
}
