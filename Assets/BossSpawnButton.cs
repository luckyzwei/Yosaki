using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using BackEnd;
using TMPro;

public class BossSpawnButton : SingletonMono<BossSpawnButton>
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private Image bossGauge;

    [SerializeField]
    private TextMeshProUGUI bossText;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        string stageKey = GameManager.Instance.CurrentStageData.Stagestringkey;

        DatabaseManager.fieldBossTable.TableDatas[stageKey].killCount.AsObservable().Subscribe(WhenStageKillCountChanged).AddTo(this);
    }

    private void WhenStageKillCountChanged(int killCount)
    {
        bossText.SetText($"{killCount / GameBalance.fieldBossSpawnRequire}회 소환 가능");

        bossGauge.fillAmount = (((float)killCount % (float)GameBalance.fieldBossSpawnRequire) / (float)GameBalance.fieldBossSpawnRequire);
    }

    public void OnClickSpawnButton()
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

        if (CanSpawnBossEnemy() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("몹 처치가 부족합니다.");
            return;
        }

        //확인팝업
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "필드 보스를 소환합니까?\n(처치 제한시간 10초)", () =>
         {
             List<TransactionValue> transactionList = new List<TransactionValue>();

            //로컬 카운트
            string stageKey = GameManager.Instance.CurrentStageData.Stagestringkey;
             DatabaseManager.fieldBossTable.TableDatas[stageKey].killCount.Value -= GameBalance.fieldBossSpawnRequire;

             Param param = new Param();
             param.Add(stageKey, DatabaseManager.fieldBossTable.TableDatas[stageKey].ConvertToString());

             transactionList.Add(TransactionValue.SetUpdate(FieldBossServerTable.tableName, FieldBossServerTable.Indate, param));

             DatabaseManager.SendTransaction(transactionList, successCallBack: () =>
             {
                 MapInfo.Instance.SpawnBossEnemy();
             });
         }, null);
    }

    private bool CanSpawnBossEnemy()
    {
        if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField) return false;

        string stageKey = GameManager.Instance.CurrentStageData.Stagestringkey;

        return DatabaseManager.fieldBossTable.TableDatas[stageKey].killCount.Value >= GameBalance.fieldBossSpawnRequire;
    }
}
