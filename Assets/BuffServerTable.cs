using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

[System.Serializable]
public class BuffServerData
{
    public int idx;
    public ReactiveProperty<int> remainSec;

    public string ConvertToString()
    {
        return $"{idx},{remainSec.Value}";
    }
}

public class BuffServerTable
{
    public static string Indate;
    public static string tableName = "BuffTable";

    private ReactiveDictionary<string, BuffServerData> tableDatas = new ReactiveDictionary<string, BuffServerData>();

    public ReactiveDictionary<string, BuffServerData> TableDatas => tableDatas;

    public void SyncData(string key)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
        {
#if UNITY_EDITOR
            if (bro.IsSuccess() == false)
            {
                Debug.Log($"SyncAllData {tableName} up failed");
                return;
            }
            else
            {
                Debug.Log($"SyncAllData {tableName} up Complete");
                return;
            }
#endif
        });
    }

    public void SyncAllDataForce()
    {
        Param param = new Param();

        for (int i = 0; i < TableManager.Instance.BuffTable.dataArray.Length; i++)
        {
            if (TableManager.Instance.BuffTable.dataArray[i].Buffseconds <= 0) continue;

            string key = TableManager.Instance.BuffTable.dataArray[i].Stringid;
            param.Add(key, tableDatas[key].ConvertToString());
        }

        Backend.GameData.Update(tableName, Indate, param);
    }

    public void SyncAllData()
    {
        Param param = new Param();

        for (int i = 0; i < TableManager.Instance.BuffTable.dataArray.Length; i++)
        {
            if (TableManager.Instance.BuffTable.dataArray[i].Buffseconds <= 0) continue;

            string key = TableManager.Instance.BuffTable.dataArray[i].Stringid;
            param.Add(key, tableDatas[key].ConvertToString());
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
        {
#if UNITY_EDITOR
            if (bro.IsSuccess() == false)
            {
                Debug.Log($"SyncAllData {tableName} up failed");
                return;
            }
            else
            {
                Debug.Log($"SyncAllData {tableName} up Complete");
                return;
            }
#endif
        });
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadPetFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.BuffTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var buffData = new BuffServerData();
                    buffData.idx = table[i].Id;

                    //매직스톤버프
                    buffData.remainSec = new ReactiveProperty<int>(0);

                    defultValues.Add(table[i].Stringid, buffData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, buffData);
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

                var table = TableManager.Instance.BuffTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var buffData = new BuffServerData();

                        var splitData = value.Split(',');

                        buffData.idx = int.Parse(splitData[0]);
                        buffData.remainSec = new ReactiveProperty<int>(int.Parse(splitData[1]));

                        tableDatas.Add(table[i].Stringid, buffData);
                    }
                    else
                    {

                        var buffData = new BuffServerData();

                        buffData.idx = table[i].Id;

                        buffData.remainSec = new ReactiveProperty<int>(0);

                        defultValues.Add(table[i].Stringid, buffData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, buffData);
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

}
