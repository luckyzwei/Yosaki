using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiBonusRoulletRedDot : UiRedDotBase
{
    protected override void Subscribe()
    {
        DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(e > 0f);
        }).AddTo(this);
    }
}
