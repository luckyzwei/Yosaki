using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class UserInfoTable
{
    public static string Indate;
    public const string tableName = "UserInfo";

    public const string Hp = "Hp";
    public const string Mp = "Mp";
    public const string LastMap = "LastMap";

    public const string LastLogin = "LastLogin";
    public const string removeAd = "removeAd";

    public const string gachaNum_Weapon = "gachaNum_Weapon";
    public const string gachaNum_Norigae = "gachaNum_Norigae";
    public const string gachaNum_Skill = "gachaNum_Skill";

    public const string hackingCount = "hackingCount";

    public const string passSelectedIdx = "passSelectedIdx";
    public const string currentFloorIdx = "currentFloorIdx";

    public const string receiveReviewReward = "receiveReviewReward";

    public const string dailyEnemyKillCount = "dailyEnemyKillCount";

    public const string dailyTicketBuyCount = "dailyTicketBuyCount";
    public const string receivedTicketReward = "receivedTicketReward";

    public const string bonusDungeonEnterCount = "bonusDungeonEnterCount";
    public const string chatBan = "chatBan";

    public const string tutorialCurrentStep = "tutorialCurrentStep";
    public const string tutorialClearFlags = "tutorialClearFlags";

    public const string managerDescriptionFlags = "managerDescriptionFlags";
    public const string attendanceCount = "attendanceCount";

    public const string marbleAwake = "marbleAwake";
    public const string resetStat = "resetStat";

    public const string buff_gold1 = "gold1_new_new";
    public const string buff_gold2 = "gold2_new_new";
    public const string buff_exp1 = "exp1_new_new";
    public const string buff_exp2 = "exp2_new_new";

    public const string bonusDungeonMaxKillCount = "bonusDungeonMaxKillCount";

    public const string wingPackageRewardReceive = "wingPackageRewardReceive";
    public const string topClearStageId = "topClearStageId";
    public const string selectedSkillGroupId = "selectedSkillGroupId";

    // public ObscuredDouble currentServerDate;
    public double currentServerDate;

    private Dictionary<string, float> tableSchema = new Dictionary<string, float>()
    {
        {Hp,100f},
        {Mp,100f},
        {LastMap,0f},
        {LastLogin,0f},
        {removeAd,0f},
        {gachaNum_Weapon,0f},
        {gachaNum_Norigae,0f},
        {gachaNum_Skill,0f},
        {hackingCount,0f},
        {passSelectedIdx,0f},
        {currentFloorIdx,0f},
        {receiveReviewReward,0f},
        {dailyEnemyKillCount,0f},
        {dailyTicketBuyCount,0f},
        {receivedTicketReward,0f},
        {bonusDungeonEnterCount,0f},
        {chatBan,0f},
        {tutorialCurrentStep,2f},
        {tutorialClearFlags,0f},
        {managerDescriptionFlags,0f},
        {attendanceCount,1f},
        {marbleAwake,-1f},
        {resetStat,0f},
        //버프
        {buff_gold1,0f},
        {buff_gold2,0f},
        {buff_exp1,0f},
        {buff_exp2,0f},
        {bonusDungeonMaxKillCount,0f},
        {wingPackageRewardReceive,0f},
        {topClearStageId,-1f},
        {selectedSkillGroupId,0f}
    };

    private Dictionary<string, ReactiveProperty<float>> tableDatas = new Dictionary<string, ReactiveProperty<float>>();
    public Dictionary<string, ReactiveProperty<float>> TableDatas => tableDatas;
    public ReactiveProperty<float> GetTableData(string key)
    {
        return tableDatas[key];
    }

    public ReactiveCommand WhenDateChanged = new ReactiveCommand();

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
                     if (e.Current.Key != LastLogin)
                     {
                         defultValues.Add(e.Current.Key, e.Current.Value);
                         tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
                     }
                     else
                     {
                         BackendReturnObject servertime = Backend.Utils.GetServerTime();

                         string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
                         DateTime currentServerTime = DateTime.Parse(time).ToUniversalTime().AddHours(9);
                         currentServerDate = Utils.ConvertToUnixTimestamp(currentServerTime);

                         defultValues.Add(e.Current.Key, (float)currentServerDate);
                         tableDatas.Add(e.Current.Key, new ReactiveProperty<float>((float)currentServerDate));
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
                             var value = data[e.Current.Key][ServerData.format_Number].ToString();
                             tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(float.Parse(value)));
                         }
                         else
                         {
                             defultValues.Add(e.Current.Key, e.Current.Value);
                             tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
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

    public void UpData(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpData(string key, float data, bool LocalOnly, Action failCallBack = null)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }
        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            Param param = new Param();
            param.Add(key, tableDatas[key].Value);

            SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
            {
                if (e.IsSuccess() == false)
                {
                    failCallBack?.Invoke();
                    Debug.LogError($"UserInfoTable {key} up failed");
                    return;
                }
            });
        }
    }

    public void AutoUpdateRoutine()
    {
        UpdateLastLoginTime();
        UpdatekillCount();
        UpdatejumpCount();
    }
    private void UpdatekillCount()
    {
        UpData(dailyEnemyKillCount, false);
    }
    private void UpdatejumpCount()
    {
        // UpData(jumpCount, false);
    }
    private bool isFirstInit = true;
    public void UpdateLastLoginTime()
    {
        SendQueue.Enqueue(Backend.Utils.GetServerTime, (bro) =>
        {
            if (bro.IsSuccess() && bro.GetStatusCode().Equals("200") && bro.GetReturnValuetoJSON() != null)
            {
                string time = bro.GetReturnValuetoJSON()["utcTime"].ToString();

                DateTime currentServerTime = DateTime.Parse(time).ToUniversalTime().AddHours(9);

                currentServerDate = Utils.ConvertToUnixTimestamp(currentServerTime);

                //day check
                DateTime savedDate = Utils.ConvertFromUnixTimestamp(tableDatas[LastLogin].Value);

                if (isFirstInit)
                {
                    isFirstInit = false;
                    SleepRewardReceiver.Instance.SetElapsedSecond((int)(currentServerTime - savedDate).TotalSeconds);
                }

                //week check
                int currentWeek = Utils.GetWeekNumber(currentServerTime);

                int savedWeek = Utils.GetWeekNumber(savedDate);

                if (savedDate.Day != currentServerTime.Day)
                {
                    //날짜 바뀜
                    DateChanged(savedWeek != currentWeek, savedDate.Month != currentServerTime.Month);
                }
                else
                {
                    UpdateLastLoginOnly();
                }
            }
        });
    }

    private void UpdateLastLoginOnly()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value = (float)currentServerDate;

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.LastLogin, ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList, true);
    }


    private void DateChanged(bool weekChanged, bool monthChanged)
    {
        WhenDateChanged.Execute();

        ClearDailyMission();

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //일일초기화
        Param dailyPassParam = new Param();
        ServerData.dailyPassServerTable.ResetDailyPassLocal();
        dailyPassParam.Add(DailyPassServerTable.DailypassFreeReward, ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassFreeReward].Value);
        dailyPassParam.Add(DailyPassServerTable.DailypassAdReward, ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassAdReward].Value);
        transactionList.Add(TransactionValue.SetUpdate(DailyPassServerTable.tableName, DailyPassServerTable.Indate, dailyPassParam));

        //일일초기화
        Param userInfoParam = new Param();
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value = 0;

        //버프
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp2).Value = 0;
        //

        ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value = (float)currentServerDate;
        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value++;

        userInfoParam.Add(UserInfoTable.dailyEnemyKillCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value);
        userInfoParam.Add(UserInfoTable.dailyTicketBuyCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value);
        userInfoParam.Add(UserInfoTable.receivedTicketReward, ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value);
        userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
        userInfoParam.Add(UserInfoTable.LastLogin, ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value);
        userInfoParam.Add(UserInfoTable.attendanceCount, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value);

        userInfoParam.Add(UserInfoTable.buff_gold1, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold1).Value);
        userInfoParam.Add(UserInfoTable.buff_gold2, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold2).Value);
        userInfoParam.Add(UserInfoTable.buff_exp1, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp1).Value);
        userInfoParam.Add(UserInfoTable.buff_exp2, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp2).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        Param iapParam = null;

        var iapTable = TableManager.Instance.InAppPurchase.dataArray;

        for (int i = 0; i < iapTable.Length; i++)
        {
            bool isDayChagned = (iapTable[i].BUYTYPE == BuyType.DayOfOne || iapTable[i].BUYTYPE == BuyType.DayOfFive);
            bool isWeekChagned = weekChanged == true && (iapTable[i].BUYTYPE == BuyType.WeekOfTwo || iapTable[i].BUYTYPE == BuyType.WeekOfFive);
            bool isMonthChanged = monthChanged == true && (iapTable[i].BUYTYPE == BuyType.MonthOfOne || iapTable[i].BUYTYPE == BuyType.MonthOfFive);

            if (isDayChagned || isWeekChagned || isMonthChanged)
            {
                if (iapParam == null)
                {
                    iapParam = new Param();
                }

                ServerData.iapServerTable.TableDatas[iapTable[i].Productid].buyCount.Value = 0;
                iapParam.Add(iapTable[i].Productid, ServerData.iapServerTable.TableDatas[iapTable[i].Productid].ConvertToString());
            }
        }

        if (iapParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));
        }

        ServerData.SendTransaction(transactionList, true);
    }
    private void WeekChanged()
    {

    }
    private void MonthChanged()
    {

    }

    public void ClearDailyMission()
    {
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.Attendance, 1);
    }

    public bool HasRemoveAd()
    {
        return tableDatas[removeAd].Value == 1;
    }
}
