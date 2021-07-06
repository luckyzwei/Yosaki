using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
public class RankTable_Level
{
    public static string Indate;
    public const string tableName_Level = RankManager.Rank_Level_TableName;

    public void Initialize() 
    {
        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName_Level,new Where(), callback =>
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

                var bro = Backend.GameData.Insert(tableName_Level, defultValues);

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

                JsonData data = rows[0];

                if (data.Keys.Contains(DatabaseManager.inDate_str))
                {
                    Indate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                }

            }
        });
    }
}

public class RankTable_Stage
{
    public static string Indate;
    public const string tableName_Level = RankManager.Rank_Stage;

    public void Initialize()
    {
        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName_Level, new Where(), callback =>
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

                var bro = Backend.GameData.Insert(tableName_Level, defultValues);

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

                JsonData data = rows[0];

                if (data.Keys.Contains(DatabaseManager.inDate_str))
                {
                    Indate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                }

            }
        });
    }
}
