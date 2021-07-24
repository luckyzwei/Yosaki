using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMarbleLockMask : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lockMaskDesc;

    private void OnEnable()
    {
        //lockMaskDesc.SetText($"{GameBalance.marbleUnlockLevel}레벨에 해방");

        //int currentLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        //this.gameObject.SetActive(currentLevel < GameBalance.marbleUnlockLevel);
    }
}
