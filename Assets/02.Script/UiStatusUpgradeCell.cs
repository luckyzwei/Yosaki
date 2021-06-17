using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;

public class UiStatusUpgradeCell : MonoBehaviour
{
    [SerializeField]
    private Image statusIcon;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI statusNameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Image upgradeButton;

    [SerializeField]
    private Sprite enableSprite;

    [SerializeField]
    private Sprite disableSprite;

    [SerializeField]
    private Sprite maxLevelSprite;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private ObscuredInt currentLevel;

    private ObscuredFloat upgradePrice_gold;

    private WaitForSeconds autuSaveDelay = new WaitForSeconds(1.0f);
    private WaitForSeconds autuUpFirstDelay = new WaitForSeconds(1.0f);
    private WaitForSeconds autuUpDelay = new WaitForSeconds(0.01f);

    private Coroutine autoUpRoutine;
    private Coroutine saveRoutine;

    private StatusSettingData statusData;

    [SerializeField]
    private GameObject coinIcon;

    [SerializeField]
    private GameObject memoryIcon;

    [SerializeField]
    private TextMeshProUGUI upgradeText;

    private void OnDestroy()
    {
        if (CoroutineExecuter.Instance == null) return;

        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }
        if (saveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(saveRoutine);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            currentLevel.RandomizeCryptoKey();
            upgradePrice_gold.RandomizeCryptoKey();
        }
    }

    public void Initialize(StatusSettingData statusData)
    {
        this.statusData = statusData;

        memoryIcon.SetActive(statusData.STATUSWHERE == StatusWhere.memory);

        coinIcon.SetActive(statusData.STATUSWHERE == StatusWhere.gold);

        Subscribe();

        SetUpgradeButtonState(CanUpgrade());

        if (IsMaxLevel())
        {
            upgradeText.SetText("최고레벨");
        }
        else
        {
            upgradeText.SetText("업그레이드");
        }

        statusIcon.sprite = CommonUiContainer.Instance.statusIcon[statusData.Statustype];
    }

    private void Subscribe()
    {
        DatabaseManager.statusTable.GetTableData(statusData.Key).AsObservable().Subscribe(currentLevel =>
        {
            float currentStatusValue = DatabaseManager.statusTable.GetStatusValue(statusData.Key, currentLevel);
            float nextStatusValue = DatabaseManager.statusTable.GetStatusValue(statusData.Key, currentLevel + 1);

            float price = 0f;
            if (statusData.STATUSWHERE == StatusWhere.gold)
            {
                price = DatabaseManager.statusTable.GetStatusUpgradePrice(statusData.Key, currentLevel);

                priceText.SetText(Utils.ConvertBigFloat(price));

                upgradePrice_gold = price;
            }
            else if (statusData.STATUSWHERE == StatusWhere.statpoint)
            {
                price = 1;
                priceText.SetText($"{price}개");
            }
            else if (statusData.STATUSWHERE == StatusWhere.memory)
            {
                price = 1;
                priceText.SetText($"{price}개");
            }

            statusNameText.SetText(CommonString.GetStatusName((StatusType)statusData.Statustype));

            if (statusData.Ispercent == false)
            {
                if (IsMaxLevel() == false)
                {
                    descriptionText.SetText($"{(int)(currentStatusValue)}->{(int)(nextStatusValue)}");
                }
                else
                {
                    descriptionText.SetText($"{(int)(currentStatusValue)}(MAX)");
                }
            }
            //%로 표시
            else
            {
                if (IsMaxLevel() == false)
                {
                    descriptionText.SetText($"{(currentStatusValue * 100f).ToString("F2")}%->{(nextStatusValue * 100f).ToString("F2")}%");
                }
                else
                {
                    descriptionText.SetText($"{(currentStatusValue * 100f).ToString("F2")}%(MAX)");
                }
            }

            levelText.SetText($"Lv : {currentLevel}");

        }).AddTo(this);

        if (this.statusData.STATUSWHERE == StatusWhere.gold)
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.memory)
        {
            DatabaseManager.statusTable.GetTableData(StatusTable.Memory).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
    }

    public void PointerDown()
    {
        autoUpRoutine = CoroutineExecuter.Instance.StartCoroutine(AutuUpgradeRoutine());

    }
    public void PointerUp()
    {
        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }

        SyncToServer();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PointerDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            PointerUp();
        }
    }

    private void SyncToServer()
    {
        if (saveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(saveRoutine);
        }

        saveRoutine = CoroutineExecuter.Instance.StartCoroutine(SaveRoutine());
    }

    private IEnumerator AutuUpgradeRoutine()
    {
        if (UpgradeStatus() == false)
        {
            DisableUpgradeButton();
            yield break;
        }

        yield return autuUpFirstDelay;

        while (true)
        {
            bool canUpgrade = UpgradeStatus();

            if (canUpgrade == false)
            {
                DisableUpgradeButton();
                break;
            }

            yield return autuUpDelay;
        }
    }
    private bool UpgradeStatus()
    {
        if (CanUpgrade(true) == false)
        {
            return false;
        }

        SoundManager.Instance.PlayButtonSound();

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.AbilUpgrade, 1);

        UsePoint();

        DatabaseManager.statusTable.GetTableData(statusData.Key).Value += 1;

        return true;
    }

    private void UsePoint()
    {
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).Value -= upgradePrice_gold;

            UiTutorialManager.Instance.SetClear(TutorialStep._4_AbilityUp);
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).Value -= 1;
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value -= 1;
        }
    }

    private bool IsMaxLevel()
    {
        return statusData.Maxlv <= DatabaseManager.statusTable.GetTableData(statusData.Key).Value;
    }

    private bool CanUpgrade(bool showPopup = false)
    {
        if (IsMaxLevel())
        {
            upgradeText.SetText("최고레벨");

            if (showPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            }

            return false;
        }

        //구현필요
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            bool ret = DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).Value >= upgradePrice_gold;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("골드가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            bool ret = DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟포인트가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            bool ret = DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("기억의 조각이 부족합니다.");
            }

            return ret;
        }

        return true;
    }

    private void DisableUpgradeButton()
    {
        SetUpgradeButtonState(false);
    }

    private void SetUpgradeButtonState(bool on)
    {
#if TEST
        if (on == false)
        {
            return;
        }
#endif
        if (upgradeButton == null) return;

        upgradeButton.raycastTarget = on;

        if (IsMaxLevel() == false)
        {
            upgradeButton.sprite = on ? enableSprite : disableSprite;
        }
        else
        {
            upgradeButton.sprite = maxLevelSprite;
        }
    }

    private IEnumerator SaveRoutine()
    {
        yield return autuSaveDelay;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        Param goodesParam = new Param();

        //능력치
        statusParam.Add(statusData.Key, DatabaseManager.statusTable.GetTableData(statusData.Key).Value);

        //스킬포인트
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            statusParam.Add(StatusTable.StatPoint, DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).Value);
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            statusParam.Add(StatusTable.Memory, DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value);
            LogManager.Instance.SendLog("기억능력치", $"key:{statusData.Key} level:{DatabaseManager.statusTable.GetTableData(statusData.Key).Value} memory:{DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value}");
        }
        else if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            goodesParam.Add(GoodsTable.Gold, DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodesParam));
        }

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        DatabaseManager.SendTransaction(transactionList);

        saveRoutine = null;
    }
}
