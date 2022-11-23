using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRewardCollection : MonoBehaviour
{
    [SerializeField]
    private Button oniButton;

    [SerializeField]
    private Button baekGuiButton;

    [SerializeField]
    private Button sonButton;

    [SerializeField]
    private Button gangChulButton;

    [SerializeField]
    private Button gumGiButton;

    [SerializeField]
    private Button hellFireButton;

    [SerializeField]
    private Button chunFlowerButton;

    [SerializeField]
    private Button hellRelicButton;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
        {
            oniButton.interactable = e >= 300;
            baekGuiButton.interactable = e >= 3000;
            sonButton.interactable = e >= 5000;
            gangChulButton.interactable = e >= 30000;
            gumGiButton.interactable = e >= 50000;
            hellFireButton.interactable = e >= 50000;
            chunFlowerButton.interactable = e >= 200000;
            hellRelicButton.interactable = e >= 50000;

        }).AddTo(this);
    }

    public void OnClickBanditReward()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >= GameBalance.bonusDungeonEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            return;
        }

        int killCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonMaxKillCount).Value;

        if (killCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기록이 없습니다.");
            return;
        }

        int clearCount = GameBalance.bonusDungeonEnterCount - (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value;

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"처치 <color=yellow>{killCount}</color>로 <color=yellow>{clearCount}회</color> 소탕 합니까?\n{CommonString.GetItemName(Item_Type.Jade)} {killCount * GameBalance.bonusDungeonGemPerEnemy * (GameBalance.bandiPlusStageJadeValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue) * clearCount}개\n{CommonString.GetItemName(Item_Type.Marble)} {killCount * GameBalance.bonusDungeonMarblePerEnemy * (GameBalance.bandiPlusStageMarbleValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue) * clearCount}개", () =>
        {
            // enterButton.interactable = false;


            int rewardNumJade = (killCount * GameBalance.bonusDungeonGemPerEnemy) * (GameBalance.bandiPlusStageJadeValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue) * clearCount;
            int rewardNumMarble = killCount * GameBalance.bonusDungeonMarblePerEnemy * (GameBalance.bandiPlusStageMarbleValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue) * clearCount;
            ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value += clearCount;

            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardNumJade;
            ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += rewardNumMarble;

            //데이터 싱크
            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
            transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactionList,
                successCallBack: () =>
                {
                    DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);
                },
                completeCallBack: () =>
                {
                    // enterButton.interactable = true;
                });

            EventMissionManager.UpdateEventMissionClear(EventMissionKey.ClearBandit, clearCount);

            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{clearCount}회 소탕 완료!\n{CommonString.GetItemName(Item_Type.Jade)} {rewardNumJade}개\n{CommonString.GetItemName(Item_Type.Marble)} {rewardNumMarble}개 획득!", null);
            SoundManager.Instance.PlaySound("GoldUse");


        }, null);

    }//★

    public void OnClickOniReward()
    {
        //난이도 고정
        int idx = 3;

        int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value;

        if (currentEnterCount >= GameBalance.dokebiEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            return;
        }

        int defeatCount = 0;

        int clearCount = GameBalance.dokebiEnterCount - currentEnterCount;

        if (idx == 0)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount0).Value;
        }
        else if (idx == 1)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount1).Value;
        }
        else if (idx == 2)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount2).Value;
        }
        else if (idx == 3)
        {
            defeatCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount3).Value;
        }

        if (defeatCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("플레이 데이터가 없습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Dokebi)} <color=yellow>{defeatCount}</color>개로 <color=yellow>{clearCount}회</color> 소탕 합니까?", () =>
        {
            int rewardNum = defeatCount;

            ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value += rewardNum * clearCount;

            ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value += clearCount;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.DokebiKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value);

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.ClearOni, clearCount);

                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Dokebi)} {rewardNum * clearCount}개 획득!");

                //사운드
                SoundManager.Instance.PlaySound("Reward");
                //LogManager.Instance.SendLog("DokClear", $"{rewardNum}개 획득 {ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value}");
            });
        }, null);
    }//★

    public void OnClickBaekguiReward()
    {

    }//★

    public void OnClickSonReward()
    {

    }

    public void OnClickGumgiReward()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SP)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLogType("GumGi", "_", score.ToString());
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SP)} {score}개 획득!", null);
            });
        }, null);
    }

    public void OnClickHellFireReward()
    {

    }

    public void OnClickChunFlowerReward()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Cw)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getFlower, ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Cw)} {score}개 획득!", null);
            });
        }, null);
    }

    public void OnClickSmithReward()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SmithFire)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getSmith, ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLogType("Smith", "_", score.ToString());
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SmithFire)} {score}개 획득!", null);
            });
        }, null);
    }

    public void OnClickGuildGumihoReward()
    {

    }

    public void OnClickTrainingReward()
    {
        RewardPopupManager.Instance.OnclickButton();
    }
    public void OnClickDokebiReward()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFire)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?", () =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getDokebiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score}개 획득!", null);
            });
        }, null);
    }
}
