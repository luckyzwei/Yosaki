using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiStatlPointIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).AsObservable().Subscribe(remainStatPoint =>
        {
            countText.SetText($"남은 스탯 포인트 : {remainStatPoint}");
        }).AddTo(this);
    }
}
