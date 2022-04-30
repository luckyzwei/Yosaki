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
    private Image upgradeButton_100;

    [SerializeField]
    private Image upgradeButton_10000;

    [SerializeField]
    private Image upgradeButton_all;

    [SerializeField]
    private Sprite enableSprite;

    [SerializeField]
    private Sprite disableSprite;

    [SerializeField]
    private Sprite maxLevelSprite;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private ObscuredInt currentLevel = -1;

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

    [SerializeField]
    private GameObject allUpgradeButton;

    [SerializeField]
    private GameObject _100UpgradeButton;

    [SerializeField]
    private GameObject _10000UpgradeButton;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;

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

        allUpgradeButton.SetActive(statusData.STATUSWHERE != StatusWhere.gold);

        _100UpgradeButton.SetActive(statusData.STATUSWHERE != StatusWhere.gold);

        _10000UpgradeButton.SetActive(statusData.STATUSWHERE != StatusWhere.gold);

        memoryIcon.SetActive(statusData.STATUSWHERE == StatusWhere.memory);

        coinIcon.SetActive(statusData.STATUSWHERE == StatusWhere.gold);

        Subscribe();

        SetUpgradeButtonState(CanUpgrade());

        if (IsMaxLevel())
        {
            upgradeText.SetText("최고단계");
        }
        else
        {
            upgradeText.SetText("수련");
        }

        statusIcon.sprite = CommonUiContainer.Instance.statusIcon[statusData.Statustype];
    }
    private void RefreshStatusText()
    {
        float currentStatusValue = ServerData.statusTable.GetStatusValue(statusData.Key, currentLevel);
        float nextStatusValue = ServerData.statusTable.GetStatusValue(statusData.Key, currentLevel + 1);

        float price = 0f;
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            price = ServerData.statusTable.GetStatusUpgradePrice(statusData.Key, currentLevel);

            priceText.SetText(Utils.ConvertBigNum(price));

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
    }
    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(statusData.Key).AsObservable().Subscribe(currentLevel =>
        {
            this.currentLevel = currentLevel;
            RefreshStatusText();
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(e =>
        {
            //초기화 됐을때만
            if (currentLevel != -1 && statusData.STATUSWHERE == StatusWhere.gold)
            {
                RefreshStatusText();
            }
        }).AddTo(this);


        if (this.statusData.STATUSWHERE == StatusWhere.gold)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            ServerData.statusTable.GetTableData(StatusTable.StatPoint).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.memory)
        {
            ServerData.statusTable.GetTableData(StatusTable.Memory).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }

        if (statusData.Unlocklevel != 0)
        {
            lockDescription.SetText($"{ TableManager.Instance.StatusDatas[statusData.Needstatuskey].Description} LV : {statusData.Unlocklevel} 이상 필요");

            ServerData.statusTable.GetTableData(statusData.Needstatuskey).AsObservable().Subscribe(e =>
            {
                lockMask.SetActive(statusData.Unlocklevel >= e + 2);
            }).AddTo(this);
        }
        else
        {
            lockMask.SetActive(false);
        }
    }

    public void PointerDown()
    {
        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }

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

#if UNITY_EDITOR
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
#endif

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

        ServerData.statusTable.GetTableData(statusData.Key).Value += 1;

        return true;
    }

    private void UsePoint()
    {
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value -= upgradePrice_gold;

            UiTutorialManager.Instance.SetClear(TutorialStep.UpgradeGoldStat);
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= 1;
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= 1;
        }
    }

    private bool IsMaxLevel()
    {
        return statusData.Maxlv <= ServerData.statusTable.GetTableData(statusData.Key).Value;
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
            bool ret = ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value >= upgradePrice_gold;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Gold)}가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            bool ret = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟포인트가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            bool ret = ServerData.statusTable.GetTableData(StatusTable.Memory).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("무공비급이 부족합니다.");
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
        if (upgradeButton == null) return;

        upgradeButton.raycastTarget = on;

        if (IsMaxLevel() == false)
        {
            upgradeButton.sprite = on ? enableSprite : disableSprite;
            upgradeButton_100.sprite = on ? enableSprite : disableSprite;
            upgradeButton_all.sprite = on ? enableSprite : disableSprite;
            upgradeButton_10000.sprite = on ? enableSprite : disableSprite;
        }
        else
        {
            upgradeButton.sprite = maxLevelSprite;
            upgradeButton_100.sprite = maxLevelSprite;
            upgradeButton_all.sprite = maxLevelSprite;
            upgradeButton_10000.sprite = maxLevelSprite;
        }
    }

    public void OnClickAllUpgradeButton()
    {
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            int currentStatPoint = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value;

            if (currentStatPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            int currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            int upgradableAmount = maxLevel - currentLevel;

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= currentStatPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentStatPoint;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            int currentMemoryPoint = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

            if (currentMemoryPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("무공비급이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            int currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            int upgradableAmount = maxLevel - currentLevel;

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= currentMemoryPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
            }
        }

        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }

    public void OnClick100_Upgrade()
    {
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            int currentStatPoint = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value;

            if (currentStatPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            int currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            int upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 100);

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= currentStatPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentStatPoint;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            int currentMemoryPoint = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

            if (currentMemoryPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("무공비급이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            int currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            int upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 100);

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= currentMemoryPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
            }
        }

        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }
    public void OnClick10000_Upgrade()
    {
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            int currentStatPoint = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value;

            if (currentStatPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            int currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            int upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 10000);

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= currentStatPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentStatPoint;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            int currentMemoryPoint = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

            if (currentMemoryPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("무공비급이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            int currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            int upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 10000);

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= currentMemoryPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
            }
        }

        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }

    private IEnumerator SaveRoutine()
    {
        yield return autuSaveDelay;

        SyncData();

        saveRoutine = null;
    }

    private void SyncData()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        Param goodesParam = new Param();

        //능력치
        statusParam.Add(statusData.Key, ServerData.statusTable.GetTableData(statusData.Key).Value);

        //스킬포인트
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            statusParam.Add(StatusTable.StatPoint, ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value);
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            statusParam.Add(StatusTable.Memory, ServerData.statusTable.GetTableData(StatusTable.Memory).Value);
            LogManager.Instance.SendLog("기억능력치", $"key:{statusData.Key} level:{ServerData.statusTable.GetTableData(statusData.Key).Value} memory:{ServerData.statusTable.GetTableData(StatusTable.Memory).Value}");
        }
        else if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            goodesParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodesParam));
        }

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactionList);

        UiPlayerStatBoard.Instance.Refresh();
    }
}
