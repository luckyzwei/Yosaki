using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class SpringAttenCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    void Start()
    {
        Subscribe();

    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountSpring).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"{e}일차");
        }).AddTo(this);
    }
}
