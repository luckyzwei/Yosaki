using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiStatusGold : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(e =>
        {
            goldText.SetText(Utils.ConvertBigNum(e));
        }).AddTo(this);
    }
}
