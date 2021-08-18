﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiTwelveRewardPopup : SingletonMono<UiTwelveRewardPopup>
{
    public class TwelveBossRewardInfo
    {
        public TwelveBossRewardInfo(int idx, float damageCut, int rewardType, float rewardAmount, string rewardCutString, float currentDamage)
        {
            this.idx = idx;

            this.damageCut = damageCut;

            this.rewardType = rewardType;

            this.rewardAmount = rewardAmount;

            this.rewardCutString = rewardCutString;

            this.currentDamage = currentDamage;
        }

        public int idx;
        public float damageCut;
        public int rewardType;
        public float rewardAmount;
        public float currentDamage;
        public string rewardCutString;
    }

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



    public void Initialize(int bossId)
    {
        var bossTableData = TableManager.Instance.TwelveBossTable.dataArray[bossId];

        var bossServerData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        float currentDamage = 0f;

        if (string.IsNullOrEmpty(bossServerData.score.Value) == false)
        {
            currentDamage = float.Parse(bossServerData.score.Value);
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
}
