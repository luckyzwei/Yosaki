using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPetEquipDragonBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI abil0Description;
    
    [SerializeField]
    private TextMeshProUGUI abil1Description;

    [SerializeField]
    private TextMeshProUGUI currentAwakeLevel;

    [SerializeField]
    private Image dragonIcon;


    private void OnEnable()
    {
        UpdateUi();
    }
    private void UpdateUi()
    {
        int currentIdx = PlayerStats.GetCurrentDragonIdx();

        if (currentIdx == -1) return;

        currentAwakeLevel.SetText($"현재 강화도 : + {ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value}강");

        var tableData = TableManager.Instance.dragonBall.dataArray[currentIdx];

        dragonIcon.sprite = CommonResourceContainer.GetDragonBallSprite(currentIdx);

        abil0Description.SetText($"{CommonString.GetStatusName((StatusType)tableData.Abiltype0)}\n{PlayerStats.GetDragonBallAbil0Value() * 100f}%");

        abil1Description.SetText($"{CommonString.GetStatusName((StatusType)tableData.Abiltype1)}\n{PlayerStats.GetDragonBallAbil1Value() * 100f}%");

        gradeText.SetText($"{currentIdx + 1}단계");
    }
}
