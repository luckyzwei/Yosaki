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
    public const string currentFloorIdx2 = "cf2";

    public const string receiveReviewReward = "receiveReviewReward";

    public const string dailyEnemyKillCount = "dailyEnemyKillCount";


    public const string dailyTicketBuyCount = "dailyTicketBuyCount";
    public const string receivedTicketReward = "receivedTicketReward";

    public const string bonusDungeonEnterCount = "bonusDungeonEnterCount4";

    public const string dokebiKillCount0 = "dokebiKillCount4";
    public const string dokebiKillCount1 = "dokebiKillCount5";
    public const string dokebiKillCount2 = "dokebiKillCount6";
    public const string dokebiKillCount3 = "dokebiKillCount7";


    public const string chatBan = "chatBan";

    public const string tutorialCurrentStep = "tutorialCurrentStep";
    public const string tutorialClearFlags = "tutorialClearFlags";

    public const string managerDescriptionFlags = "managerDescriptionFlags";
    public const string attendanceCount = "attendanceCount2";
    public const string attendanceCount_100Day = "attendanceCount_100Day";
    public const string attendanceCount_Seol = "atten_Seol";

    public const string marbleAwake = "marbleAwake";
    public const string resetStat = "resetStat";

    public const string buff_gold1 = "gold1_new_new_new";
    public const string buff_gold2 = "gold2_new_new_new";
    public const string buff_exp1 = "exp1_new_new_new";
    public const string buff_exp2 = "exp2_new_new_new";

    public const string guild_buff0 = "guild_buff0";
    public const string guild_buff1 = "guild_buff1";
    public const string guild_buff2 = "guild_buff2";
    public const string guild_buff3 = "guild_buff3";
    public const string one_Buff = "ob";

    public const string yomul0_buff = "yomul0_buff";
    public const string yomul1_buff = "yomul1_buff";
    public const string yomul2_buff = "yomul2_buff";
    public const string yomul3_buff = "yomul3_buff";
    public const string yomul4_buff = "yomul4_buff";
    public const string yomul5_buff = "yomul5_buff";
    public const string yomul6_buff = "yomul6_buff";
    public const string yomul7_buff = "yomul7_buff";

    public const string bonusDungeonMaxKillCount = "bonusDungeonMaxKillCount";

    public const string wingPackageRewardReceive = "wingPackageRewardReceive";
    public const string topClearStageId = "topClearStageId";
    public const string selectedSkillGroupId = "selectedSkillGroupId";
    public const string dokebiEnterCount = "dec3";
    public const string chatFrame = "chatFrame";

    public const string freeWeapon = "freeWeapon";
    public const string freeNorigae = "freeNorigae";
    public const string freeSkill = "freeSkill";

    public const string oakpensionAttendance = "oakpension";
    public const string marblepensionAttendance = "marblepension";
    public const string hellpensionAttendance = "hellpension";

    public const string marblePackChange = "marblePackChange";

    public const string yoguiSogulLastClear = "yoguiSogulLastClear";
    public const string smithClear = "smithClear";
    public const string gumGiClear = "gumGiClear";


    //6월월간
    public const string killCountTotal = "k8w";
    //7월월간
    public const string killCountTotal2 = "k7w";
    public const string killCountTotalChild = "kt9";
    public const string attenCountBok = "kb";
    public const string attenCountOne = "oy";

    public const string relicKillCount = "relicKillCount";

    public const string usedRelicTicketNum = "usedRelicTicketNum";

    public const string relicpensionAttendance = "relicpension";
    public const string peachAttendance = "peachpension";
    public const string smithpensionAttendance = "smithpension";
    public const string weaponpensionAttendance = "weaponpension";

    public const string monthreset = "monthreset";
    public const string sonScore = "son6";
    public const string hellScore = "hels";
    public const string susanoScore = "susa";
    public const string sleepRewardSavedTime = "sleepRewardSavedTime";
    public const string buffAwake = "buffAwake";
    public const string petAwake = "petAwake";
    public const string IgnoreDamDec = "IgnoreDamDec";
    public const string SendGuildPoint = "SendGuildPoint3";
    public const string cockAwake = "cockAwake";
    public const string dogAwake = "dogAwake";
    public const string basicPackRefund = "basicPackRefund";
    public const string skillInitialized = "ski";
    public const string smithExp = "smith";
    public const string getSmith = "getSmith";
    public const string getGumGi = "getGumGi";
    public const string sendPetExp = "sendPetExp";

    public const string exchangeCount = "ex_0";
    public const string exchangeCount_1 = "ex_1";
    public const string exchangeCount_2 = "ex_2";
    public const string refundFox = "rf";
    public const string sendGangChul = "sg";
    public const string foxMask = "fm";
    public const string relicPackReset = "rr";
    public const string oneAttenEvent = "oe";


    public double currentServerDate;
    public double attendanceUpdatedTime;
    public DateTime currentServerTime { get; private set; }
    public ReactiveCommand whenServerTimeUpdated = new ReactiveCommand();

    private Dictionary<string, double> tableSchema = new Dictionary<string, double>()
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
        {currentFloorIdx2,0f},
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

        {guild_buff0,0f},
        {guild_buff1,0f},
        {guild_buff2,0f},
        {guild_buff3,0f},
        {one_Buff,0f},

        {bonusDungeonMaxKillCount,0f},
        {wingPackageRewardReceive,0f},
        {topClearStageId,-1f},
        {selectedSkillGroupId,0f},
        {dokebiEnterCount,0f},
        {dokebiKillCount0,0f},
        {dokebiKillCount1,0f},
        {dokebiKillCount2,0f},
        {chatFrame,0f},

        {freeWeapon,0f},
        {freeNorigae,0f},
        {freeSkill,0f},

        {dokebiKillCount3,0f},

        {oakpensionAttendance,0f},
        {marblepensionAttendance,0f},
        {hellpensionAttendance,0f},

        {marblePackChange,0f},
        {yoguiSogulLastClear,0f},
        {smithClear,0f},
        {gumGiClear,0f},


        {yomul0_buff,0f},
        {yomul1_buff,0f},
        {yomul2_buff,0f},
        {yomul3_buff,0f},
        {killCountTotal,0f},
        {relicKillCount,0f},
        {usedRelicTicketNum,0f},
        {relicpensionAttendance,0f},
        {yomul4_buff,0f},

        {yomul5_buff,0f},
        {killCountTotal2,0f},
        {killCountTotalChild,0f},
        {attenCountBok,1f},
        {attenCountOne,0f},
        {yomul6_buff,0f},
        {sonScore,0f},
        {susanoScore,0f},
        {sleepRewardSavedTime,0f},
        {yomul7_buff,0f},
        {attendanceCount_100Day,1f},
        {peachAttendance,0f},
        {smithpensionAttendance,0f},
        {weaponpensionAttendance,0f},
        {buffAwake,0f},
        {petAwake,0f},
        {IgnoreDamDec,0f},
        {SendGuildPoint,0},
        {cockAwake,0},
        {attendanceCount_Seol,1},
        {dogAwake,0},
        {basicPackRefund,0},
        {skillInitialized,0},
        {smithExp,0},
        {getSmith,0},
        {getGumGi,0},
        {sendPetExp,0},
        {exchangeCount,0},
        {exchangeCount_1,0},
        {exchangeCount_2,0},
        {monthreset,0},
        {refundFox,0},
        {sendGangChul,0},
        {foxMask,0},
        {relicPackReset,0},
        {hellScore,0},
        {oneAttenEvent,0},
    };

    private Dictionary<string, ReactiveProperty<double>> tableDatas = new Dictionary<string, ReactiveProperty<double>>();
    public Dictionary<string, ReactiveProperty<double>> TableDatas => tableDatas;
    public ReactiveProperty<double> GetTableData(string key)
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
                         tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(e.Current.Value));
                     }
                     else
                     {
                         BackendReturnObject servertime = Backend.Utils.GetServerTime();

                         string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
                         DateTime currentServerTime = DateTime.Parse(time).ToUniversalTime().AddHours(9);

                         currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);

                         defultValues.Add(e.Current.Key, (double)currentServerDate);
                         tableDatas.Add(e.Current.Key, new ReactiveProperty<double>((double)currentServerDate));
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
                             tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(double.Parse(value)));
                         }
                         else
                         {
                             defultValues.Add(e.Current.Key, e.Current.Value);
                             tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(e.Current.Value));

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
                         return;//
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

    public void UpData(string key, double data, bool LocalOnly, Action failCallBack = null)
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
    }
    private void UpdatekillCount()
    {
        UpData(dailyEnemyKillCount, false);

        if (ServerData.userInfoTable.IsMonthlyPass2() == false)
        {
            UpData(killCountTotal, false);
        }
        else
        {
            UpData(killCountTotal2, false);
        }

        UpData(killCountTotalChild, false);
        UpData(attenCountOne, false);
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
            var isSuccess = bro.IsSuccess();
            var statusCode = bro.GetStatusCode();
            var returnValue = bro.GetReturnValue();

            if (isSuccess && statusCode.Equals("200") && returnValue != null)
            {
                string time = bro.GetReturnValuetoJSON()["utcTime"].ToString();

                currentServerTime = DateTime.Parse(time).ToUniversalTime().AddHours(9);

#if UNITY_EDITOR
                //currentServerTime = currentServerTime.AddDays(15);
#endif

                whenServerTimeUpdated.Execute();

                currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);

                //day check
                DateTime savedDate = Utils.ConvertFromUnixTimestamp(tableDatas[LastLogin].Value - 2f);

                if (isFirstInit)
                {
                    isFirstInit = false;
                    int elapsedTime = (int)(currentServerTime - savedDate).TotalSeconds;

                    //최소조건 안됨 (시간,첫 접속)
                    if (elapsedTime < GameBalance.sleepRewardMinValue || ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value == -1)
                    {
                        return;
                    }
                    else
                    {
                        //서버에 저장시켜봄
                        Param userInfoParam = new Param();

                        ServerData.userInfoTable.tableDatas[UserInfoTable.sleepRewardSavedTime].Value += elapsedTime;

                        userInfoParam.Add(sleepRewardSavedTime, ServerData.userInfoTable.tableDatas[UserInfoTable.sleepRewardSavedTime].Value);

                        var returnBro = Backend.GameData.Update(tableName, Indate, userInfoParam);

                        if (returnBro.IsSuccess() == false)
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크가 불안정 합니다.\n앱을 재실행 합니다.", () =>
                            {
                                Utils.RestartApplication();
                            });

                            return;
                        }
                    }
                }

                //week check
                int currentWeek = Utils.GetWeekNumber(currentServerTime);

                int savedWeek = Utils.GetWeekNumber(savedDate);

                if (savedDate.Day != currentServerTime.Day)
                {
                    Debug.LogError("@@@Day Changed!");
                    //날짜 바뀜
                    DateChanged(currentServerTime.Day, savedWeek != currentWeek, savedDate.Month != currentServerTime.Month);
                    attendanceUpdatedTime = currentServerTime.Day;
                }
                else
                {
                    UpdateLastLoginOnly();
                }
            }
            else
            {
                // LogManager.Instance.SendLog("출석", $"{isSuccess}/{statusCode}/{returnValue}");
            }
        });
    }

    private void UpdateLastLoginOnly()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value = (double)currentServerDate;

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.LastLogin, Math.Truncate(ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value));
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList, true);
    }


    private void DateChanged(int day, bool weekChanged, bool monthChanged)
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
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value = 0;

        ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.SendGuildPoint).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.sendGangChul).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getSmith).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.sendPetExp).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.oneAttenEvent).Value = 0;

        //버프
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp2).Value = 0;

        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff0).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff3).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.one_Buff).Value = 0;

        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul0_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul1_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul2_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul3_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul4_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul5_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul6_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul7_buff).Value = 0;
        //

        ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value = (double)currentServerDate;

        //두번타는거 방지
        if (attendanceUpdatedTime != day)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_100Day).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Seol).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value++;

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.oakpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.oakpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.marblepensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.marblepensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.hellpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.hellpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.relicpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.relicpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.peachAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.peachAttendance).Value++;
            }


            if (ServerData.iapServerTable.TableDatas[UserInfoTable.smithpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.smithpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.weaponpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.weaponpensionAttendance).Value++;
            }
        }

        attendanceUpdatedTime = ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value;

        userInfoParam.Add(UserInfoTable.dailyEnemyKillCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value);
        userInfoParam.Add(UserInfoTable.dailyTicketBuyCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value);
        userInfoParam.Add(UserInfoTable.receivedTicketReward, ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value);
        userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
        userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);
        userInfoParam.Add(UserInfoTable.LastLogin, Math.Truncate(ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value));
        userInfoParam.Add(UserInfoTable.attendanceCount, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value);
        userInfoParam.Add(UserInfoTable.attendanceCount_100Day, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_100Day).Value);
        userInfoParam.Add(UserInfoTable.attendanceCount_Seol, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Seol).Value);
        userInfoParam.Add(UserInfoTable.attenCountBok, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value);

        userInfoParam.Add(UserInfoTable.oakpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.oakpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.marblepensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.marblepensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.relicpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.relicpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.peachAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.peachAttendance).Value);
        userInfoParam.Add(UserInfoTable.smithpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.smithpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.weaponpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.weaponpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.hellpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.hellpensionAttendance).Value);

        userInfoParam.Add(UserInfoTable.freeWeapon, ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value);
        userInfoParam.Add(UserInfoTable.freeNorigae, ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value);
        userInfoParam.Add(UserInfoTable.freeSkill, ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value);
        userInfoParam.Add(UserInfoTable.SendGuildPoint, ServerData.userInfoTable.GetTableData(UserInfoTable.SendGuildPoint).Value);
        userInfoParam.Add(UserInfoTable.sendGangChul, ServerData.userInfoTable.GetTableData(UserInfoTable.sendGangChul).Value);
        userInfoParam.Add(UserInfoTable.getSmith, ServerData.userInfoTable.GetTableData(UserInfoTable.getSmith).Value);
        userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value);
        userInfoParam.Add(UserInfoTable.sendPetExp, ServerData.userInfoTable.GetTableData(UserInfoTable.sendPetExp).Value);
        userInfoParam.Add(UserInfoTable.oneAttenEvent, ServerData.userInfoTable.GetTableData(UserInfoTable.oneAttenEvent).Value);

        userInfoParam.Add(UserInfoTable.buff_gold1, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold1).Value);
        userInfoParam.Add(UserInfoTable.buff_gold2, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold2).Value);
        userInfoParam.Add(UserInfoTable.buff_exp1, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp1).Value);
        userInfoParam.Add(UserInfoTable.buff_exp2, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp2).Value);

        userInfoParam.Add(UserInfoTable.guild_buff0, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff0).Value);
        userInfoParam.Add(UserInfoTable.guild_buff1, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff1).Value);
        userInfoParam.Add(UserInfoTable.guild_buff2, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff2).Value);
        userInfoParam.Add(UserInfoTable.guild_buff3, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff3).Value);
        userInfoParam.Add(UserInfoTable.one_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.one_Buff).Value);

        userInfoParam.Add(UserInfoTable.yomul0_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul0_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul1_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul1_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul2_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul2_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul3_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul3_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul4_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul4_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul5_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul5_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul6_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul6_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul7_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul7_buff).Value);

        //요괴소굴
        Param yoguiSogulParam = new Param();

        //채팅 테두리 초기화
        if (weekChanged)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 0f;
            userInfoParam.Add(UserInfoTable.chatFrame, ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value);
        }

        //손오공
        ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value = string.Empty;
        ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.sonReward, ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value);
        yoguiSogulParam.Add(EtcServerTable.hellReward, ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        //로컬
        ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.yoguiSogulReward, ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value);

        ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.guildAttenReward, ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].Value);

        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, yoguiSogulParam));

        Param iapParam = null;

        var iapTable = TableManager.Instance.InAppPurchase.dataArray;

        for (int i = 0; i < iapTable.Length; i++)
        {
            bool isDayChagned = (iapTable[i].BUYTYPE == BuyType.DayOfOne || iapTable[i].BUYTYPE == BuyType.DayOfFive);
            bool isWeekChagned = weekChanged == true && (iapTable[i].BUYTYPE == BuyType.WeekOfTwo || iapTable[i].BUYTYPE == BuyType.WeekOfFive);
            bool isMonthChanged = monthChanged == true && (iapTable[i].BUYTYPE == BuyType.MonthOfOne || iapTable[i].BUYTYPE == BuyType.MonthOfFive || iapTable[i].BUYTYPE == BuyType.MonthOfTen);

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

        //티켓
        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += GameBalance.DailyRelicTicketGetCount;

        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //길드보상 초기화
        ServerData.bossServerTable.TableDatas["boss12"].rewardedId.Value = string.Empty;
        ServerData.bossServerTable.TableDatas["boss20"].rewardedId.Value = string.Empty;

        Param bossParam = new Param();

        bossParam.Add("boss12", ServerData.bossServerTable.TableDatas["boss12"].ConvertToString());
        bossParam.Add("boss20", ServerData.bossServerTable.TableDatas["boss20"].ConvertToString());

        transactionList.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

        ServerData.SendTransaction(transactionList, false);
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

    public bool CanSpawnEventItem()
    {
        if (currentServerTime.Month == 11) return true;
        if (currentServerTime.Month == 12) return true;
        if (currentServerTime.Month == 1) return true;
        if (currentServerTime.Month == 2) return true;

        return false;
    }
    public bool CanMakeEventItem()
    {
        //if (currentServerTime.Month == 11) return true;
        //if (currentServerTime.Month == 12) return true;
        //if (currentServerTime.Month == 1) return true;
        //if (currentServerTime.Month == 2) return true;

        return false;
    }

    public bool CanBuyEventPackage()
    {
        if (currentServerTime.Month == 11) return true;
        if (currentServerTime.Month == 12) return true;
        if (currentServerTime.Month == 1) return true;
        if (currentServerTime.Month == 2) return true;

        return false;
    }

    public bool CanRecordGuildScore()
    {
        if (currentServerTime.Hour == 23
            || currentServerTime.Hour == 0
            || currentServerTime.Hour == 1
            || currentServerTime.Hour == 2
            || currentServerTime.Hour == 3
            || currentServerTime.Hour == 4) return false;

        return true;
    }

    public bool IsHotTime()
    {
#if UNITY_EDITOR
        //return true;
#endif

        if (currentServerTime.DayOfWeek != DayOfWeek.Sunday && currentServerTime.DayOfWeek != DayOfWeek.Saturday)
        {
            int currentHour = currentServerTime.Hour;
            return currentHour >= GameBalance.HotTime_Start && currentHour < GameBalance.HotTime_End;
        }
        else
        {
            int currentHour = currentServerTime.Hour;
            return currentHour >= GameBalance.HotTime_Start_Weekend && currentHour < GameBalance.HotTime_End;
        }


    }

    public bool IsWeekend()
    {
        return currentServerTime.DayOfWeek == DayOfWeek.Sunday || currentServerTime.DayOfWeek == DayOfWeek.Saturday;
    }

    static int totalKillCount = 0;
    static double updateRequireNum = 100;
    public void GetKillCountTotal()
    {
        totalKillCount += (int)GameManager.Instance.CurrentStageData.Marbleamount;

        if (totalKillCount < updateRequireNum)
        {

        }
        else
        {
            totalKillCount = 0;

            if (IsMonthlyPass2() == false)
            {
                tableDatas[killCountTotal].Value += updateRequireNum;
            }
            else
            {
                tableDatas[killCountTotal2].Value += updateRequireNum;
            }

            tableDatas[killCountTotalChild].Value += updateRequireNum;
            tableDatas[attenCountOne].Value += updateRequireNum;
        }

    }

    public bool IsLastFloor()
    {
        return tableDatas[currentFloorIdx].Value == 301;
    }

    public bool CanPlayGangChul()
    {
        return currentServerTime.DayOfWeek == DayOfWeek.Monday ||
         currentServerTime.DayOfWeek == DayOfWeek.Tuesday ||
         currentServerTime.DayOfWeek == DayOfWeek.Wednesday ||
         currentServerTime.DayOfWeek == DayOfWeek.Thursday ||
         currentServerTime.DayOfWeek == DayOfWeek.Friday;
    }

    public bool IsMonthlyPass2()
    {
#if UNITY_EDITOR
        return false;
#endif

        return currentServerTime.Month == 7;
    }
}
