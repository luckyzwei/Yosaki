using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class UiCostume : SingletonMono<UiCostume>
{
    [SerializeField]
    private UiCostumeCell costumeCellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UiCostumeSlotView costumeSlotViewPrefab;

    [SerializeField]
    private Transform costumeSlotParent;

    private List<UiCostumeCell> uiCostumeCells = new List<UiCostumeCell>();
    private List<UiCostumeSlotView> uiCostumeSlotCells = new List<UiCostumeSlotView>();

    [SerializeField]
    private UiCostumeAbilityBoard uiCostumeAbilityBoard;

    void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        uiCostumeCells.Clear();

        var tableData = TableManager.Instance.Costume.dataArray;

        int currentSelectedIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            var costumeLookCell = Instantiate<UiCostumeCell>(costumeCellPrefab, cellParent);
            costumeLookCell.Initialize(tableData[i]);
            uiCostumeCells.Add(costumeLookCell);

            var costumeSlotCell = Instantiate<UiCostumeSlotView>(costumeSlotViewPrefab, costumeSlotParent);
            costumeSlotCell.Initialize(tableData[i], WhenCurrentSelectChanged);
            uiCostumeSlotCells.Add(costumeSlotCell);

            //디폴트 선택
            if (tableData[i].Id == currentSelectedIdx)
            {
                uiCostumeAbilityBoard.Initialize(tableData[i]);
            }
        }

        WhenCurrentSelectChanged(currentSelectedIdx);
    }

    private void WhenCurrentSelectChanged(int idx)
    {
        for (int i = 0; i < uiCostumeSlotCells.Count; i++)
        {
            uiCostumeSlotCells[i].SetCurrentSelect(idx == i);
        }
    }
}
