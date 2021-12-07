using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class SonCostumeBanner : MonoBehaviour
{
    [SerializeField]
    private Button getButtonObject;

    [SerializeField]
    private TextMeshProUGUI description;

    private ObscuredString costumeKey = "costume16";

    void Start()
    {
        Subscribe();

        Initialize();
    }

    private void Initialize() 
    {
        description.SetText($"손오공 레벨 {GameBalance.SonCostumeUnlockLevel} 달성시 획득 가능!");
    }

    private void Subscribe()
    {
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(!e);
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.Son_Level).AsObservable().Subscribe(e =>
        {
            getButtonObject.gameObject.SetActive(e >= GameBalance.SonCostumeUnlockLevel);
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "제천대성 외형 획득!", null);
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
