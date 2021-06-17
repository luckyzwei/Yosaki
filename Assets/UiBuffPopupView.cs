using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System;

public class UiBuffPopupView : MonoBehaviour
{
    [SerializeField]
    private Image buffIcon;

    [SerializeField]
    private TextMeshProUGUI buffDescription;

    [SerializeField]
    private GameObject useButtonObject;

    [SerializeField]
    private TextMeshProUGUI remainSecText;

    private BuffTableData buffTableData;

    [SerializeField]
    private Image buffGetButton;

    [SerializeField]
    private Sprite getEnable;

    [SerializeField]
    private Sprite getDisable;

    private bool initialized = false;

    public void Initalize(BuffTableData buffTableData)
    {
        this.buffTableData = buffTableData;

        TimeSpan ts = TimeSpan.FromSeconds(buffTableData.Buffseconds);
        buffDescription.SetText($"{CommonString.GetStatusName((StatusType)buffTableData.Bufftype)}+{buffTableData.Buffvalue * 100f}%({ts.TotalMinutes}분)");

        buffIcon.sprite = CommonUiContainer.Instance.buffIconList[buffTableData.Id];

        Subscribe();

        initialized = true;
    }

    private void OnEnable()
    {
        if (initialized == false) return;

        WhenRemainSecChanged(DatabaseManager.buffServerTable.TableDatas[buffTableData.Stringid].remainSec.Value);
    }

    private void WhenRemainSecChanged(float remainSec)
    {
        if (this.gameObject.activeInHierarchy == false) return;

        if (remainSec <= 0f)
        {
            remainSecText.SetText("0초");
        }
        else
        {
            TimeSpan ts = TimeSpan.FromSeconds(remainSec);

            if (ts.Hours != 0)
            {
                remainSecText.SetText($"{ts.Hours}시간 {ts.Minutes}분 {ts.Seconds}초");
            }
            else
            {
                remainSecText.SetText($"{ts.Minutes}분 {ts.Seconds}초");
            }
        }
    }

    private void Subscribe()
    {
        DatabaseManager.buffServerTable.TableDatas[buffTableData.Stringid].remainSec.AsObservable().Subscribe(e =>
        {
            WhenRemainSecChanged(e);
        }).AddTo(this);

        DatabaseManager.userInfoTable.GetTableData(buffTableData.Stringid).AsObservable().Subscribe(e =>
        {
            buffGetButton.sprite = e == 0f ? getEnable : getDisable;
        }).AddTo(this);
    }

    public void OnClickGetBuffButton()
    {
        if (DatabaseManager.userInfoTable.GetTableData(buffTableData.Stringid).Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 획득할 수 없습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            BuffGetRoutine();
        });

    }

    private void BuffGetRoutine() 
    {
        DatabaseManager.userInfoTable.GetTableData(buffTableData.Stringid).Value = 1;
        DatabaseManager.buffServerTable.TableDatas[buffTableData.Stringid].remainSec.Value += buffTableData.Buffseconds;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(buffTableData.Stringid, DatabaseManager.userInfoTable.GetTableData(buffTableData.Stringid).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        Param buffParam = new Param();

        buffParam.Add(buffTableData.Stringid, DatabaseManager.buffServerTable.TableDatas[buffTableData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(BuffServerTable.tableName, BuffServerTable.Indate, buffParam));

        DatabaseManager.SendTransaction(transactions);
    }
}
