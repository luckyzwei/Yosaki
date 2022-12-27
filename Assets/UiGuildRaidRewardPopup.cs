﻿using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiTwelveRewardPopup;

public class UiGuildRaidRewardPopup : SingletonMono<UiGuildRaidRewardPopup>
{
    [SerializeField]
    private GameObject rootObject;

    private TwelveBossTableData bossTableData;

    [SerializeField]
    private UiTwelveBossRewardView uiTwelveBossRewardView;

    private List<UiTwelveBossRewardView> uiTwelveBossRewardViews = new List<UiTwelveBossRewardView>();

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI damText;

    private void OnEnable()
    {
        Initialize(73);
    }


    public void Initialize(int bossId)
    {
        var bossTableData = TableManager.Instance.TwelveBossTable.dataArray[bossId];

        var bossServerData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        double currentDamage = 0f;

        if (string.IsNullOrEmpty(bossServerData.score.Value) == false)
        {
            currentDamage = double.Parse(bossServerData.score.Value);
        }

        damText.SetText($"최고 피해량 : {Utils.ConvertBigNum(currentDamage)}");

        rootObject.SetActive(true);

        bossTableData = TableManager.Instance.TwelveBossTable.dataArray[bossId];

        int makeCellAmount = bossTableData.Rewardcut.Length - uiTwelveBossRewardViews.Count;

        for (int i = 0; i < makeCellAmount; i++)
        {
            var cell = Instantiate<UiTwelveBossRewardView>(uiTwelveBossRewardView, cellParent);

            uiTwelveBossRewardViews.Add(cell);
        }

        for (int i = 0; i < uiTwelveBossRewardViews.Count; i++)
        {
            if (i < bossTableData.Rewardcut.Length)
            {
                uiTwelveBossRewardViews[i].gameObject.SetActive(true);

                TwelveBossRewardInfo info = new TwelveBossRewardInfo(i, bossTableData.Rewardcut[i], bossTableData.Rewardtype[i], bossTableData.Rewardvalue[i], bossTableData.Cutstring[i], currentDamage);

                uiTwelveBossRewardViews[i].Initialize(info, bossServerData);
            }
            else
            {
                uiTwelveBossRewardViews[i].gameObject.SetActive(false);
            }

        }
    }

    public void OnClickAllReceiveButton()
    {
        if (double.TryParse(ServerData.bossServerTable.TableDatas["b73"].score.Value, out double score) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("점수를 등록해주세요!");
            return;
        }

        var tableData = TableManager.Instance.BossTable.dataArray[12];

        var sangunRewardedIdxList = ServerData.bossServerTable.GetGuildRaidBossRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;


        for (int i = 0; i < uiTwelveBossRewardViews.Count; i++)
        {
            if (score < uiTwelveBossRewardViews[i].RewardInfo.damageCut)
            {
                break;
            }
            else
            {
                if (sangunRewardedIdxList.Contains(uiTwelveBossRewardViews[i].RewardInfo.idx) == false)
                {

                    float amount = uiTwelveBossRewardViews[i].RewardInfo.rewardAmount;

                    addStringValue += $"{BossServerTable.rewardSplit}{uiTwelveBossRewardViews[i].RewardInfo.idx}";

                    ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += (int)amount;

                    rewardCount++;
                }
            }
        }

        if (rewardCount != 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
            ServerData.bossServerTable.TableDatas["b73"].rewardedId.Value += addStringValue;

            Param bossParam = new Param();
            bossParam.Add("b73", ServerData.bossServerTable.TableDatas["b73"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.GuildReward, ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을수 있는 보상이 없습니다.");
        }
        //
        //int rewardCount = 0;

        //for (int i = 0; i < uiTwelveBossRewardViews.Count; i++)
        //{
        //    bool hasReward = uiTwelveBossRewardViews[i].GetRewardByScript();

        //    if (hasReward)
        //    {
        //        rewardCount++;
        //    }
        //}

        //if (rewardCount != 0)
        //{
        //    List<TransactionValue> transactions = new List<TransactionValue>();

        //    Param bossParam = new Param();
        //    bossParam.Add("b73", ServerData.bossServerTable.TableDatas["b73"].ConvertToString());
        //    transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

        //    Param goodsParam = new Param();
        //    goodsParam.Add(GoodsTable.GuildReward, ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value);
        //    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //    ServerData.SendTransaction(transactions, successCallBack: () =>
        //    {
        //        PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
        //        SoundManager.Instance.PlaySound("Reward");
        //    });
        //}
        //else
        //{
        //    PopupManager.Instance.ShowAlarmMessage("받을수 있는 보상이 없습니다.");
        //}
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.LastLogin].Value = 0;
        }
    }
#endif
}
