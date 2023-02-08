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
    public static string Jade = "Jade"; //옥
    public static string GrowthStone = "GrowthStone";
    public static string Ticket = "Ticket";
    public static string BonusSpinKey = "BonusSpin";
    public static string MarbleKey = "Marble"; //여우구슬
    public static string DokebiKey = "Dokebi2";
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
    public static string Event_Item_1 = "Event2";
    public static string Event_Item_SnowMan = "SnowMan";//수박이었음
    public static string StageRelic = "StageRelic";
    public static string Peach = "PeachReal";
    public static string MiniGameReward = "MiniGameReward";
    public static string MiniGameReward2 = "mgr2";
    public static string GuildReward = "GuildReward";
    public static string SulItem = "SulItem";
    public static string SmithFire = "SmithFire";
    public static string FeelMulStone = "FeelMulStone";

    public static string Asura0 = "a0";
    public static string Asura1 = "a1";
    public static string Asura2 = "a2";
    public static string Asura3 = "a3";
    public static string Asura4 = "a4";
    public static string Asura5 = "a5";

    public static string Indra0 = "i0";
    public static string Indra1 = "i1";
    public static string Indra2 = "i2";
    public static string IndraPower = "ipw";

    public static string Aduk = "ad";

    public static string SinSkill0 = "s0";
    public static string SinSkill1 = "s1";
    public static string SinSkill2 = "s2";
    public static string SinSkill3 = "s3";
    public static string LeeMuGiStone = "LeeMuGiStone";
    public static string ZangStone = "ZS";
    public static string SwordPartial = "SP";

    public static string Hae_Norigae = "hn";
    public static string Hae_Pet = "hp";
    public static string NataSkill = "NataSkill";
    public static string OrochiSkill = "OrochiSkill";
    public static string GangrimSkill = "GangrimSkill";

    public static string OrochiTooth0 = "or0";
    public static string OrochiTooth1 = "or1";

    public static string gumiho0 = "g0";
    public static string gumiho1 = "g1";
    public static string gumiho2 = "g2";
    public static string gumiho3 = "g3";
    public static string gumiho4 = "g4";
    public static string gumiho5 = "g5";
    public static string gumiho6 = "g6";
    public static string gumiho7 = "g7";
    public static string gumiho8 = "g8";

    public const string Hel = "Hel";
    public static string h0 = "h0";
    public static string h1 = "h1";
    public static string h2 = "h2";
    public static string h3 = "h3";
    public static string h4 = "h4";
    public static string h5 = "h5";
    public static string h6 = "h6";
    public static string h7 = "h7";
    public static string h8 = "h8";
    public static string h9 = "h9";
    public static string Ym = "Ym";
    //두루마리
    public static string du = "du";


    public static string Sun0 = "Sun0";
    public static string Sun1 = "Sun1";
    public static string Sun2 = "Sun2";
    public static string Sun3 = "Sun3";
    public static string Sun4 = "Sun4";

    public static string Chun0 = "Chun0";
    public static string Chun1 = "Chun1";
    public static string Chun2 = "Chun2";
    public static string Chun3 = "Chun3";
    public static string Chun4 = "Chun4";

    public static string DokebiSkill0 = "DokebiSkill0";
    public static string DokebiSkill1 = "DokebiSkill1";
    public static string DokebiSkill2 = "DokebiSkill2";
    public static string DokebiSkill3 = "DokebiSkill3";
    public static string DokebiSkill4 = "DokebiSkill4";


    public static string FourSkill0 = "FS0";
    public static string FourSkill1 = "FS1";
    public static string FourSkill2 = "FS2";
    public static string FourSkill3 = "FS3";

    public static string Fw = "Fw";
    public const string Cw = "Cw"; //천계꽃

    public static string c0 = "c0"; //천계꽃
    public static string c1 = "c1"; //천계꽃
    public static string c2 = "c2"; //천계꽃
    public static string c3 = "c3"; //천계꽃
    public static string c4 = "c4"; //천계꽃
    public static string c5 = "c5"; //천계꽃
    public static string c6 = "c6"; //천계꽃

    public static string Event_Fall = "Event_Fall"; //곶감
    public static string Event_Fall_Gold = "Event_Fall_Gold"; //황금 곶감
    public static string Event_NewYear = "Event_NewYear"; //신년재화
    public static string Event_NewYear_All = "Event_NewYear_All"; //신년재화 총수급량

    public static string FoxMaskPartial = "FoxMaskPartial"; //여우 탈 재화
    public static string SusanoTreasure = "ST"; // 악귀퇴치 재화


    public const string DokebiFire = "DokebiFire"; //도깨비 나라 재화
    public static string DokebiFireKey = "DokebiFireKey"; //도깨비 불 입장권

    public static string Mileage = "Mileage"; //마일리지

    public static string HellPowerUp = "HellPowerUp";
    public static string DokebiTreasure = "DT";
    public static string DokebiFireEnhance = "DFE";
    public static string SumiFire = "SumiFire";
    public static string SumiFireKey = "SumiFireKey";
    public static string NewGachaEnergy = "NGE";
    public static string DokebiBundle = "DB";



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
       // {Songpyeon,0f},
        {TigerStone,0f},
        {Relic,0f},

        {RelicTicket,GameBalance.DailyRelicTicketGetCount},
        {RabitStone,0f},
        {Event_Item_0,0f},
        {Event_Item_1,0f},
        {Event_Item_SnowMan,0f},
        {DragonStone,0f},
        {StageRelic,0f},
        {SnakeStone,0f},
        {Peach,0f},
        {HorseStone,0f},
        {SheepStone,0f},
        {MonkeyStone,0f},
        {MiniGameReward,0f},
        {MiniGameReward2,0f},
        {GuildReward,0f},
        {CockStone,0f},
        {DogStone,0f},
        {SulItem,0f},
        {PigStone,0f},
        {SmithFire,0f},
        {FeelMulStone,0f},

        {Asura0,0f},
        {Asura1,0f},
        {Asura2,0f},
        {Asura3,0f},
        {Asura4,0f},
        {Asura5,0f},
        {Aduk,0f},

        {SinSkill0,0f},
        {SinSkill1,0f},
        {SinSkill2,0f},
        {SinSkill3,0f},
        {LeeMuGiStone,0f},
        {ZangStone,0f},
        {SwordPartial,0f},

        {Hae_Norigae,0f},
        {Hae_Pet,0f},

        {Indra0,0f},
        {Indra1,0f},
        {Indra2,0f},
        {IndraPower,0f},
        {NataSkill,0f},
        {OrochiSkill,0f},
        {GangrimSkill,0f},
        {OrochiTooth0,0f},
        {OrochiTooth1,0f},
        //
        {gumiho0,0f},
        {gumiho1,0f},
        {gumiho2,0f},
        {gumiho3,0f},
        {gumiho4,0f},
        {gumiho5,0f},
        {gumiho6,0f},
        {gumiho7,0f},
        {gumiho8,0f},
        //
        {Hel,0f},
        {h0,0f},
        {h1,0f},
        {h2,0f},
        {h3,0f},
        {h4,0f},
        {h5,0f},
        {h6,0f},
        {h7,0f},
        {h8,0f},
        {h9,0f},
        {Ym,0f},
        {du,0f},

        {Sun0,0f},
        {Sun1,0f},
        {Sun2,0f},
        {Sun3,0f},
        {Sun4,0f},

        {Chun0,0f},
        {Chun1,0f},
        {Chun2,0f},
        {Chun3,0f},
        {Chun4,0f},

        {DokebiSkill0,0f},
        {DokebiSkill1,0f},
        {DokebiSkill2,0f},
        {DokebiSkill3,0f},
        {DokebiSkill4,0f},

        {FourSkill0,0f},
        {FourSkill1,0f},
        {FourSkill2,0f},
        {FourSkill3,0f},

        {Fw,0f},
        {Cw,0f},
        //

        {c0,0f},
        {c1,0f},
        {c2,0f},
        {c3,0f},
        {c4,0f},
        {c5,0f},
        {c6,0f},

        {Event_Fall,0f},
        {Event_Fall_Gold,0f},
        {Event_NewYear,0f},
        {Event_NewYear_All,0f},
        {FoxMaskPartial,0f},

        {DokebiFire,0f},
        {DokebiFireKey,0f},
        {DokebiFireEnhance,0f},

        {Mileage,0f},
        {HellPowerUp,0f},
        {DokebiTreasure,0f},
        {SusanoTreasure,0f},
        {SumiFire,0f},
        {SumiFireKey,0f},
        {NewGachaEnergy,0f},
        {DokebiBundle,0f},
    };

    private ReactiveDictionary<string, ReactiveProperty<float>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<float>>();
    public ReactiveDictionary<string, ReactiveProperty<float>> TableDatas => tableDatas;

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
        amount += PlayerStats.GetSmithValue(StatusType.growthStoneUp);

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
        eventItemAddNum += (int)amount;

        if (eventItemAddNum < updateRequireNum)
        {

        }
        else
        {
            tableDatas[Event_Item_SnowMan].Value += eventItemAddNum;
            eventItemAddNum = 0;
        }
    }
    //

    public int GetFourSkillHasCount()
    {
        int fourLevel = 0;

        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).Value == 1)
        {
            fourLevel++;
        }
        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill1).Value == 1)
        {
            fourLevel++;
        }
        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill2).Value == 1)
        {
            fourLevel++;
        }
        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill3).Value == 1)
        {
            fourLevel++;
        }
        return fourLevel;
    }

    static int eventItemAddNum_Spring = 0;
    public void GetSpringEventItem(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Event_Fall)} 획득(+{(int)amount})");

        eventItemAddNum_Spring += (int)amount;

        if (eventItemAddNum_Spring < updateRequireNum)
        {

        }
        else
        {
            //tableDatas[Event_Item_1].Value += eventItemAddNum_Spring;
            tableDatas[Event_Fall].Value += eventItemAddNum_Spring;
            eventItemAddNum_Spring = 0;
        }
    }

    //

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

    public void SyncAllData(List<string> ignoreList = null)
    {
        Param param = new Param();

        var e = tableDatas.GetEnumerator();
        while (e.MoveNext())
        {
            if (ignoreList != null && ignoreList.Contains(e.Current.Key) == true) continue;
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

        //if (ServerData.userInfoTable.CanSpawnEventItem())
        //{
        //    goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
        //}

        //goodsParam.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);

        if (ServerData.userInfoTable.CanSpawnGotGamEventItem())
        {
            goodsParam.Add(GoodsTable.Event_Fall, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall).Value);
        }

        goodsParam.Add(GoodsTable.Event_Item_SnowMan, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan).Value);

        goodsParam.Add(GoodsTable.Event_NewYear, ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear).Value);

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
    public ReactiveProperty<float> GetTableData(Item_Type key)
    {
        string stringKey = ItemTypeToServerString(key);
        return tableDatas[stringKey];
    }
    public string ItemTypeToServerString(Item_Type type)
    {
        switch (type)
        {
            case Item_Type.Gold:
                {
                    return GoodsTable.Gold;
                }
            case Item_Type.Jade:
                {
                    return GoodsTable.Jade;
                }
            case Item_Type.GrowthStone:
                {
                    return GoodsTable.GrowthStone;
                }
            case Item_Type.Ticket:
                {
                    return GoodsTable.Ticket;
                }
                
            case Item_Type.Marble:
                {
                    return GoodsTable.MarbleKey;
                }
                
            case Item_Type.Songpyeon:
                {
                    return GoodsTable.Songpyeon;
                }
                
            case Item_Type.RelicTicket:
                {
                    return GoodsTable.RelicTicket;
                }
                
            case Item_Type.Event_Item_0:
                {
                    return GoodsTable.Event_Item_0;
                }
                

            case Item_Type.Event_Item_1:
                {
                    return GoodsTable.Event_Item_1;
                }
                
            case Item_Type.Event_Item_SnowMan:
                {
                    return GoodsTable.Event_Item_SnowMan;
                }
                

            case Item_Type.SulItem:
                {
                    return GoodsTable.SulItem;
                }
                

            case Item_Type.FeelMulStone:
                {
                    return GoodsTable.FeelMulStone;
                }
                

            case Item_Type.Asura0:
                {
                    return GoodsTable.Asura0;
                }
                

            case Item_Type.Asura1:
                {
                    return GoodsTable.Asura1;
                }
                

            case Item_Type.Asura2:
                {
                    return GoodsTable.Asura2;
                }
                

            case Item_Type.Asura3:
                {
                    return GoodsTable.Asura3;
                }
                
            case Item_Type.Asura4:
                {
                    return GoodsTable.Asura4;
                }
                

            case Item_Type.Asura5:
                {
                    return GoodsTable.Asura5;
                }
                
            case Item_Type.Aduk:
                {
                    return GoodsTable.Aduk;
                }
                

            case Item_Type.LeeMuGiStone:
                {
                    return GoodsTable.LeeMuGiStone;
                }
                

            //
            case Item_Type.SinSkill0:
                {
                    return GoodsTable.SinSkill0;
                }
                
            case Item_Type.SinSkill1:
                {
                    return GoodsTable.SinSkill1;
                }
                
            case Item_Type.SinSkill2:
                {
                    return GoodsTable.SinSkill2;
                }
                
            case Item_Type.SinSkill3:
                {
                    return GoodsTable.SinSkill3;
                }
                
            case Item_Type.NataSkill:
                {
                    return GoodsTable.NataSkill;
                }
                
            case Item_Type.OrochiSkill:
                {
                    return GoodsTable.OrochiSkill;
                }
                
            //
            case Item_Type.Sun0:
                {
                    return GoodsTable.Sun0;
                }
                
            case Item_Type.Sun1:
                {
                    return GoodsTable.Sun1;
                }
                
            case Item_Type.Sun2:
                {
                    return GoodsTable.Sun2;
                }
                
            case Item_Type.Sun3:
                {
                    return GoodsTable.Sun3;
                }
                
            case Item_Type.Sun4:
                {
                    return GoodsTable.Sun4;
                }
                
            //
            case Item_Type.Chun0:
                {
                    return GoodsTable.Chun0;
                }
                
            case Item_Type.Chun1:
                {
                    return GoodsTable.Chun1;
                }
                
            case Item_Type.Chun2:
                {
                    return GoodsTable.Chun2;
                }
                
            case Item_Type.Chun3:
                {
                    return GoodsTable.Chun3;
                }
                
            case Item_Type.Chun4:
                {
                    return GoodsTable.Chun4;
                }
                
            case Item_Type.DokebiSkill0:
                {
                    return GoodsTable.DokebiSkill0;
                }
                
            case Item_Type.DokebiSkill1:
                {
                    return GoodsTable.DokebiSkill1;
                }
                
            case Item_Type.DokebiSkill2:
                {
                    return GoodsTable.DokebiSkill2;
                }
                
            case Item_Type.DokebiSkill3:
                {
                    return GoodsTable.DokebiSkill3;
                }
                
            case Item_Type.DokebiSkill4:
                {
                    return GoodsTable.DokebiSkill4;
                }
                
            //            //
            case Item_Type.FourSkill0:
                {
                    return GoodsTable.FourSkill0;
                }
                
            case Item_Type.FourSkill1:
                {
                    return GoodsTable.FourSkill1;
                }
                
            case Item_Type.FourSkill2:
                {
                    return GoodsTable.FourSkill2;
                }
                
            case Item_Type.FourSkill3:
                {
                    return GoodsTable.FourSkill3;
                }
                
            //
            case Item_Type.GangrimSkill:
                {
                    return GoodsTable.GangrimSkill;
                }
                
            //

            case Item_Type.SmithFire:
                {
                    return GoodsTable.SmithFire;
                }
                

            case Item_Type.StageRelic:
                {
                    return GoodsTable.StageRelic;
                }
                

            case Item_Type.PeachReal:
                {
                    return GoodsTable.Peach;
                }
                
            case Item_Type.GuildReward:
                {
                    return GoodsTable.GuildReward;
                }
                
            case Item_Type.SP:
                {
                    return GoodsTable.SwordPartial;
                }
                
            case Item_Type.Hel:
                {
                    return GoodsTable.Hel;
                }
                
            case Item_Type.Ym:
                {
                    return GoodsTable.Ym;
                }
                
            case Item_Type.Fw:
                {
                    return GoodsTable.Fw;
                }
                

            case Item_Type.Cw:
                {
                    return GoodsTable.Cw;
                }
                
            case Item_Type.DokebiFire:
                {
                    return GoodsTable.DokebiFire;
                }
                
            case Item_Type.SumiFire:
                {
                    return GoodsTable.SumiFire;
                }
                
            case Item_Type.NewGachaEnergy:
                {
                    return GoodsTable.NewGachaEnergy;
                }
                
            case Item_Type.DokebiBundle:
                {
                    return GoodsTable.DokebiBundle;
                }
                
            case Item_Type.DokebiFireKey:
                {
                    return GoodsTable.DokebiFireKey;
                }
                
            case Item_Type.SumiFireKey:
                {
                    return GoodsTable.SumiFireKey;
                }
                
            default:
                return type.ToString();
        }
    }
}
