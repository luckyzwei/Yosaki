using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;

public class EtcServerTable
{
    public static string Indate;
    public const string tableName = "Etc";
    public const string email = "email";
    public const string yoguiSogulReward = "ys";
    public const string sonReward = "sonRewardReal";
    public const string iosCoupon = "iosCoupon";
    public const string guildAttenReward = "gar";

    private Dictionary<string, ReactiveProperty<string>> tableSchema = new Dictionary<string, ReactiveProperty<string>>()
    {
        {email,new ReactiveProperty<string>(GoogleManager.email)},
        {yoguiSogulReward,new ReactiveProperty<string>(string.Empty)},
        {sonReward,new ReactiveProperty<string>(string.Empty)},
        {iosCoupon,new ReactiveProperty<string>(string.Empty)},
        {guildAttenReward,new ReactiveProperty<string>(string.Empty)},
    };

    private Dictionary<string, ReactiveProperty<string>> tableDatas = new Dictionary<string, ReactiveProperty<string>>();
    public Dictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

    public bool YoguiSoguilRewarded(int stageId) 
    {
        var rewards = tableDatas[yoguiSogulReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public bool SonRewarded(float stageId)
    {
        var rewards = tableDatas[sonReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public bool GuildAttenRewarded(float stageId)
    {
        var rewards = tableDatas[guildAttenReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public bool IosCouponRewarded(float id)
    {
        var rewards = tableDatas[iosCoupon].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(id.ToString());
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
                    if (e.Current.Key.Equals(email))
                    {
                        defultValues.Add(e.Current.Key, GoogleManager.email);
                        tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(GoogleManager.email));

                    }
                    else
                    {
                        defultValues.Add(e.Current.Key, e.Current.Value.Value);
                        tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(string.Empty));
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

                if (data.Keys.Contains(ServerData.inDate_str))
                {
                    Indate = data[ServerData.inDate_str][ServerData.format_string].ToString();
                }

                var e = tableSchema.GetEnumerator();

                for (int i = 0; i < data.Keys.Count; i++)
                {
                    while (e.MoveNext())
                    {
                        if (data.Keys.Contains(e.Current.Key))
                        {
                            //값로드
                            var value = data[e.Current.Key][ServerData.format_string].ToString();
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(value));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value.Value);
                            tableDatas.Add(e.Current.Key, e.Current.Value);
                            paramCount++;
                        }
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
