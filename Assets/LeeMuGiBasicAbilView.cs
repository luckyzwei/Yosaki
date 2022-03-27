using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;

public class LeeMuGiBasicAbilView : MonoBehaviour
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
        ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).AsObservable().Subscribe(e =>
        {

            levelDescription.SetText($"LV : {e}");

            basicAbilDescription.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical2DamPer)} {PlayerStats.GetLeeMuGiAddDam() * 100f}");

        }).AddTo(this);
    }

    public void OnClickUpgradeButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.LeeMuGiStone)}가 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value--;
        ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).Value++;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.LeeMuGiStone, ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.LeeMuGi, ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).Value);

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
            ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).Value++;
        }
    }
#endif
}
