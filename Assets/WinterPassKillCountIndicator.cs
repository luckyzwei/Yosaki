using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class WinterPassKillCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.killCountTotalWinterPass).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"처치 : {Utils.ConvertBigNum(e)}");
        }).AddTo(this);
    }
}
