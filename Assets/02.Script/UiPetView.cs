using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Spine.Unity;
using BackEnd;

public enum PetGetType
{
    Ad, Gem, Shop, Evolution
}

public class UiPetView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI description1;

    [SerializeField]
    private TextMeshProUGUI description2;

    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    [SerializeField]
    private TextMeshProUGUI petName;

    private PetTableData petData;
    private PetServerData petServerData;

    [SerializeField]
    private GameObject adIcon;

    [SerializeField]
    private GameObject gemIcon;

    [SerializeField]
    private Image button;

    [SerializeField]
    private Sprite normalButtonSprite;

    [SerializeField]
    private Sprite equipButtonSprite;

    [SerializeField]
    private GameObject tutorialObject;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice_Soul;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice_Marble;

    [SerializeField]
    private GameObject petAwakeButton;

    [SerializeField]
    private TextMeshProUGUI awakePrice;

    [SerializeField]
    private GameObject awakeButton;

    private void SetPetSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];
        skeletonGraphic.startingAnimation = "walk";
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }


    private bool initialized = false;

    public void Initialize(PetTableData petData)
    {
        this.petData = petData;
        this.petServerData = ServerData.petTable.TableDatas[petData.Stringid];

        SetPetSpine(petData.Id);

      
        petName.color = CommonUiContainer.Instance.itemGradeColor[petData.Id / 4];

        awakePrice.SetText(Utils.ConvertBigNum(petData.Awakeprice));

        UpdateUi();

        if (initialized == false)
        {
            initialized = true;
            Subscribe();
        }

        SetAbilityText();

        tutorialObject.SetActive(petData.PETGETTYPE == PetGetType.Gem && petData.Price == 0f);
    }

    private void SetAbilityText()
    {
        string abilityStr = string.Empty;

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (petData.Hasvalue1 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype1;
            bool isPercent = statusType.IsPercentStat();
            abilityStr += $"{CommonString.GetStatusName(statusType)} : {(petData.Hasvalue1 + currentLevel * petData.Hasaddvalue1) * (isPercent ? 100 : 1f)}(<color=red>MAX:{(petData.Hasvalue1 + maxLevel * petData.Hasaddvalue1) * (isPercent ? 100 : 1f)}</color>)\n";
        }

        if (petData.Hasvalue2 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype2;
            bool isPercent = statusType.IsPercentStat();
            abilityStr += $"{CommonString.GetStatusName(statusType)} : {(petData.Hasvalue2 + currentLevel * petData.Hasaddvalue2) * (isPercent ? 100 : 1f)}(<color=red>MAX:{(petData.Hasvalue2 + maxLevel * petData.Hasaddvalue2) * (isPercent ? 100 : 1f)}</color>)\n";
        }

        if (petData.Hasvalue3 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype3;
            bool isPercent = statusType.IsPercentStat();
            abilityStr += $"{CommonString.GetStatusName(statusType)} : {(petData.Hasvalue3 + currentLevel * petData.Hasaddvalue3) * (isPercent ? 100 : 1f)}(<color=red>MAX:{(petData.Hasvalue3 + maxLevel * petData.Hasaddvalue3) * (isPercent ? 100 : 1f)}</color>)";
        }


        if (abilityStr.Equals(string.Empty))
        {
            abilityStr = "효과 없음";
        }

        description2.SetText(abilityStr);
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(this);

        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            if (e == 1)
            {
                UpdateUi();
            }
            string defix = e == 1 ? "" : "(미보유)";
            petName.SetText($"{petData.Name}{defix}");
        }).AddTo(this);

        petServerData.level.AsObservable().Subscribe(e =>
        {
            if (e == petData.Maxlevel)
            {
                levelText.SetText($"LV : {petData.Maxlevel}(MAX)");

                levelUpPrice_Soul.SetText("최대레벨");

                levelUpPrice_Marble.SetText("최대레벨");
            }
            else
            {
                levelText.SetText($"(LV : {e})");

                levelUpPrice_Soul.SetText($"{Utils.ConvertBigNum(GetPetLevelUpPrice_Soul(e))}\n레벨업");

                levelUpPrice_Marble.SetText($"{Utils.ConvertBigNum(GetPetLevelUpPrice_Marble(e))}\n레벨업");
            }

            SetAbilityText();

        }).AddTo(this);

        if (petData.Nextpetid != -1)
        {
            var nextPetTableData = TableManager.Instance.PetTable.dataArray[petData.Nextpetid];

            var newPetServerData = ServerData.petTable.TableDatas[nextPetTableData.Stringid];

            newPetServerData.hasItem.Subscribe(e =>
            {
                awakeButton.SetActive(e == 0);
            }).AddTo(this);

        }

    }

    private void UpdateUi()
    {
        petAwakeButton.SetActive(petData.Nextpetid != -1);

        adIcon.SetActive(petData.PETGETTYPE == PetGetType.Ad);
        gemIcon.SetActive(petData.PETGETTYPE == PetGetType.Gem && ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value != 1);

        button.sprite = normalButtonSprite;

        //장착중
        if (ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value == petData.Id)
        {
            if (petData.PETGETTYPE != PetGetType.Ad)
            {
                buttonDescription.SetText("장착중");
                button.sprite = equipButtonSprite;
                return;
            }
            //광고펫
            else
            {
                //시간남아있음
                if (petServerData.remainSec.Value > 0)
                {
                    buttonDescription.SetText($"장착중\n{petServerData.remainSec.Value / 60}분 남음");
                    button.sprite = equipButtonSprite;
                    return;
                }
                //시간 다씀
                else
                {
                    buttonDescription.SetText($"광고보고 대여\n({petData.Time / 60}분)");
                    return;
                }
            }
        }

        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            //보유
            if (petServerData.hasItem.Value == 1 && petServerData.remainSec.Value > 0)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                buttonDescription.SetText($"광고보고 대여\n({petData.Time / 60}분)");
            }
        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                if (petData.Price == 0f)
                {
                    buttonDescription.SetText("무료");
                }
                else
                {
                    buttonDescription.SetText($"{Utils.ConvertBigNum(petData.Price)}");
                }
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Shop)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                buttonDescription.SetText("상점에서\n구매가능");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Evolution)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                int prefPetId = petData.Id - 4;

                var prefPetData = TableManager.Instance.PetTable.dataArray[prefPetId];

                buttonDescription.SetText($"{prefPetData.Name}\n각성시 획득");
            }
        }

    }

    public void OnClickGetButton()
    {
        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            //보유
            if (petServerData.hasItem.Value == 1 && petServerData.remainSec.Value > 0)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                BuyPetRoutine();
            }
        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                int currentBlueStone = (int)ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

                if (currentBlueStone >= petData.Price)
                {
                    BuyPetRoutine();
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
                }
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Shop)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                PopupManager.Instance.ShowAlarmMessage("상점에서 구매 가능합니다.\n(외형&환수)");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Evolution)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                PopupManager.Instance.ShowAlarmMessage("환수를 각성시켜 획득 가능합니다.");
            }
        }

        UpdateUi();
    }

    private void BuyPetRoutine()
    {
        UiTutorialManager.Instance.SetClear(TutorialStep.GetPet);

        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            AdManager.Instance.ShowRewardedReward(BuyFreePet);
        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= petData.Price;
            //
            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param petParam = new Param();
            petParam.Add(petData.Stringid, ServerData.petTable.TableDatas[petData.Stringid].ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            ServerData.SendTransaction(transactionList, successCallBack: UpdateUi);
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
        }

    }

    public void BuyFreePet()
    {
        var localTableData = TableManager.Instance.PetDatas[petData.Id];
        ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;
        ServerData.petTable.TableDatas[petData.Stringid].remainSec.Value = (int)petData.Time;
        ServerData.petTable.UpdateData(petData.Stringid);
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
        PlayerPet.Instance.WhenPetEquipIdxChanged(0);
        UpdateUi();
    }

    public void OnClickLevelUpButton_Marble()
    {
        if (ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("환수가 없습니다.");
            return;
        }

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨 입니다.");
            return;
        }

        int currentMarble = (int)ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;
        int price_Marble = GetPetLevelUpPrice_Marble(currentLevel);

        if (currentMarble < price_Marble)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        //로컬데이터
        ServerData.petTable.TableDatas[petData.Stringid].level.Value++;
        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= price_Marble;

        if (serverSyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(serverSyncRoutine);
        }

        serverSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncServerData());
    }


    public void OnClickLevelUpButton_Soul()
    {
        if (ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("환수가 없습니다.");
            return;
        }

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨 입니다.");
            return;
        }

        int currentSoul = (int)ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value;
        int price_Soul = GetPetLevelUpPrice_Soul(currentLevel);

        if (currentSoul < price_Soul)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)}이 부족합니다.");
            return;
        }

        //로컬데이터
        ServerData.petTable.TableDatas[petData.Stringid].level.Value++;
        ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value -= price_Soul;

        if (serverSyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(serverSyncRoutine);
        }

        serverSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncServerData());
    }

    //서버 싱크

    private Coroutine serverSyncRoutine;

    private IEnumerator SyncServerData()
    {
        yield return new WaitForSeconds(1.0f);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);

        Param petParam = new Param();
        petParam.Add(petData.Stringid, ServerData.petTable.TableDatas[petData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

        ServerData.SendTransaction(transactions);
    }

    public void OnClickAwakeButton()
    {
        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (currentLevel < maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨이 아닙니다.");
            return;
        }

        int currentMarble = (int)ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;
        int price_Marble = petData.Awakeprice;

        if (currentMarble < price_Marble)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= price_Marble;

        var nextPetTableData = TableManager.Instance.PetTable.dataArray[petData.Nextpetid];

        var newPetServerData = ServerData.petTable.TableDatas[nextPetTableData.Stringid];

        newPetServerData.hasItem.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        Param petParam = new Param();
        petParam.Add(nextPetTableData.Stringid, ServerData.petTable.TableDatas[nextPetTableData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{nextPetTableData.Name} 획득!", null);
          });
    }


    public int GetPetLevelUpPrice_Soul(int level)
    {
        return (int)Mathf.Pow(level + 1, 2.45f + petData.Id * 0.01f);
    }

    public int GetPetLevelUpPrice_Marble(int level)
    {
        return (int)Mathf.Pow(level + 1, 2.29f);
    }
}
