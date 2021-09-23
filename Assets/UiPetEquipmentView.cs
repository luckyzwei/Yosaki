using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPetEquipmentView : MonoBehaviour
{
    [SerializeField]
    private GameObject unlockButton;

    [SerializeField]
    private GameObject levelUpButton;

    [SerializeField]
    private TextMeshProUGUI unlockButtonDesc;

    [SerializeField]
    private TextMeshProUGUI levelUpButtonDesc;

    [SerializeField]
    private TextMeshProUGUI levelUpButtonDesc_marble;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI equipTitle;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private Image itemIcon;

    private PetEquipmentData petEquipmentData;

    private PetEquipServerData petEquipServerData;

    private bool initialized = false;

    private ObscuredFloat marbleDiscountRatio = 0.8f;


    public void Initialize(PetEquipmentData petEquipmentData)
    {
        this.petEquipmentData = petEquipmentData;

        petEquipServerData = ServerData.petEquipmentServerTable.TableDatas[petEquipmentData.Stringid];

        unlockButtonDesc.SetText(Utils.ConvertBigNum(petEquipmentData.Unlockprice));

        itemIcon.sprite = CommonUiContainer.Instance.petEquipment[petEquipmentData.Id];

        equipTitle.SetText(petEquipmentData.Name);

        equipTitle.color = CommonUiContainer.Instance.petEquipColor[petEquipmentData.Id];

        if (initialized == false)
        {
            initialized = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        petEquipServerData.hasAbil.AsObservable().Subscribe(e =>
        {
            unlockButton.SetActive(e == 0);
            levelUpButton.SetActive(e == 1);
        }).AddTo(this);

        petEquipServerData.level.AsObservable().Subscribe(e =>
        {
            levelText.SetText($"(LV:{e})");

            levelUpButtonDesc.SetText(e == petEquipmentData.Maxlevel ? "최고레벨" : Utils.ConvertBigNum(petEquipmentData.Upgradeprice));

            levelUpButtonDesc_marble.SetText(e == petEquipmentData.Maxlevel ? "최고레벨" : Utils.ConvertBigNum(petEquipmentData.Upgradeprice* marbleDiscountRatio));

            string abilDesc = string.Empty;

            StatusType type1 = (StatusType)petEquipmentData.Abiltype1;
            float abilValue1 = petEquipmentData.Abilvalue1 + petEquipmentData.Abiladdvalue1 * e;
            float maxAbil1 = petEquipmentData.Abilvalue1 + petEquipmentData.Abiladdvalue1 * petEquipmentData.Maxlevel;

            abilDesc += $"{CommonString.GetStatusName(type1)} {abilValue1}(<color=red>MAX:{maxAbil1}</color>)\n";

            StatusType type2 = (StatusType)petEquipmentData.Abiltype2;
            float abilValue2 = petEquipmentData.Abilvalue2 + petEquipmentData.Abiladdvalue2 * e;
            float maxAbil2 = petEquipmentData.Abilvalue2 + petEquipmentData.Abiladdvalue2 * petEquipmentData.Maxlevel;

            abilDesc += $"{CommonString.GetStatusName(type2)} {abilValue2 * 100f}%(<color=red>MAX:{maxAbil2 * 100f}</color>)";

            abilDescription.SetText(abilDesc);

        }).AddTo(this);
    }

    public void OnClickUnlockButton()
    {
        var petTableData = TableManager.Instance.PetTable.dataArray[petEquipmentData.Requippetid];

        if (ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{petTableData.Name} 보유중일때 획득 가능");
            return;
        }

        if (petEquipServerData.hasAbil.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다.");
            return;
        }

        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;

        if (currentMarble < petEquipmentData.Unlockprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= petEquipmentData.Unlockprice;

        petEquipServerData.hasAbil.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petEquipParam = new Param();
        petEquipParam.Add(petEquipmentData.Stringid, petEquipServerData.ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        transactions.Add(TransactionValue.SetUpdate(PetEquipmentServerTable.tableName, PetEquipmentServerTable.Indate, petEquipParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        LogManager.Instance.SendLogType("환수장비", petEquipmentData.Stringid, "g");

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "환수 장비 획득!", null);
          });

    }

    public void OnClickLevelUpButton()
    {
        int currentLevel = petEquipServerData.level.Value;

        if (currentLevel >= petEquipmentData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고 레벨 입니다.");
            return;
        }

        float currentYoguiMarble = ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value;

        if (currentYoguiMarble < petEquipmentData.Upgradeprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value -= petEquipmentData.Upgradeprice;
        petEquipServerData.level.Value++;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petEquipParam = new Param();
        petEquipParam.Add(petEquipmentData.Stringid, petEquipServerData.ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);

        transactions.Add(TransactionValue.SetUpdate(PetEquipmentServerTable.tableName, PetEquipmentServerTable.Indate, petEquipParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }


    public void OnClickLevelUpButton_Marble()
    {
        int currentLevel = petEquipServerData.level.Value;

        if (currentLevel >= petEquipmentData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고 레벨 입니다.");
            return;
        }

        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;

        float upgradePrice = petEquipmentData.Upgradeprice * marbleDiscountRatio;

        if (currentMarble < upgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= upgradePrice;
        petEquipServerData.level.Value++;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petEquipParam = new Param();
        petEquipParam.Add(petEquipmentData.Stringid, petEquipServerData.ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        transactions.Add(TransactionValue.SetUpdate(PetEquipmentServerTable.tableName, PetEquipmentServerTable.Indate, petEquipParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value += 1000000;
        }
    }
#endif
}
