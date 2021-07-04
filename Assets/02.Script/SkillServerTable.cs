using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BackEnd;
using LitJson;
using System.Linq;

public class SkillServerTable
{
    public static string Indate;
    public static string tableName = "Skill";

    public static string SkillHasAmount = "SkillHasAmount";
    public static string SkillAlreadyHas = "SkillAlreadyHas";
    public static string SkillAwakeNum = "SkillAwakeNum";
    public static string SkillLevel = "SkillLevel";
    public static string SkillSlotIdx = "SkillSlotIdx";

    private Dictionary<string, List<int>> tableSchema = new Dictionary<string, List<int>>()
    {
        {SkillHasAmount,new List<int>()},
        {SkillAlreadyHas,new List<int>()},
        {SkillAwakeNum,new List<int>()},
        {SkillLevel,new List<int>()},
        {SkillSlotIdx,new List<int>(){0,-1,-1,-1,-1 }}
    };

    private Dictionary<string, List<ReactiveProperty<int>>> tableDatas = new Dictionary<string, List<ReactiveProperty<int>>>();
    public Dictionary<string, List<ReactiveProperty<int>>> TableDatas => tableDatas;

    public List<ReactiveProperty<int>> SelectedSkillIdx => tableDatas[SkillSlotIdx];

    public void UpdateSelectedSkillIdx(List<int> selectedSkillIdx)
    {
        List<ReactiveProperty<int>> skillData = new List<ReactiveProperty<int>>();

        for (int i = 0; i < selectedSkillIdx.Count; i++)
        {
            skillData.Add(new ReactiveProperty<int>(selectedSkillIdx[i]));
        }

        tableDatas[SkillSlotIdx] = skillData;

        whenSelectedSkillIdxChanged.Execute(tableDatas[SkillSlotIdx]);

        SyncSkillSlotIdxToServer();
    }

    public void RemoveSkillInEquipList(int idx)
    {
        for (int i = 0; i < tableDatas[SkillSlotIdx].Count; i++)
        {
            if (tableDatas[SkillSlotIdx][i].Value == idx)
            {
                tableDatas[SkillSlotIdx][i].Value = -1;
            }
        }

        whenSelectedSkillIdxChanged.Execute(tableDatas[SkillSlotIdx]);

        SyncSkillSlotIdxToServer();
    }

    public bool AlreadyEquipedSkill(int idx)
    {
        for (int i = 0; i < tableDatas[SkillSlotIdx].Count; i++)
        {
            if (tableDatas[SkillSlotIdx][i].Value == idx)
            {
                return true;
            }
        }

        return false;
    }

    public ReactiveCommand<List<ReactiveProperty<int>>> whenSelectedSkillIdxChanged = new ReactiveCommand<List<ReactiveProperty<int>>>();

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
                     List<ReactiveProperty<int>> firstData = new List<ReactiveProperty<int>>();

                     if (e.Current.Key != SkillSlotIdx)
                     {
                         var tableData = TableManager.Instance.SkillData.GetEnumerator();

                         if (e.Current.Key != SkillAwakeNum)
                         {
                             while (tableData.MoveNext())
                             {
                                 firstData.Add(new ReactiveProperty<int>(0));
                             }
                         }
                         else
                         {
                             bool isFirst = true;
                             while (tableData.MoveNext())
                             {
                                 int initValue = isFirst ? 1 : 0;
                                 firstData.Add(new ReactiveProperty<int>(initValue));
                                 isFirst = false;
                             }
                         }

                         defultValues.Add(e.Current.Key, firstData.Select(reactiveValue => reactiveValue.Value).ToList());
                     }
                     //스킬 슬롯 인덱스
                     else
                     {
                         defultValues.Add(e.Current.Key, e.Current.Value);

                         for (int i = 0; i < e.Current.Value.Count; i++)
                         {
                             var datavalue = new ReactiveProperty<int>(e.Current.Value[i]);
                             firstData.Add(datavalue);
                         }
                     }

