using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using static UiRewardView;

public static class ServerData
{
    public static UserInfoTable userInfoTable { get; private set; } = new UserInfoTable();
    public static StatusTable statusTable { get; private set; } = new StatusTable();
    public static GrowthTable growthTable { get; private set; } = new GrowthTable();
    public static GoodsTable goodsTable { get; private set; } = new GoodsTable();
    public static WeaponTable weaponTable { get; private set; } = new WeaponTable();
    public static SkillServerTable skillServerTable { get; private set; } = new SkillServerTable();
    public static DailyMissionTable dailyMissionTable { get; private set; } = new DailyMissionTable();
    public static CollectionTable collectionTable { get; private set; } = new CollectionTable();
    public static EquipmentTable equipmentTable { get; private set; } = new EquipmentTable();
    public static MagicBookTable magicBookTable { get; private set; } = new MagicBookTable();
    public static PetServerTable petTable { get; private set; } = new PetServerTable();
    public static RankTable_Level rankTables_level { get; private set; } = new RankTable_Level();
    public static RankTable_Stage rankTables_Stage { get; private set; } = new RankTable_Stage();
    public static RankTable_Boss rankTables_Boss { get; private set; } = new RankTable_Boss();
    public static RankTable_Real_Boss rankTables_Real_Boss { get; private set; } = new RankTable_Real_Boss();
    public static PassServerTable passServerTable { get; private set; } = new PassServerTable();

    public static CostumeServerTable costumeServerTable { get; private set; } = new CostumeServerTable();

    public static DailyPassServerTable dailyPassServerTable { get; private set; } = new DailyPassServerTable();
    public static IAPServerTable iapServerTable { get; private set; } = new IAPServerTable();

    public static BossServerTable bossServerTable { get; private set; } = new BossServerTable();
    public static AttendanceServerTable attendanceServerTable { get; private set; } = new AttendanceServerTable();

    // public static FieldBossServerTable fieldBossTable { get; private set; } = new FieldBossServerTable();

    public static BuffServerTable buffServerTable { get; private set; } = new BuffServerTable();
    public static PassiveServerTable passiveServerTable { get; private set; } = new PassiveServerTable();

    public static MarbleServerTable marbleServerTable { get; private set; } = new MarbleServerTable();
    public static EtcServerTable etcServerTable { get; private set; } = new EtcServerTable();
    public static NewLevelPass newLevelPass { get; private set; } = new NewLevelPass();
    public static IAPServerTableTotal iAPServerTableTotal { get; private set; } = new IAPServerTableTotal();
    public static TitleServerTable titleServerTable { get; private set; } = new TitleServerTable();
    public static PensionServerTable pensionServerTable { get; private set; } = new PensionServerTable();
    public static YomulServerTable yomulServerTable { get; private set; } = new YomulServerTable();
    public static CostumePreset costumePreset { get; private set; } = new CostumePreset();

    #region string
    public static string inDate_str = "inDate";
    public static string format_string = "S";
    public static string format_Number = "N";
    public static string format_bool = "BOOL";
    public static string format_dic = "M";
    public static string format_list = "L";

    //  BOOL bool boolean 형태의 데이터가 이에 해당됩니다.
    //  N   numbers int, float, double 등 모든 숫자형 데이터는 이에 해당됩니다.
    //  S   string  string 형태의 데이터가 이에 해당됩니다.
    //  L list    list 형태의 데이터가 이에 해당됩니다.
    //  M map map, dictionary 형태의 데이터가 이에 해당됩니다.
    //  NULL    null	값이 존재하지 않는 경우 이에 해당됩니다.
    #endregion

    //트렌젝션으로 수정
    public static void LoadTables()
    {
        statusTable.Initialize();
        growthTable.Initialize();
        goodsTable.Initialize();
        weaponTable.Initialize();
        skillServerTable.Initialize();
        dailyMissionTable.Initialize();
        //collectionTable.Initialize();
        equipmentTable.Initialize();
        magicBookTable.Initialize();
        petTable.Initialize();
        userInfoTable.Initialize();
        rankTables_level.Initialize();
        rankTables_Stage.Initialize();
        rankTables_Boss.Initialize();
        rankTables_Real_Boss.Initialize();
        passServerTable.Initialize();
        costumeServerTable.Initialize();
        dailyPassServerTable.Initialize();
        iapServerTable.Initialize();
        //rankTable_InfinityTower.Initialize();
        bossServerTable.Initialize();
        attendanceServerTable.Initialize();
        //fieldBossTable.Initialize();
        buffServerTable.Initialize();
        passiveServerTable.Initialize();
        //rankTables_Boss1.Initialize();
        marbleServerTable.Initialize();
        etcServerTable.Initialize();

        newLevelPass.Initialize();

        iAPServerTableTotal.Initialize();

        titleServerTable.Initialize();

        pensionServerTable.Initialize();

        yomulServerTable.Initialize();

        costumePreset.Initialize();
    }

