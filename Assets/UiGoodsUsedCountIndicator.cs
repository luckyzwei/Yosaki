using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiGoodsUsedCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usedCountText;

    [SerializeField]
    private string usedGoodsKey;

    [SerializeField]
    private string usedGoodsName;

    void Start()
    {
        //
        Subscribe();
    }


    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(usedGoodsKey).AsObservable().Subscribe(e =>
        {
            usedCountText.SetText($"교환한 {usedGoodsName} 수 : {Utils.ConvertBigNum(e)}");
        }).AddTo(this);
    }
}
