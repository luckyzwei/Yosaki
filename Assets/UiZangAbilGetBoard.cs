using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;
using UniRx;

public class UiZangAbilGetBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    private BossServerData bossServerData;
    private TwelveBossTableData bossTableData;
    private void Start()
    {
        bossTableData = TableManager.Instance.TwelveBossTable.dataArray[21];

        bossServerData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.ZangStone).AsObservable().Subscribe(e =>
        {
            buttonDescription.SetText(e >= 1 ? "획득 완료" : "획득");
        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit);

        if (bossTableData.Rewardcut.Length != rewards.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 전부 수령해야 획득 가능합니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.ZangStone).Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 영혼이 있습니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.ZangStone).Value = 1;

        Param param = new Param();

        param.Add(GoodsTable.ZangStone, ServerData.goodsTable.GetTableData(GoodsTable.ZangStone).Value);

        SendQueue.Enqueue(Backend.GameData.Update, GoodsTable.tableName, GoodsTable.Indate, param, e =>
        {
            if (e.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "영혼 획득 완료!", null);
            }
        });
    }
}
