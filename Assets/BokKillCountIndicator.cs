using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class BokKillCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"{e}일차");
        }).AddTo(this);
    }
}
