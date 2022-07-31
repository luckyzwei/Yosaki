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

    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite equiped;

    [SerializeField]
    private Sprite unEquiped;

    [SerializeField]
    private TextMeshProUGUI equipText;

    [SerializeField]
    private GameObject equipButton;

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

        SetTopParent();
    }

    private string GetAbilDescription()
    {
        bool isEquiped = ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].Value == tableData.Id;

        StatusType type = (StatusType)tableData.Abiltype1;

        string abilValue = string.Empty;

        if (type.IsPercentStat())
        {
            float abil = isEquiped ? (tableData.Abilvalue1 * 100) * GameBalance.TitleEquipAddPer : (tableData.Abilvalue1 * 100);

            abilValue = Utils.ConvertBigNum(abil) + "%";
        }
        else
        {
            float abil = isEquiped ? (tableData.Abilvalue1) * GameBalance.TitleEquipAddPer : (tableData.Abilvalue1);

            abilValue = Utils.ConvertBigNum(abil);
        }

        return $"{CommonString.GetStatusName(type)} {abilValue}";
    }

    private void Subscribe()
    {
        serverTable.clearFlag.AsObservable().Subscribe(e =>
        {

            lockMask.SetActive(e == 0);

            equipButton.SetActive(e == 1);

        }).AddTo(this);

        serverTable.rewarded.AsObservable().Subscribe(e =>
        {

            completeIcon.SetActive(e == 1);

        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].AsObservable().Subscribe(e =>
        {
            image.sprite = e == tableData.Id ? equiped : unEquiped;

            equipText.SetText(e == tableData.Id ? "장착중" : "장착");

            abilDescription.color = e == tableData.Id ? Color.green : Color.red;

            abilDescription.SetText(GetAbilDescription());

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

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance((Item_Type)tableData.Rewardtype, tableData.Rewardvalue));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상 수령 완료!", null);

              //     LogManager.Instance.SendLogType("TitleReward", tableData.Id.ToString(), "");
          });
    }

    public void OnClickSelectButton()
    {
        if (ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].Value == tableData.Id)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 장착 중입니다!");
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.TitleSelectId, tableData.Id);

        PlayerStats.ResetAbilDic();
    }

    private void OnEnable()
    {
        SetTopParent();
    }

    private void SetTopParent()
    {
        if (tableData == null) return;

        if (tableData.Displaygroup != 0 && tableData.Displaygroup != 1 && tableData.Displaygroup != 2) return;

        if (serverTable.clearFlag.Value == 1)
        {
            this.transform.SetAsFirstSibling();
        }

    }
}
