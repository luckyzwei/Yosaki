using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

[System.Serializable]
public class YomulServerData
{
    public int idx;
    public ReactiveProperty<int> hasAbil;
    public ReactiveProperty<int> level;

    public string ConvertToString()
    {
        return $"{idx},{hasAbil.Value},{level.Value}";
    }
}

public class YomulServerTable
{
    public static string Indate;
    public static string tableName = "YomulAbil";


    private ReactiveDictionary<string, YomulServerData> tableDatas = new ReactiveDictionary<string, YomulServerData>();

    public ReactiveDictionary<string, YomulServerData> TableDatas => tableDatas;

    //public float GetStatusValue(StatusType statusType)
    //{
    //    float ret = 0f;
    //    int status = (int)statusType;

    //    var e = tableDatas.GetEnumerator();
    //    while (e.MoveNext())
    //    {
    //        //무료펫 능력치XX
    //        //미보유 X
    //        if (e.Current.Value.hasItem.Value == 0) continue;

    //        var petTableData = TableManager.Instance.PetDatas[e.Current.Value.idx];
    //        if (petTableData.Hastype1 == status)
    //        {
    //            ret += petTableData.Hasvalue1 + e.Current.Value.level.Value * petTableData.Hasaddvalue1;
    //        }

    //        if (petTableData.Hastype2 == status)
    //        {
    //            ret += petTableData.Hasvalue2 + e.Current.Value.level.Value * petTableData.Hasaddvalue2;
    //        }

    //        if (petTableData.Hastype3 == status)
    //        {
    //            ret += petTableData.Hasvalue3 + e.Current.Value.level.Value * petTableData.Hasaddvalue3;
    //        }
    //    }

    //    return ret;
    //}


    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadYomulAbilFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.YomulAbilTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var yomulData = new YomulServerData();
                    yomulData.idx = table[i].Id;
                    yomulData.hasAbil = new ReactiveProperty<int>(0);
                    yomulData.level = new ReactiveProperty<int>(0);

                    defultValues.Add(table[i].Stringid, yomulData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, yomulData);
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

                var table = TableManager.Instance.YomulAbilTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var yomulData = new YomulServerData();

                        var splitData = value.Split(',');

                        yomulData.idx = int.Parse(splitData[0]);
                        yomulData.hasAbil = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        yomulData.level = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table[i].Stringid, yomulData);
                    }
                    else
                    {

                        var yomulData = new YomulServerData();
                        yomulData.idx = table[i].Id;
                        yomulData.hasAbil = new ReactiveProperty<int>(0);
                        yomulData.level = new ReactiveProperty<int>(0);

                        defultValues.Add(table[i].Stringid, yomulData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, yomulData);
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
