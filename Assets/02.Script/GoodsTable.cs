using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
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

    //포션
    public static string Potion_0 = "Potion_0";
    public static string Potion_1 = "Potion_1";
    public static string Potion_2 = "Potion_2";


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
        {DokebiKey,0f}
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

    public void GetMagicStone(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.GrowthStone)} 획득(+{(int)amount})");
        tableDatas[GrowthStone].Value += amount;
    }
    public void GetMarble(float amount)
    {
        SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Marble)} 획득(+{(int)amount})");
        tableDatas[MarbleKey].Value += amount;
    }
    public void GetBlueStone(float amount)
    {
        tableDatas[Jade].Value += amount;
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
        Param param = new Param();

        var e = tableDatas.GetEnumerator();
        while (e.MoveNext())
        {
            param.Add(e.Current.Key, e.Current.Value.Value);
        }

        Backend.GameData.Update(tableName, Indate, param);
    }
}
