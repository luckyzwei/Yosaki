using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiGoodsIndicator : MonoBehaviour
{
    [SerializeField]
    private string goodsKey;

    [SerializeField]
    private TextMeshProUGUI goodsText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.goodsTable.GetTableData(goodsKey).AsObservable().Subscribe(goods =>
        {
            goodsText.SetText($"{Utils.ConvertBigNum(goods).ToString()}");
        }).AddTo(this);
    }
}
