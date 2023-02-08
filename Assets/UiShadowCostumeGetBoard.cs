using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;
using UniRx;

public class UiShadowCostumeGetBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    [SerializeField]
    private TextMeshProUGUI getDescription;

    private BossServerData bossServerData;

    private TwelveBossTableData bossTableData;

    void Start()
    {
        bossTableData = TableManager.Instance.TwelveBossTable.dataArray[91];

        bossServerData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        getDescription.SetText($"그림자 동굴 {GameBalance.shadowCostumeGetLevel}단계 이상일때 획득 가능");

        Subscribe();
    }

    private void Subscribe()
    {
        var costumeServerData = ServerData.costumeServerTable.TableDatas["costume86"];

        costumeServerData.hasCostume.AsObservable().Subscribe(e =>
        {

            buttonDescription.SetText(e ? "획득 완료" : "획득");

        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        int score = 0;

        if (int.TryParse(bossServerData.score.Value, out score))
        {
            if (score < GameBalance.shadowCostumeGetLevel)
            {
                PopupManager.Instance.ShowAlarmMessage($"그림자 동굴 {GameBalance.shadowCostumeGetLevel}단계 이상일때 획득하실 수 있습니다.");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"그림자 동굴 {GameBalance.shadowCostumeGetLevel}단계 이상일때 획득하실 수 있습니다.");
            return;
        }
     
        var costumeServerData = ServerData.costumeServerTable.TableDatas["costume86"];

        if (costumeServerData.hasCostume.Value == true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 외형이 있습니다.");
            return;
        }

        costumeServerData.hasCostume.Value = true;

        Param param = new Param();

        param.Add("costume86", costumeServerData.ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, CostumeServerTable.tableName, CostumeServerTable.Indate, param, e =>
        {
            if (e.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "그림자 외형 획득!", null);
            }
        });

        ServerData.costumeServerTable.SyncCostumeData("costume86");

    }
}
