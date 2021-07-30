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

    public readonly static ObscuredFloat potionUseDelay = 2f;
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

    public readonly static ObscuredFloat potionUseDelay = 2.0f;

    public readonly static ObscuredInt ticketPrice = 500;
    public readonly static ObscuredInt contentsEnterprice = 0;
    public readonly static ObscuredInt dailyTickBuyCountMax = 5;
    public readonly static ObscuredInt bonusDungeonEnterCount = 5;

    public readonly static List<ObscuredFloat> potion_Option = new List<ObscuredFloat>() { 0.3f, 0.6f, 0.9f };

    public readonly static ObscuredInt bonusDungeonUnlockLevel = 30;
    public readonly static ObscuredInt InfinityDungeonUnlockLevel = 60;
    public readonly static ObscuredInt bossUnlockLevel = 100;

    public readonly static ObscuredInt fieldBossSpawnRequire = 30000;

    public readonly static ObscuredFloat fieldBossHpValue = 15f;

    public readonly static ObscuredInt bonusDungeonGemPerEnemy = 300;
    public readonly static ObscuredInt effectActiveDistance = 15;
    public readonly static ObscuredInt firstSkillAwakeNum = 1;

    public readonly static ObscuredInt spawnIntervalTime = 1;
    public readonly static ObscuredInt spawnDivideNum = 2;

    //1시간
    // public readonly static ObscuredInt sleepRewardMinValue = 3600;
    public readonly static ObscuredInt sleepRewardMinValue = 600;
    //10시간
    public readonly static ObscuredInt sleepRewardMaxValue = 36000;
    public readonly static ObscuredFloat sleepRewardRatio = 0.5f;

    public readonly static ObscuredFloat marbleSpawnProb = 1;

    public readonly static ObscuredInt marbleAwakePrice = 10000000;

    public readonly static ObscuredInt skillSlotGroupNum = 3;

    public readonly static ObscuredInt marbleUnlockLevel = 100;
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

