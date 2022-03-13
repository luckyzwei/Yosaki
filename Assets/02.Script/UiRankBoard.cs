using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiRankBoard : MonoBehaviour
{
    private void OnEnable()
    {
        RankManager.Instance.UpdateStage_Score(ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value);
        RankManager.Instance.UpdateUserRank_Level();
        //RankManager.Instance.RequestMyLevelRank();
        //RankManager.Instance.RequestMyStageRank();

        //if (GameManager.contentsType == GameManager.ContentsType.NormalField)
        //{
        //    RankManager.Instance.RequestMyBossRank();
        //    RankManager.Instance.RequestMyRealBossRank();
        //    RankManager.Instance.RequestMyRelicRank();
        //    RankManager.Instance.RequestMyMiniGameRank();
        //}
    }

}
