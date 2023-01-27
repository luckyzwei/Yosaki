using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiFoxEquipLockMask : MonoBehaviour
{
    [SerializeField]
    private int unlockLevel;


    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(currentLevel =>
        {
            this.gameObject.SetActive(currentLevel < unlockLevel);
        }).AddTo(this);
    }
}
