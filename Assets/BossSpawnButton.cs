﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using BackEnd;
using TMPro;

public class BossSpawnButton : SingletonMono<BossSpawnButton>
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).AsObservable().Subscribe(e=> 
        {
            int nextStageId = GameManager.Instance.CurrentStageData.Id + 1;

            int lastClearStage = (int)e;

            if (lastClearStage == TableManager.Instance.GetLastStageIdx())
            {
                buttonDescription.SetText("최고 단계");
                return;
            }

            if (nextStageId > lastClearStage + 1)
            {
                buttonDescription.SetText("보스 도전");
                return;
            }

            buttonDescription.SetText("다음 스테이지");

        }).AddTo(this);
    }

    public void OnClickSpawnButton()
    {
        int lastClearStage = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (GameManager.Instance.CurrentStageData.Id == TableManager.Instance.GetLastStageIdx())
        {
            PopupManager.Instance.ShowAlarmMessage("최고 단계 입니다. 다음 업데이트를 기다려주세요!");
            return;
        }

        int nextStageId = GameManager.Instance.CurrentStageData.Id + 1;

        if (nextStageId > lastClearStage + 1)
        {

            if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField)
            {
                PopupManager.Instance.ShowAlarmMessage("필드보스를 소환할 수 없는 곳 입니다.");
                return;
            }

            if (MapInfo.Instance.HasSpawnedBossEnemy())
            {
                PopupManager.Instance.ShowAlarmMessage("이미 필드에 보스가 있습니다!");
                return;
            }

            //확인팝업
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "스테이지 보스를 소환합니까?\n(처치 제한시간 10초)\n(1층에 소환됩니다.)", () =>
            {
                List<TransactionValue> transactionList = new List<TransactionValue>();

                MapInfo.Instance.SpawnBossEnemy();
            }, null);

            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "다음 스테이지로 이동합니까?", () =>
        {
            SoundManager.Instance.PlayButtonSound();
            GameManager.Instance.LoadNextScene();
        }, null);

    }
}
