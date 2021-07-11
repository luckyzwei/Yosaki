using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using static UiGachaResultView;

[System.Serializable]
public class WeaponServerData
{
    public int idx;
    public ReactiveProperty<int> hasItem;
    public ReactiveProperty<int> level;
    public ReactiveProperty<int> amount;

    public string ConvertToString()
    {
        return $"{idx},{hasItem.Value},{level.Value},{amount.Value}";
    }
}

public class WeaponTable
{
    public static string Indate;
    public static string tableName = "Weapon";


    private ReactiveDictionary<string, WeaponServerData> tableDatas = new ReactiveDictionary<string, WeaponServerData>();

    public ReactiveDictionary<string, WeaponServerData> TableDatas => tableDatas;

    public float GetWeaponEffectValue(string id, float baseValue, float addValue, int level = -1)
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
    public WeaponServerData GetWeaponData(string idx)
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
    public int GetCurrentWeaponCount(string idx)
    {
        return tableDatas[idx].amount.Value;
    }

    public float GetWeaponLevelUpPrice(string idx)
    {
        int level = tableDatas[idx].level.Value;
        int id = tableDatas[idx].idx;
        level += 1;

        return Mathf.Pow(level, 3.35f + (float)id * 0.015f);
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadWeaponFail");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.WeaponTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    //기본무기
                    if (i == 0)
                    {
                        defultValues.Add(table[i].Stringid, "0,1,0,1");

                        var weaponData = new WeaponServerData();
                        weaponData.idx = table[i].Id;
                        weaponData.hasItem = new ReactiveProperty<int>(1);
                        weaponData.level = new ReactiveProperty<int>(0);
                        weaponData.amount = new ReactiveProperty<int>(1);

                        tableDatas.Add(table[i].Stringid, weaponData);
                    }
                    else
                    {
                        var weaponData = new WeaponServerData();
                        weaponData.idx = table[i].Id;
                        weaponData.hasItem = new ReactiveProperty<int>(0);
                        weaponData.level = new ReactiveProperty<int>(0);
                        weaponData.amount = new ReactiveProperty<int>(0);

                        tableDatas.Add(table[i].Stringid, weaponData);
                        defultValues.Add(table[i].Stringid, weaponData.ConvertToString());
                    }
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

                var table = TableManager.Instance.WeaponTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var weapondata = new WeaponServerData();

                        var splitData = value.Split(',');

                        weapondata.idx = int.Parse(splitData[0]);
                        weapondata.hasItem = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        weapondata.level = new ReactiveProperty<int>(int.Parse(splitData[2]));
                        weapondata.amount = new ReactiveProperty<int>(int.Parse(splitData[3]));

                        tableDatas.Add(table[i].Stringid, weapondata);
                    }
                    else
                    {
                        var weaponData = new WeaponServerData();
                        weaponData.idx = table[i].Id;
                        weaponData.hasItem = new ReactiveProperty<int>(0);
                        weaponData.level = new ReactiveProperty<int>(0);
                        weaponData.amount = new ReactiveProperty<int>(0);

                        tableDatas.Add(table[i].Stringid, weaponData);
                        defultValues.Add(table[i].Stringid, weaponData.ConvertToString());
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

    public void UpData(WeaponData weaponData, int addNum)
    {
        if (tableDatas[weaponData.Stringid].hasItem.Value == 0)
        {
            tableDatas[weaponData.Stringid].hasItem.Value = 1;
        }

        tableDatas[weaponData.Stringid].amount.Value += addNum;
    }

    public void SyncToServerEach(string key)
    {
        Param defultValues = new Param();

        //hasitem 1
        defultValues.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, bro =>
           {
               if (bro.IsSuccess() == false)
               {
                   ServerData.ShowCommonErrorPopup(bro, () => { SyncToServerEach(key); });
                   return;
               }
           });
    }

    public void SyncToServerAll(List<int> updateList = null)
    {
        Param defultValues = new Param();

        var table = TableManager.Instance.WeaponTable.dataArray;

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

}
