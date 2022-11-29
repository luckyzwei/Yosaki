using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiDokebiPensionReset : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID
        //Check();
#endif
    }

    private void Check()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiPensionReset].Value != 0)
        {
            return;
        }

        int dokebiPensionBuy = ServerData.iapServerTable.TableDatas["dokebipension"].buyCount.Value;


        if (dokebiPensionBuy == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiPensionReset).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param param = new Param();

            param.Add(UserInfoTable.dokebiPensionReset, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiPensionReset).Value);

            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, param));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebipensionAttendance).Value++;

        ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiPensionReset].Value = 1;

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dokebiPensionReset, ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiPensionReset].Value);
        userInfoParam.Add(UserInfoTable.dokebipensionAttendance, ServerData.userInfoTable.TableDatas[UserInfoTable.dokebipensionAttendance].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }
}
