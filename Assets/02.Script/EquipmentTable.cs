using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
public class EquipmentTable
{
    public static string Indate;
    public const string tableName = "Equipment";
    public static string Weapon = "Weapon";
    public static string Pet = "Pet";
    public static string MagicBook = "MagicBook";
    public static string Potion = "Potion";
    public static string Costume = "Costume";

    private Dictionary<string, int> tableSchema = new Dictionary<string, int>()
    {
        {Weapon,0},
        {Pet,0},
        {MagicBook,-1},
        {Potion,0},
        {Costume,0}
    };

    private ReactiveDictionary<string, ReactiveProperty<int>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<int>>();
    public ReactiveDictionary<string, ReactiveProperty<int>> TableDatas => tableDatas;

    public void ChangeEquip(string key, int idx)
    {
        tableDatas[key].Value = idx;

        SyncData(key);
    }

    public void SyncData(string key)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
#if UNITY_EDITOR
            if (e.IsSuccess() == false)
            {
                Debug.Log($"ChangeEquipe {key} {tableDatas[key].Value} up failed");
                return;
            }
            else
            {
                Debug.Log($"ChangeEquiped {key} {tableDatas[key].Value} up complete");
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
                Debug.LogError("LoadStatusFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    defultValues.Add(e.Current.Key, e.Current.Value);
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(e.Current.Value));
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

                    // data.
                    // statusIndate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
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

                var e = tableSchema.GetEnumerator();

                for (int i = 0; i < data.Keys.Count; i++)
                {
                    while (e.MoveNext())
                    {
                        if (data.Keys.Contains(e.Current.Key))
                        {
                            //값로드
                            var value = data[e.Current.Key][DatabaseManager.format_Number].ToString();
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(int.Parse(value)));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(e.Current.Value));
                            paramCount++;
                        }
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
