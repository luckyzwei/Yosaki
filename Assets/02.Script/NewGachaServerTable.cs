using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BackEnd;
using LitJson;
using System.Linq;


public class NewGachaServerData
{
    public int idx;
    public ReactiveProperty<int> hasItem;
    public ReactiveProperty<int> level;
    public ReactiveProperty<int> amount;
    public ReactiveProperty<int> getReward0;
    public ReactiveProperty<int> getReward1;
    public string ConvertToString()
    {
        return $"{idx},{hasItem.Value},{level.Value},{amount.Value},{getReward0.Value},{getReward1.Value}";
    }
}

public class NewGachaServerTable
{
    public static string Indate;
    public static string tableName = "New_Gacha";

    private ReactiveDictionary<string, NewGachaServerData> tableDatas = new ReactiveDictionary<string, NewGachaServerData>();
    public ReactiveDictionary<string, NewGachaServerData> TableDatas => tableDatas;

    public NewGachaServerData GetNewGachaData(string idx)
    {
        if (tableDatas.TryGetValue(idx, out var data))
        {
            return data;
        }
        else
        {
            return null;
        }
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

                var table = TableManager.Instance.NewGachaTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var newGachaData = new NewGachaServerData();
                    newGachaData.idx = table[i].Id;
                    newGachaData.hasItem = new ReactiveProperty<int>(0);
                    newGachaData.level = new ReactiveProperty<int>(0);
                    newGachaData.amount = new ReactiveProperty<int>(0);
                    newGachaData.getReward0 = new ReactiveProperty<int>(0);
                    newGachaData.getReward1 = new ReactiveProperty<int>(0);
                    tableDatas.Add(table[i].Stringid, newGachaData);
                    defultValues.Add(table[i].Stringid, newGachaData.ConvertToString());
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

                var table = TableManager.Instance.NewGachaTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var newGacha = new NewGachaServerData();

                        var splitData = value.Split(',');

                        newGacha.idx = int.Parse(splitData[0]);
                        newGacha.hasItem = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        newGacha.level = new ReactiveProperty<int>(int.Parse(splitData[2]));
                        newGacha.amount = new ReactiveProperty<int>(int.Parse(splitData[3]));


                        if (splitData.Length < 6)
                        {
                            newGacha.getReward0 = new ReactiveProperty<int>(0);
                            newGacha.getReward1 = new ReactiveProperty<int>(0);
                            paramCount++;
                            defultValues.Add(table[i].Stringid, newGacha.ConvertToString());
                        }
                        else
                        {
                            newGacha.getReward0 = new ReactiveProperty<int>(int.Parse(splitData[4]));
                            newGacha.getReward1 = new ReactiveProperty<int>(int.Parse(splitData[5]));
                        }

                        tableDatas.Add(table[i].Stringid, newGacha);
                    }
                    else
                    {

                        var newGachaData = new NewGachaServerData();
                        newGachaData.idx = table[i].Id;
                        newGachaData.hasItem = new ReactiveProperty<int>(0);
                        newGachaData.level = new ReactiveProperty<int>(0);
                        newGachaData.amount = new ReactiveProperty<int>(0);
                        newGachaData.getReward0 = new ReactiveProperty<int>(0);
                        newGachaData.getReward1 = new ReactiveProperty<int>(0);

                        tableDatas.Add(table[i].Stringid, newGachaData);
                        defultValues.Add(table[i].Stringid, newGachaData.ConvertToString());
                        paramCount++;
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

    public void UpData(NewGachaTableData newGachaData, int addNum)
    {
        if (tableDatas[newGachaData.Stringid].hasItem.Value == 0)
        {
            tableDatas[newGachaData.Stringid].hasItem.Value = 1;
        }

        tableDatas[newGachaData.Stringid].amount.Value += addNum;
    }

    public int GetCurrentNewGachaCount(string idx)
    {
        return tableDatas[idx].amount.Value;
    }

    public float GetNewGachaEffectValue(string id, float baseValue, float addValue, int level = -1)
    {
        if (level == -1)
        {
            return baseValue + addValue * tableDatas[id].level.Value;
        }
        else
        {
            return baseValue + addValue * level;
        }
    }

    public void SyncToServerAll(List<int> updateList = null)
    {
        Param defultValues = new Param();

        var table = TableManager.Instance.NewGachaTable.dataArray;

        for (int i = 0; i < table.Length; i++)
        {
            if (updateList != null && updateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            defultValues.Add(key, tableDatas[key].ConvertToString());
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, bro =>
        {
            if (bro.IsSuccess() == false)
            {
                ServerData.ShowCommonErrorPopup(bro, () => { SyncToServerAll(updateList); });
                return;
            }
        });
    }

    public float GetNewGachaLevelUpPrice(string idx)
    {
        int level = tableDatas[idx].level.Value;
        int id = tableDatas[idx].idx;
        level += 1;

        if (id < 16)
        {
            return Mathf.Pow(level, 3.54f + (float)id * 0.015f);
        }
        //영물
        else if (id == 20)
        {
            return Mathf.Pow(level, 3.55f + (float)id * 0.015f);
        }
        else
        {
            return Mathf.Pow(level, 3.65f + (float)id * 0.015f);
        }


    }
}
