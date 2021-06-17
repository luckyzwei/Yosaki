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
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(e =>
        {
            goldText.SetText(Utils.ConvertBigFloat(e));
        }).AddTo(this);
    }
}
