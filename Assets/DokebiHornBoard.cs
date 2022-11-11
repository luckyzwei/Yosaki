using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class DokebiHornBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UiHornView uiHornView;

    [SerializeField]
    private TextMeshProUGUI currentFloor;

    public void Start()
    {

        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.DokebiHorn.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiHornView>(uiHornView, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

    public void OnClickUpEquipButton()
    {
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.DokebiHornView, -1);
    }
}
