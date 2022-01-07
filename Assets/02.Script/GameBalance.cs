using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBalance
{
    public readonly static ObscuredFloat moveSpeed = 8f;
    public readonly static ObscuredFloat jumpPower = 17f;
    public readonly static ObscuredFloat doubleJumpPower = 25f;
}

public class PotionBalance
{
    public readonly static List<ObscuredFloat> recover_Potion = new List<ObscuredFloat>() { 0.2f, 0.5f, 1.0f };
    public readonly static List<ObscuredFloat> price_Potion = new List<ObscuredFloat>() { 1, 1, 1 };

    public readonly static ObscuredFloat potionUseDelay = 0.9f;
}


public static class GameBalance
{
    //레벨업시 얻는 스탯포인트
    public readonly static ObscuredInt StatPoint = 3;
    //레벨업시 얻는 스킬포인트
    public readonly static ObscuredInt SkillPointGet = 1;
    public readonly static ObscuredInt SkillPointResetPrice = 100000;

    //시작골드
    public readonly static ObscuredInt StartingMoney = 1000;

    //스킬 각성당 올릴수있는 스킬갯수
    public readonly static ObscuredInt SkillAwakePlusNum = 10;

    public readonly static ObscuredFloat initHp = 1000f;
    public readonly static ObscuredFloat initMp = 100f;

    public static ObscuredInt costumeMaxGrade = 5;

    public readonly static ObscuredInt levelUpSpinGet = 3;

    public readonly static ObscuredFloat potionUseDelay = 0.9f;

    public readonly static ObscuredInt ticketPrice = 500;
    public readonly static ObscuredInt contentsEnterprice = 0;
    public readonly static ObscuredInt dailyTickBuyCountMax = 5;
    public readonly static ObscuredInt bonusDungeonEnterCount = 10;

    public readonly static ObscuredInt dokebiEnterCount = 10;

    public readonly static List<ObscuredFloat> potion_Option = new List<ObscuredFloat>() { 0.3f, 0.6f, 0.9f };

    public readonly static ObscuredInt bonusDungeonUnlockLevel = 30;
    public readonly static ObscuredInt InfinityDungeonUnlockLevel = 60;
    public readonly static ObscuredInt bossUnlockLevel = 100;

    public readonly static ObscuredInt fieldBossSpawnRequire = 30000;

    public readonly static ObscuredFloat fieldBossHpValue = 15f;

    public readonly static ObscuredInt bonusDungeonGemPerEnemy = 2000;
    public readonly static ObscuredInt bonusDungeonMarblePerEnemy = 200;
    public readonly static ObscuredInt effectActiveDistance = 15;
    public readonly static ObscuredInt firstSkillAwakeNum = 1;

    public readonly static ObscuredInt spawnIntervalTime = 1;
    public readonly static ObscuredInt spawnDivideNum = 2;

    //1시간
    // public readonly static ObscuredInt sleepRewardMinValue = 3600;
    public readonly static ObscuredInt sleepRewardMinValue = 600;
    //10시간
    public readonly static ObscuredInt sleepRewardMaxValue = 43200;
    public readonly static ObscuredFloat sleepRewardRatio = 0.9f;

    public readonly static ObscuredFloat marbleSpawnProb = 1;

    public readonly static ObscuredInt marbleAwakePrice = 8000000;

    public readonly static ObscuredInt skillSlotGroupNum = 3;

    public readonly static ObscuredInt marbleUnlockLevel = 400;

    public readonly static ObscuredInt skillCraftUnlockGachaLevel = 10;

    public readonly static ObscuredInt twelveDungeonEnterPrice = 50000;

    public readonly static ObscuredInt nickNameChangeFee = 500000;


    public readonly static ObscuredInt rankRewardTicket_1 = 500;
    public readonly static ObscuredInt rankRewardTicket_2 = 450;
    public readonly static ObscuredInt rankRewardTicket_3 = 400;
    public readonly static ObscuredInt rankRewardTicket_4 = 350;
    public readonly static ObscuredInt rankRewardTicket_5 = 300;
    public readonly static ObscuredInt rankRewardTicket_6_20 = 200;
    public readonly static ObscuredInt rankRewardTicket_21_100 = 150;
    public readonly static ObscuredInt rankRewardTicket_101_1000 = 100;
    public readonly static ObscuredInt rankRewardTicket_1001_10000 = 50;

