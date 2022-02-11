using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UiInventoryWeaponView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private WeaponView weaponView;

    [SerializeField]
    private GameObject equipText;

    [SerializeField]
    private GameObject hasMask;

    private Action<WeaponData, MagicBookData> onClickCallBack;

    private WeaponData weaponData;
    private MagicBookData magicBookData;

    [SerializeField]
    private GameObject upgradeButton;

    [SerializeField]
    private TextMeshProUGUI weaponAbilityDescription;

    [SerializeField]
    private Button levelUpButton;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice;

    [SerializeField]
    private Button equipButton;

    [SerializeField]
    private GameObject tutorialObject;

    [SerializeField]
    private GameObject yomulDescription;

    [SerializeField]
    private GameObject yomulUpgradeButton;

    [SerializeField]
    private GameObject sinsuCreateButton;

    [SerializeField]
    private GameObject youngMulCreateButton;

    [SerializeField]
    private GameObject yachaDescription;

    [SerializeField]
    private GameObject yachaUpgradeButton;

    [SerializeField]
    private GameObject feelMulCraftButton;

    [SerializeField]
    private GameObject feelMulUpgradeButton;

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData, Action<WeaponData, MagicBookData> onClickCallBack)
    {
        this.weaponData = weaponData;
        this.magicBookData = magicBookData;

        this.onClickCallBack = onClickCallBack;

        tutorialObject.SetActive(weaponData != null && weaponData.Id != 0 && ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.Value == 1);

        //요물 설명
        yomulDescription.SetActive(weaponData != null && weaponData.Id == 19);

        yomulUpgradeButton.SetActive(weaponData != null && weaponData.Id == 20);

        //야차 설명
        yachaDescription.SetActive(weaponData != null && weaponData.Id == 20);

        yachaUpgradeButton.SetActive(weaponData != null && weaponData.Id == 21);

        feelMulCraftButton.SetActive(weaponData != null && weaponData.Id == 21);

        feelMulUpgradeButton.SetActive(weaponData != null && weaponData.Id == 22);


        //신수
        sinsuCreateButton.gameObject.SetActive(magicBookData != null && magicBookData.Id / 4 == 4);

        youngMulCreateButton.gameObject.SetActive(magicBookData != null && magicBookData.Id == 20);

        if (weaponData != null)
        {
            title.SetText(weaponData.Name);
            weaponView.Initialize(weaponData, null);


        }
        else if (magicBookData != null)
        {
            title.SetText(magicBookData.Name);
            weaponView.Initialize(null, magicBookData);
        }


        SubscribeWeapon();

        SetParent();
    }


    private void SubscribeWeapon()
    {
        if (weaponData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].AsObservable().Subscribe(WhenEquipWeaponChanged).AddTo(this);
            ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(this);
            ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(this);
        }
        else if (magicBookData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(WhenEquipMagicBookChanged).AddTo(this);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(this);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(this);

        }

        if (weaponData != null)
        {
            ServerData.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(this);
        }
        else
        {
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(this);
        }
    }



    private void WhenItemLevelChanged(int level)
    {
        SetCurrentWeapon();
        UpdateLevelUpUi();
    }

    private void UpdateLevelUpUi()
    {
        if (weaponData == null && magicBookData == null) return;

        if ((weaponData != null && ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel) ||
            (magicBookData != null && ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel)
            )
        {
            levelUpButton.interactable = false;
            levelUpPrice.SetText("최대레벨");
            return;
        }


        float price = 0f;
        float currentMagicStoneAmount = 0f;

        if (weaponData != null)
        {
            price = ServerData.weaponTable.GetWeaponLevelUpPrice(weaponData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        else
        {
            price = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }

        levelUpPrice.SetText(Utils.ConvertBigNum(price));
        levelUpButton.interactable = currentMagicStoneAmount >= price;
    }

    private void WhenAmountChanged(int amount)
    {
        if (weaponData != null)
        {
            //요물일때 X
            upgradeButton.SetActive(amount >= weaponData.Requireupgrade && weaponData.Id != 19 && weaponData.Id != 20 && weaponData.Id != 21);
        }
        else if (magicBookData != null)
        {
            upgradeButton.SetActive(amount >= magicBookData.Requireupgrade && magicBookData.Id < 15);
        }
    }

    private void WhenHasStageChanged(int state)
    {
        hasMask.SetActive(state == 0);

        equipButton.gameObject.SetActive(state == 1);

        levelUpButton.gameObject.SetActive(state == 1);
    }
    private void WhenEquipWeaponChanged(int idx)
    {
        equipText.SetActive(idx == this.weaponData.Id);

        UpdateEquipButton();
    }
    private void WhenEquipMagicBookChanged(int idx)
    {
        equipText.SetActive(idx == this.magicBookData.Id);

        UpdateEquipButton();
    }

    public void OnClickIcon()
    {
        onClickCallBack?.Invoke(weaponData, magicBookData);
    }

    public void OnClickUpgradeButton()
    {
        if (weaponData != null)
        {
            int amount = ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.Value;

            if (amount < weaponData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.WeaponData.TryGetValue(weaponData.Id + 1, out var nextWeaponData))
            {
                int currentWeaponCount = ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid);
                int nextWeaponCount = ServerData.weaponTable.GetCurrentWeaponCount(nextWeaponData.Stringid);

                int upgradeNum = currentWeaponCount / weaponData.Requireupgrade;

                ServerData.weaponTable.UpData(weaponData, upgradeNum * weaponData.Requireupgrade * -1);
                ServerData.weaponTable.UpData(nextWeaponData, upgradeNum);

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.WeaponUpgrade, upgradeNum);
                ServerData.weaponTable.SyncToServerAll(new List<int>() { weaponData.Id, nextWeaponData.Id });
            }
            else
            {
                //맥스레벨 처리
                PopupManager.Instance.ShowAlarmMessage("더이상 승급이 불가능 합니다.");
            }
        }
        else
        {
            int amount = ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.Value;

            if (amount < magicBookData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.MagicBoocDatas.TryGetValue(magicBookData.Id + 1, out var nextMagicBook))
            {
                int currentWeaponCount = ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid);
                int nextWeaponCount = ServerData.magicBookTable.GetCurrentMagicBookCount(nextMagicBook.Stringid);

                int upgradeNum = currentWeaponCount / magicBookData.Requireupgrade;

                ServerData.magicBookTable.UpData(magicBookData, upgradeNum * magicBookData.Requireupgrade * -1);
                ServerData.magicBookTable.UpData(nextMagicBook, upgradeNum);

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.MagicbookUpgrade, upgradeNum);
                ServerData.magicBookTable.SyncToServerAll(new List<int>() { magicBookData.Id, nextMagicBook.Id });
            }
            else
            {
                //맥스레벨 처리
                PopupManager.Instance.ShowAlarmMessage("더이상 승급이 불가능 합니다.");
            }
        }
    }

    public void OnClickAllUpgradeButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "이전 단계 장비들도 전부 승급 합니까?", () =>
        {
            if (weaponData != null)
            {
                UiEnventoryBoard.Instance.AllUpgradeWeapon(weaponData.Id);
            }
            else
            {
                UiEnventoryBoard.Instance.AllUpgradeMagicBook(magicBookData.Id);
            }

        }, null);
    }

    private void SetCurrentWeapon()
    {
        weaponView.Initialize(weaponData, magicBookData);

        //currentCompareView.Initialize(weaponData, magicBookData);

        //if (weaponData != null)
        //{
        //    compareAmount1.SetText($"{DatabaseManager.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{weaponData.Requireupgrade}");
        //}
        //else
        //{
        //    compareAmount1.SetText($"{DatabaseManager.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{magicBookData.Requireupgrade}");
        //}

        SetWeaponAbilityDescription();
    }


    private void SetWeaponAbilityDescription()
    {
        WeaponEffectData effectData;
        string stringid;
        int weaponLevel = 0;


        if (weaponData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.weaponData.Weaponeffectid];
            weaponLevel = ServerData.weaponTable.TableDatas[this.weaponData.Stringid].level.Value;
            stringid = this.weaponData.Stringid;
        }
        else
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;

        }

        string description = string.Empty;

        description += "<color=#ff00ffff>장착 효과</color>\n";

        float equipValue1 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
        float equipValue1_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, magicBookData.Maxlevel);

        float equipValue2 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
        float equipValue2_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, magicBookData.Maxlevel);

        float hasValue1 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
        float hasValue1_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, magicBookData.Maxlevel);

        float hasValue2 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
        float hasValue2_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, magicBookData.Maxlevel);

        float hasValue3 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
        float hasValue3_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, magicBookData.Maxlevel);


        if (effectData.Equipeffecttype1 != -1)
        {
            //%효과
            if (effectData.Equipeffectvalue1 <= 1f)
            {
                float value = equipValue1 * 100f;
                float value_max = equipValue1_max * 100f;
                StatusType type = (StatusType)(effectData.Equipeffecttype1);

                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
            else
            {
                float value = equipValue1;
                float value_max = equipValue1_max;
                StatusType type = (StatusType)(effectData.Equipeffecttype1);

                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
        }

        if (effectData.Equipeffecttype2 != -1)
        {
            //%효과
            if (effectData.Equipeffectvalue2 <= 1f)
            {
                float value = equipValue2 * 100f;
                float value_max = equipValue2_max * 100f;
                StatusType type = (StatusType)effectData.Equipeffecttype2;

                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
            else
            {
                float value = equipValue2;
                float value_max = equipValue2_max;
                StatusType type = (StatusType)effectData.Equipeffecttype2;

                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}";
            }

        }

        description += "\n<color=#ffff00ff>보유 효과</color>\n";

        if (effectData.Haseffecttype1 != -1)
        {
            StatusType type = (StatusType)effectData.Haseffecttype1;

            if (type.IsPercentStat())
            {
                float value = hasValue1 * 100f;
                float value_max = hasValue1_max * 100f;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
            else
            {
                float value = hasValue1;
                float value_max = hasValue1_max;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
        }

        if (effectData.Haseffecttype2 != -1)
        {
            StatusType type = (StatusType)effectData.Haseffecttype2;

            if (type.IsPercentStat())
            {
                float value = hasValue2 * 100f;
                float value_max = hasValue2_max * 100f;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
            else
            {
                float value = hasValue2;
                float value_max = hasValue2_max;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }

        }

        if (effectData.Haseffecttype3 != -1)
        {
            StatusType type = (StatusType)effectData.Haseffecttype3;

            if (type.IsPercentStat())
            {
                float value = hasValue3 * 100f;
                float value_max = hasValue3_max * 100f;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}";
            }
            else
            {
                float value = hasValue3;
                float value_max = hasValue3_max;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}";
            }

        }

        weaponAbilityDescription.SetText(description);
    }


    public void OnClickEquipButton()
    {
        if (weaponData != null)
        {
            if (weaponData.Id < 20)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon, weaponData.Id);

            }
            else
            {
                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 무기를 변경 할까요?", () =>
                {
                    ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon, weaponData.Id);
                }, () => { });
            }
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
        else
        {
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook, magicBookData.Id);
        }

        UpdateEquipButton();
    }

    private void UpdateEquipButton()
    {
        int id = this.weaponData != null ? weaponData.Id : magicBookData.Id;

        int has = 0;

        if (weaponData != null)
        {
            has = ServerData.weaponTable.GetWeaponData(weaponData.Stringid).hasItem.Value;
        }
        else
        {
            has = ServerData.magicBookTable.GetMagicBookData(magicBookData.Stringid).hasItem.Value;
        }

        equipButton.gameObject.SetActive(has == 1);
        levelUpButton.gameObject.SetActive(has == 1);

        if (equipButton.gameObject.activeSelf)
        {
            string key = weaponData != null ? EquipmentTable.Weapon : EquipmentTable.MagicBook;
            int equipIdx = ServerData.equipmentTable.TableDatas[key].Value;

            equipButton.interactable = equipIdx != id;
            //equipDescription.SetText(equipIdx == id ? "장착중" : "장착");
        }
        // ShowSubDetailView();
    }
    public void OnClickLevelUpButton()
    {
        if (weaponData != null)
        {
            float currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
            float levelUpPrice = ServerData.weaponTable.GetWeaponLevelUpPrice(weaponData.Stringid);

            if (ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel)
            {
                PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
                return;
            }

            if (currentMagicStoneAmount < levelUpPrice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GrowthStone)} 부족합니다.");
                return;
            }

            SoundManager.Instance.PlayButtonSound();
            //재화 차감
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= levelUpPrice;
            //레벨 상승
            ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value++;
            //일일 미션
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.WeaponLevelUp, 1);

            //서버에 반영
            SyncServerRoutineWeapon();
        }
        else
        {
            float currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
            float levelUpPrice = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);

            if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel)
            {
                PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
                return;
            }

            if (currentMagicStoneAmount < levelUpPrice)
            {
                PopupManager.Instance.ShowAlarmMessage("수련의돌이 부족합니다.");
                return;
            }

            //재화 차감
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= levelUpPrice;
            //레벨 상승
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value++;
            //일일 미션
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.MagicBookLevelUp, 1);
            //서버에 반영
            SyncServerRoutineMagicBook();
        }

    }

    private Dictionary<int, Coroutine> SyncRoutine_weapon = new Dictionary<int, Coroutine>();
    private WaitForSeconds syncWaitTime_weapon = new WaitForSeconds(2.0f);
    private void SyncServerRoutineWeapon()
    {
        if (SyncRoutine_weapon.ContainsKey(weaponData.Id) == false)
        {
            SyncRoutine_weapon.Add(weaponData.Id, null);
        }

        if (SyncRoutine_weapon[weaponData.Id] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine_weapon[weaponData.Id]);
        }

        SyncRoutine_weapon[weaponData.Id] = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutineWeapon(weaponData.Id));
    }

    private Dictionary<int, Coroutine> SyncRoutineMagicBook = new Dictionary<int, Coroutine>();
    private WaitForSeconds syncWaitTimeMagicBook = new WaitForSeconds(2.0f);
    private void SyncServerRoutineMagicBook()
    {
        if (SyncRoutineMagicBook.ContainsKey(magicBookData.Id) == false)
        {
            SyncRoutineMagicBook.Add(magicBookData.Id, null);
        }

        if (SyncRoutineMagicBook[magicBookData.Id] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutineMagicBook[magicBookData.Id]);
        }

        SyncRoutineMagicBook[magicBookData.Id] = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutineMagicBook(magicBookData.Id));
    }

    private IEnumerator SyncDataRoutineWeapon(int id)
    {
        yield return syncWaitTime_weapon;

        WeaponData weapon = TableManager.Instance.WeaponData[id];

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        Param weaponParam = new Param();

        //재화 차감
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //레벨 상승
        string updateValue = ServerData.weaponTable.TableDatas[weapon.Stringid].ConvertToString();
        weaponParam.Add(weapon.Stringid, updateValue);
        transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransaction(transactionList);

        if (SyncRoutine_weapon != null)
        {
            SyncRoutine_weapon[id] = null;
        }
    }

    private IEnumerator SyncDataRoutineMagicBook(int id)
    {
        yield return syncWaitTimeMagicBook;

        MagicBookData magicbook = TableManager.Instance.MagicBoocDatas[id];

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        Param magicBookParam = new Param();

        //재화 차감
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //레벨 상승
        string updateValue = ServerData.magicBookTable.TableDatas[magicbook.Stringid].ConvertToString();
        magicBookParam.Add(magicbook.Stringid, updateValue);

        transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));


        ServerData.SendTransaction(transactionList);

        if (SyncRoutineMagicBook != null)
        {
            SyncRoutineMagicBook[id] = null;
        }
    }

    public void OnClickSinsuCreateButton()
    {
        if (magicBookData == null) return;

        UiNorigaeCraftBoard.Instance.Initialize(magicBookData.Id);
    }

    public void OnClickYoungMulCreateButton()
    {
        if (magicBookData == null) return;

        UiYoungMulCraftBoard.Instance.Initialize(magicBookData.Id);
    }

    private void OnEnable()
    {
        SetParent();
    }

    private void SetParent()
    {
        if (weaponData != null)
        {
            //요물 야차만
            if (weaponData.Id >= 20)
            {
                if (ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.Value == 1)
                {
                    this.transform.SetAsFirstSibling();
                }
            }
        }
    }

}
