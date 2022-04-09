﻿using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//삭제하거나순서절대바꾸면안됨
public enum StatusType
{
    AttackAddPer,// icon
    CriticalProb,//icon
    CriticalDam,//
    SkillCoolTime,//icon
    SkillDamage, //icon
    MoveSpeed,
    DamBalance,
    HpAddPer,//icon
    MpAddPer,//icon
    GoldGainPer,//icon
    ExpGainPer,//icon
    AttackAdd,//icon
    Hp,//icon
    Mp,//icon
    HpRecover,//icon
    MpRecover, //icon
    MagicStoneAddPer, //icon
    Damdecrease,
    IgnoreDefense,
    DashCount,
    DropAmountAddPer,
    BossDamAddPer,
    SkillAttackCount,
    PenetrateDefense,
    SuperCritical1Prob,
    SuperCritical1DamPer,
    MarbleAddPer,
    SuperCritical2DamPer,
    //Smith
    growthStoneUp, WeaponHasUp, NorigaeHasUp, PetEquipHasUp, PetEquipProbUp,
    DecreaseBossHp
}


public static class PlayerStats
{
    public static float GetTotalPower()
    {
        float baseAttack = GetBaseAttackPower();
        float baseAttackPer = GetBaseAttackAddPercentValue();
        float criProb = GetCriticalProb();
        float criDam = CriticalDam();
        float coolTIme = GetSkillCoolTimeDecreaseValue();
        float skillDam = GetSkillDamagePercentValue();
        float hpBase = GetMaxHp();
        float hpAddPer = GetMaxHpPercentAddValue();
        float mpBase = GetMaxMp();
        float mpAddPer = GetMaxMpPercentAddValue();

        float ignoreDefense = GetIgnoreDefenseValue();
        float decreaseDam = GetDamDecreaseValue();
        float skillAttackCount = GetSkillHitAddValue();
        float penetration = GetPenetrateDefense();
        float superCriticalProb = GetSuperCriticalProb();

        float feelMulDam = GetSuperCritical2DamPer();

        float totalPower =
          ((baseAttack + baseAttack * baseAttackPer)
           * (Mathf.Max(criProb, 0.01f) * 100f * Mathf.Max(criDam, 0.01f))
           * (Mathf.Max(skillDam, 0.01f) * 100f)
           * (Mathf.Max(coolTIme, 0.01f)) * 100f)

           + ((hpBase + hpBase * hpAddPer) + (mpBase + mpBase * mpAddPer))
           + ((baseAttack + baseAttack * baseAttackPer)
           * (Mathf.Max(ignoreDefense, 0.01f)) * 100f
           * (Mathf.Max(decreaseDam, 0.01f)) * 100f
           * (Mathf.Max(skillAttackCount, 0.01f)) * 100f
           * (Mathf.Max(penetration, 0.01f)) * 100f
           );

        totalPower += totalPower * GetSuperCriticalDamPer() * superCriticalProb;

        totalPower += totalPower * feelMulDam;

        //     float totalPower =
        //((baseAttack + baseAttack * baseAttackPer)
        // * (Mathf.Max(criProb, 0.01f) * 100f * Mathf.Max(criDam, 0.01f))
        // * (Mathf.Max(skillDam, 0.01f) * 100f)
        // * (Mathf.Max(coolTIme, 0.01f)) * 100)
        // + ((hpBase + hpBase * hpAddPer) + (mpBase + mpBase * mpAddPer));

        return totalPower * 0.01f;
    }

    public static float GetMoveSpeedValue()
    {
        float ret = 0f;
        ret += GetMarbleValue(StatusType.MoveSpeed);

        return ret;
    }
    public static float GetDropAmountAddValue()
    {
        float ret = 0f;

        ret += GetMarbleValue(StatusType.DropAmountAddPer);

        return ret;
    }

    public static float GetDamDecreaseValue()
    {
        float ret = 0f;

        ret += GetMarbleValue(StatusType.Damdecrease);
        ret += GetMagicBookHasPercentValue(StatusType.Damdecrease);
        ret += GetSinsuEquipEffect(StatusType.Damdecrease);
        ret += GetRelicHasEffect(StatusType.Damdecrease);

        return ret;
    }
    public static float GetBossDamAddValue()
    {
        float ret = 0f;

        ret += GetMarbleValue(StatusType.BossDamAddPer);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.BossDamage_memory);
        ret += GetMagicBookHasPercentValue(StatusType.BossDamAddPer);