    public readonly static ObscuredInt rankRewardTicket_1_relic = 80;
    public readonly static ObscuredInt rankRewardTicket_2_relic = 70;
    public readonly static ObscuredInt rankRewardTicket_3_relic = 65;
    public readonly static ObscuredInt rankRewardTicket_4_relic = 60;
    public readonly static ObscuredInt rankRewardTicket_5_relic = 55;
    public readonly static ObscuredInt rankRewardTicket_6_20_relic = 50;
    public readonly static ObscuredInt rankRewardTicket_21_100_relic = 40;
    public readonly static ObscuredInt rankRewardTicket_101_1000_relic = 35;
    public readonly static ObscuredInt rankRewardTicket_1001_10000_relic = 25;

    public readonly static ObscuredInt rankReward_1_MiniGame = 10;
    public readonly static ObscuredInt rankReward_2_MiniGame = 9;
    public readonly static ObscuredInt rankReward_3_MiniGame = 8;
    public readonly static ObscuredInt rankReward_4_MiniGame = 7;
    public readonly static ObscuredInt rankReward_5_MiniGame = 6;
    public readonly static ObscuredInt rankReward_6_20_MiniGame = 5;
    public readonly static ObscuredInt rankReward_21_100_MiniGame = 4;
    public readonly static ObscuredInt rankReward_101_1000_MiniGame = 3;
    public readonly static ObscuredInt rankReward_1001_10000_MiniGame = 2;

    public readonly static ObscuredInt rankReward_1_guild = 800;
    public readonly static ObscuredInt rankReward_2_guild = 700;
    public readonly static ObscuredInt rankReward_3_guild = 600;
    public readonly static ObscuredInt rankReward_4_guild = 500;
    public readonly static ObscuredInt rankReward_5_guild = 400;
    public readonly static ObscuredInt rankReward_6_20_guild = 300;
    public readonly static ObscuredInt rankReward_21_100_guild = 200;
    public readonly static ObscuredInt rankReward_101_1000_guild = 150;

    public readonly static ObscuredInt EventDropEndDay = 14;
    public readonly static ObscuredInt EventMakeEndDay = 14;
    public readonly static ObscuredInt EventPackageSaleEndDay = 14;

    public readonly static ObscuredFloat TitleEquipAddPer = 2;

    public readonly static ObscuredFloat HotTime_Start = 20;
    public readonly static ObscuredFloat HotTime_End = 22;

    public readonly static ObscuredFloat HotTime_Exp = 15;
    public readonly static ObscuredFloat HotTime_Gold = 15;
    public readonly static ObscuredFloat HotTime_GrowthStone = 30;
    public readonly static ObscuredFloat HotTime_Marble = 4;

    public readonly static ObscuredInt DailyRelicTicketGetCount = 3;
    public readonly static ObscuredInt StageRelicUnlockLevel = 3000;
    public readonly static ObscuredFloat StageRelicUpgradePrice = 1000;

    public readonly static ObscuredFloat BossScoreSmallizeValue = 0.00000000000000000001f;
    public readonly static ObscuredFloat BossScoreConvertToOrigin = 100000000000000000000f;

    public readonly static ObscuredInt SonEvolutionDivdeNum = 3000;

    public readonly static ObscuredInt MaxDamTextNum = 120;

    public readonly static ObscuredInt YachaRequireLevel = 5300;

    public readonly static ObscuredFloat YachaSkillAddValuePerLevel = 0.08f;
    public readonly static ObscuredFloat YachaIgnoreDefenseAddValuePerLevel = 0.08f;

    public readonly static ObscuredInt SonCostumeUnlockLevel = 80000;
    public readonly static ObscuredInt YoungMulCreateEquipLevel = 120;
    public readonly static ObscuredFloat PetAwakeValuePerLevel = 0.003f;
    public readonly static ObscuredFloat AwakePetUpgradePrice = 100000000;

    public readonly static ObscuredFloat GuildMakePrice = 100000000;
    public readonly static ObscuredInt GuildMemberMax = 20;

    public readonly static ObscuredInt GuildCreateMinLevel = 5000;
    public readonly static ObscuredInt GuildEnterMinLevel = 5000;

    public static int GetSonIdx()
    {
        int level = ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value;

        if (level >= 12000 && level < 50000)
        {
            return 3;
        }

        if (level >= 50000 && level < 100000)
        {
            return 4;
        }

        if (level >= 100000)
        {
            return 5;
        }

        int ret = 0;



        if (level >= 9000)
        {
            level -= 3000;
        }

        ret = level / SonEvolutionDivdeNum;
        ret = Mathf.Min(ret, CommonUiContainer.Instance.sonThumbNail.Count - 1);
        return ret;
    }
}

public static class DamageBalance
{
    public readonly static ObscuredFloat baseMinDamage = 0.8f;
    public readonly static ObscuredFloat baseMaxDamage = 1.2f;
    public static float GetRandomDamageRange()
    {
        return Random.Range(baseMinDamage + PlayerStats.GetDamBalanceAddValue(), baseMaxDamage);
    }
}

