using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventMissionKey
{
    KillEnemy,//몬스터잡기 ★
    ClearBandi,//반딧불전
    ClearOni,//도깨비전
    ClearCat,//고양이 요괴전
    AbilUpgrade,//능력치레벨업 ★
    SkillUpgrade,//스킬 레벨업 ★
    Attendance,//출석 ★
}

public static class EventMissionManager
{
    private static Dictionary<EventMissionKey, Coroutine> SyncRoutines = new Dictionary<EventMissionKey, Coroutine>();

    private static WaitForSeconds syncDelay = new WaitForSeconds(3.0f);

    private static WaitForSeconds syncDelay_slow = new WaitForSeconds(300.0f);

    public static void UpdateEventMission(EventMissionKey missionKey, int count)
    {


        string key = TableManager.Instance.EventMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionData(key, count);



        //서버저장
        if (SyncRoutines.ContainsKey(missionKey) == false)
        {
            SyncRoutines.Add(missionKey, null);
        }

        if (SyncRoutines[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines[missionKey]);
        }

        SyncRoutines[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }

    private static string Mission0 = "Mission0";

    private static string Mission1 = "Mission1";



    private static IEnumerator SyncToServerRoutine(string key, EventMissionKey missionKey)
    {
        if (key.Equals(Mission0) || key.Equals(Mission1))
        {
            yield return syncDelay_slow;
        }
        else
        {
            yield return syncDelay;
        }


        ServerData.eventMissionTable.SyncToServerEach(key);

        SyncRoutines[missionKey] = null;
    }

    public static void SyncAllMissions()
    {
 
        var tableData = TableManager.Instance.EventMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            ServerData.eventMissionTable.SyncToServerEach(tableData[i].Stringid);
        }
    }
}
