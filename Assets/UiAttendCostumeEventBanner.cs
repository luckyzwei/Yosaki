using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiAttendCostumeEventBanner : MonoBehaviour
{
    [SerializeField]
    private Button getButtonObject;


    private ObscuredString costumeKey = "costume1";

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(!e);
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).AsObservable().Subscribe(e =>
        {
            getButtonObject.gameObject.SetActive(e >= 4);
        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        getButtonObject.interactable = false;

        var costumeServerData = ServerData.costumeServerTable.TableDatas[costumeKey];

        costumeServerData.hasCostume.Value = true;

        Param param = new Param();

        param.Add(costumeKey, costumeServerData.ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, CostumeServerTable.tableName, CostumeServerTable.Indate, param, e =>
         {
             if (e.IsSuccess())
             {
                 PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사냥꾼 호연 외형 획득!", null);
             }
             else if (e.IsSuccess() == false)
             {
                 costumeServerData.hasCostume.Value = false;
                 getButtonObject.interactable = true;
                 return;
             }
         });

        ServerData.costumeServerTable.SyncCostumeData(costumeKey);
    }

}
