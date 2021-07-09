﻿using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiContentsEnterPopup : SingletonMono<UiContentsEnterPopup>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI ticketPrice;

    [SerializeField]
    private GameObject bonusDungeonObject;

    [SerializeField]
    private GameObject infinityTowerObject;

    private ContentsType contentsType;

    public ObscuredInt bossId { get; private set; }

    [SerializeField]
    private Button enterButton;

    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        var delay = new WaitForSeconds(1.0f);

        while (true)
        {
            RandomizeKey();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        RandomizeKey();
    }

    private void RandomizeKey()
    {
        bossId.RandomizeCryptoKey();
    }
    public void Initialize(ContentsType contentsType, int bossId)
    {
        this.transform.SetAsLastSibling();

        this.bossId = bossId;

        rootObject.SetActive(true);

        this.contentsType = contentsType;

        UpdateUi();

        ticketPrice.SetText($"X{GameBalance.contentsEnterprice}");

        bonusDungeonObject.SetActive(contentsType == ContentsType.FireFly);

        infinityTowerObject.SetActive(contentsType == ContentsType.InfiniteTower);
    }

    private void UpdateUi()
    {
        title.SetText($"{CommonString.GetContentsName(contentsType)}");
    }
    public void ClosePopup()
    {
        rootObject.SetActive(false);
    }
    public void OnClickEnterButton()
    {
        switch (contentsType)
        {
            case ContentsType.FireFly:
                {
                    BonusDefenseEnterRoutine();
                }
                break;
            case ContentsType.InfiniteTower:
                {
                    InfiniteTowerEnterRoutine();
                }
                break;
        }
    }
    private void BonusDefenseEnterRoutine()
    {
        int currentBlueStone = (int)DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value;

        if (currentBlueStone < GameBalance.contentsEnterprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}가 부족합니다.");
            return;
        }

        int currentEnterCount = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value;
        if (currentEnterCount >= GameBalance.bonusDungeonEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage($"오늘은 더이상 입장할 수 없습니다.");
            return;
        }

        enterButton.interactable = false;

        DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.contentsEnterprice;
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value++;

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        DatabaseManager.SendTransaction(transactionList,
            successCallBack: () =>
            {
                GameManager.Instance.LoadContents(contentsType);
            },
            completeCallBack: () =>
            {
                enterButton.interactable = true;
            });
    }

    private void InfiniteTowerEnterRoutine()
    {
        int currentBlueStone = (int)DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value;

        if (currentBlueStone < GameBalance.contentsEnterprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}가 부족합니다.");
            return;
        }

        enterButton.interactable = false;

        DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.contentsEnterprice;

        DatabaseManager.goodsTable.SyncToServerEach(GoodsTable.Jade, () =>
        {
            GameManager.Instance.LoadContents(contentsType);
        },
       () =>
       {
           enterButton.interactable = true;
       },
       //실패
       () =>
       {
           DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value += GameBalance.contentsEnterprice;
           enterButton.interactable = true;
           PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "서버가 불안정 합니다. 잠시후 다시 시도해 주세요.", null);
       });
    }

    public void BonusDungeonInstantClear() 
    {
        if (DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >= GameBalance.bonusDungeonEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            return;
        }

        int killCount = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonMaxKillCount).Value;

        if (killCount==0)
        {
            PopupManager.Instance.ShowAlarmMessage("기록이 없습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"처치 {killCount}로 소탕 합니까?\n보상 : {killCount * GameBalance.bonusDungeonGemPerEnemy}개", () => 
        {
            enterButton.interactable = false;

            int rewardNum = killCount * GameBalance.bonusDungeonGemPerEnemy;
            DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value++;
            DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value += killCount * GameBalance.bonusDungeonGemPerEnemy;

            //데이터 싱크
            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
            transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            DatabaseManager.SendTransaction(transactionList,
                successCallBack: () =>
                {

                },
                completeCallBack: () =>
                {
                    enterButton.interactable = true;
                });

            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)} {rewardNum}개 획득!");
            SoundManager.Instance.PlaySound("GoldUse");
        }, null);
    }
}
