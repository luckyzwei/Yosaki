using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class SusanoUpgradeAbil : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleDescription;


    void Start()
    {

        Subscribe();

    }

    private void Subscribe()
    {

        ServerData.goodsTable.GetTableData(GoodsTable.SusanoTreasure).AsObservable().Subscribe(e =>
        {
            marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.SusanoTreasure)} 1개당 악귀퇴치 보유효과 {PlayerStats.susanoUpgradelValue * 100f}% 증가\n<color=yellow>({PlayerStats.GetSusanoUpgradeAbilPlusValue() * 100f}% 강화됨)");
        }).AddTo(this);

    }

}
