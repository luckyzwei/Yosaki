using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiBonusDefenseResultPopup : MonoBehaviour
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
        var prefab = CommonPrefabContainer.Instance.uiRewardViewPrefab;
        int blueStoneRewardNum = ContentsRewardManager.Instance.GetDefenseReward_BlueStone(defeatEnemiesNum);
        var blueStoneReward = Instantiate<UiRewardView>(prefab, rewardParent);

        int marbleRewardNum = ContentsRewardManager.Instance.GetDefenseReward_Marble(defeatEnemiesNum);
        var marbleReward = Instantiate<UiRewardView>(prefab, rewardParent);

        RewardData rewardDataJade = new RewardData(Item_Type.Jade, blueStoneRewardNum);
        blueStoneReward.Initialize(rewardDataJade);

        RewardData rewardDataMarble = new RewardData(Item_Type.Marble, marbleRewardNum);
        marbleReward.Initialize(rewardDataMarble);

        RewardManager.Instance.GetReward(Item_Type.Jade, blueStoneRewardNum);
        RewardManager.Instance.GetReward(Item_Type.Marble, marbleRewardNum);

        int prefMaxKillCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value;

        if (defeatEnemiesNum > prefMaxKillCount)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonMaxKillCount).Value = defeatEnemiesNum;
            ServerData.userInfoTable.UpData(UserInfoTable.bonusDungeonMaxKillCount, false);
        }
    }
}
