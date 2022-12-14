using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class NewAttendCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;
    

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"출석일 : {Utils.ConvertBigNum(e)}일");
        }).AddTo(this);
    }
}
