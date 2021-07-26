using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class UiStatus : SingletonMono<UiStatus>
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image costumeIcon;


    private int loadedMyRank = -1;

    void Start()
    {
        Subscribe();
        RequestMyRank();
    }

    private void RequestMyRank()
    {
        RankManager.Instance.RequestMyLevelRank();
        RankManager.Instance.RequestMyStageRank();
    }
    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(WhenLevelChanged).AddTo(this);

        RankManager.Instance.WhenMyLevelRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                loadedMyRank = e.Rank;

                WhenLevelChanged(ServerData.statusTable.GetTableData(StatusTable.Level).Value);

            }
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            costumeIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail((int)e);
        }).AddTo(this);

    }

    private void WhenLevelChanged(int level)
    {
        if (loadedMyRank == -1)
        {
            nameText.SetText($"Lv:{level} {PlayerData.Instance.NickName}");
        }
        else
        {
            nameText.SetText($"Lv:{level} {PlayerData.Instance.NickName} ({loadedMyRank}등)");
        }
    }
}
