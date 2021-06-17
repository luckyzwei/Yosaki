using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiSkillPointIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).AsObservable().Subscribe(remainSkillPoint =>
        {
            countText.SetText($"남은 스킬 포인트 : {remainSkillPoint}");
        }).AddTo(this);
    }
}
