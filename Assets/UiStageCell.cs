﻿using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiStageCell : MonoBehaviour
{
    private StageMapData stageMapData;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI stageName;

    private CompositeDisposable compositDisposable = new CompositeDisposable();

    //패스 보상
    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    [SerializeField]
    private GameObject rewardlockMask;

    [SerializeField]
    private GameObject rewardCompleteObject;

    //광고보상
    [SerializeField]
    private Image rewardIcon_ad;


    [SerializeField]
    private TextMeshProUGUI rewardAmount_ad;

    [SerializeField]
    private GameObject rewardCompleteObject_Ad;


    private bool HasStagePassItem()
    {
        return DatabaseManager.iapServerTable.TableDatas["stagepass"].buyCount.Value > 0;
    }

    public void OnClickRewardButton()
    {
        int lastClearData = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (lastClearData < stageMapData.Id)
        {
            PopupManager.Instance.ShowAlarmMessage("스테이지를 클리어 해야 합니다.");
            return;
        }

        if (HasStagePassItem() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("스테이지 패스 아이템이 필요합니다.");
            return;
        }

        if (DatabaseManager.passServerTable.HasReward(PassServerTable.stagePassReward, stageMapData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        //로컬
        DatabaseManager.AddLocalValue((Item_Type)(int)stageMapData.Pre_Bossrewardtype, stageMapData.Pre_Bossrewardvalue);
        DatabaseManager.passServerTable.TableDatas[PassServerTable.stagePassReward].Value += $",{stageMapData.Id}";

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param passParam = new Param();
        passParam.Add(PassServerTable.stagePassReward, DatabaseManager.passServerTable.TableDatas[PassServerTable.stagePassReward].Value);
        transactions.Add(TransactionValue.SetUpdate(PassServerTable.tableName, PassServerTable.Indate, passParam));

        var rewardTransactionValue = DatabaseManager.GetItemTypeTransactionValue((Item_Type)(int)stageMapData.Pre_Bossrewardtype);
        transactions.Add(rewardTransactionValue);

        DatabaseManager.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("보상 획득!");
          });
    }

    public void OnClickRewardButton_Ad()
    {
        int lastClearData = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (lastClearData < stageMapData.Id)
        {
            PopupManager.Instance.ShowAlarmMessage("스테이지를 클리어 해야 합니다.");
            return;
        }

        if (DatabaseManager.passServerTable.HasReward(PassServerTable.stagePassAdReward, stageMapData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            //로컬
            DatabaseManager.AddLocalValue((Item_Type)(int)stageMapData.Ad_Bossrewardtype, stageMapData.Ad_Bossrewardvalue);
            DatabaseManager.passServerTable.TableDatas[PassServerTable.stagePassAdReward].Value += $",{stageMapData.Id}";

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param passParam = new Param();
            passParam.Add(PassServerTable.stagePassAdReward, DatabaseManager.passServerTable.TableDatas[PassServerTable.stagePassAdReward].Value);
            transactions.Add(TransactionValue.SetUpdate(PassServerTable.tableName, PassServerTable.Indate, passParam));

            var rewardTransactionValue = DatabaseManager.GetItemTypeTransactionValue((Item_Type)(int)stageMapData.Ad_Bossrewardtype);
            transactions.Add(rewardTransactionValue);

            DatabaseManager.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상 획득!");
            });
        });
    }

    public void Initialize(StageMapData stageMapData)
    {
        this.stageMapData = stageMapData;

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)stageMapData.Pre_Bossrewardtype);
        rewardAmount.SetText(Utils.ConvertBigNum(stageMapData.Pre_Bossrewardvalue));

        rewardIcon_ad.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)stageMapData.Ad_Bossrewardtype);
        rewardAmount_ad.SetText(Utils.ConvertBigNum(stageMapData.Ad_Bossrewardvalue));

        int lastClearData = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        string sufix = stageMapData.Id <= lastClearData ? "(클리어)" : "";

        stageName.SetText($"{(stageMapData.Id + 1).ToString()}단계{sufix}");

        Subscribe();
    }

    private void OnDestroy()
    {
        compositDisposable.Dispose();
    }

    private void Subscribe()
    {
        compositDisposable.Clear();

        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).AsObservable().Subscribe(topClearStageId =>
        {
            lockMask.SetActive(stageMapData.Id - 1 > topClearStageId);

        }).AddTo(compositDisposable);

        DatabaseManager.iapServerTable.TableDatas["stagepass"].buyCount.AsObservable().Subscribe(e =>
        {
            rewardlockMask.SetActive(e == 0);

        }).AddTo(compositDisposable);

        DatabaseManager.passServerTable.TableDatas[PassServerTable.stagePassReward].AsObservable().Subscribe(e =>
        {
            rewardCompleteObject.SetActive(DatabaseManager.passServerTable.HasReward(PassServerTable.stagePassReward, stageMapData.Id));

        }).AddTo(compositDisposable);

        DatabaseManager.passServerTable.TableDatas[PassServerTable.stagePassAdReward].AsObservable().Subscribe(e =>
        {
            rewardCompleteObject_Ad.SetActive(DatabaseManager.passServerTable.HasReward(PassServerTable.stagePassAdReward, stageMapData.Id));

        }).AddTo(compositDisposable);
    }

    public void OnClickButton()
    {
#if UNITY_EDITOR
        GameManager.Instance.MoveMapByIdx(stageMapData.Id);
        return;
#endif

        int lastClearData = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (GameManager.Instance.CurrentStageData.Id == stageMapData.Id)
        {
            PopupManager.Instance.ShowAlarmMessage("현재 스테이지 입니다.");
            return;
        }

        if (stageMapData.Id - 1 > lastClearData)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 스테이지를 클리어 해야 합니다.");
            return;
        }

        GameManager.Instance.MoveMapByIdx(stageMapData.Id);
    }

}
