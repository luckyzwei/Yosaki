using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiOrochiAbilCell : MonoBehaviour
{
    [SerializeField]
    private ObscuredString key;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        Subscribe();

        SetDescription();
    }

    private void SetDescription()
    {
        if (key == PlayerStats.orochi0)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.OrochiTooth0)}</color>\n{CommonString.GetStatusName(StatusType.PenetrateDefense)}\n{PlayerStats.orochi0Value * 100f}");
        }
        else if (key == PlayerStats.orochi1)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.OrochiTooth1)}</color>\n{CommonString.GetStatusName(StatusType.PenetrateDefense)}\n{PlayerStats.orochi1Value * 100f}");
        }
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(key).AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(e == 0);
        }).AddTo(this);
    }
}
