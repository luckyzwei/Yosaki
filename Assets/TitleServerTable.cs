using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using static UiGachaResultView;

[System.Serializable]
public class TitleServerData
{
    public int idx;
    public ReactiveProperty<int> clearFlag;
    public ReactiveProperty<int> rewarded;

    public string ConvertToString()
    {
        return $"{idx},{clearFlag.Value},{rewarded.Value}";
    }
}

public class TitleServerTable
{
    public static string Indate;
    public static string tableName = "TitleMission";

    private ReactiveDictionary<string, TitleServerData> tableDatas = new ReactiveDictionary<string, TitleServerData>();

    public ReactiveDictionary<string, TitleServerData> TableDatas => tableDatas;

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadTitleFail");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.TitleTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var titleData = new TitleServerData();
                    titleData.idx = table[i].Id;
                    titleData.clearFlag = new ReactiveProperty<int>(0);
                    titleData.rewarded = new ReactiveProperty<int>(0);

                    tableDatas.Add(table[i].Stringid, titleData);
                    defultValues.Add(table[i].Stringid, titleData.ConvertToString());
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

                var table = TableManager.Instance.TitleTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var titleData = new TitleServerData();

                        var splitData = value.Split(',');

                        titleData.idx = int.Parse(splitData[0]);
                        titleData.clearFlag = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        titleData.rewarded = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table[i].Stringid, titleData);
                    }
                    else
                    {
                        var titleData = new TitleServerData();
                        titleData.idx = table[i].Id;
                        titleData.clearFlag = new ReactiveProperty<int>(0);
                        titleData.rewarded = new ReactiveProperty<int>(0);

                        tableDatas.Add(table[i].Stringid, titleData);
                        defultValues.Add(table[i].Stringid, titleData.ConvertToString());
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
