using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiAsuraAbilCell : MonoBehaviour
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

        if (key == PlayerStats.asuraKey0)
        {
            description.SetText($"<color=white>{CommonString.GetItemName(Item_Type.Asura0)}</color>\n{CommonString.GetStatusName(StatusType.AttackAddPer)}\n{Utils.ConvertBigNum(PlayerStats.asura0Value * 100f)}");
        }
        else if (key == PlayerStats.asuraKey1)
        {
            description.SetText($"<color=blue>{CommonString.GetItemName(Item_Type.Asura1)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{Utils.ConvertBigNum(PlayerStats.asura1Value)}");

        }
        else if (key == PlayerStats.asuraKey2)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.Asura2)}</color>\n{CommonString.GetStatusName(StatusType.SuperCritical1DamPer)}\n{Utils.ConvertBigNum(PlayerStats.asura2Value * 100f)}");
        }
        else if (key == PlayerStats.asuraKey3)
        {
            description.SetText($"<color=red>{CommonString.GetItemName(Item_Type.Asura3)}</color>\n{CommonString.GetStatusName(StatusType.SuperCritical2DamPer)}\n{Utils.ConvertBigNum(PlayerStats.asura3Value * 100f)}");
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
