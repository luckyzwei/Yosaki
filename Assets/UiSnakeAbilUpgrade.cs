using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiSnakeAbilUpgrade : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI unlockPriceText;

    private YomulAbilData yomulAbilData;

    private YomulServerData yomulServerData;

    [SerializeField]
    private TextMeshProUGUI abilName;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI upgradePriceText;

    [SerializeField]
    private GameObject upgradeButton;

    [SerializeField]
    private GameObject unlockButton;

    private void Start()
    {
        Initialize();
        Subscribe();
    }
    private void Initialize()
    {
        this.yomulAbilData = TableManager.Instance.YomulAbilTable.dataArray[6];

        yomulServerData = ServerData.yomulServerTable.TableDatas[this.yomulAbilData.Stringid];

        unlockPriceText.SetText(Utils.ConvertBigNum(yomulAbilData.Unlockprice));
    }

    private void Subscribe()
    {
        yomulServerData.level.AsObservable().Subscribe(e =>
        {
            int upgradePrice = GetUpgradePrice();

            abilDescription.SetText($"{CommonString.GetStatusName((StatusType)yomulAbilData.Abiltype2)} {PlayerStats.GetYomulUpgradeValue(StatusType.SuperCritical1Prob, onlyType2: true) * 100f}\n{CommonString.GetStatusName((StatusType)yomulAbilData.Abiltype)} {PlayerStats.GetYomulUpgradeValue(StatusType.SuperCritical1DamPer, targetId: yomulAbilData.Id) * 100f}");
            
            levelText.SetText($"Lv : {e}");

            if (e < yomulAbilData.Maxlevel)
            {
                upgradePriceText.SetText($"{Utils.ConvertBigNum(upgradePrice)}");
            }
            else
            {
                upgradePriceText.SetText($"최고레벨");
            }

        }).AddTo(this);

        yomulServerData.hasAbil.AsObservable().Subscribe(e =>
        {
            upgradeButton.SetActive(e == 1);
            unlockButton.SetActive(e == 0);
        }).AddTo(this);

    }

    private int GetUpgradePrice()
    {
        return 50000;
    }

    public void OnClickUpgradeButton()
    {
        if (yomulServerData.hasAbil.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("먼저 계약을 해야 합니다.");
            return;
        }

        if (yomulServerData.level.Value >= yomulAbilData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고 레벨 입니다.");
            return;
        }

        int price = GetUpgradePrice();

        if (ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value < price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        yomulServerData.level.Value++;
        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= price;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(1.0f);
    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        Param yomulParam = new Param();
        yomulParam.Add(yomulAbilData.Stringid, yomulServerData.ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(YomulServerTable.tableName, YomulServerTable.Indate, yomulParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }

    public void OnClickUnlockButton()
    {
        if (yomulServerData.hasAbil.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 계약 됐습니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.SnakeStone).Value < yomulAbilData.Unlockprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SnakeStone)}이 부족합니다.");
            return;
        }

        //로컬
        yomulServerData.hasAbil.Value = 1;
        //ServerData.goodsTable.GetTableData(GoodsTable.TigerStone).Value -= yomulAbilData.Unlockprice;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //Param goodsParam = new Param();
        //goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        Param yomulParam = new Param();
        yomulParam.Add(yomulAbilData.Stringid, yomulServerData.ConvertToString());

        //transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(YomulServerTable.tableName, YomulServerTable.Indate, yomulParam));

        //abilDescription.SetText($"{CommonString.GetStatusName((StatusType)yomulAbilData.Abiltype)} {PlayerStats.GetYomulUpgradeValue(StatusType.SkillCoolTime) * 100f}");

        abilDescription.SetText($"{CommonString.GetStatusName((StatusType)yomulAbilData.Abiltype2)} {PlayerStats.GetYomulUpgradeValue(StatusType.SuperCritical1Prob, onlyType2: true) * 100f}\n{CommonString.GetStatusName((StatusType)yomulAbilData.Abiltype)} {PlayerStats.GetYomulUpgradeValue(StatusType.SuperCritical1DamPer,targetId: yomulAbilData.Id) * 100f}");

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
      //      LogManager.Instance.SendLogType("Yomul", "해제", yomulAbilData.Id.ToString());
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "계약 완료!", null);
        });
    }
}
