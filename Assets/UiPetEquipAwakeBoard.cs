using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;

public class UiPetEquipAwakeBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI awakeDescription;

    [SerializeField]
    private TextMeshProUGUI yoguiMarblePrice;

    [SerializeField]
    private TextMeshProUGUI marblePrice;

    [SerializeField]
    private TextMeshProUGUI awakeProb;

    [SerializeField]
    private GameObject upgradeBlockMask;

    [SerializeField]
    private GameObject allLevelUpButton;

    void Start()
    {
        Subscribe();
        Initialize();
    }


    public void ShowAwakeBoard()
    {
        if (IsAllMaxLevel() == false)
        {
            rootObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage($"모든 환수장비가 최대 레벨 이어야 합니다.(각성 장비 제외)");

        }
        else
        {
            rootObject.SetActive(true);
        }
    }

    private bool IsAllMaxLevel()
    {
        var tableData = TableManager.Instance.PetEquipment.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            int currentLevel = ServerData.petEquipmentServerTable.TableDatas[tableData[i].Stringid].level.Value;
            if (currentLevel < tableData[i].Maxlevel)
            {
                return false;
            }
        }

        return true;
    }

    private void Initialize()
    {
        yoguiMarblePrice.SetText(Utils.ConvertBigNum(GetYoguiMarbleUpgradePrice()));

        marblePrice.SetText(Utils.ConvertBigNum(GetMarbleUpgradePrice()));

        SetAbilAddDescription();
    }

    private void SetAbilAddDescription()
    {
        var tableDatas = TableManager.Instance.PetEquipment.dataArray;

        string desc = string.Empty;

        for (int i = 0; i < tableDatas.Length - 1; i++)
        {
            desc += tableDatas[i].Name;

            if (tableDatas[i].Leveladdvalue1 != 0)
            {
                StatusType abilType = (StatusType)tableDatas[i].Abiltype1;

                desc += $" {CommonString.GetStatusName(abilType)} : {tableDatas[i].Leveladdvalue1 * (abilType.IsPercentStat() ? 100f : 1f)}";

            }

            if (tableDatas[i].Leveladdvalue2 != 0)
            {
                StatusType abilType = (StatusType)tableDatas[i].Abiltype2;

                desc += $" {CommonString.GetStatusName(abilType)} : {tableDatas[i].Leveladdvalue2 * (abilType.IsPercentStat() ? 100f : 1f)}";
            }

            desc += "\n";

            abilDescription.SetText(desc);
        }
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(currentAwake =>
        {

            awakeDescription.SetText($"현재강화도 + {currentAwake}강");

            UpdateAwakeProb();

        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.smithExp).AsObservable().Subscribe(e =>
        {

            UpdateAwakeProb();

        }).AddTo(this);

    }

    private void UpdateAwakeProb()
    {
        awakeProb.SetText($"강화 성공 확률 : {GetAwakeProb()}%");

        float prob = GetAwakeProb();

        allLevelUpButton.SetActive(prob >= 100);
    }

    private float GetYoguiMarbleUpgradePrice()
    {
        return 3000000;
    }

    private float GetMarbleUpgradePrice()
    {
        return 2500000;
    }

    private float GetAwakeProb()
    {
        int prob = 130 - ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;

        return Mathf.Clamp(prob, 50, 100) + PlayerStats.GetSmithValue(StatusType.PetEquipProbUp);
    }

    private bool TryAwake()
    {
        return Random.Range(0, 100) < GetAwakeProb();
    }

    public void OnClickMarbleAwakeButton()
    {
        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;

        if (currentMarble < GetMarbleUpgradePrice())
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        upgradeBlockMask.SetActive(true);

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= GetMarbleUpgradePrice();

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        bool awakeSuccess = TryAwake();

        if (awakeSuccess)
        {
            ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value++;

            Param statusParam = new Param();
            statusParam.Add(StatusTable.PetEquip_Level, ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        }

#if UNITY_EDITOR
        Debug.LogError(awakeSuccess ? "강화성공" : "강화실패");
#endif

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            upgradeBlockMask.SetActive(false);

            if (awakeSuccess)
            {
                PopupManager.Instance.ShowAlarmMessage("강화 성공!");
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("강화 실패!");
            }
        });
    }

    //
    public void OnClickMarbleAwakeButton_All()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "모든 여우구슬로 강화 할까요?", () =>
        {
            int prefDragonBall = PlayerStats.GetCurrentDragonIdx();

            int prefFoxCup = PlayerStats.GetCurrentFoxCupIdx();


            float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;

            if (currentMarble < GetMarbleUpgradePrice())
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
                return;
            }

            upgradeBlockMask.SetActive(true);

            int upgradableNum = (int)(ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value / GetMarbleUpgradePrice());

            ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= (GetMarbleUpgradePrice() * upgradableNum);

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


            ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value += upgradableNum;

            Param statusParam = new Param();
            statusParam.Add(StatusTable.PetEquip_Level, ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));


            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                upgradeBlockMask.SetActive(false);

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"+{upgradableNum} 강화 성공!", null);

                LogManager.Instance.SendLogType("PetEquip", "all", $"pref {ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value - upgradableNum} +{upgradableNum}");

                int currentDragonBall = PlayerStats.GetCurrentDragonIdx();

                int currentFoxCup = PlayerStats.GetCurrentFoxCupIdx();

                if (prefDragonBall < currentDragonBall)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "용보주 단계 상승!", null);
                }

                if (prefFoxCup < currentFoxCup)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우 호리병 단계 상승!", null);
                }

            });


        }, null);
    }
    //

    public void OnClickYoguiMarbleAwakeButton()
    {
        float currentSoul = ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value;

        if (currentSoul < GetYoguiMarbleUpgradePrice())
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)}이 부족합니다.");
            return;
        }

        upgradeBlockMask.SetActive(true);

        ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value -= GetYoguiMarbleUpgradePrice();

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        bool awakeSuccess = TryAwake();

        if (awakeSuccess)
        {
            ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value++;

            Param statusParam = new Param();
            statusParam.Add(StatusTable.PetEquip_Level, ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        }

        //    LogManager.Instance.SendLogType("PetEquip", "요청", awakeSuccess ? "성공":"실패");


#if UNITY_EDITOR
        Debug.LogError(awakeSuccess ? "강화성공" : "강화실패");
#endif

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              if (upgradeBlockMask != null)
              {
                  upgradeBlockMask.SetActive(false);
              }

              if (awakeSuccess)
              {
                  PopupManager.Instance.ShowAlarmMessage("강화 성공!");
              }
              else
              {
                  PopupManager.Instance.ShowAlarmMessage("강화 실패!");
              }


          });
    }



}
