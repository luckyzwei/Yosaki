using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiBlueStoneIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).AsObservable().Subscribe(Jade =>
        {
            goldText.SetText($"{Utils.ConvertBigNum(Jade)}");
        }).AddTo(this);
    }
}
