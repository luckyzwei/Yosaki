using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChunAbilMarkDescriptionBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameDescription;

    [SerializeField]
    private TextMeshProUGUI hasAbilDescription;

    [SerializeField]
    private TextMeshProUGUI specialAbilDescription;

    [SerializeField]
    private GameObject descriptionObject;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private GameObject commingSoonObject;

    [SerializeField]
    private TextMeshProUGUI specialLockObject;

    private void Start()
    {
        Initialize(0);
    }

    public void Initialize(int idx)
    {
        var tableData = TableManager.Instance.chunMarkAbil.dataArray[idx];

        nameDescription.SetText(tableData.Name);

        commingSoonObject.SetActive(tableData.Islock);

        descriptionObject.SetActive(!tableData.Islock);

        specialAbilDescription.SetText(tableData.Specialabildescription);

        hasAbilDescription.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical4DamPer)}피해량 {tableData.Abilbasevalue * 100}% 증가");

        var goods = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;

        float needAmount = tableData.Requirespeicalabilflower;

        specialLockObject.SetText(tableData.Requirespeicalabilflower < goods ? $"비활성화\n({CommonString.GetItemName(Item_Type.Cw)}{Utils.ConvertBigNum(needAmount)}이상 필요)" : "활성화됨");
    }

}
