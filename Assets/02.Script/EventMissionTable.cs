using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

public class EventMissionTable
{
    public static string Indate;
    public const string tableName = "EventMission";

    private ReactiveDictionary<string, ReactiveProperty<int>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<int>>();
    public ReactiveDictionary<string, ReactiveProperty<int>> TableDatas => tableDatas;

    public void UpdateMissionData(string key, int amount)
    {
        tableDatas[key].Value += amount;
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

                var table = TableManager.Instance.DailyMission.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    defultValues.Add(table[i].Stringid, 0);
                    tableDatas.Add(table[i].Stringid, new ReactiveProperty<int>(0));
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

                var table = TableManager.Instance.DailyMission.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_Number].ToString();
                        tableDatas.Add(table[i].Stringid, new ReactiveProperty<int>(int.Parse(value)));
                    }
                    else
                    {
                        defultValues.Add(table[i].Stringid, 0);
                        tableDatas.Add(table[i].Stringid, new ReactiveProperty<int>(0));
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

    public void SyncToServerEach(string key)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.Log($"이벤트 일일미션 {key} up failed");
                return;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError($"이벤트 일일미션 Key {key} sync complete");
            }
#endif
        });
    }
}
