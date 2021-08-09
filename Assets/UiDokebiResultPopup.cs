using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiDokebiResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Transform rewardParent;

    public void OnClickReturnButton()
    {
        GameManager.Instance.LoadNormalField();
    }

    public void Initialize(int defeatEnemiesNum)
    {
        SoundManager.Instance.PlaySound("BonusEnd");

        description.SetText($"{defeatEnemiesNum} 처치 완료!");

        SetReward(defeatEnemiesNum);
    }

    private void SetReward(int defeatEnemiesNum)
    {
        int dokebiIdx = GameManager.Instance.dokebiIdx;

        var tableData = TableManager.Instance.DokebiTable.dataArray[dokebiIdx];

        var prefab = CommonPrefabContainer.Instance.uiRewardViewPrefab;

        int rewardNum = defeatEnemiesNum * tableData.Rewardamount;

        var rewardPrefab = Instantiate<UiRewardView>(prefab, rewardParent);

        RewardData rewardData = new RewardData(Item_Type.Dokebi, rewardNum);

        rewardPrefab.Initialize(rewardData);

        RewardManager.Instance.GetReward(Item_Type.Dokebi, rewardNum);

        int prefMaxKillCount = 0;

        if (dokebiIdx == 0) 
        {
            prefMaxKillCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount0).Value;
        }
        else if (dokebiIdx == 1) 
        {
            prefMaxKillCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount1).Value;
        }
        else if (dokebiIdx == 2) 
        {
            prefMaxKillCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount2).Value;
        }

        if (defeatEnemiesNum > prefMaxKillCount)
        {
            if (dokebiIdx == 0)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount0).Value = defeatEnemiesNum;
                ServerData.userInfoTable.UpData(UserInfoTable.dokebiKillCount0, false);
            }
            else if (dokebiIdx == 1)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount1).Value = defeatEnemiesNum;
                ServerData.userInfoTable.UpData(UserInfoTable.dokebiKillCount1, false);
            }
            else if (dokebiIdx == 2)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount2).Value = defeatEnemiesNum;
                ServerData.userInfoTable.UpData(UserInfoTable.dokebiKillCount2, false);
            }
        }
    }
}
