using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class UiSkillPointResetButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    private void Start()
    {
        SetPriceText();
    }

    private void SetPriceText()
    {
        priceText.SetText(Utils.ConvertBigNum(GameBalance.SkillPointResetPrice));
    }

    public void OnClickResetSkillPoint()
    {
        if (DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value < GameBalance.SkillPointResetPrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Jade)} {Utils.ConvertBigNum(GameBalance.SkillPointResetPrice)}개를 사용해서\n스킬 포인트를 초기화 하시겠습니까?", () =>
          {
              PurchaseProcess();
          }, null);

    }

    private void PurchaseProcess()
    {
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            DatabaseManager.passiveServerTable.TableDatas[tableData[i].Stringid].level.Value = 0;
        }
        //

        var skillLevelList = DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillLevel];

        for (int i = 0; i < skillLevelList.Count; i++)
        {
            //로컬 데이터 초기화
            skillLevelList[i].Value = 0;
        }

        var skillSlotList = DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillSlotIdx];

        for (int i = 0; i < skillSlotList.Count; i++)
        {
            skillSlotList[i].Value = -1;
        }

        DatabaseManager.skillServerTable.UpdateSelectedSkillIdx(DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillSlotIdx].Select(e => e.Value).ToList());

        int currentLevel = DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value;

        //리셋한 데이터 적용
        DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).Value = currentLevel;

        //비용 차감
        DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.SkillPointResetPrice;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param skillParam = new Param();
        skillParam.Add(SkillServerTable.SkillLevel, DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillLevel].Select(e => e.Value).ToList());
        skillParam.Add(SkillServerTable.SkillSlotIdx, DatabaseManager.skillServerTable.TableDatas[SkillServerTable.SkillSlotIdx].Select(e => e.Value).ToList());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.SkillPoint, DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        Param passiveSkillParam = new Param();
        var passiveTableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < passiveTableData.Length; i++)
        {
            passiveSkillParam.Add(passiveTableData[i].Stringid, DatabaseManager.passiveServerTable.TableDatas[passiveTableData[i].Stringid].ConvertToString());
        }

        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(PassiveServerTable.tableName, PassiveServerTable.Indate, passiveSkillParam));

        DatabaseManager.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "스킬포인트 초기화 성공!", null);
        });
    }
}
