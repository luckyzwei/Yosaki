using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepRewardReceiver : SingletonMono<SleepRewardReceiver>
{
    public class SleepRewardInfo
    {
        public readonly float gold;

        public readonly float jade;

        public readonly float GrowthStone;

        public readonly float marble;

        public readonly float exp;

        public readonly int elapsedSeconds;

        public SleepRewardInfo(float gold, float jade, float GrowthStone, float marble, float exp, int elapsedSeconds)
        {
            this.gold = gold;

            this.jade = jade;

            this.GrowthStone = GrowthStone;

            this.marble = marble;

            this.exp = exp;

            this.elapsedSeconds = elapsedSeconds;
        }
    }

    public SleepRewardInfo sleepRewardInfo { get; private set; }

    private bool SetComplete = false;
    public void SetElapsedSecond(int elapsedSeconds)
    {
        if (SetComplete == true) return;

        elapsedSeconds = Mathf.Min(elapsedSeconds, GameBalance.sleepRewardMaxValue);

        SetComplete = true;

        //맨처음
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value == -1)
        {
            return;
        }

        sleepRewardInfo = null;

        //일정시간 이하는 안됨
        if (elapsedSeconds < GameBalance.sleepRewardMinValue)
        {
            return;
        }

        float elapsedMinutes = (float)elapsedSeconds / 60f;

        Debug.LogError($"Elapsed {elapsedSeconds}");

        int currentStageIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (currentStageIdx == TableManager.Instance.GetLastStageIdx())
        {
            currentStageIdx = TableManager.Instance.GetLastStageIdx();
        }

        var stageTableData = TableManager.Instance.StageMapData[currentStageIdx];

        MapInfo mapInfo = BattleObjectManager.GetMapPrefabObject(stageTableData.Mappreset).GetComponent<MapInfo>();

        var spawnedEnemyData = TableManager.Instance.EnemyData[stageTableData.Monsterid1];

        var spawnInterval = stageTableData.Spawndelay + ((float)GameBalance.spawnIntervalTime * (float)GameBalance.spawnDivideNum);
        //
        int platformNum = mapInfo.spawnPlatforms.Count;

        float spawnEnemyNumPerSec = (float)(platformNum * stageTableData.Spawnamountperplatform) / spawnInterval;

        float killedEnemyPerMin = spawnEnemyNumPerSec * 60f;

        float goldBuffRatio = PlayerStats.GetGoldPlusValueExclusiveBuff() * 0.5f;
        float expBuffRatio = PlayerStats.GetExpPlusValueExclusiveBuff() * 0.5f;

        float gold = killedEnemyPerMin * spawnedEnemyData.Gold * GameBalance.sleepRewardRatio * elapsedMinutes;
        gold += gold * goldBuffRatio;

        float enemyKilldailyMissionRequire = TableManager.Instance.DailyMissionDatas[0].Rewardrequire;
        float enemyKilldailyMissionReward = TableManager.Instance.DailyMissionDatas[0].Rewardvalue;

        float jade = killedEnemyPerMin / enemyKilldailyMissionRequire * enemyKilldailyMissionReward * GameBalance.sleepRewardRatio * elapsedMinutes * 1.8f;

        float GrowthStone = killedEnemyPerMin * stageTableData.Magicstoneamount * GameBalance.sleepRewardRatio * elapsedMinutes;

        float marble = killedEnemyPerMin * stageTableData.Marbleamount * GameBalance.sleepRewardRatio * elapsedMinutes;

        float exp = killedEnemyPerMin * spawnedEnemyData.Exp * GameBalance.sleepRewardRatio * elapsedMinutes;
        exp += exp * expBuffRatio;

        this.sleepRewardInfo = new SleepRewardInfo(gold: gold, jade: jade, GrowthStone: GrowthStone, marble: marble, exp: exp, elapsedSeconds: elapsedSeconds);
    }

    public void GetSleepReward(Action successCallBack)
    {
        if (sleepRewardInfo == null) return;

        LogManager.Instance.SendLog("휴식보상 요청", $"gold {sleepRewardInfo.gold} jade {sleepRewardInfo.jade} marble {sleepRewardInfo.marble} growthStone {sleepRewardInfo.GrowthStone} exp {sleepRewardInfo.exp}");

        GrowthManager.Instance.GetExp(sleepRewardInfo.exp, false, false);

        ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value += sleepRewardInfo.gold;
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += sleepRewardInfo.jade;
        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += sleepRewardInfo.marble;
        ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += sleepRewardInfo.GrowthStone;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);

        List<TransactionValue> transantions = new List<TransactionValue>();

        transantions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        successCallBack?.Invoke();

        ServerData.SendTransaction(transantions, successCallBack: () =>
          {
              LogManager.Instance.SendLog("휴식보상 수령", "수령완료");
          });
    }

    public void GetRewardSuccess()
    {
        sleepRewardInfo = null;
    }
}
