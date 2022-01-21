using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiBonusDungeonEnterCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).AsObservable().Subscribe(WhenEnterCountChanged).AddTo(this);
    }

    private void WhenEnterCountChanged(double enterCount)
    {
        description.SetText($"오늘 입장({(int)enterCount}/{GameBalance.bonusDungeonEnterCount})");
    }
}
