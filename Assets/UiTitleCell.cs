using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiTitleCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private GameObject completeIcon;

    private TitleTableData tableData;
    private TitleServerData serverTable;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    public void Initialize(TitleTableData tableData)
    {
        this.tableData = tableData;

        serverTable = ServerData.titleServerTable.TableDatas[this.tableData.Stringid];

        title.SetText(tableData.Title);

        title.color = CommonUiContainer.Instance.itemGradeColor[tableData.Grade];

        description.SetText(tableData.Description);

        abilDescription.SetText(GetAbilDescription());

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Rewardtype);

        rewardAmount.SetText(Utils.ConvertBigNum(tableData.Rewardvalue));

        Subscribe();
    }

    private string GetAbilDescription()
    {
        StatusType type = (StatusType)tableData.Abiltype1;

        string abilValue = string.Empty;

        if (type.IsPercentStat()) 
        {
            abilValue = (tableData.Abilvalue1*100).ToString()+"%";
        }
        else 
        {
            abilValue = tableData.Abilvalue1.ToString();
        }

        return $"{CommonString.GetStatusName(type)} {abilValue}";
    }

    private void Subscribe()
    {
        serverTable.clearFlag.AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(e == 0);
        }).AddTo(this);

        serverTable.rewarded.AsObservable().Subscribe(e =>
        {
            completeIcon.SetActive(e == 1);
        }).AddTo(this);
    }

    public void OnClickRewardButton()
    {
        if (serverTable.clearFlag.Value != 1)
        {
            PopupManager.Instance.ShowAlarmMessage("조건이 안됩니다.");
            return;
        }

        if (serverTable.rewarded.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 받았습니다.");
            return;
        }
        serverTable.rewarded.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param titleParam = new Param();
        titleParam.Add(tableData.Stringid, serverTable.ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(TitleServerTable.tableName, TitleServerTable.Indate, titleParam));

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance((Item_Type)tableData.Rewardtype, (int)tableData.Rewardvalue));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상 수령 완료!", null);

              LogManager.Instance.SendLogType("TitleReward", tableData.Id.ToString(), "");
          });
    }
}
