using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class FoxMarbleAbil : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleDescription;


    void Start()
    {

        Subscribe();

    }

    private void Subscribe()
    {

        ServerData.goodsTable.GetTableData(GoodsTable.FoxMaskPartial).AsObservable().Subscribe(e =>
        {
            marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.FoxMaskPartial)} 1개당 요괴탈 보유효과 {PlayerStats.foxMaskPartialValue * 100f}% 증가\n<color=yellow>({PlayerStats.GetFoxMaskAbilPlusValue() * 100f}% 강화됨)");
        }).AddTo(this);

    }

}