                     tableDatas.Add(e.Current.Key, firstData);
                 }

                 var bro = Backend.GameData.Insert(tableName, defultValues);

                 if (bro.IsSuccess() == false)
                 {
                     // 이후 처리
                     DatabaseManager.ShowCommonErrorPopup(bro, Initialize);
                     return;
                 }
                 else
                 {

                     var jsonData = bro.GetReturnValuetoJSON();
                     if (jsonData.Keys.Count > 0)
                     {

                         Indate = jsonData[0].ToString();

                     }
                 }

                 return;
             }
             //나중에 칼럼 추가됐을때 업데이트
             else
             {
                 Param defultValues = new Param();
                 int paramCount = 0;

                 JsonData data = rows[0];

                 if (data.Keys.Contains(DatabaseManager.inDate_str))
                 {
                     Indate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                 }

                 var e = tableSchema.GetEnumerator();

                 for (int i = 0; i < data.Keys.Count; i++)
                 {
                     while (e.MoveNext())
                     {
                         if (data.Keys.Contains(e.Current.Key))
                         {
                             //값로드
                             var value = data[e.Current.Key][DatabaseManager.format_list];
                             if (value.IsArray)
                             {
                                 List<ReactiveProperty<int>> loadedData = new List<ReactiveProperty<int>>();
                                 for (int j = 0; j < value.Count; j++)
                                 {
                                     var intData = Int32.Parse(value[j][DatabaseManager.format_Number].ToString());
                                     loadedData.Add(new ReactiveProperty<int>(intData));
                                 }

                                 tableDatas.Add(e.Current.Key, loadedData);
                             }
                         }
                         else
                         {
                             List<ReactiveProperty<int>> inputData = new List<ReactiveProperty<int>>();
                             for (int j = 0; j < e.Current.Value.Count; j++)
                             {
                                 inputData.Add(new ReactiveProperty<int>(e.Current.Value[j]));
                             }

                             tableDatas.Add(e.Current.Key, inputData);
                         }
                     }
                 }


                 //스킬테이블 값 추가됐을때 처리
                 e = tableSchema.GetEnumerator();
                 while (e.MoveNext())
                 {
                     if (e.Current.Key ==SkillHasAmount || e.Current.Key == SkillAlreadyHas || e.Current.Key == SkillAwakeNum || e.Current.Key == SkillLevel)
                     {
                         //신규스킬
                         List<ReactiveProperty<int>> firstData = new List<ReactiveProperty<int>>();

                         int needAddCount = TableManager.Instance.SkillData.Count - tableDatas[e.Current.Key].Count;

                         for (int i = 0; i < tableDatas[e.Current.Key].Count; i++)
                         {
                             firstData.Add(new ReactiveProperty<int>(tableDatas[e.Current.Key][i].Value));
                         }

                         for (int j = 0; j < needAddCount; j++)
                         {
                             firstData.Add(new ReactiveProperty<int>(0));
                             tableDatas[e.Current.Key].Add(new ReactiveProperty<int>(0));
                             paramCount++;
                         }

                         defultValues.Add(e.Current.Key, firstData.Select(reactiveValue => reactiveValue.Value).ToList());
                     }
                 }
                 //


                 if (paramCount != 0)
                 {
                     var bro = Backend.GameData.Update(tableName, Indate, defultValues);

                     if (bro.IsSuccess() == false)
                     {
                         DatabaseManager.ShowCommonErrorPopup(bro, Initialize);
                         return;
                     }
                 }

             }
         });
    }

    private List<int> skillSlotSyncData = new List<int>();
    private List<int> skillAmountSyncData = new List<int>();
    public void SyncSkillSlotIdxToServer()
    {
        skillSlotSyncData.Clear();

        Param param = new Param();

        for (int i = 0; i < tableDatas[SkillSlotIdx].Count; i++)
        {
            skillSlotSyncData.Add(tableDatas[SkillSlotIdx][i].Value);
        }

        param.Add(SkillSlotIdx, skillSlotSyncData);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.LogError($"Skill slot update failed");
                return;
            }
        });
    }

    public bool HasSkill(int skillIdx)
    {
        return true;
    }
    public int GetSkillAwakeNum(int idx)
    {
        return (tableDatas[SkillAwakeNum][idx].Value);
    }
    public int GetSkillMaxLevel(int idx)
    {
        return (tableDatas[SkillAwakeNum][idx].Value) * GameBalance.SkillAwakePlusNum;
    }
    public int GetSkillCurrentLevel(int idx)
    {
        return tableDatas[SkillLevel][idx].Value;
    }

    //UI팝업에서도 쓰는부분이라 함부로 건들면 안됨
    public float GetSkillDamagePer(int idx, int addLevel = 0, bool applySkillDamAbility = true)
    {
        int currentLevel = GetSkillCurrentLevel(idx);

        currentLevel += addLevel;

        float originDamage = (TableManager.Instance.SkillData[idx].Damageper + TableManager.Instance.SkillData[idx].Damageaddvalue * (currentLevel));

        float addDamageValue = 0f;

        if (applySkillDamAbility)
        {
            addDamageValue = PlayerStats.GetSkillDamagePercentValue();
        }

        float ret = originDamage + originDamage * addDamageValue;

        return ret;
    }

    public void UpdateSkillAmountLocal(SkillTableData skillData, int addAmount)
    {
        tableDatas[SkillHasAmount][skillData.Id].Value += addAmount;
        tableDatas[SkillAlreadyHas][skillData.Id].Value = 1;
    }

    public void UpdateSKillAmount()
    {
        skillAmountSyncData.Clear();

        Param param = new Param();

        for (int i = 0; i < tableDatas[SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(tableDatas[SkillHasAmount][i].Value);
        }

        param.Add(SkillHasAmount, skillAmountSyncData);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.LogError($"Skill slot update failed");
                return;
            }
        });
    }
}
