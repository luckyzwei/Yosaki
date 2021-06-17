using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class UiSkillPointRedDot : UiRedDotBase
{
    protected override void Subscribe()
    {
        DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(e > 0);
        }).AddTo(this);
    }
}
