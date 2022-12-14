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
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value == 1)
        {
            return; 
        }

        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value = 1;
       

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.attendanceCount, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        rootObject.gameObject.SetActive(false);

        ServerData.SendTransaction(transactionList, successCallBack:()=>
        {
            SoundManager.Instance.PlaySound("Reward");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "출석 시작!!", null);
        });
    }
}
