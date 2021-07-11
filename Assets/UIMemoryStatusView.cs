using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIMemoryStatusView : MonoBehaviour
{
    private BossTableData bossTableData;

    [SerializeField]
    private Image bossIcon;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI skillTitle;

    private CompositeDisposable compositeDisposable = new CompositeDisposable();

    [SerializeField]
    private TextMeshProUGUI levelUpText;

    private void OnDestroy()
    {
        compositeDisposable.Dispose();
    }
    private void Subscribe()
    {
        compositeDisposable.Clear();

        ServerData.bossServerTable.TableDatas[bossTableData.Stringid].artifactLevel.AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(compositeDisposable);
    }

    public void Initialize(BossTableData bossTableData)
    {
        this.bossTableData = bossTableData;

        UpdateUi();

        Subscribe();
    }

    private void UpdateUi()
    {
        int skillLevel = ServerData.bossServerTable.TableDatas[bossTableData.Stringid].artifactLevel.Value;

        if (skillLevel >= bossTableData.Maxlevel)
        {
            levelUpText.SetText("최고레벨");
        }
        else
        {
            levelUpText.SetText($"레벨업\n{CommonString.GetItemName(Item_Type.GrowThStone)} {Utils.ConvertBigNum(bossTableData.Upgradeprice)}개");
        }

        skillTitle.SetText($"LV:{skillLevel}");

        bossIcon.sprite = CommonUiContainer.Instance.bossIcon[bossTableData.Id];

        StatusType statusType = (StatusType)bossTableData.Abilitytype;

        string desString;

        if (statusType.IsPercentStat())
        {
            desString = $"{CommonString.GetStatusName(statusType)} {skillLevel * bossTableData.Abilityvalue * 100f}% 증가";
        }
        else
        {
            desString = $"{CommonString.GetStatusName(statusType)} {skillLevel * bossTableData.Abilityvalue} 증가";
        }

        description.SetText(desString);
    }

    public void OnClickUpgradeButton()
    {
        int currentMagicStone = (int)ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value;

        if (currentMagicStone < bossTableData.Upgradeprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GrowThStone)}이 부족합니다.");
            return;
        }

        int currentLevel = ServerData.bossServerTable.TableDatas[bossTableData.Stringid].artifactLevel.Value;
        if (currentLevel >= bossTableData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고레벨 입니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말 레벨업 합니까?", () => 
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= bossTableData.Upgradeprice;
            ServerData.bossServerTable.TableDatas[bossTableData.Stringid].artifactLevel.Value++;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param bossParam = new Param();
            bossParam.Add(bossTableData.Stringid, ServerData.bossServerTable.TableDatas[bossTableData.Stringid].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLog("기억능력치레벨업",$"{bossTableData.Id.ToString()}");
            });
        }, null);
    }
}
