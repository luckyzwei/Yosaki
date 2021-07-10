using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;

public class UiMarbleIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI marbleName;

    [SerializeField]
    private TextMeshProUGUI marbleDesc;

    private MarbleTableData currentTableData;

    [SerializeField]
    private Transform selecIcon;

    [SerializeField]
    private List<Transform> marbleCircles;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private Button unlockButton;

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    private void Start()
    {
        OnClickMarbleView(0);
    }

    public void OnClickMarbleView(int id)
    {
        currentTableData = TableManager.Instance.MarbleTable.dataArray[id];
        RefreshMarbleUi();
        Subscribe();
    }

    private void Subscribe()
    {
        disposable.Clear();

        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(e =>
        {

        }).AddTo(disposable);

        DatabaseManager.marbleServerTable.TableDatas[currentTableData.Stringid].hasItem.AsObservable().Subscribe(e =>
        {
            unlockButton.gameObject.SetActive(e == 0);
        }).AddTo(disposable);
    }

    private void RefreshMarbleUi()
    {
        selecIcon.transform.position = marbleCircles[currentTableData.Id].transform.position;

        marbleName.SetText(currentTableData.Name);

        priceText.SetText(Utils.ConvertBigNum(currentTableData.Unlockprice));

        bool marbleAwaked = DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value == 1f;

        string abilDesc = null;
        //능력치 한개짜리
        if (currentTableData.Abilitytype.Length == 1)
        {
            StatusType statusType = (StatusType)currentTableData.Abilitytype[0];

            float statusValue = currentTableData.Abilityvalue[0] * (marbleAwaked ? currentTableData.Awakevalue : 1f);

            if (statusType.IsPercentStat())
            {
                abilDesc += $"{CommonString.GetStatusName(statusType)} {statusValue * 100f}";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(statusType)} {statusValue}";
            }
        }
        else
        {
            for (int i = 0; i < currentTableData.Abilitytype.Length; i++)
            {
                StatusType statusType = (StatusType)currentTableData.Abilitytype[i];

                float statusValue = currentTableData.Abilityvalue[i] * (marbleAwaked ? currentTableData.Awakevalue : 1f);

                if (statusType.IsPercentStat())
                {
                    abilDesc += $"{CommonString.GetStatusName(statusType)} {statusValue * 100f}";
                }
                else
                {
                    abilDesc += $"{CommonString.GetStatusName(statusType)} {statusValue}";
                }

                if (i != currentTableData.Abilitytype.Length - 1)
                {
                    abilDesc += "\n";
                }
            }
        }

        marbleDesc.SetText(abilDesc);
    }

    public void OnClickAwakeButton()
    {
        //전부 열려있는지 체크
        if (DatabaseManager.marbleServerTable.AllMarblesUnlocked() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"모든 구슬이 해방되어야 합니다.");
            return;
        }
    }

    public void OnClickUpgradeButton()
    {

        var serverData = DatabaseManager.marbleServerTable.TableDatas[currentTableData.Stringid];

        if (serverData.hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 봉인 해제 됐습니다.");
            return;
        }

        if (DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value < currentTableData.Unlockprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactionList = new List<TransactionValue>();

        DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= currentTableData.Unlockprice;
        DatabaseManager.marbleServerTable.TableDatas[currentTableData.Stringid].hasItem.Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param marbleParam = new Param();
        marbleParam.Add(currentTableData.Stringid, DatabaseManager.marbleServerTable.TableDatas[currentTableData.Stringid].ConvertToString());
        transactionList.Add(TransactionValue.SetUpdate(MarbleServerTable.tableName, MarbleServerTable.Indate, marbleParam));

        DatabaseManager.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage($"{currentTableData.Name} 획득!!");
        });
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += 100000;
        }
    }

#endif
}
