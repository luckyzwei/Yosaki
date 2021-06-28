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
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).AsObservable().Subscribe(blueStone =>
        {
            goldText.SetText($"{Utils.ConvertBigNum(blueStone)}");
        }).AddTo(this);
    }
}
