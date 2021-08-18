using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiMarbleAwakeIndicator : MonoBehaviour
{
    void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(e == 1);
        }).AddTo(this);
    }

}
