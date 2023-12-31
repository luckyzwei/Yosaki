﻿using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGachaResultView;
using UniRx;

public class UiNewGacha : MonoBehaviour
{
    private List<NewGachaTableData> newGachaTableDatas = new List<NewGachaTableData>();
    private List<float> probs = new List<float>();
    private List<GachaResultCellInfo> gachaResultCellInfos = new List<GachaResultCellInfo>();

    [SerializeField]
    private List<ObscuredInt> gachaAmount;

    [SerializeField]
    private List<ObscuredInt> gachaPrice;

    private ObscuredInt lastGachaIdx = 0;

    [SerializeField]
    private List<TextMeshProUGUI> gachaNumTexts;

    [SerializeField]
    private List<TextMeshProUGUI> priceTexts;

    [SerializeField]
    private TextMeshProUGUI freeButtonDesc;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Subscribe(e =>
        {
            freeButtonDesc.SetText(e == 0 ? "무료 뽑기!" : "내일 다시!");
        }).AddTo(this);
    }

    private void Initialize()
    {
        for (int i = 0; i < gachaNumTexts.Count; i++)
        {
            gachaNumTexts[i].SetText($"{gachaAmount[i]}번 소환");
        }

        for (int i = 0; i < priceTexts.Count; i++)
        {
            priceTexts[i].SetText($"{gachaPrice[i]}");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        WaitForSeconds randomizeDelay = new WaitForSeconds(1.0f);

        while (true)
        {
            Randomize();
            yield return randomizeDelay;
        }
    }

    private void Randomize()
    {
        gachaAmount.ForEach(e => e.RandomizeCryptoKey());
        gachaPrice.ForEach(e => e.RandomizeCryptoKey());
        lastGachaIdx.RandomizeCryptoKey();
    }

    private void OnApplicationPause(bool pause)
    {
        Randomize();
    }


    private bool CanGacha(int price)
    {
        int currentBlueStoneNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value;
        return currentBlueStoneNum >= price;
    }

    public void OnClickFreeGacha()
    {
        bool canFreeGacha = ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value == 0;

        if (canFreeGacha == false)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받을 수 없습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value = 1;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.freeNewGacha, ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                this.lastGachaIdx = 2;
                int amount = gachaAmount[2];
                int price = gachaPrice[2];

                //무료라
                ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += price;

                OnClickOpenButton(2);

            });
        });
    }

    public void OnClickOpenButton(int idx)
    {
        this.lastGachaIdx = idx;
        int amount = gachaAmount[idx];
        int price = gachaPrice[idx];

        //재화 체크
        if (CanGacha(price) == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}이 부족합니다.");
            UiGachaResultView.Instance.autoToggle.isOn = false;
            return;
        }

        this.newGachaTableDatas.Clear();
        probs.Clear();
        gachaResultCellInfos.Clear();

        var newGachaData = TableManager.Instance.NewGachaData;

        var e = newGachaData.GetEnumerator();

        int gachaLevel = UiGachaPopup.GachaLevel(UserInfoTable.gachaNum_NewGacha);

        while (e.MoveNext())
        {
            this.newGachaTableDatas.Add(e.Current.Value);

            if (gachaLevel == 0)
            {
                probs.Add(e.Current.Value.Gachalv1);
            }
            else if (gachaLevel == 1)
            {
                probs.Add(e.Current.Value.Gachalv2);
            }
            else if (gachaLevel == 2)
            {
                probs.Add(e.Current.Value.Gachalv3);
            }
            else if (gachaLevel == 3)
            {
                probs.Add(e.Current.Value.Gachalv4);
            }
            else if (gachaLevel == 4)
            {
                probs.Add(e.Current.Value.Gachalv5);
            }
            else if (gachaLevel == 5)
            {
                probs.Add(e.Current.Value.Gachalv6);
            }
            else if (gachaLevel == 6)
            {
                probs.Add(e.Current.Value.Gachalv7);
            }
            else if (gachaLevel == 7)
            {
                probs.Add(e.Current.Value.Gachalv8);
            }
            else if (gachaLevel == 8)
            {
                probs.Add(e.Current.Value.Gachalv9);
            }
            else if (gachaLevel == 9)
            {
                probs.Add(e.Current.Value.Gachalv10);
            }
        }

        List<int> serverUpdateList = new List<int>();

        //로컬 데이터 갱신
        //재화
        ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -= price;

        //가챠갯수
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value += amount;


        //마법책
        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(probs);
            var cellInfo = new GachaResultCellInfo();

            cellInfo.amount = 1;
            cellInfo.newGachaData = this.newGachaTableDatas[randomIdx];
            gachaResultCellInfos.Add(cellInfo);

            ServerData.newGachaServerTable.UpData(this.newGachaTableDatas[randomIdx], cellInfo.amount);
            serverUpdateList.Add(newGachaTableDatas[randomIdx].Id);
        }

        SyncServer(serverUpdateList, price, serverUpdateList.Count);

        UiGachaResultView.Instance.Initialize(gachaResultCellInfos, () =>
        {
            OnClickOpenButton(lastGachaIdx);
        });

        SoundManager.Instance.PlaySound("Reward");
    }
    private void SyncServer(List<int> serverUpdateList, int price, int gachaCount)
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);

        //가챠횟수
        Param gachaNumParam = new Param();
        gachaNumParam.Add(UserInfoTable.gachaNum_NewGacha, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value);

        //마법책
        Param newGachaParam = new Param();
        var table = TableManager.Instance.NewGachaTable.dataArray;
        var tableDatas = ServerData.newGachaServerTable.TableDatas;
        for (int i = 0; i < table.Length; i++)
        {
            if (serverUpdateList != null && serverUpdateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            newGachaParam.Add(key, tableDatas[key].ConvertToString());
        }
        //

        //재화
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        //가챠횟수
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gachaNumParam));
        //마법책
        transactionList.Add(TransactionValue.SetUpdate(NewGachaServerTable.tableName, NewGachaServerTable.Indate, newGachaParam));

        ServerData.SendTransaction(transactionList);
    }
}
