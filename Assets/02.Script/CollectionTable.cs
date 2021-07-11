using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
public class CollectionServerData
{
    public int idx;
    public ReactiveProperty<int> level;
    public ReactiveProperty<int> amount;
}

public class CollectionTable
{
    public static string Indate;
    public static string tableName = "Collection";

    private ReactiveDictionary<string, CollectionServerData> tableDatas = new ReactiveDictionary<string, CollectionServerData>();

    public ReactiveDictionary<string, CollectionServerData> TableDatas => tableDatas;

    public float GetCollectionAbilValue(EnemyTableData tableData)
    {
        return tableData.Collectionabilvalue * (tableDatas[tableData.Collectionkey].level.Value);
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

                var table = TableManager.Instance.EnemyTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (table[i].Usecollection == false) continue;

                    defultValues.Add(table[i].Collectionkey, $"{table[i].Id},0,0");

                    var enemyData = new CollectionServerData();
                    enemyData.idx = table[i].Id;
                    enemyData.level = new ReactiveProperty<int>(0);
                    enemyData.amount = new ReactiveProperty<int>(0);

                    tableDatas.Add(table[i].Collectionkey, enemyData);
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

                var table = TableManager.Instance.EnemyTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (table[i].Usecollection == false) continue;

                    if (data.Keys.Contains(table[i].Collectionkey))
                    {
                        //값로드
                        var value = data[table[i].Collectionkey][ServerData.format_string].ToString();

                        var enemyData = new CollectionServerData();

                        var splitData = value.Split(',');

                        enemyData.idx = int.Parse(splitData[0]);
                        enemyData.level = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        enemyData.amount = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table[i].Collectionkey, enemyData);
                    }
                    else
                    {
                        defultValues.Add(table[i].Collectionkey, $"{table[i].Id},0,0");

                        var enemyData = new CollectionServerData();
                        enemyData.idx = table[i].Id;
                        enemyData.level = new ReactiveProperty<int>(0);
                        enemyData.amount = new ReactiveProperty<int>(0);

                        tableDatas.Add(table[i].Collectionkey, enemyData);
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
