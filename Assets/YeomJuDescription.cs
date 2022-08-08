using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class YeomJuDescription : MonoBehaviour
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
            marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.Ym)} 1개당 손오공 보유효과\n{PlayerStats.yeoRaeMarbleValue * 100f}% 강화");
        }).AddTo(this);

    }
}
