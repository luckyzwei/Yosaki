using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDokebiShopCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemTitle;

    [SerializeField]
    private TextMeshProUGUI itemAmount;

    [SerializeField]
    private TextMeshProUGUI itemPrice;

    private DokebiRewardTableData tableData;

    public void Initialize(DokebiRewardTableData tableData)
    {
        this.tableData = tableData;
        UpdateUi();
    }

    private void UpdateUi()
    {
        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemTitle.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));

        itemAmount.SetText($"{Utils.ConvertBigNum(tableData.Rewardamount)}개");

        itemPrice.SetText(Utils.ConvertBigNum(tableData.Price));
    }

    public void OnClickExChangeButton()
    {
        var currentDokebiHornNum = ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey);

        if (currentDokebiHornNum.Value < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Dokebi)}이 부족합니다.");
            return;
        }

        currentDokebiHornNum.Value -= tableData.Price;

        string goodsKey = ((Item_Type)tableData.Itemtype).ToString();

        ServerData.goodsTable.GetTableData(goodsKey).Value += tableData.Rewardamount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.DokebiKey, currentDokebiHornNum.Value);

        goodsParam.Add(goodsKey, ServerData.goodsTable.GetTableData(goodsKey).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("교환 성공");

              LogManager.Instance.SendLog("도깨비", $"{goodsKey} {tableData.Rewardamount}");
          });
    }
}
