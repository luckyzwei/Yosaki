using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;
using UniRx;
public class UiNataBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    private BossServerData bossServerData;
    private TwelveBossTableData bossTableData;
    private void Start()
    {
        bossTableData = TableManager.Instance.TwelveBossTable.dataArray[26];

        bossServerData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        Subscribe();
    }

    private void Subscribe()
    {
        var costumeServerData = ServerData.costumeServerTable.TableDatas["costume35"];

        costumeServerData.hasCostume.AsObservable().Subscribe(e =>
        {

            buttonDescription.SetText(e ? "획득 완료" : "획득");

        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit);

        if (rewards.Length - 1 < 12)
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 12개 받아야 획득 가능합니다.");
            return;
        }
        var costumeServerData = ServerData.costumeServerTable.TableDatas["costume35"];

        if (costumeServerData.hasCostume.Value == true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 외형이 있습니다.");
            return;
        }

        costumeServerData.hasCostume.Value = true;

        Param param = new Param();

        param.Add("costume35", costumeServerData.ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, CostumeServerTable.tableName, CostumeServerTable.Indate, param, e =>
        {
            if (e.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "나타 외형 획득!", null);
            }
        });

        ServerData.costumeServerTable.SyncCostumeData("costume35");

    }
}
