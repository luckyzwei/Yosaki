using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

public enum StatusWhere
{
    gold, statpoint, memory
}



public class StatusTable
{
    public static string Indate;
    public const string tableName = "Status";
    public const string Level = "Level";
    public const string SkillPoint = "SkillPoint";
    public const string StatPoint = "StatPoint";
    public const string Memory = "memory";

    public const string AttackLevel_Gold = "AttackLevel_Gold";
    public const string CriticalLevel_Gold = "CriticalLevel_Gold";
    public const string CriticalDamLevel_Gold = "CriticalDamLevel_Gold";
    public const string HpLevel_Gold = "HpLevel_Gold";
    public const string MpLevel_Gold = "MpLevel_Gold";
    public const string HpRecover_Gold = "HpRecover_Gold";
    public const string MpRecover_Gold = "MpRecover_Gold";

    public const string IntLevelAddPer_StatPoint = "IntLevelAddPer_StatPoint";
    public const string CriticalLevel_StatPoint = "CriticalLevel_StatPoint";
    public const string CriticalDamLevel_StatPoint = "CriticalDamLevel_StatPoint";
    public const string GoldGain_memory = "GoldGain_memory";
    public const string ExpGain_memory = "ExpGain_memory";
    public const string HpPer_StatPoint = "HpPer_StatPoint";
    public const string MpPer_StatPoint = "MpPer_StatPoint";

    public const string DamageBalance_memory = "DamageBalance_memory";
    public const string SkillDamage_memory = "SkillDamage_memory";
    public const string SkillCoolTime_memory = "SkillCoolTime_memory";
    public const string CriticalLevel_memory = "CriticalLevel_memory";
    public const string CriticalDamLevel_memory = "CriticalDamLevel_memory";
    public const string IgnoreDefense_memory = "IgnoreDefense_memory";
    public const string BossDamage_memory = "BossDamage_memory";
    public const string Son_Level = "Son_Level_Real";
    public const string PetEquip_Level = "PetEquip_Level";
    public const string ChunSlash_memory = "ChunSlash_memory";
    public const string PetAwakeLevel = "PetAwakeLevel";


    public const string Skill0_AddValue = "Sk0_Add";
    public const string Skill1_AddValue = "Sk1_Add";
    public const string Skill2_AddValue = "Sk2_Add";
    public const string SkillAdPoint = "Sk_AdPoint";
    public const string FeelMul = "FeelMul";



    private Dictionary<string, int> tableSchema = new Dictionary<string, int>()
    {
        {Level,1},
        {SkillPoint,GameBalance.SkillPointGet},
        {StatPoint,0},
        {Memory,0},

        {AttackLevel_Gold,0},
        {CriticalLevel_Gold,0},
        {CriticalDamLevel_Gold,0},
        {HpLevel_Gold,0},
        {MpLevel_Gold,0},
        {HpRecover_Gold,0},
        {MpRecover_Gold,0},

        //스텟초기화도 같이추가해
        {IntLevelAddPer_StatPoint,0},
        {CriticalLevel_StatPoint,0},
        {CriticalDamLevel_StatPoint,0},
        {HpPer_StatPoint,0},
        {MpPer_StatPoint,0},
        //스텟초기화도 같이추가해

        {DamageBalance_memory,0},
        {SkillDamage_memory,0},
        {SkillCoolTime_memory,0},
        {CriticalLevel_memory,0},
        {CriticalDamLevel_memory,0},
        {IgnoreDefense_memory,0},
        {BossDamage_memory,0},
        {GoldGain_memory,0},
        {ExpGain_memory,0},
        {Son_Level,0},
        {PetEquip_Level,0},
        {ChunSlash_memory,0},
        {PetAwakeLevel,0},

        {Skill0_AddValue,0},
        {Skill1_AddValue,0},
        {Skill2_AddValue,0},
        {SkillAdPoint,0},
        {FeelMul,0},
    };

    private Dictionary<string, ReactiveProperty<int>> tableDatas = new Dictionary<string, ReactiveProperty<int>>();

    public void SyncAllData()
    {
        Param param = new Param();

        var e = tableSchema.GetEnumerator();
        while (e.MoveNext())
        {
            param.Add(e.Current.Key, tableDatas[e.Current.Key].Value);
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
        {
            if (bro.IsSuccess() == false)
            {
                PopupManager.Instance.ShowAlarmMessage("데이터 동기화 실패\n재접속 후에 다시 시도해보세요");
                return;
            }
        });
    }

