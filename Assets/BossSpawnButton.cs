using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using BackEnd;
using TMPro;

public class BossSpawnButton : SingletonMono<BossSpawnButton>
{
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

        //확인팝업
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "스테이지 보스를 소환합니까?\n(처치 제한시간 10초)", () =>
         {
             List<TransactionValue> transactionList = new List<TransactionValue>();

             MapInfo.Instance.SpawnBossEnemy();
         }, null);
    }
}
