using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class EnemyKillCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"오늘 처치 : {(int)e}");
        }).AddTo(this);
    }
}
