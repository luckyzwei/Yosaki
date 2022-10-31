using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMarbleRelicRefund : MonoBehaviour
{
    private void Start()
    {
        Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value != 0)
        {
            return;
        }

        int marblePack1buyCount = ServerData.iAPServerTableTotal.TableDatas["relic1"].buyCount.Value;
        int marblePack2buyCount = ServerData.iAPServerTableTotal.TableDatas["relic2"].buyCount.Value;
        int marblePack3buyCount = ServerData.iAPServerTableTotal.TableDatas["relic3"].buyCount.Value;
        int marblePack4buyCount = ServerData.iAPServerTableTotal.TableDatas["relic4"].buyCount.Value;

        int relicPack1buyCount = ServerData.iAPServerTableTotal.TableDatas["relic1"].buyCount.Value;
        int relicPack2buyCount = ServerData.iAPServerTableTotal.TableDatas["relic2"].buyCount.Value;
        int relicPack3buyCount = ServerData.iAPServerTableTotal.TableDatas["relic3"].buyCount.Value;
        int relicPack4buyCount = ServerData.iAPServerTableTotal.TableDatas["relic4"].buyCount.Value;

        if (marblePack1buyCount == 0 && marblePack2buyCount == 0 && marblePack3buyCount == 0 && marblePack4buyCount == 0 &&
            relicPack1buyCount == 0 && relicPack2buyCount == 0 && relicPack3buyCount == 0 && relicPack4buyCount == 0)
        {

            ServerData.userInfoTable.GetTableData(UserInfoTable.marRelicRefund).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.marRelicRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.marRelicRefund).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, marbleParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        //소급코드


        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "소급양", null);


    }

}
