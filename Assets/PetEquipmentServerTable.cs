using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

[System.Serializable]
public class PetEquipServerData
{
    public int idx;
    public ReactiveProperty<int> hasAbil;
    public ReactiveProperty<int> level;

    public string ConvertToString()
    {
        return $"{idx},{hasAbil.Value},{level.Value}";
    }
}

public class PetEquipmentServerTable
{
    public static string Indate;
    public static string tableName = "PetEquipment";

    private ReactiveDictionary<string, PetEquipServerData> tableDatas = new ReactiveDictionary<string, PetEquipServerData>();

    public ReactiveDictionary<string, PetEquipServerData> TableDatas => tableDatas;
    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("PetEquipment Load Complete");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.PetEquipment.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var petEquip = new PetEquipServerData();
                    petEquip.idx = table[i].Id;
                    petEquip.hasAbil = new ReactiveProperty<int>(0);
                    petEquip.level = new ReactiveProperty<int>(0);

                    defultValues.Add(table[i].Stringid, petEquip.ConvertToString());
                    tableDatas.Add(table[i].Stringid, petEquip);
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

                var table = TableManager.Instance.PetEquipment.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var petEquip = new PetEquipServerData();

                        var splitData = value.Split(',');

                        petEquip.idx = int.Parse(splitData[0]);
                        petEquip.hasAbil = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        petEquip.level = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table[i].Stringid, petEquip);
                    }
                    else
                    {

                        var petEquip = new PetEquipServerData();
                        petEquip.idx = table[i].Id;
                        petEquip.hasAbil = new ReactiveProperty<int>(0);
                        petEquip.level = new ReactiveProperty<int>(0);

                        defultValues.Add(table[i].Stringid, petEquip.ConvertToString());

                        tableDatas.Add(table[i].Stringid, petEquip);
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
