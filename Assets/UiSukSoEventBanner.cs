using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSukSoEventBanner : MonoBehaviour
{
    [SerializeField]
    private Button getButtonObject;

    [SerializeField]
    private int getLevel;

    private ObscuredString costumeKey = "costume2";

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

        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
        {
            getButtonObject.gameObject.SetActive(e >= getLevel);
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
                 PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "썩연 외형 획득!", null);
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
