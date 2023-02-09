using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuideMissionKey
{
    ClearOni,
    ClearBackgui,
    ClearRelic,
    ClearSmith,
    ClearSon,
    ClearGangChul,
    ClearFoxMask,
    ClearSwordPartial,
    JoinGuild,
    ClearGradeTest,
    ClearChunma,
    ClearCave,
}

public static class GuideMissionManager
{
    public static void UpdateGuideMissionClear(GuideMissionKey missionKey)
    {
        //로컬 데이터 갱신
        if (!ServerData.etcServerTable.GuideMissionCleared((int)missionKey))
        {
            ServerData.etcServerTable.UpdateGuideMissionClear(missionKey);
            ServerData.etcServerTable.UpdateData(EtcServerTable.GuideMissionClear);

            var tableData = TableManager.Instance.GuideMission.dataArray[(int)missionKey];

            PopupManager.Instance.ShowAlarmMessage($"요린이 임무 클리어!({tableData.Name})", 0.5f);
        }
        else
        {
            //이미 꺰
            return;
        }
    }
    public static void UpdateGuideMissionReward(GuideMissionKey missionKey)
    {
        if (!ServerData.etcServerTable.GuideMissionRewarded((int)missionKey))
        {
            ServerData.etcServerTable.UpdateGuideMissionReward(missionKey);
            ServerData.etcServerTable.UpdateData(EtcServerTable.GuideMissionReward);
        }
        else
        {
            //이미 받음
            return;
        }
    }




}
