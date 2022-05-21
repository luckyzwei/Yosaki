using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
public class UiIndraAbilCell : MonoBehaviour
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

        if (key == PlayerStats.indraKey0)
        {
            description.SetText($"<color=white>{CommonString.GetItemName(Item_Type.Indra0)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{Utils.ConvertBigNum(PlayerStats.indra0Value)}");
        }
        else if (key == PlayerStats.indraKey1)
        {
            description.SetText($"<color=blue>{CommonString.GetItemName(Item_Type.Indra1)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{Utils.ConvertBigNum(PlayerStats.indra1Value)}");

        }
        else if (key == PlayerStats.indraKey2)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.Indra2)}</color>\n{CommonString.GetStatusName(StatusType.PenetrateDefense)}\n{PlayerStats.indra2Value * 100f}");
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
