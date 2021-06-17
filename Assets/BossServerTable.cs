using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[System.Serializable]
public class BossServerData
{
    public int idx;
    public ReactiveProperty<int> clear;
    public ReactiveProperty<int> artifactLevel;

    public string ConvertToString()
    {
        return $"{idx},{clear.Value},{artifactLevel.Value}";
    }
}

public class BossServerTable
{
    public static string Indate;
    public static string tableName = "BossTable";

    private ReactiveDictionary<string, BossServerData> tableDatas = new ReactiveDictionary<string, BossServerData>();

    public ReactiveDictionary<string, BossServerData> TableDatas => tableDatas;

    public void UpdateData(string key) 
    {
        Param defultValues = new Param();

        //hasitem 1
        defultValues.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, e =>
        {

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
                Debug.LogError("LoadBossFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.BossTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var bossTable = new BossServerData();
                    bossTable.idx = table[i].Id;
                    bossTable.clear = new ReactiveProperty<int>(0);
                    bossTable.artifactLevel = new ReactiveProperty<int>(0);

                    defultValues.Add(table[i].Stringid, bossTable.ConvertToString());
                    tableDatas.Add(table[i].Stringid, bossTable);
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

                var table = TableManager.Instance.BossTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][DatabaseManager.format_string].ToString();

                        var bossData = new BossServerData();

                        var splitData = value.Split(',');

                        bossData.idx = int.Parse(splitData[0]);
                        bossData.clear = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        bossData.artifactLevel = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table[i].Stringid, bossData);
                    }
                    else
                    {

                        var bossData = new BossServerData();
                        bossData.idx = table[i].Id;
                        bossData.clear = new ReactiveProperty<int>(0);
                        bossData.artifactLevel = new ReactiveProperty<int>(0);

                        defultValues.Add(table[i].Stringid, bossData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, bossData);
                        paramCount++;
                    }
                }

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
}
