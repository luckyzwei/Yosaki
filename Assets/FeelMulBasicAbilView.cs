using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;

public class FeelMulBasicAbilView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI basicAbilDescription;
    [SerializeField]
    private TextMeshProUGUI levelDescription;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.FeelMul).AsObservable().Subscribe(e =>
        {

            levelDescription.SetText($"LV : {e}");

            basicAbilDescription.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical2DamPer)} {PlayerStats.GetFeelMulAddDam() * 100f}");

        }).AddTo(this);
    }

    public void OnClickUpgradeButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.FeelMulStone)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value--;
        ServerData.statusTable.GetTableData(StatusTable.FeelMul).Value++;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.FeelMulStone, ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.FeelMul, ServerData.statusTable.GetTableData(StatusTable.FeelMul).Value);

        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions);

        PopupManager.Instance.ShowAlarmMessage("강화 성공!");
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.statusTable.GetTableData(StatusTable.FeelMul).Value++;
        }
    }
#endif
}