    public static void GetUserInfo()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValue());
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
        }
    }

    public static void ShowCommonErrorPopup(BackendReturnObject bro, Action retryCallBack)
    {
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.DataLoadFailedRetry}\n{bro.GetStatusCode()}", retryCallBack);

    }


    public static void SendTransaction(List<TransactionValue> transactionList, bool retry = true, Action completeCallBack = null, Action successCallBack = null)
    {
        SendQueue.Enqueue(Backend.GameData.TransactionWrite, transactionList, (bro) =>
       {
           if (bro.IsSuccess())
           {
               successCallBack?.Invoke();
           }
           else
           {
               Debug.LogError($"SendTransaction error!!! {bro.GetMessage()}");

               if (retry)
               {
                   CoroutineExecuter.Instance.StartCoroutine(TransactionRetryRoutine(transactionList));
               }
               else
               {
                   PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크가 불안정 합니다.\n앱을 재실행 합니다.", () =>
                   {
                       Utils.RestartApplication();
                   });
               }
           }

           completeCallBack?.Invoke();
       });
    }

    private static WaitForSeconds retryWs = new WaitForSeconds(3.0f);
    private static IEnumerator TransactionRetryRoutine(List<TransactionValue> transactionList)
    {
        yield return retryWs;
        SendTransaction(transactionList, retry: false);
    }

    public static void AddLocalValue(Item_Type type, float rewardValue)
    {
        switch (type)
        {
            case Item_Type.Gold:
                //로컬
                ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value += rewardValue;
                break;
            case Item_Type.Jade:
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardValue;
                break;
            case Item_Type.Marble:
                ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += rewardValue;
                break;
            case Item_Type.GrowthStone:
                ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += rewardValue;
                break;
            case Item_Type.Memory:
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value += (int)rewardValue;
                break;
            case Item_Type.Ticket:
                ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += rewardValue;
                break;
            case Item_Type.costume1:
                ServerData.costumeServerTable.TableDatas[type.ToString()].hasCostume.Value = true;
                break;
            default:
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"Item_Type {type} is not defined", null);
                }
                break;
        }
    }

    public static TransactionValue GetItemTypeTransactionValue(Item_Type type)
    {
        Param passParam = new Param();
        //패스 보상

        switch (type)
        {
            case Item_Type.Gold:
                passParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, passParam);
                break;
            case Item_Type.Jade:
                passParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, passParam);
                break;
            case Item_Type.GrowthStone:
                passParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, passParam);
                break;
            case Item_Type.Memory:
                passParam.Add(StatusTable.Memory, ServerData.statusTable.GetTableData(StatusTable.Memory).Value);
                return TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, passParam);
                break;
            case Item_Type.Ticket:
                passParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, passParam);
            case Item_Type.Marble:
                passParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, passParam);
            case Item_Type.Dokebi:
                passParam.Add(GoodsTable.DokebiKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, passParam);
            case Item_Type.costume1:
                string costumeKey = type.ToString();
                passParam.Add(costumeKey, ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());
                return TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, passParam);
                break;
        }

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 트랜젝션 타입 {type}", null);

        return null;
    }

    public static TransactionValue GetItemTypeTransactionValueForAttendance(Item_Type type, int amount)
    {
        Param param = new Param();
        //패스 보상

        switch (type)
        {
            case Item_Type.Jade:
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                param.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, param);
                break;
            case Item_Type.GrowthStone:
                ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                param.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, param);
                break;
            case Item_Type.Memory:
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value += amount;
                param.Add(StatusTable.Memory, ServerData.statusTable.GetTableData(StatusTable.Memory).Value);
                return TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, param);
                break;
            case Item_Type.Ticket:
                ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                param.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, param);
            case Item_Type.Marble:
                ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                param.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, param);
            case Item_Type.WeaponUpgradeStone:
                ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).Value += amount;
                param.Add(GoodsTable.WeaponUpgradeStone, ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, param);

            case Item_Type.YomulExchangeStone:
                ServerData.goodsTable.GetTableData(GoodsTable.YomulExchangeStone).Value += amount;
                param.Add(GoodsTable.YomulExchangeStone, ServerData.goodsTable.GetTableData(GoodsTable.YomulExchangeStone).Value);
                return TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, param);

            case Item_Type.costume1:
            case Item_Type.costume4:
                {
                    string costumeKey = type.ToString();
                    ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value = true;
                    param.Add(costumeKey, ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());
                    return TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, param);
                }
                break;
            case Item_Type.weapon0:
            case Item_Type.weapon1:
            case Item_Type.weapon2:
            case Item_Type.weapon3:
            case Item_Type.weapon4:
            case Item_Type.weapon5:
            case Item_Type.weapon6:
            case Item_Type.weapon7:
            case Item_Type.weapon8:
            case Item_Type.weapon9:
            case Item_Type.weapon10:
            case Item_Type.weapon11:
            case Item_Type.weapon12:
            case Item_Type.weapon13:
            case Item_Type.weapon14:
            case Item_Type.weapon15:
            case Item_Type.weapon16:
                {
                    string key = type.ToString();
                    ServerData.weaponTable.TableDatas[key].hasItem.Value = 1;
                    ServerData.weaponTable.TableDatas[key].amount.Value += amount;

                    param.Add(key, ServerData.weaponTable.TableDatas[key].ConvertToString());

                    return TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, param);
                }
                break;
            case Item_Type.magicBook0:
            case Item_Type.magicBook1:
            case Item_Type.magicBook2:
            case Item_Type.magicBook3:
            case Item_Type.magicBook4:
            case Item_Type.magicBook5:
            case Item_Type.magicBook6:
            case Item_Type.magicBook7:
            case Item_Type.magicBook8:
            case Item_Type.magicBook9:
            case Item_Type.magicBook10:
            case Item_Type.magicBook11:
                {
                    string key = type.ToString();
                    ServerData.magicBookTable.TableDatas[key].hasItem.Value = 1;
                    ServerData.magicBookTable.TableDatas[key].amount.Value += amount;

                    param.Add(key, ServerData.magicBookTable.TableDatas[key].ConvertToString());

                    return TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, param);
                }
                break;
        }

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 트랜젝션 타입 {type}", null);

        return null;
    }

    //addValue에는 반드시 goods나 status는 들어가면 안됨. 업적같은것만.
    public static void SendTransaction(List<RewardData> rewardList, TransactionValue addValue = null, Action successCallBack = null, Action completeCallBack = null)
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        if (addValue != null)
        {
            transactionList.Add(addValue);
        }

        Param goodsParam = new Param();
        Param statusParam = new Param();

        for (int i = 0; i < rewardList.Count; i++)
        {
            switch ((Item_Type)rewardList[i].itemType)
            {
                case Item_Type.Gold:
                    if (goodsParam.ContainsKey(GoodsTable.Gold) == false)
                        goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
                    break;
                case Item_Type.Jade:
                    if (goodsParam.ContainsKey(GoodsTable.Jade) == false)
                        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                    break;
                case Item_Type.GrowthStone:
                    if (goodsParam.ContainsKey(GoodsTable.GrowthStone) == false)
                        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                    break;
                case Item_Type.Memory:
                    if (statusParam.ContainsKey(StatusTable.Memory) == false)
                        statusParam.Add(StatusTable.Memory, ServerData.statusTable.GetTableData(StatusTable.Memory).Value);
                    break;
                case Item_Type.Ticket:
                    if (goodsParam.ContainsKey(GoodsTable.Ticket) == false)
                        goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                    break;
            }
        }

        if (goodsParam.Count != 0)
        {
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }
        if (statusParam.Count != 0)
        {
            transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        }

        SendTransaction(transactionList, completeCallBack: completeCallBack, successCallBack: successCallBack);
    }

    public static void GetPostItem(Item_Type type, float amount)
    {
        if (type.IsRankFrameItem())
        {
            switch (type)
            {
                case Item_Type.RankFrame1:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_1;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 8;
                    break;
                case Item_Type.RankFrame2:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_2;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 7;
                    break;
                case Item_Type.RankFrame3:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_3;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 6;
                    break;
                case Item_Type.RankFrame4:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_4;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 5;
                    break;
                case Item_Type.RankFrame5:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_5;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 4;
                    break;
                case Item_Type.RankFrame6_20:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_6_20;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 3;
                    break;
                case Item_Type.RankFrame21_100:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_21_100;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 2;
                    break;
                case Item_Type.RankFrame101_1000:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += GameBalance.rankRewardTicket_101_1000;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 1;
                    break;
            }

            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.chatFrame, ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

            transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            SendTransaction(transactionList, successCallBack: () =>
              {
                  LogManager.Instance.SendLog("랭킹보상 수령완료", $"{type}");
              });
        }
        else
        {
            switch (type)
            {
                case Item_Type.Gold:
                    ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value += amount;
                    break;
                case Item_Type.Jade:
                    ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                    break;
                case Item_Type.GrowthStone:
                    ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                    break;
                case Item_Type.Ticket:
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                    break;
                case Item_Type.Marble:
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                    break;
                case Item_Type.Dokebi:
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value += amount;
                    break;
                case Item_Type.WeaponUpgradeStone:
                    ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).Value += amount;
                    break;
            }

            List<TransactionValue> transactionList = new List<TransactionValue>();

            var tramsaction = GetItemTypeTransactionValue(type);
            transactionList.Add(tramsaction);

            SendTransaction(transactionList);
        }

    }
}