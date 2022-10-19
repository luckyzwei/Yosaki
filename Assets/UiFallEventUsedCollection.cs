using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiFallEventUsedCollection : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usedCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.usedFallCollectionCount).AsObservable().Subscribe(e =>
        {
            usedCountText.SetText($"교환한 곶감 수 : {Utils.ConvertBigNum(e)}");
        }).AddTo(this);
    }
}
