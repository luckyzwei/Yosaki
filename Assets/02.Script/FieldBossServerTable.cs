using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

//[System.Serializable]
//public class FieldBossServerData
//{
//    public int idx;
//    public ReactiveProperty<int> killCount;

//    public string ConvertToString()
//    {
//        return $"{idx},{killCount.Value}";
//    }
//}

public class FieldBossServerTable
{
    //삭제된 기능
    //public static string Indate;
    //public static string tableName = "FieldBossTable";

    //private ReactiveDictionary<string, FieldBossServerData> tableDatas = new ReactiveDictionary<string, FieldBossServerData>();

    //public ReactiveDictionary<string, FieldBossServerData> TableDatas => tableDatas;

    //public void Initialize()
    //{
    //    tableDatas.Clear();

    //    SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
    //    {
    //        // 이후 처리
    //        if (callback.IsSuccess() == false)
    //        {
    //            Debug.LogError("LoadFieldBoss Failed");
    //            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
    //            return;
    //        }

    //        var rows = callback.Rows();

    //        //맨처음 초기화
    //        if (rows.Count <= 0)
    //        {
    //            Param defultValues = new Param();

    //            var table = TableManager.Instance.StageMapTable.dataArray;

    //            for (int i = 0; i < table.Length; i++)
    //            {
    //                var fieldBossServerData = new FieldBossServerData();
    //                fieldBossServerData.idx = table[i].Id;
    //                fieldBossServerData.killCount = new ReactiveProperty<int>();

    //                if (tableDatas.ContainsKey(table[i].Stagestringkey) == false)
    //                {
    //                    tableDatas.Add(table[i].Stagestringkey, fieldBossServerData);

    //                    defultValues.Add(table[i].Stagestringkey, fieldBossServerData.ConvertToString());
    //                }
    //            }

    //            var bro = Backend.GameData.Insert(tableName, defultValues);

    //            if (bro.IsSuccess() == false)
    //            {
    //                // 이후 처리
    //                DatabaseManager.ShowCommonErrorPopup(bro, Initialize);
    //                return;
    //            }
    //            else
    //            {
    //                var jsonData = bro.GetReturnValuetoJSON();
    //                if (jsonData.Keys.Count > 0)
    //                {
    //                    Indate = jsonData[0].ToString();
    //                }
    //            }

    //            return;
    //        }
    //        //나중에 칼럼 추가됐을때 업데이트
    //        else
    //        {
    //            Param defultValues = new Param();
    //            int paramCount = 0;

    //            JsonData data = rows[0];

    //            if (data.Keys.Contains(DatabaseManager.inDate_str))
    //            {
    //                Indate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
    //            }

    //            var table = TableManager.Instance.StageMapTable.dataArray;

    //            for (int i = 0; i < table.Length; i++)
    //            {
    //                if (data.Keys.Contains(table[i].Stagestringkey))
    //                {
    //                    if (tableDatas.ContainsKey(table[i].Stagestringkey) == false)
    //                    {
    //                        //값로드
    //                        var value = data[table[i].Stagestringkey][DatabaseManager.format_string].ToString();

    //                        var fieldBossData = new FieldBossServerData();

    //                        var splitData = value.Split(',');

    //                        fieldBossData.idx = int.Parse(splitData[0]);
    //                        fieldBossData.killCount = new ReactiveProperty<int>(int.Parse(splitData[1]));

    //                        tableDatas.Add(table[i].Stagestringkey, fieldBossData);
    //                    }
    //                }
    //                else
    //                {
    //                    if (tableDatas.ContainsKey(table[i].Stagestringkey) == false)
    //                    {
    //                        var fieldBossServerData = new FieldBossServerData();
    //                        fieldBossServerData.idx = table[i].Id;
    //                        fieldBossServerData.killCount = new ReactiveProperty<int>(0);

    //                        tableDatas.Add(table[i].Stagestringkey, fieldBossServerData);

    //                        defultValues.Add(table[i].Stagestringkey, fieldBossServerData.ConvertToString());

    //                        paramCount++;
    //                    }
    //                }
    //            }

    //            if (paramCount != 0)
    //            {
    //                var bro = Backend.GameData.Update(tableName, Indate, defultValues);

    //                if (bro.IsSuccess() == false)
    //                {
    //                    DatabaseManager.ShowCommonErrorPopup(bro, Initialize);
    //                    return;
    //                }
    //            }

    //        }
    //    });
    //}

    //public void SyncCurrentStageKillCount()
    //{
    //    //if (GameManager.Instance.CurrentStageData == null) return;

    //    //string stageKey = GameManager.Instance.CurrentStageData.Stagestringkey;

    //    //Param param = new Param();
    //    //param.Add(stageKey, DatabaseManager.fieldBossTable.TableDatas[stageKey].ConvertToString());

    //    //SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
    //    //    {


    //    //    });
    //}

    //public void SyncCurrentStageKillCountForce()
    //{
    //    //if (GameManager.Instance == null) return;

    //    //string stageKey = GameManager.Instance.CurrentStageData.Stagestringkey;

    //    //Param param = new Param();
    //    //param.Add(stageKey, DatabaseManager.fieldBossTable.TableDatas[stageKey].ConvertToString());

    //    //Backend.GameData.Update(tableName, Indate, param);
    //}
}
