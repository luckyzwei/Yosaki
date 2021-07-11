using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class UiSkillPointRedDot : UiRedDotBase
{
    protected override void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.SkillPoint).AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(e > 0);
        }).AddTo(this);
    }
}
