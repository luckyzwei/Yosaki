using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRelicCell : MonoBehaviour
{
    [SerializeField]
    private Image relicIcon;

    [SerializeField]
    private TextMeshProUGUI relicName;

    [SerializeField]
    private TextMeshProUGUI relicDescription;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private RelicTableData relicLocalData;

    private RelicServerData relicServerData;

    private bool subscribed = false;
    private bool IsMaxLevel()
    {
        return relicServerData.level.Value >= relicLocalData.Maxlevel;
    }

    private void Subscribe()
    {
        relicServerData.level.AsObservable().Subscribe(level =>
        {
            if (IsMaxLevel() == false)
            {
                levelText.SetText($"LV:{level.ToString()}");
            }
            else
            {
                levelText.SetText($"MAX({level.ToString()})");
            }

        }).AddTo(this);
    }

    private void Initialize(RelicTableData relicLocalData)
    {
        this.relicLocalData = relicLocalData;

        this.relicServerData = ServerData.relicServerTable.TableDatas[this.relicLocalData.Stringid];

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }
    public void OnClickLevelupButton()
    {
        if (IsMaxLevel()) 
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum <=0) 
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }


    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    private IEnumerator SyncRoutine() 
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();

        Param RelicParam = new Param();

        ServerData.SendTransaction(transactions);

    }
}
