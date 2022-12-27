using UnityEngine;
using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
///
/// !!! Machine generated code !!!
///
/// A class which deriveds ScritableObject class so all its data 
/// can be serialized onto an asset data file.
/// 
[System.Serializable]
public class SasinsuServerData
{
    
    public ReactiveProperty<double> score;
    

    public string ConvertToString()
    {
        return $"{score.Value}";
    }
}

public class SasinsuTable : ScriptableObject 
{
    public static string Indate;
    public static string tableName = "Sasinsu";

    private ReactiveDictionary<string, SasinsuServerData> tableDatas = new ReactiveDictionary<string, SasinsuServerData>();

    public ReactiveDictionary<string, SasinsuServerData> TableDatas => tableDatas;


    [HideInInspector] [SerializeField] 
    public string SheetName = "";
    
    [HideInInspector] [SerializeField] 
    public string WorksheetName = "";
    
    // Note: initialize in OnEnable() not here.
    public SasinsuTableData[] dataArray;
    
    void OnEnable()
    {		
//#if UNITY_EDITOR
        //hideFlags = HideFlags.DontSave;
//#endif
        // Important:
        //    It should be checked an initialization of any collection data before it is initialized.
        //    Without this check, the array collection which already has its data get to be null 
        //    because OnEnable is called whenever Unity builds.
        // 		
        if (dataArray == null)
            dataArray = new SasinsuTableData[0];

    }

    //
    // Highly recommand to use LINQ to query the data sources.
    //

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
                Debug.LogError("LoadSasinsuFail");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.sasinsuTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    //기본세팅
                    if (i == 0)
                    {
                        defultValues.Add(table[i].Stringid, "0");

                        var sasinsuData = new SasinsuServerData();
                        sasinsuData.score = new ReactiveProperty<double>(0);

                        tableDatas.Add(table[i].Stringid, sasinsuData);
                    }
                    else
                    {
                        var sasinsuData = new SasinsuServerData();
                        
                        sasinsuData.score = new ReactiveProperty<double>(0);
                        

                        tableDatas.Add(table[i].Stringid, sasinsuData);
                        defultValues.Add(table[i].Stringid, sasinsuData.ConvertToString());
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

                var table = TableManager.Instance.sasinsuTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var sasinsuData = new SasinsuServerData();

                        var splitData = value.Split(',');

                        sasinsuData.score = new ReactiveProperty<double>(double.Parse(splitData[0]));

                        tableDatas.Add(table[i].Stringid, sasinsuData);
                    }
                    else
                    {
                        var sasinsuData = new SasinsuServerData();
                        sasinsuData.score = new ReactiveProperty<double>(0);

                        tableDatas.Add(table[i].Stringid, sasinsuData);
                        defultValues.Add(table[i].Stringid, sasinsuData.ConvertToString());
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