    public ReactiveProperty<int> GetTableData(string key)
    {
        return tableDatas[key];
    }
    public float GetStatusValue(string key)
    {
        return GetStatusValue(key, tableDatas[key].Value);
    }
    public float GetStatusValue(string key, float level)
    {
        if (TableManager.Instance.StatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                #region Gold
                case AttackLevel_Gold:
                    {
                        return Mathf.Pow(level, 1.07f) * 2f + 10;
                    }
                    break;
                case CriticalLevel_Gold:
                    {
                        return level * 0.0002f;
                    }
                    break;
                case CriticalDamLevel_Gold:
                    {
                        return level * 0.01f;
                    }
                    break;

                case HpLevel_Gold:
                    {
                        return GameBalance.initHp + level * 50f;
                    }
                    break;
                case MpLevel_Gold:
                    {
                        //합 무조건 100
                        return GameBalance.initMp + level * 50f;
                    }
                    break;
                case HpRecover_Gold:
                    {
                        return level * 0.0002f;
                    }
                    break;
                case MpRecover_Gold:
                    {
                        return level * 0.0001f;
                    }
                    break;
                #endregion
                #region Stat
                case IntLevelAddPer_StatPoint:
                    {
                        return level * 0.015f;
                    }
                    break;
                case CriticalLevel_StatPoint:
                    {
                        return level * 0.0005f;
                    }
                    break;
                case CriticalDamLevel_StatPoint:
                    {
                        return level * 0.015f;
                    }
                    break;
                case ExpGain_memory:
                    {
                        return level * 6f;
                    }
                    break;
                case GoldGain_memory:
                    {
                        return level * 6f;
                    }
                    break;
                case HpPer_StatPoint:
                    {
                        return level * 0.005f;
                    }
                    break;
                case MpPer_StatPoint:
                    {
                        return level * 0.005f;
                    }
                    break;
                #endregion
                #region Memory
                case DamageBalance_memory:
                    {
                        return level * 0.002f;
                    }
                    break;
                case SkillDamage_memory:
                    {
                        return level * 0.2f;
                    }
                    break;
                case SkillCoolTime_memory:
                    {
                        return level * 0.0005f;
                    }
                    break;
                case CriticalLevel_memory:
                    {
                        return level * 0.001f;
                    }
                    break;
                case CriticalDamLevel_memory:
                    {
                        return level * 0.01f;
                    }
                    break;
                case IgnoreDefense_memory:
                    {
                        return level;
                    }
                    break;
                case BossDamage_memory:
                    {
                        return level * 0.05f;
                    }
                case ChunSlash_memory:
                    {
                        return level * 0.0005f;
                    }

                #endregion
                default:
                    {
                        return 0f;
                    }
                    break;
            }
        }
        else
        {
            return 0f;
        }

        return 0f;
    }

    public float GetStatusUpgradePrice(string key, int level)
    {
        if (TableManager.Instance.StatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                case AttackLevel_Gold:
                    {
                        //7월 12일버전
                        //return (Mathf.Pow(level, 2.9f + (level / 1000) * 0.1f));

                        return (Mathf.Pow(level, 2.7f + (level / 1000) * 0.1f));
                    }
                    break;
                case CriticalDamLevel_Gold:
                case CriticalLevel_Gold:
                case HpLevel_Gold:
                case MpLevel_Gold:
                case HpRecover_Gold:
                case MpRecover_Gold:
                    {
                        //7월 12일버전
                        //return Mathf.Pow(level, 3.0f + (level / 100) * 0.1f);
                        return Mathf.Pow(level, 2.9f + (level / 100) * 0.1f);
                    }
                    break;
            }
        }
        else
        {
            Debug.LogError($"key {key} is not exist in GetStatusUpgradePrice");
            return -1f;
        }

        return -1f;
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
         {
             // 이후 처리
             if (callback.IsSuccess() == false)
             {
                 Debug.LogError("LoadStatusFailed");
                 PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                 return;
             }

             var rows = callback.Rows();

             //맨처음 초기화
             if (rows.Count <= 0)
             {
                 Param defultValues = new Param();

                 var e = tableSchema.GetEnumerator();

                 while (e.MoveNext())
                 {
                     defultValues.Add(e.Current.Key, e.Current.Value);
                     tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(e.Current.Value));
                 }

                 var bro = Backend.GameData.Insert(tableName, defultValues);

                 if (bro.IsSuccess() == false)
                 {
                     // 이후 처리
                     ServerData.ShowCommonErrorPopup(bro, Initialize);
                     return;
                 }
                 else
                 {

                     var jsonData = bro.GetReturnValuetoJSON();
                     if (jsonData.Keys.Count > 0)
                     {

                         Indate = jsonData[0].ToString();

                     }

                     // data.
                     // statusIndate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                 }

                 return;
             }
             //나중에 칼럼 추가됐을때 업데이트
             else
             {
                 Param defultValues = new Param();
                 int paramCount = 0;

                 JsonData data = rows[0];

                 if (data.Keys.Contains(ServerData.inDate_str))
                 {
                     Indate = data[ServerData.inDate_str][ServerData.format_string].ToString();
                 }

                 var e = tableSchema.GetEnumerator();

                 for (int i = 0; i < data.Keys.Count; i++)
                 {
                     while (e.MoveNext())
                     {
                         if (data.Keys.Contains(e.Current.Key))
                         {
                             //값로드
                             var value = data[e.Current.Key][ServerData.format_Number].ToString();
                             tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(Int32.Parse(value)));
                         }
                         else
                         {
                             defultValues.Add(e.Current.Key, e.Current.Value);
                             tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(e.Current.Value));
                             paramCount++;
                         }
                     }
                 }

                 if (paramCount != 0)
                 {
                     var bro = Backend.GameData.Update(tableName, Indate, defultValues);

                     if (bro.IsSuccess() == false)
                     {
                         ServerData.ShowCommonErrorPopup(bro, Initialize);
                         return;
                     }
                 }

             }
         });
    }

    public void UpData(string key, bool localOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, localOnly);
    }

    public void UpData(string key, int data, bool localOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }
        tableDatas[key].Value = data;

        if (localOnly == false)
        {
            Param param = new Param();
            param.Add(key, tableDatas[key].Value);

            SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
            {
                if (e.IsSuccess() == false)
                {
                    Debug.Log($"Status {key} up failed");
                    return;
                }
            });
        }
    }
}
