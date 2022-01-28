using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Game.GameInfo;
using LitJson;
using System;
using UniRx;



public class GoodsTable
{
    public static string Indate;
    public const string tableName = "Goods";
    public static string Gold = "Gold";
    public static string Jade = "Jade";
    public static string GrowthStone = "GrowthStone";
    public static string Ticket = "Ticket";
    public static string BonusSpinKey = "BonusSpin";
    public static string MarbleKey = "Marble";
    public static string DokebiKey = "Dokebi";
    public static string SkillPartion = "SkillPartion";
    public static string WeaponUpgradeStone = "WeaponUpgradeStone";
    public static string PetUpgradeSoul = "PetUpgradeSoul";
    public static string YomulExchangeStone = "YomulExchangeStone";
    public static string Songpyeon = "Songpyeon";
    public static string TigerStone = "TigerStone";
    public static string RabitStone = "RabitStone";
    public static string DragonStone = "DragonStone";
    public static string SnakeStone = "SnakeStone";
    public static string HorseStone = "HorseStone";
    public static string SheepStone = "SheepStone";
    public static string MonkeyStone = "MonkeyStone";
    public static string CockStone = "CockStone";
    public static string DogStone = "DogStone";
    public static string PigStone = "PigStone";

    //포션
    public static string Potion_0 = "Potion_0";
    public static string Potion_1 = "Potion_1";
    public static string Potion_2 = "Potion_2";

    public static string Relic = "Relic";
    public static string RelicTicket = "RelicTicket";

    //할로윈 도깨비
    public static string Event_Item_0 = "Event_Item_1";
    public static string StageRelic = "StageRelic";
    public static string Peach = "PeachReal";
    public static string MiniGameReward = "MiniGameReward";
    public static string GuildReward = "GuildReward";
    public static string SulItem = "SulItem";

    private Dictionary<string, float> tableSchema = new Dictionary<string, float>()
    {
        {Gold,GameBalance.StartingMoney},
        {Jade,0f},
        {GrowthStone,0f},
        {Ticket,0f},
        {Potion_0,0f},
        {Potion_1,0f},
        {Potion_2,0f},
        {BonusSpinKey,0f},
        {MarbleKey,0f},
        {DokebiKey,0f},
        {SkillPartion,0f},
        {WeaponUpgradeStone,0f},
        {PetUpgradeSoul,0f},
        {YomulExchangeStone,0f},
        {Songpyeon,0f},
        {TigerStone,0f},
        {Relic,0f},

        {RelicTicket,GameBalance.DailyRelicTicketGetCount},
        {RabitStone,0f},
        {Event_Item_0,0f},
        {DragonStone,0f},
        {StageRelic,0f},
        {SnakeStone,0f},
        {Peach,0f},
        {HorseStone,0f},
        {SheepStone,0f},
        {MonkeyStone,0f},
        {MiniGameReward,0f},
        {GuildReward,0f},
        {CockStone,0f},
        {DogStone,0f},
        {SulItem,0f},
        {PigStone,0f},
    };

    private ReactiveDictionary<string, ReactiveProperty<float>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<float>>();

    public static string GetPosionKey(int idx)
    {
        if (idx == 0)
        {
            return Potion_0;
        }
        else if (idx == 1)
        {
            return Potion_1;

        }
        else
        {
            return Potion_2;
        }
    }

    public ReactiveProperty<float> GetTableData(string key)
    {
        return tableDatas[key];
    }

    public float GetCurrentGoods(string key)
    {
        return tableDatas[key].Value;
    }

    public void GetGold(float amount)
    {
        tableDatas[Gold].Value += amount;
    }

    static int growThStoneAddAmount = 0;
    static float updateRequireNum_GrowthStone = 100000;

