using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//삭제하거나순서절대바꾸면안됨
public enum StatusType
{
    AttackAddPer,// icon
    CriticalProb,//icon
    CriticalDam,//
    SkillCoolTime,///icon
    SkillDamage, //icon
    MoveSpeed,
    DamBalance,
    HpAddPer,//icon
    MpAddPer,//icon
    GoldGainPer,//icon
    ExpGainPer,//icon
    IntAdd,//icon
    Hp,//icon
    Mp,//icon
    HpRecover,//icon
    MpRecover, //icon
    MagicStoneAddPer, //icon
    Damdecrease,
    IgnoreDefense,
    DashCount,
    DropProbAddPer,
    BossDamAddPer,
    SkillAttackCount
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

        float totalPower =
           ((baseAttack + baseAttack * baseAttackPer)
            * (Mathf.Max(criProb, 0.01f) * 100f * Mathf.Max(criDam, 0.01f))
            * (Mathf.Max(skillDam, 0.01f) * 100f)
            * (Mathf.Max(coolTIme, 0.01f)) * 100)
            + ((hpBase + hpBase * hpAddPer) + (mpBase + mpBase * mpAddPer));

        return totalPower * 0.01f;
    }

    public static float GetPassiveSkillValue(StatusType statusType)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            var serverData = DatabaseManager.passiveServerTable.TableDatas[tableData[i].Stringid];

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

        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.AttackLevel_Gold);
        //  ret += GetCollectionAbilValue(StatusType.IntAdd);
        ret += DatabaseManager.petTable.GetStatusValue(StatusType.IntAdd);
        ret += GetWeaponEquipPercentValue(StatusType.IntAdd);
        ret += GetMagicBookCollectionValue(StatusType.IntAdd);
        ret += GetWingAbilValue(StatusType.IntAdd);
        ret += GetPassiveSkillValue(StatusType.IntAdd);

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
                ret += DatabaseManager.collectionTable.GetCollectionAbilValue(enemyTable[i]);
            }
        }

        return ret;
    }


    public static float GetBaseAttackAddPercentValue()
    {
        float ret = 0f;

        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.IntLevelAddPer_StatPoint);
        ret += GetWeaponHasPercentValue(StatusType.AttackAddPer);
        ret += GetMagicBookHasPercentValue(StatusType.AttackAddPer);
        ret += GetCostumeAttackPowerValue();
        ret += GetMagicBookCollectionValue(StatusType.AttackAddPer);
        ret += GetWingAbilValue(StatusType.AttackAddPer);
        ret += GetPassiveSkillValue(StatusType.AttackAddPer);
        return ret;
    }

    public static float GetCostumeAttackPowerValue()
    {
        float ret = DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.AttackAddPer);
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
        int equipId = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;

        var e = TableManager.Instance.WeaponData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if (e.Current.Value.Id != equipId) continue;
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Weaponeffectid, out var effectData) == false) continue;

            int currentLevel = DatabaseManager.weaponTable.GetWeaponData(e.Current.Value.Stringid).level.Value;

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

            int currentLevel = DatabaseManager.weaponTable.GetWeaponData(e.Current.Value.Stringid).level.Value;

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

        return ret;
    }

    public static float GetMagicBookEquipPercentValue(StatusType type)
    {
        var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        int equipId = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;

        float ret = 0f;
        while (e.MoveNext())
        {
            if (e.Current.Value.Id != equipId) continue;
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Magicbookeffectid, out var effectData) == false) continue;

            int currentLevel = DatabaseManager.magicBookTable.GetMagicBookData(e.Current.Value.Stringid).level.Value;

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

            int currentLevel = DatabaseManager.magicBookTable.GetMagicBookData(e.Current.Value.Stringid).level.Value;

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

        return ret;
    }

    public static float GetMagicBookCollectionValue(StatusType type)
    {
        var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if ((StatusType)e.Current.Value.Collectionabiltype != type) continue;

            var magicBookServerData = DatabaseManager.magicBookTable.GetMagicBookData(e.Current.Value.Stringid);

            if (magicBookServerData.collectLevel.Value != 0)
            {
                ret += magicBookServerData.collectLevel.Value * e.Current.Value.Collectionvalue;
            }
        }

        return ret;
    }

    public static float GetWingAbilValue(StatusType type)
    {
        //사용안함
        return 0f;

        int currentWingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

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

    public static float GetMarbleValue(StatusType type)
    {
        float ret = 0f;

        bool isMarbleAwaked = DatabaseManager.userInfoTable.TableDatas[UserInfoTable.marbleAwake].Value == 1;

        var tableDatas = TableManager.Instance.MarbleTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            for (int j = 0; j < tableDatas[i].Abilitytype.Length; j++)
            {
                if (tableDatas[i].Abilitytype[j] == (int)type)
                {
                    ret += isMarbleAwaked == false ? tableDatas[i].Abilityvalue[j] : tableDatas[i].Abilityvalue[j] * tableDatas[i].Awakevalue;
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
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.SkillDamage_memory);
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.SkillDamage);
        ret += GetMagicBookCollectionValue(StatusType.SkillDamage);
        ret += GetWingAbilValue(StatusType.SkillDamage);
        ret += GetPassiveSkillValue(StatusType.SkillDamage);

        return ret;
    }
    #endregion
    #region SkillCoolTime
    public static float GetSkillCoolTimeDecreaseValue()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SkillCoolTime);
        ret += GetMagicBookEquipPercentValue(StatusType.SkillCoolTime);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.SkillCoolTime_memory);
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.SkillCoolTime);

        return ret;
    }
    #endregion
    #region DamBalance
    public static float GetDamBalanceAddValue()
    {
        float addValue1 = DatabaseManager.statusTable.GetStatusValue(StatusTable.DamageBalance_memory);
        return addValue1;
    }

    #endregion
    #region Critical
    public static bool ActiveCritical()
    {
        return Random.value < GetCriticalProb();
    }
    public static float GetCriticalProb()
    {
        float ret = 0f;
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.CriticalLevel_Gold);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.CriticalLevel_StatPoint);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.CriticalLevel_memory);
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.CriticalProb);
        ret += DatabaseManager.petTable.GetStatusValue(StatusType.CriticalProb);
        ret += GetMagicBookCollectionValue(StatusType.CriticalProb);

        return ret;
    }

    public static float CriticalDam()
    {
        float ret = 0f;

        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_Gold);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_StatPoint);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_memory);
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.CriticalDam);
        ret += GetWeaponHasPercentValue(StatusType.CriticalDam);
        ret += GetMagicBookHasPercentValue(StatusType.CriticalDam);
        ret += GetMagicBookCollectionValue(StatusType.CriticalDam);
        ret += GetWingAbilValue(StatusType.CriticalDam);

        return ret;
    }
    #endregion
    #region BuffEffect
    public static float GetGoldPlusValue()
    {
        float ret = 0f;
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.GoldGain_StatPoint);
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.GoldGainPer);
        ret += GetBuffValue(StatusType.GoldGainPer);
        return ret;
    }
    public static float GetExpPlusValue()
    {
        float ret = 0f;
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.ExpGain_StatPoint);
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.ExpGainPer);
        ret += GetBuffValue(StatusType.ExpGainPer);

        return ret;
    }
    public static float GetMagicStonePlusValue()
    {
        float ret = 0f;
        ret += GetBuffValue(StatusType.MagicStoneAddPer);
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
                if (DatabaseManager.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.Value != 0f)
                {
                    ret += tableData[i].Buffvalue;
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
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.HpLevel_Gold);
        ret += DatabaseManager.petTable.GetStatusValue(StatusType.Hp);
        return ret;
    }
    public static float GetMaxHpPercentAddValue()
    {
        float ret = 0f;
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.HpAddPer);
        ret += DatabaseManager.petTable.GetStatusValue(StatusType.HpAddPer);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.HpPer_StatPoint);
        ret += GetWingAbilValue(StatusType.HpAddPer);
        ret += GetPassiveSkillValue(StatusType.HpAddPer);

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
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.MpLevel_Gold);
        ret += DatabaseManager.petTable.GetStatusValue(StatusType.Mp);
        return ret;
    }
    public static float GetMaxMpPercentAddValue()
    {
        float ret = 0f;
        ret += DatabaseManager.costumeServerTable.GetCostumeAbility(StatusType.MpAddPer);
        ret += DatabaseManager.petTable.GetStatusValue(StatusType.MpAddPer);
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.MpPer_StatPoint);
        return ret;
    }

    public static float GetHpRecover()
    {
        float ret = 0f;
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.HpRecover_Gold);
        return ret;
    }
    public static float GetMpRecover()
    {
        float ret = 0f;
        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.MpRecover_Gold);
        return ret;
    }

    #endregion

    public static float GetIgnoreDefenseValue()
    {
        float ret = 0f;

        ret += DatabaseManager.statusTable.GetStatusValue(StatusTable.IgnoreDefense_memory);

        return ret;
    }
}
