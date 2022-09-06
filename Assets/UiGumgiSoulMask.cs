using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiGumgiSoulMask : MonoBehaviour
{
    [SerializeField]
    private float gumgiAmount;

    [SerializeField]
    private TextMeshProUGUI description;

    private void Start()
    {
        description.SetText($"검조각 {Utils.ConvertBigNum(gumgiAmount)}이상일때 개방");
    }

    private void OnEnable()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(e < gumgiAmount);
        }).AddTo(this);
    }
}
