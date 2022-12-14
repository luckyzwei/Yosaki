using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UserInfoCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;
    
    [SerializeField]
    private string keyword;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(keyword).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"출석일 : {Utils.ConvertBigNum(e)}일");
        }).AddTo(this);
    }
}
