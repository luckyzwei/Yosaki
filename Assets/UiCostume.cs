using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        int currentSelectedIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            var costumeLookCell = Instantiate<UiCostumeCell>(costumeCellPrefab, cellParent);
            costumeLookCell.Initialize(tableData[i]);
            uiCostumeCells.Add(costumeLookCell);

            var costumeSlotCell = Instantiate<UiCostumeSlotView>(costumeSlotViewPrefab, costumeSlotParent);
            costumeSlotCell.Initialize(tableData[i]);

            //디폴트 선택
            if (tableData[i].Id == currentSelectedIdx)
            {
                uiCostumeAbilityBoard.Initialize(tableData[i]);
            }
        }
    }
}
