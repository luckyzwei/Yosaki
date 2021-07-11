using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAttendance : MonoBehaviour
{
    [SerializeField]
    private List<UiAttendanceCell> attendanceCellList;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.AttendanceReward.dataArray;

        if (attendanceCellList.Count != tableDatas.Length)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "출석 데이터 다름", null);
            return;
        }

        for (int i = 0; i < tableDatas.Length; i++)
        {
            attendanceCellList[i].Initialize(tableDatas[i]);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value++;
        }
    }
#endif

}
