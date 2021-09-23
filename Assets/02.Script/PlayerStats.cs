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
    PenetrateDefense
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

        return ret;
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
        ret += GetHotTimeBuffEffect(StatusType.ExpGainPer);

        return ret;
    }
    public static float GetMagicStonePlusValue()
    {
        float ret = 0f;
        ret += GetHotTimeBuffEffect(StatusType.MagicStoneAddPer);
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
                if (ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.Value != 0f)
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
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.Hp);
        ret += GetMagicBookEquipPercentValue(StatusType.Hp);

        ret += GetSinsuEquipEffect(StatusType.Hp);
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

        return ret;
    }

    public static float GetPenetrateDefense()
    {
        float ret = 0f;

        ret += GetYomulUpgradeValue(StatusType.PenetrateDefense);

        ret += GetBuffValue(StatusType.PenetrateDefense);

        return ret;
    }

    public static float GetYomulUpgradeValue(StatusType type)
    {
        float ret = 0f;
        var tableDatas = TableManager.Instance.YomulAbilTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.yomulServerTable.TableDatas[tableDatas[i].Stringid];
            if (serverData.hasAbil.Value == 0) continue;
            if (tableDatas[i].Abiltype != (int)type) continue;

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

        return ret;
    }

    public static float GetSinsuEquipEffect(StatusType statusType)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.PetEquipment.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.petEquipmentServerTable.TableDatas[tableDatas[i].Stringid];

            if (serverData.hasAbil.Value == 0) continue;

            if (tableDatas[i].Abiltype1 == (int)statusType) 
            {
                ret += (tableDatas[i].Abilvalue1 + serverData.level.Value * tableDatas[i].Abiladdvalue1);
            }

            if (tableDatas[i].Abiltype2 == (int)statusType)
            {
                ret += (tableDatas[i].Abilvalue2 + serverData.level.Value * tableDatas[i].Abiladdvalue2);
            }
        }

        return ret;
    }
}
