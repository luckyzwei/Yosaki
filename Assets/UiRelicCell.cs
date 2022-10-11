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

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockText;


    [SerializeField]
    private GameObject lockMask_San;

    private bool subscribed = false;
    private bool IsMaxLevel()
    {
        return relicServerData.level.Value >= relicLocalData.Maxlevel;
    }

    private void Subscribe()
    {
        relicServerData.level.AsObservable().Subscribe(level =>
        {
            levelText.SetText($"LV:{level.ToString()}");

            if (IsMaxLevel() == false)
            {
                priceText.SetText(Utils.ConvertBigNum(relicLocalData.Upgradeprice));
            }
            else
            {
                priceText.SetText("MAX");
            }

            StatusType abilType = (StatusType)relicLocalData.Abiltype;

            if (abilType.IsPercentStat())
            {
                var abilValue = PlayerStats.GetRelicHasEffect(abilType);

                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertBigNum(abilValue * 100f)}%");

            }
            else
            {
                var abilValue = PlayerStats.GetRelicHasEffect(abilType);

                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertBigNum(abilValue)}");
            }

        }).AddTo(this);

        lockMask.SetActive(false);

        if (relicLocalData.Requirerelic != -1)
        {
            var requireServerData = ServerData.relicServerTable.TableDatas[TableManager.Instance.RelicTable.dataArray[relicLocalData.Requirerelic].Stringid];

            lockText.color = CommonUiContainer.Instance.itemGradeColor[TableManager.Instance.RelicTable.dataArray[relicLocalData.Requirerelic].Grade + 1];

            requireServerData.level.AsObservable().Subscribe(requireLevel =>
            {
                //장산범 아닐때
                if (relicLocalData.Id != 7)
                {
                    lockMask.SetActive(requireLevel < relicLocalData.Requirelevel);
                    lockText.SetText($"{TableManager.Instance.RelicTable.dataArray[relicLocalData.Requirerelic].Name} {relicLocalData.Requirelevel}레벨 필요");
                }

            }).AddTo(this);
        }

        //장산범
        if (relicLocalData.Id == 7)
        {
            lockMask.SetActive(false);

            ServerData.goodsTable.GetTableData(GoodsTable.ZangStone).AsObservable().Subscribe(e =>
            {
                lockMask_San.SetActive(e < 1);
            }).AddTo(this);
        }
    }

    public void Initialize(RelicTableData relicLocalData)
    {
        this.relicLocalData = relicLocalData;

        this.relicServerData = ServerData.relicServerTable.TableDatas[this.relicLocalData.Stringid];

        relicName.SetText(this.relicLocalData.Name);
        relicName.color = CommonUiContainer.Instance.itemGradeColor[relicLocalData.Grade + 1];

        relicIcon.sprite = CommonUiContainer.Instance.relicIconList[this.relicLocalData.Id];

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }
    public void OnClickUpgrade100Button()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum < 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 5000);

        upgradeableNum = Mathf.Min(upgradeableNum, currentRelicNum);

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= upgradeableNum;

        relicServerData.level.Value += upgradeableNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }

    public void OnClickUpgrade10000Button()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum < 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 30000);

        upgradeableNum = Mathf.Min(upgradeableNum, currentRelicNum);

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= upgradeableNum;

        relicServerData.level.Value += upgradeableNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }

    public void OnClickUpgradeAllButton()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, currentRelicNum);

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= upgradeableNum;

        relicServerData.level.Value += upgradeableNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
    public void OnClickLevelupButton()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= relicLocalData.Upgradeprice;

        relicServerData.level.Value++;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);

        Param relicParam = new Param();
        relicParam.Add(relicLocalData.Stringid, ServerData.relicServerTable.TableDatas[relicLocalData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(RelicServerTable.tableName, RelicServerTable.Indate, relicParam));

        ServerData.SendTransaction(transactions);
    }
}
