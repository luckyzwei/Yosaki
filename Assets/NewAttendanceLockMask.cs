using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class NewAttendanceLockMask : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    // Start is called before the first frame update
    
     
    private void OnEnable()
    {
        if(ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value!=0)
        {
            rootObject.gameObject.SetActive(false);
        }
    }

    public void AttendStartButton()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value = 1;
        rootObject.gameObject.SetActive(false);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.attendanceCount, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList, false);
    }
}
