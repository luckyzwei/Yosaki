using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiHellAbilCell : MonoBehaviour
{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI name;

    [SerializeField]
    private TextMeshProUGUI description;


    private HellAbilData tableData;

    public void Initialize(HellAbilData tableData)
    {
        this.tableData = tableData;

        icon.sprite = CommonResourceContainer.GetHellIconSprite(tableData.Id);
        this.name.SetText(tableData.Name);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Hel).AsObservable().Subscribe(e =>
        {
            StatusType type = (StatusType)(tableData.Abiltype);

            description.SetText($"{CommonString.GetStatusName(type)}");

        }).AddTo(this);
    }
}
