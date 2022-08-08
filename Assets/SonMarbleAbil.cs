using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class SonMarbleAbil : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleDescription;


    void Start()
    {

        Subscribe();

    }

    private void Subscribe()
    {

        ServerData.goodsTable.GetTableData(GoodsTable.Ym).AsObservable().Subscribe(e =>
        {
            marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.Ym)} 1개당 보유효과 {PlayerStats.yeoRaeMarbleValue * 100f}% 증가\n<color=yellow>+{PlayerStats.GetSonAbilPlusValue() * 100f}% 강화됨");
        }).AddTo(this);

    }

}
