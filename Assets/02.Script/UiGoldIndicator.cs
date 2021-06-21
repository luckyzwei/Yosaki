using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiGoldIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(gold =>
        {
            goldText.SetText($"{Utils.ConvertBigNum(gold)}");
        }).AddTo(this);
    }

}
