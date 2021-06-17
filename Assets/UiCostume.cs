using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCostume : SingletonMono<UiCostume>
{
    [SerializeField]
    private UiCostumeCell costumeCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiCostumeCell> uiCostumeCells = new List<UiCostumeCell>();

    void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        uiCostumeCells.Clear();

        var tableData = TableManager.Instance.Costume.dataArray;

        int currentSelectedIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Costume].Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            var costumeCell = Instantiate<UiCostumeCell>(costumeCellPrefab, cellParent);
            costumeCell.Initialize(tableData[i]);
            uiCostumeCells.Add(costumeCell);

            //디폴트 선택
            if (tableData[i].Id == currentSelectedIdx)
            {
                UiCostumeAbilityBoard.Instance.Initialize(tableData[i]);
            }
        }

        WhenSelectIdxChanged(currentSelectedIdx);
    }

    public void WhenSelectIdxChanged(int idx)
    {
        for (int i = 0; i < uiCostumeCells.Count; i++)
        {
            uiCostumeCells[i].ShowSelectFrame(i == idx);
        }
    }
}