    public void GetMagicStone(float amount)
    {
        float magicStonePlusValue = PlayerStats.GetMagicStonePlusValue();

        int amount_int = (int)(amount + amount * magicStonePlusValue);

        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.GrowthStone)} 획득(+{amount_int})");

        growThStoneAddAmount += amount_int;

        if (growThStoneAddAmount < updateRequireNum_GrowthStone)
        {

        }
        else
        {
            tableDatas[GrowthStone].Value += growThStoneAddAmount;
            growThStoneAddAmount = 0;
        }
    }
    static int marbleAddAmount = 0;
    static float updateRequireNum = 100;
    public void GetMarble(float amount)
    {
        float magicMarblePlusValue = PlayerStats.GetMarblePlusValue();

        int amount_int = (int)(amount + amount * magicMarblePlusValue);

        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Marble)} 획득(+{amount_int})");

        marbleAddAmount += amount_int;

        if (marbleAddAmount < updateRequireNum)
        {

        }
        else
        {
            tableDatas[MarbleKey].Value += marbleAddAmount;
            marbleAddAmount = 0;
        }
    }
    static int soulAddAmount = 0;

    public void GetPetUpgradeSoul(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)} 획득(+{(int)amount})");

        soulAddAmount += (int)amount;

        if (soulAddAmount < updateRequireNum)
        {

        }
        else
        {
            tableDatas[PetUpgradeSoul].Value += soulAddAmount;
            soulAddAmount = 0;
        }
    }

    static int eventItemAddNum = 0;
    public void GetEventItem(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Event_Item_0)} 획득(+{(int)amount})");

        eventItemAddNum += (int)amount;

        if (eventItemAddNum < updateRequireNum)
        {

        }
        else
        {
            tableDatas[Event_Item_0].Value += eventItemAddNum;
            eventItemAddNum = 0;
        }
    }

    static int sulAddNum = 0;
    public void GetsulItem(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.SulItem)} 획득(+{(int)amount})");

        sulAddNum += (int)amount;

        if (sulAddNum < updateRequireNum)
        {

        }
        else
        {
            tableDatas[SulItem].Value += sulAddNum;
            sulAddNum = 0;
        }
    }

    static int stageRelicAddNum = 0;
    public void GetStageRelic(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.StageRelic)} 획득(+{(int)amount})");

        stageRelicAddNum += (int)amount;

        if (stageRelicAddNum < updateRequireNum)
        {

        }
        else
        {
            tableDatas[StageRelic].Value += stageRelicAddNum;
            stageRelicAddNum = 0;
        }
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
                      tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
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

    public void AddLocalData(string key, float amount)
    {
        tableDatas[key].Value += amount;
    }

    public void UpData(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpData(string key, float data, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Growth {key} is not exist");
            return;
        }
        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            SyncToServerEach(key);
        }
    }

    public void SyncToServerEach(string key, Action whenSyncSuccess = null, Action whenRequestComplete = null, Action whenRequestFailed = null)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            whenRequestComplete?.Invoke();

            if (e.IsSuccess())
            {
                whenSyncSuccess?.Invoke();
            }
            else if (e.IsSuccess() == false)
            {
                if (whenRequestFailed != null)
                {
                    whenRequestFailed.Invoke();

                }
                Debug.Log($"Growth {key} sync failed");
                return;
            }
        });
    }

    public void SyncAllData(List<string> syncData = null)
    {
        Param param = new Param();

        var e = tableDatas.GetEnumerator();
        while (e.MoveNext())
        {
            if (syncData != null && syncData.Contains(e.Current.Key) == false) continue;
            param.Add(e.Current.Key, e.Current.Value.Value);
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
        {
#if UNITY_EDITOR
            if (bro.IsSuccess() == false)
            {
                Debug.Log($"SyncAllData {tableName} up failed");
                return;
            }
            else
            {
                Debug.Log($"SyncAllData {tableName} up Complete");
                return;
            }
#endif
        });
    }

    public void SyncAllDataForce()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);
        goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
        goodsParam.Add(GoodsTable.SulItem, ServerData.goodsTable.GetTableData(GoodsTable.SulItem).Value);
        goodsParam.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);
        goodsParam.Add(GoodsTable.BonusSpinKey, ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.sleepRewardSavedTime, ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value);

        //경험치
        Param statusParam = new Param();
        //레벨
        statusParam.Add(StatusTable.Level, ServerData.statusTable.GetTableData(StatusTable.Level).Value);

        //스킬포인트
        statusParam.Add(StatusTable.SkillPoint, ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        //스탯포인트
        statusParam.Add(StatusTable.StatPoint, ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value);

        Param growthParam = new Param();
        growthParam.Add(GrowthTable.Exp, ServerData.growthTable.GetTableData(GrowthTable.Exp).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactions.Add(TransactionValue.SetUpdate(GrowthTable.tableName, GrowthTable.Indate, growthParam));

        var bro = Backend.GameData.TransactionWrite(transactions);
    }
}
