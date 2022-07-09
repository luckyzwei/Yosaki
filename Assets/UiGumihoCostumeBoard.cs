using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;
using UniRx;

public class UiGumihoCostumeBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        var costumeServerData = ServerData.costumeServerTable.TableDatas["costume40"];

        costumeServerData.hasCostume.AsObservable().Subscribe(e =>
        {

            buttonDescription.SetText(e ? "획득 완료" : "획득");

        }).AddTo(this);
    }

    public void OnClickGetButton()
    {

        if (ServerData.goodsTable.GetTableData(GoodsTable.gumiho6).Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("구미호 꼬리7이 필요 합니다.");
            return;
        }

        var costumeServerData = ServerData.costumeServerTable.TableDatas["costume40"];

        if (costumeServerData.hasCostume.Value == true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 외형이 있습니다.");
            return;
        }

        costumeServerData.hasCostume.Value = true;

        Param param = new Param();

        param.Add("costume40", costumeServerData.ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, CostumeServerTable.tableName, CostumeServerTable.Indate, param, e =>
        {
            if (e.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "구미호순 획득!", null);
            }
        });

        ServerData.costumeServerTable.SyncCostumeData("costume40");

    }
}
