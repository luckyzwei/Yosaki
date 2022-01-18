using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiStageRelicCell : MonoBehaviour
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

    private StageRelicData relicLocalData;

    private RelicServerData relicServerData;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockText;

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
                var abilValue = PlayerStats.GetStageRelicHasEffect(abilType);

                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {abilValue * 100f}%");

            }
            else
            {
                var abilValue = PlayerStats.GetStageRelicHasEffect(abilType);

                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {abilValue}");
            }

        }).AddTo(this);

        lockMask.SetActive(false);

        if (relicLocalData.Requirerelic != -1)
        {
            var requireServerData = ServerData.stageRelicServerTable.TableDatas[TableManager.Instance.StageRelic.dataArray[relicLocalData.Requirerelic].Stringid];

            lockText.color = CommonUiContainer.Instance.itemGradeColor[TableManager.Instance.StageRelic.dataArray[relicLocalData.Requirerelic].Grade + 1];

            requireServerData.level.AsObservable().Subscribe(requireLevel =>
            {
                lockMask.SetActive(requireLevel < relicLocalData.Requirelevel);

                lockText.SetText($"{TableManager.Instance.StageRelic.dataArray[relicLocalData.Requirerelic].Name} {relicLocalData.Requirelevel}레벨 필요");
            }).AddTo(this);
        }
    }

    public void Initialize(StageRelicData relicLocalData)
    {
        this.relicLocalData = relicLocalData;

        this.relicServerData = ServerData.stageRelicServerTable.TableDatas[this.relicLocalData.Stringid];

        relicName.SetText(this.relicLocalData.Name);
        relicName.color = CommonUiContainer.Instance.itemGradeColor[relicLocalData.Grade + 1];

        if (CommonUiContainer.Instance.stageRelicIconList.Count != 0)
        {
            relicIcon.sprite = CommonUiContainer.Instance.stageRelicIconList[this.relicLocalData.Id];
        }

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

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;

        if (currentRelicNum < 1000 * GameBalance.StageRelicUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        int upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 1000);

        ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= upgradeableNum * GameBalance.StageRelicUpgradePrice;

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

        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;

        if (currentRelicNum < GameBalance.StageRelicUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        float upgradeableMaxNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        float upgradableMaxPrice = (float)upgradeableMaxNum * (float)GameBalance.StageRelicUpgradePrice;

        float diffPrice = currentRelicNum - upgradableMaxPrice;

        //유물 남을때(최대업가능)
        if (diffPrice >= 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= (upgradeableMaxNum * GameBalance.StageRelicUpgradePrice);

            relicServerData.level.Value += (int)upgradeableMaxNum;
        }
        else
        {
            int fullUpgradeNum = (int)(currentRelicNum / GameBalance.StageRelicUpgradePrice);

            ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= (fullUpgradeNum * GameBalance.StageRelicUpgradePrice);

            relicServerData.level.Value += (int)fullUpgradeNum;
        }


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

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;

        if (currentRelicNum < GameBalance.StageRelicUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= GameBalance.StageRelicUpgradePrice;

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
        goodsParam.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);

        Param relicParam = new Param();
        relicParam.Add(relicLocalData.Stringid, ServerData.stageRelicServerTable.TableDatas[relicLocalData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(StageRelicServerTable.tableName, StageRelicServerTable.Indate, relicParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
            //  LogManager.Instance.SendLogType("StageRelic", relicLocalData.Stringid, ServerData.stageRelicServerTable.TableDatas[relicLocalData.Stringid].level.Value.ToString());
          });
    }
}
