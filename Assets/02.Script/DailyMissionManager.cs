using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DailyMissionKey
{
    KillEnemy,//몬스터잡기 ★
    LevelUp,//레벨업 ★
    GachaWeapon,//무기뽑기 ★
    GachaMagicBook,//마도서뽑기  ★
    AbilUpgrade,//능력치레벨업 ★
    WeaponLevelUp,//무기레벨업 ★
    MagicBookLevelUp,//마도서레벨업 ★
    ClearInfinityTower,//무한의탑클리어 ★
    ClearBonusDungeon,//도적단소탕입장 ★
    RewardedBossContents,//보스컨텐츠 입장 ★
    SkillAwake,//스킬각성 ★
    Collection,//영혼흡수 ★
    Attendance,//출석 ★
    WeaponUpgrade, //★
    MagicbookUpgrade, //★
    GachaSkillBook,//스킬뽑기  ★
}

public static class DailyMissionManager
{
    private static Dictionary<DailyMissionKey, Coroutine> SyncRoutines = new Dictionary<DailyMissionKey, Coroutine>();

    private static WaitForSeconds syncDelay = new WaitForSeconds(3.0f);

    public static void UpdateDailyMission(DailyMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.DailyMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        DatabaseManager.dailyMissionTable.UpdateMissionData(key, count);



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

    private static IEnumerator SyncToServerRoutine(string key, DailyMissionKey missionKey)
    {
        yield return syncDelay;

        DatabaseManager.dailyMissionTable.SyncToServerEach(key);

        SyncRoutines[missionKey] = null;
    }
}