        return ret;
    }
    public static int GetSkillHitAddValue()
    {
        int ret = 0;

        ret += (int)GetMarbleValue(StatusType.SkillAttackCount);
        ret += (int)GetMagicBookHasPercentValue(StatusType.SkillAttackCount);
        ret += (int)GetSinsuEquipEffect(StatusType.SkillAttackCount);

        return ret;
    }

    public static float GetPassiveSkillValue(StatusType statusType)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            var serverData = ServerData.passiveServerTable.TableDatas[tableData[i].Stringid];

            int level = serverData.level.Value;

            if (level != 0)
            {
                ret += level * tableData[i].Abilityvalue;
            }
        }

        return ret;
    }

    #region AttackPower
    public static float GetBaseAttackPower()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.AttackLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.AttackAdd);
        ret += GetWeaponEquipPercentValue(StatusType.AttackAdd);
        ret += GetSkillCollectionValue(StatusType.AttackAdd);
        ret += GetWingAbilValue(StatusType.AttackAdd);
        ret += GetPassiveSkillValue(StatusType.AttackAdd);
        ret += GetMarbleValue(StatusType.AttackAdd);
        ret += GetSkillHasValue(StatusType.AttackAdd);
        ret += GetYomulUpgradeValue(StatusType.AttackAdd);
        ret += GetTitleAbilValue(StatusType.AttackAdd);

        ret += GetBuffValue(StatusType.AttackAdd);
        ret += GetRelicHasEffect(StatusType.AttackAdd);

        ret += GetStageRelicHasEffect(StatusType.AttackAdd);
        ret += GetSonAbilHasEffect(StatusType.AttackAdd);

        return ret;
    }

    public static float GetCollectionAbilValue(StatusType type)
    {
        var enemyTable = TableManager.Instance.EnemyTable.dataArray;
        float ret = 0f;

        for (int i = 0; i < enemyTable.Length; i++)
        {
            if ((StatusType)enemyTable[i].Collectionabiltype == type)
            {
                ret += ServerData.collectionTable.GetCollectionAbilValue(enemyTable[i]);
            }
        }

        return ret;
    }


    public static float GetBaseAttackAddPercentValue()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.IntLevelAddPer_StatPoint);
        ret += GetWeaponHasPercentValue(StatusType.AttackAddPer);
        ret += GetMagicBookHasPercentValue(StatusType.AttackAddPer);
        ret += GetCostumeAttackPowerValue();
        ret += GetSkillCollectionValue(StatusType.AttackAddPer);
        ret += GetWingAbilValue(StatusType.AttackAddPer);
        ret += GetPassiveSkillValue(StatusType.AttackAddPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.AttackAddPer);
        ret += GetMarbleValue(StatusType.AttackAddPer);
        ret += GetStageRelicHasEffect(StatusType.AttackAddPer);
        ret += GetSonAbilHasEffect(StatusType.AttackAddPer);
        ret += GetAsuraAbilValue(StatusType.AttackAddPer);
        ret += GetGuildPetEffect(StatusType.AttackAddPer);


        return ret;
    }

    public static float GetCostumeAttackPowerValue()
    {
        float ret = ServerData.costumeServerTable.GetCostumeAbility(StatusType.AttackAddPer);
        return ret;
    }

    public static float GetCalculatedAttackPower()
    {
        float ret = 0f;

        float baseAttackPower = GetBaseAttackPower();

        float baseAttackAddPercentValue = GetBaseAttackAddPercentValue();

        ret += baseAttackPower;

        ret += baseAttackPower * baseAttackAddPercentValue;

        return ret;
    }

    public static float GetWeaponEquipPercentValue(StatusType type)
    {
        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;

        var e = TableManager.Instance.WeaponData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if (e.Current.Value.Id != equipId) continue;
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Weaponeffectid, out var effectData) == false) continue;

            int currentLevel = ServerData.weaponTable.GetWeaponData(e.Current.Value.Stringid).level.Value;

            if (effectData.Equipeffecttype1 == (int)type)
            {
                ret += effectData.Equipeffectbase1;
                ret += currentLevel * effectData.Equipeffectvalue1;
            }
            if (effectData.Equipeffecttype2 == (int)type)
            {
                ret += effectData.Equipeffectbase2;
                ret += currentLevel * effectData.Equipeffectvalue2;
            }

            break;
        }

        return ret;
    }

    public static float GetWeaponHasPercentValue(StatusType type)
    {
        var e = TableManager.Instance.WeaponData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Weaponeffectid, out var effectData) == false) continue;

            var weaponServertable = ServerData.weaponTable.TableDatas[e.Current.Value.Stringid];

            if (weaponServertable.hasItem.Value == 0) continue;

            int currentLevel = ServerData.weaponTable.GetWeaponData(e.Current.Value.Stringid).level.Value;

            if (effectData.Haseffecttype1 == (int)type)
            {
                ret += effectData.Haseffectbase1;
                ret += currentLevel * effectData.Haseffectvalue1;
            }
            if (effectData.Haseffecttype2 == (int)type)
            {
                ret += effectData.Haseffectbase2;
                ret += currentLevel * effectData.Haseffectvalue2;
            }
        }

        if (ActiveSmithValue(type))
        {
            ret = ret * GetSmithValue(StatusType.WeaponHasUp);
        }
        else
        {

        }


        return ret;
    }

    public static float GetMagicBookEquipPercentValue(StatusType type)
    {
        var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;

        float ret = 0f;
        while (e.MoveNext())
        {
            if (e.Current.Value.Id != equipId) continue;
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Magicbookeffectid, out var effectData) == false) continue;

            int currentLevel = ServerData.magicBookTable.GetMagicBookData(e.Current.Value.Stringid).level.Value;

            if (effectData.Equipeffecttype1 == (int)type)
            {
                ret += effectData.Equipeffectbase1;
                ret += currentLevel * effectData.Equipeffectvalue1;
            }
            if (effectData.Equipeffecttype2 == (int)type)
            {
                ret += effectData.Equipeffectbase2;
                ret += currentLevel * effectData.Equipeffectvalue2;
            }
            break;
        }

        return ret;
    }

    public static float GetMagicBookHasPercentValue(StatusType type)
    {
        var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Magicbookeffectid, out var effectData) == false) continue;

            var magieBookServerData = ServerData.magicBookTable.TableDatas[e.Current.Value.Stringid];

            if (magieBookServerData.hasItem.Value == 0) continue;

            int currentLevel = ServerData.magicBookTable.GetMagicBookData(e.Current.Value.Stringid).level.Value;

            if (effectData.Haseffecttype1 == (int)type)
            {
                ret += effectData.Haseffectbase1;
                ret += currentLevel * effectData.Haseffectvalue1;
            }
            if (effectData.Haseffecttype2 == (int)type)
            {
                ret += effectData.Haseffectbase2;
                ret += currentLevel * effectData.Haseffectvalue2;
            }
            if (effectData.Haseffecttype3 == (int)type)
            {
                ret += effectData.Haseffectbase3;
                ret += currentLevel * effectData.Haseffectvalue3;
            }
        }


        if (ActiveSmithValue(type))
        {
            ret = ret * GetSmithValue(StatusType.NorigaeHasUp);
        }
        else
        {

        }

        return ret;
    }

    public static float GetSkillCollectionValue(StatusType type)
    {
        var e = TableManager.Instance.SkillData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if ((StatusType)e.Current.Value.Collectionabiltype != type) continue;

            int skillCurrentLevel = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][e.Current.Value.Id].Value;

            if (skillCurrentLevel != 0)
            {
                ret += skillCurrentLevel * e.Current.Value.Collectionvalue;
            }
        }

        return ret;
    }

    public static float GetWingAbilValue(StatusType type)
    {
        //사용안함
        return 0f;

        int currentWingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        if (currentWingIdx < 0f || currentWingIdx >= TableManager.Instance.WingTable.dataArray.Length) return 0f;

        var tableData = TableManager.Instance.WingTable.dataArray[currentWingIdx];

        float ret = 0f;

        for (int i = 0; i < tableData.Abilitytype.Length; i++)
        {
            if (tableData.Abilitytype[i] == (int)type)
            {
                ret += tableData.Abilityvalue[i];
            }
        }

        return ret;
    }
    public static float GetSkillHasValue(StatusType type)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.SkillTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Haseffecttype != (int)type) continue;

            int awakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][tableDatas[i].Id].Value;

            if (awakeNum == 0) continue;


            ret += awakeNum * tableDatas[i].Haseffectvalue;
        }

        return ret;
    }

    public static float GetMarbleValue(StatusType type)
    {
        float ret = 0f;

        bool isMarbleAwaked = ServerData.userInfoTable.TableDatas[UserInfoTable.marbleAwake].Value == 1;

        var tableDatas = TableManager.Instance.MarbleTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (ServerData.marbleServerTable.TableDatas[tableDatas[i].Stringid].hasItem.Value == 0) continue;

            for (int j = 0; j < tableDatas[i].Abilitytype.Length; j++)
            {
                if (tableDatas[i].Abilitytype[j] == (int)type)
                {
                    ret += isMarbleAwaked == false ? tableDatas[i].Abilityvalue[j] : tableDatas[i].Awakevalue[j];
                }
            }
        }

        return ret;
    }


    #endregion
    #region SkillDamage
    public static float GetSkillDamagePercentValue()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SkillDamage);
        ret += GetMagicBookEquipPercentValue(StatusType.SkillDamage);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.SkillDamage_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.SkillDamage);
        ret += GetSkillCollectionValue(StatusType.SkillDamage);
        ret += GetWingAbilValue(StatusType.SkillDamage);
        ret += GetPassiveSkillValue(StatusType.SkillDamage);
        ret += ServerData.petTable.GetStatusValue(StatusType.SkillDamage);

        ret += GetTitleAbilValue(StatusType.SkillDamage);
        ret += GetRelicHasEffect(StatusType.SkillDamage);

        ret += GetSinsuEquipEffect(StatusType.SkillDamage);
        ret += GetStageRelicHasEffect(StatusType.SkillDamage);
        ret += GetSonAbilHasEffect(StatusType.SkillDamage);
        ret += GetYachaSkillPercentValue();

        return ret;
    }

    private const string yachaKey = "weapon21";
    public static float GetYachaSkillPercentValue()
    {
        bool hasYacha = ServerData.weaponTable.TableDatas[yachaKey].hasItem.Value == 1;

        if (hasYacha == false) return 0f;

        return ServerData.statusTable.GetTableData(StatusTable.Level).Value * GameBalance.YachaSkillAddValuePerLevel;
    }
    public static float GetYachaIgnoreDefenseValue()
    {
        bool hasYacha = ServerData.weaponTable.TableDatas[yachaKey].hasItem.Value == 1;

        bool cockAwake = ServerData.userInfoTable.TableDatas[UserInfoTable.cockAwake].Value == 1;

        if (hasYacha == false || cockAwake == false) return 0f;

        return ServerData.statusTable.GetTableData(StatusTable.Level).Value * GameBalance.YachaIgnoreDefenseAddValuePerLevel;
    }

    public static float GetYachaChunSlashValue()
    {
        bool hasYacha = ServerData.weaponTable.TableDatas[yachaKey].hasItem.Value == 1;

        bool dogAwake = ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].Value == 1;

        if (hasYacha == false || dogAwake == false) return 0f;

        return ServerData.statusTable.GetTableData(StatusTable.Level).Value * GameBalance.YachaChunSlashAddValuePerLevel;
    }

    #endregion
    #region SkillCoolTime
    public static float GetSkillCoolTimeDecreaseValue()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SkillCoolTime);
        ret += GetMagicBookEquipPercentValue(StatusType.SkillCoolTime);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.SkillCoolTime_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.SkillCoolTime);
        ret += GetBuffValue(StatusType.SkillCoolTime);
        ret += GetYomulUpgradeValue(StatusType.SkillCoolTime);

        return ret;
    }
    #endregion
    #region DamBalance
    public static float GetDamBalanceAddValue()
    {
        float addValue1 = ServerData.statusTable.GetStatusValue(StatusTable.DamageBalance_memory);
        return addValue1;
    }

    #endregion
    #region Critical
    public static bool ActiveCritical()
    {
        return Random.value < GetCriticalProb();
    }
    public static bool ActiveSuperCritical()
    {
        return Random.value < GetSuperCriticalProb();
    }
    public static float GetCriticalProb()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalLevel_Gold);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalLevel_StatPoint);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalLevel_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.CriticalProb);
        ret += ServerData.petTable.GetStatusValue(StatusType.CriticalProb);
        ret += GetSkillCollectionValue(StatusType.CriticalProb);

        return ret;
    }

    public static float CriticalDam()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_Gold);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_StatPoint);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.CriticalDam);
        ret += GetWeaponHasPercentValue(StatusType.CriticalDam);
        ret += GetMagicBookHasPercentValue(StatusType.CriticalDam);
        ret += GetSkillCollectionValue(StatusType.CriticalDam);
        ret += GetWingAbilValue(StatusType.CriticalDam);
        ret += ServerData.petTable.GetStatusValue(StatusType.CriticalDam);
        ret += GetStageRelicHasEffect(StatusType.CriticalDam);
        ret += GetSonAbilHasEffect(StatusType.CriticalDam);
        ret += GetSinsuEquipEffect(StatusType.CriticalDam);


        return ret;
    }
    #endregion
    #region BuffEffect
    public static float GetGoldPlusValue()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.GoldGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.GoldGainPer);
        ret += GetBuffValue(StatusType.GoldGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.GoldGainPer);
        ret += GetMarbleValue(StatusType.GoldGainPer);
        ret += GetMagicBookHasPercentValue(StatusType.GoldGainPer);

        ret += GetTitleAbilValue(StatusType.GoldGainPer);
        ret += GetHotTimeBuffEffect(StatusType.GoldGainPer);

        ret += GetBuffValue(StatusType.GoldGainPer);
        ret += GetTitleAbilValue(StatusType.GoldGainPer);
        ret += GetGuildPetEffect(StatusType.GoldGainPer);

        return ret;
    }
    public static float GetGoldPlusValueExclusiveBuff()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.GoldGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.GoldGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.GoldGainPer);
        ret += GetMarbleValue(StatusType.GoldGainPer);
        ret += GetMagicBookHasPercentValue(StatusType.GoldGainPer);

        ret += GetTitleAbilValue(StatusType.GoldGainPer);
        ret += GetHotTimeBuffEffect(StatusType.GoldGainPer);
        ret += GetGuildPetEffect(StatusType.GoldGainPer);
        return ret;
    }
    public static float GetExpPlusValue()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.ExpGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.ExpGainPer);
        ret += GetBuffValue(StatusType.ExpGainPer);
        ret += GetMarbleValue(StatusType.ExpGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.ExpGainPer);

        ret += GetTitleAbilValue(StatusType.ExpGainPer);
        ret += GetHotTimeBuffEffect(StatusType.ExpGainPer);
        ret += GetGuildPetEffect(StatusType.ExpGainPer);

        return ret;
    }

    public static float GetExpPlusValueExclusiveBuff()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.ExpGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.ExpGainPer);
        ret += GetMarbleValue(StatusType.ExpGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.ExpGainPer);

        ret += GetTitleAbilValue(StatusType.ExpGainPer);
        ret += GetGuildPetEffect(StatusType.ExpGainPer);

        return ret;
    }
    public static float GetMagicStonePlusValue()
    {
        float ret = 0f;

        ret += GetHotTimeBuffEffect(StatusType.MagicStoneAddPer);
        ret += GetBuffValue(StatusType.MagicStoneAddPer);

        return ret;
    }
    public static float GetMarblePlusValue()
    {
        float ret = 0f;

        ret += GetHotTimeBuffEffect(StatusType.MarbleAddPer);
        ret += GetBuffValue(StatusType.MarbleAddPer);

        return ret;
    }

    public static float GetBuffValue(StatusType type)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if ((int)type == tableData[i].Bufftype)
            {
                //-1은 무한

                if (tableData[i].Yomulid == -1)
                {
                    if (ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.Value != 0f)
                    {
                        ret += tableData[i].Buffvalue;
                    }
                }
                //요물
                else
                {
                    if (ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 0)
                    {
                        if (ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.Value != 0f)
                        {
                            ret += tableData[i].Buffvalue;
                        }
                    }
                    //각성후
                    else
                    {
                        ret += tableData[i].Buffawakevalue;
                    }
                }
            }
        }

        return ret;
    }
    #endregion
    #region HP&MP
    public static float GetMaxHp()
    {
        float originHp = GetOriginHp();

        float hpAddPerValue = GetMaxHpPercentAddValue();

        return originHp + originHp * hpAddPerValue;
    }

    public static float GetOriginHp()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.Hp);
        ret += GetMagicBookEquipPercentValue(StatusType.Hp);

        ret += GetSinsuEquipEffect(StatusType.Hp);
        ret += GetRelicHasEffect(StatusType.Hp);

        return ret;
    }
    public static float GetMaxHpPercentAddValue()
    {
        float ret = 0f;
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.HpAddPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.HpAddPer);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpPer_StatPoint);
        ret += GetWingAbilValue(StatusType.HpAddPer);
        ret += GetPassiveSkillValue(StatusType.HpAddPer);

        ret += GetTitleAbilValue(StatusType.HpAddPer);
        ret += GetRelicHasEffect(StatusType.HpAddPer);

        ret += GetMagicBookEquipPercentValue(StatusType.HpAddPer);

        return ret;
    }
    public static float GetMaxMp()
    {
        float originMp = GetOriginMp();

        float mpAddPerValue = GetMaxMpPercentAddValue();

        return originMp + originMp * mpAddPerValue;
    }
    public static float GetOriginMp()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.MpLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.Mp);
        return ret;
    }
    public static float GetMaxMpPercentAddValue()
    {
        float ret = 0f;
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.MpAddPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.MpAddPer);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.MpPer_StatPoint);
        return ret;
    }

    public static float GetHpRecover()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpRecover_Gold);
        return ret;
    }
    public static float GetMpRecover()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.MpRecover_Gold);
        return ret;
    }

    #endregion

    public static float GetIgnoreDefenseValue()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.IgnoreDefense_memory);
        ret += GetWeaponEquipPercentValue(StatusType.IgnoreDefense);
        ret += GetWeaponHasPercentValue(StatusType.IgnoreDefense);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.IgnoreDefense);
        ret += ServerData.petTable.GetStatusValue(StatusType.IgnoreDefense);

        ret += GetTitleAbilValue(StatusType.IgnoreDefense);
        ret += GetYomulUpgradeValue(StatusType.IgnoreDefense);

        ret += GetBuffValue(StatusType.IgnoreDefense);

        ret += GetRelicHasEffect(StatusType.IgnoreDefense);
        ret += GetStageRelicHasEffect(StatusType.IgnoreDefense);

        ret += GetSinsuEquipEffect(StatusType.IgnoreDefense);

        ret += GetYachaIgnoreDefenseValue();

        ret += GetAsuraAbilValue(StatusType.IgnoreDefense);

        ret += GetPassiveSkillValue(StatusType.IgnoreDefense);

        return ret;
    }

    public static float GetPenetrateDefense()
    {
        float ret = 0f;

        ret += GetYomulUpgradeValue(StatusType.PenetrateDefense);

        ret += GetBuffValue(StatusType.PenetrateDefense);

        ret += GetStageRelicHasEffect(StatusType.PenetrateDefense);

        ret += GetPassiveSkillValue(StatusType.PenetrateDefense);

        return ret;
    }

    public static float GetSuperCriticalDamPer()
    {
        float ret = 0.5f;

        ret += GetSinsuEquipEffect(StatusType.SuperCritical1DamPer);
        ret += GetYomulUpgradeValue(StatusType.SuperCritical1DamPer);
        ret += GetStageRelicHasEffect(StatusType.SuperCritical1DamPer);
        ret += GetBuffValue(StatusType.SuperCritical1DamPer);
        ret += GetRelicHasEffect(StatusType.SuperCritical1DamPer);

        ret += ServerData.petTable.GetStatusValue(StatusType.SuperCritical1DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.ChunSlash_memory);

        ret += GetYachaChunSlashValue();

        ret += GetAsuraAbilValue(StatusType.SuperCritical1DamPer);

        return ret;
    }


    public static float GetSuperCriticalProb()
    {
        float ret = 0f;

        ret += GetYomulUpgradeValue(StatusType.SuperCritical1Prob);
        ret += GetBuffValue(StatusType.SuperCritical1Prob);
        ret += GetSinsuEquipEffect(StatusType.SuperCritical1Prob);

        return ret;
    }

    public static float GetSuperCritical2DamPer()
    {
        float ret = 0f;

        ret += GetWeaponHasPercentValue(StatusType.SuperCritical2DamPer);

        ret += GetFeelMulAddDam();

        ret += GetAsuraAbilValue(StatusType.SuperCritical2DamPer);

        ret += GetLeeMuGiAddDam();

        return ret;
    }

    public static float GetYomulUpgradeValue(StatusType type, bool onlyType1 = false, bool onlyType2 = false, int targetId = -1)
    {
        float ret = 0f;
        var tableDatas = TableManager.Instance.YomulAbilTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.yomulServerTable.TableDatas[tableDatas[i].Stringid];
            if (serverData.hasAbil.Value == 0) continue;
            if (targetId != -1 && i != targetId) continue;

            if (tableDatas[i].Abiltype == (int)type && onlyType2 == false)
            {
                if (type == StatusType.PenetrateDefense)
                {
                    float addValue = serverData.level.Value < 80 ? tableDatas[i].Abiladdvalue : tableDatas[i].Abiladdvalue * 2f;
                    ret += tableDatas[i].Abilvalue + (serverData.level.Value * addValue);
                }
                else
                {
                    ret += tableDatas[i].Abilvalue + (serverData.level.Value * tableDatas[i].Abiladdvalue);
                }
            }

            if (tableDatas[i].Abiltype2 == (int)type && onlyType1 == false)
            {
                if (type == StatusType.PenetrateDefense)
                {
                    float addValue = tableDatas[i].Abiladdvalue2;
                    ret += tableDatas[i].Abilvalue2 + (serverData.level.Value * addValue);
                }
                else
                {
                    ret += tableDatas[i].Abilvalue2 + (serverData.level.Value * tableDatas[i].Abiladdvalue2);
                }
            }

        }

        return ret;
    }

    public static float GetTitleAbilValue(StatusType type)
    {
        var e = ServerData.titleServerTable.TableDatas.GetEnumerator();

        float ret = 0f;

        while (e.MoveNext())
        {
            if (e.Current.Value.clearFlag.Value == 0) continue;

            var tableData = TableManager.Instance.TitleTable.dataArray[e.Current.Value.idx];

            if (type == (StatusType)tableData.Abiltype1)
            {
                if (tableData.Id == ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].Value)
                {
                    ret += tableData.Abilvalue1 * GameBalance.TitleEquipAddPer;
                }
                else
                {
                    ret += tableData.Abilvalue1;
                }
            }

            if (type == (StatusType)tableData.Abiltype2)
            {
                if (tableData.Id == ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].Value)
                {
                    ret += tableData.Abilvalue2 * GameBalance.TitleEquipAddPer;
                }
                else
                {
                    ret += tableData.Abilvalue2;
                }
            }
        }

        return ret;
    }

    public static float GetHotTimeBuffEffect(StatusType statusType)
    {
        if (ServerData.userInfoTable.IsWeekend() == false)
        {
            float ret = 0f;

            if (ServerData.userInfoTable.IsHotTime() == false) return 0f;

            if (statusType == StatusType.GoldGainPer)
            {
                ret = GameBalance.HotTime_Gold;
            }
            else if (statusType == StatusType.ExpGainPer)
            {
                ret = GameBalance.HotTime_Exp;
            }
            else if (statusType == StatusType.MagicStoneAddPer)
            {
                ret = GameBalance.HotTime_GrowthStone;
            }
            else if (statusType == StatusType.MarbleAddPer)
            {
                ret = GameBalance.HotTime_Marble;
            }

            return ret;
        }
        else
        {
            float ret = 0f;

            if (ServerData.userInfoTable.IsHotTime() == false) return 0f;

            if (statusType == StatusType.GoldGainPer)
            {
                ret = GameBalance.HotTime_Gold_Weekend;
            }
            else if (statusType == StatusType.ExpGainPer)
            {
                ret = GameBalance.HotTime_Exp_Weekend;
            }
            else if (statusType == StatusType.MagicStoneAddPer)
            {
                ret = GameBalance.HotTime_GrowthStone_Weekend;
            }
            else if (statusType == StatusType.MarbleAddPer)
            {
                ret = GameBalance.HotTime_Marble_Weekend;
            }

            return ret;
        }


    }

    public static float GetSinsuEquipEffect(StatusType statusType)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.PetEquipment.dataArray;

        int petEquipLevel = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.petEquipmentServerTable.TableDatas[tableDatas[i].Stringid];

            if (serverData.hasAbil.Value == 0) continue;

            if (tableDatas[i].Abiltype1 == (int)statusType)
            {
                ret += (tableDatas[i].Abilvalue1 + serverData.level.Value * tableDatas[i].Abiladdvalue1 + tableDatas[i].Leveladdvalue1 * petEquipLevel);
            }

            if (tableDatas[i].Abiltype2 == (int)statusType)
            {
                ret += (tableDatas[i].Abilvalue2 + serverData.level.Value * tableDatas[i].Abiladdvalue2 + tableDatas[i].Leveladdvalue2 * petEquipLevel);
            }
        }

        if (ActiveSmithValue(statusType))
        {
            ret = ret * GetSmithValue(StatusType.PetEquipHasUp);
        }
        else
        {

        }

        return ret;
    }

    private static bool ActiveSmithValue(StatusType statustype)
    {
        return statustype != StatusType.Damdecrease && statustype != StatusType.SuperCritical1Prob;
    }

    public static float GetRelicHasEffect(StatusType statusType)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.RelicTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid];

            if (serverData.level.Value == 0) continue;

            if (tableDatas[i].Abiltype != (int)statusType) continue;

            ret += serverData.level.Value * tableDatas[i].Abilvalue;
        }

        return ret;
    }

    public static float GetStageRelicHasEffect(StatusType statusType)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.StageRelic.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.stageRelicServerTable.TableDatas[tableDatas[i].Stringid];

            if (serverData.level.Value == 0) continue;

            if (tableDatas[i].Abiltype != (int)statusType) continue;

            ret += serverData.level.Value * tableDatas[i].Abilvalue;
        }

        return ret;
    }

    public static float GetSonAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.SonAbil.dataArray;

        int currentLevel = ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        return ret;
    }

    public static float GetSmithValue(StatusType statusType)
    {
        int currentExp = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value;

        if (statusType == StatusType.growthStoneUp)
        {
            if (currentExp < 1000)
            {
                return 0;
            }
            else
            {
                currentExp = Mathf.Min(currentExp, 60000);

                int divide = currentExp / 1000;

                return (1 + (divide)) * 10;
            }

        }
        else if (statusType == StatusType.WeaponHasUp ||
            statusType == StatusType.NorigaeHasUp ||
            statusType == StatusType.PetEquipHasUp)
        {
            return 1f + (currentExp / 2500) * 0.05f;
        }
        else if (statusType == StatusType.PetEquipProbUp)
        {
            currentExp = Mathf.Min(currentExp, 50000);

            int divide = currentExp / 10000;

            return divide * 10;
        }

        return 0f;
    }

    public static float GetFeelMulAddDam()
    {
        return ServerData.statusTable.GetTableData(StatusTable.FeelMul).Value * 0.1f;
    }
    public static float GetLeeMuGiAddDam()
    {
        return ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).Value * 0.2f;
    }

    public static string asuraKey0 = "a0";
    public static string asuraKey1 = "a1";
    public static string asuraKey2 = "a2";
    public static string asuraKey3 = "a3";
    public static string asuraKey4 = "a4";
    public static string asuraKey5 = "a5";

    public static ObscuredFloat asura0Value = 15000f;
    public static ObscuredFloat asura1Value = 25000f;
    public static ObscuredFloat asura2Value = 300f;
    public static ObscuredFloat asura3Value = 0.5f;
    public static ObscuredFloat asura4Value = 0.8f;
    public static ObscuredFloat asura5Value = 1.0f;

    public static float GetAsuraAbilValue(StatusType type)
    {
        switch (type)
        {
            case StatusType.AttackAddPer:
                {
                    if (ServerData.goodsTable.GetTableData(asuraKey0).Value == 0)
                    {
                        return 0f;
                    }

                    return asura0Value;
                }
                break;
            case StatusType.IgnoreDefense:
                {
                    if (ServerData.goodsTable.GetTableData(asuraKey1).Value == 0)
                    {
                        return 0f;
                    }

                    return asura1Value;
                }
                break;
            case StatusType.SuperCritical1DamPer:
                {
                    if (ServerData.goodsTable.GetTableData(asuraKey2).Value == 0)
                    {
                        return 0f;
                    }

                    return asura2Value;
                }
                break;
            case StatusType.SuperCritical2DamPer:
                {
                    float ret = 0f;

                    if (ServerData.goodsTable.GetTableData(asuraKey3).Value == 0)
                    {

                    }
                    else
                    {
                        ret += asura3Value;
                    }

                    if (ServerData.goodsTable.GetTableData(asuraKey4).Value == 0)
                    {

                    }
                    else
                    {
                        ret += asura4Value;
                    }

                    if (ServerData.goodsTable.GetTableData(asuraKey5).Value == 0)
                    {

                    }
                    else
                    {
                        ret += asura5Value;
                    }

                    return ret;
                }
                break;
        }

        return 0f;
    }

    public static float GetGuildPetEffect(StatusType type)
    {
        int petLevel = GuildManager.Instance.guildPetExp.Value;

        switch (type)
        {
            case StatusType.AttackAddPer:
                {
                    return petLevel * 0.1f;
                }
                break;
            case StatusType.ExpGainPer:
                {
                    return petLevel * 0.01f;
                }
                break;
            case StatusType.GoldGainPer:
                {
                    return petLevel * 0.01f;
                }
                break;
        }

        return 0f;
    }

    private static string adukCostumeKey = "costume26";
    private static string leeMuGiCostumeKey = "costume27";
    public static float DecreaseBossHp()
    {
        float ret = 0f;

        if (ServerData.costumeServerTable.TableDatas[adukCostumeKey].hasCostume.Value == true)
        {
            ret += 0.05f;
        }

        if (ServerData.costumeServerTable.TableDatas[leeMuGiCostumeKey].hasCostume.Value == true)
        {
            ret += 0.05f;
        }

        return ret;
    }
}
