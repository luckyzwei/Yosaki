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
    private NewGachaTableData newGachaData;

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
    private GameObject youngMulCreateButton2;

    [SerializeField]
    private GameObject yachaDescription;

    [SerializeField]
    private GameObject yachaUpgradeButton;

    [SerializeField]
    private GameObject feelMulCraftButton;

    [SerializeField]
    private GameObject feelMulUpgradeButton;

    [SerializeField]
    private Image weaponViewEquipButton;

    [SerializeField]
    private TextMeshProUGUI weaponViewEquipDesc;

    [SerializeField]
    private Image magicBookViewEquipButton;

    [SerializeField]
    private TextMeshProUGUI magicBookViewEquipDesc;

    [SerializeField]
    private Image newGachaViewEquipButton;

    [SerializeField]
    private TextMeshProUGUI newGachaViewEquipDesc;

    [SerializeField]
    private Sprite weaponViewEquipDisable;

    [SerializeField]
    private Sprite weaponViewEquipEnable;

    [SerializeField]
    private GameObject feelMul2Lock;

    [SerializeField]
    private GameObject feelMul3Lock;

    [SerializeField]
    private GameObject feelMul4Lock;

    [SerializeField]
    private GameObject indraLock;

    [SerializeField]
    private GameObject nataLock;
    [SerializeField]
    private GameObject orochiLock;
    [SerializeField]
    private GameObject feelPaeLock;
    [SerializeField]
    private GameObject gumihoWeaponLock;
    [SerializeField]
    private GameObject hellWeaponLock;
    [SerializeField]
    private GameObject yeoRaeWeaponLock;
    [SerializeField]
    private GameObject weaponLockObject;
    [SerializeField]
    private TextMeshProUGUI weaponLockDescription;

    [SerializeField]
    private GameObject armDescription;

    [SerializeField]
    private GameObject chunDescription;

    [SerializeField]
    private TextMeshProUGUI norigaeDescription;

    [SerializeField]
    private TextMeshProUGUI suhoSinDescription;

    [SerializeField]
    private GameObject foxNorigaeGetButton;

    private void SetEquipButton(bool onOff)
    {
        equipButton.gameObject.SetActive(onOff);

        if (magicBookData != null && magicBookData.MAGICBOOKTYPE == MagicBookType.View)
        {
            equipButton.gameObject.SetActive(false);
        }

        if (weaponData != null && weaponData.WEAPONTYPE == WeaponType.View)
        {
            equipButton.gameObject.SetActive(false);
        }

        //사신수
        if (weaponData != null && weaponData.Id >= 67 && weaponData.Id <= 70)
        {
            equipButton.gameObject.SetActive(false);
            //
        }
    }

    public void OnClickWeaponViewButton()
    {
        if (weaponData != null)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 무기 외형을 변경 할까요?", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon_View, weaponData.Id);
            }, () => { });
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
    }

    public void OnClickMagicBookViewButton()
    {
        if (magicBookData != null)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 노리개 외형을 변경 할까요?", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook_View, magicBookData.Id);
            }, () => { });
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
    }

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData, NewGachaTableData newGachaTableData, Action<WeaponData, MagicBookData> onClickCallBack)
    {
        this.weaponData = weaponData;
        this.magicBookData = magicBookData;
        this.newGachaData = newGachaTableData;

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

        //weaponViewEquipButton.SetActive(weaponData != null);

        armDescription.gameObject.SetActive(weaponData != null && weaponData.Id == 23);
        chunDescription.gameObject.SetActive(weaponData != null && weaponData.Id == 24);

        //신수
        sinsuCreateButton.gameObject.SetActive(magicBookData != null && magicBookData.Id / 4 == 4);

        youngMulCreateButton.gameObject.SetActive(magicBookData != null && magicBookData.Id == 20);
        youngMulCreateButton2.gameObject.SetActive(magicBookData != null && magicBookData.Id == 21);

        norigaeDescription.gameObject.SetActive(true);

        suhoSinDescription.gameObject.SetActive(magicBookData != null &&
            (magicBookData.Id == 22 ||
            magicBookData.Id == 23 ||
            magicBookData.Id == 24 ||
            magicBookData.Id == 25 ||
            magicBookData.Id == 26 ||
            magicBookData.Id == 27 ||
            magicBookData.Id == 28 ||
            magicBookData.Id == 29 ||
            magicBookData.Id == 30 ||
            magicBookData.Id == 31 ||
            magicBookData.Id == 32 ||
            magicBookData.Id == 33 ||
            //
            magicBookData.Id == 34 ||
            magicBookData.Id == 35 ||
            magicBookData.Id == 36 ||
            magicBookData.Id == 37 ||
            magicBookData.Id == 38 ||
            magicBookData.Id == 39 ||
            magicBookData.Id == 40 ||
            magicBookData.Id == 41 ||
            magicBookData.Id == 42 ||
            magicBookData.Id == 43 ||
            magicBookData.Id == 44 ||
            magicBookData.Id == 45 ||
            magicBookData.Id == 46 ||
            magicBookData.Id == 47 ||
            magicBookData.Id == 48 ||
            magicBookData.Id == 49 ||
            magicBookData.Id == 50 ||
            magicBookData.Id == 51 ||
            magicBookData.Id == 52 ||
            magicBookData.Id == 53 ||
            magicBookData.Id == 54 ||
            magicBookData.Id == 55 ||
            magicBookData.Id == 56 ||
            magicBookData.Id == 57 ||
            magicBookData.Id == 58 
            ));
        foxNorigaeGetButton.SetActive(false);
        

        if (magicBookData != null)
        {
            if ((magicBookData.Id == 22 || magicBookData.Id == 24 || magicBookData.Id == 25 || magicBookData.Id == 26 || magicBookData.Id == 27 || magicBookData.Id == 29))
            {
                suhoSinDescription.SetText($"요괴사냥\n수호신 에서 획득!");
            }
            else if (magicBookData.Id == 23)
            {
                suhoSinDescription.SetText($"여름훈련에서\n획득!");
            }
            else if (magicBookData.Id == 28)
            {
                suhoSinDescription.SetText($"요괴사냥\n구미호꼬리 8획득시\n획득 가능");
            }
            else if (magicBookData.Id == 30 || magicBookData.Id == 31)
            {
                suhoSinDescription.SetText($"요괴지옥\n지옥불꽃에서 획득!");
            }
            else if (magicBookData.Id == 32)
            {
                suhoSinDescription.SetText($"여래전에서\n획득!");
            }
            else if (magicBookData.Id == 33)
            {
                suhoSinDescription.SetText($"요괴지옥\n강림도령에서 획득!");
            }
            else if (magicBookData.Id == 34)
            {
                suhoSinDescription.SetText($"요괴지옥\n지옥불꽃에서 획득!");
            }
            //
            else if (
                magicBookData.Id == 35 ||
                magicBookData.Id == 36 ||
                magicBookData.Id == 37 ||
                magicBookData.Id == 38 ||
                magicBookData.Id == 39 ||
                magicBookData.Id == 40 ||
                magicBookData.Id == 41
                )
            {
                suhoSinDescription.SetText($"천상계\n칠선녀에서 획득!");
            }
            else if (
                magicBookData.Id == 42 ||
                magicBookData.Id == 43 ||
                magicBookData.Id == 44 ||
                magicBookData.Id == 46 ||
                magicBookData.Id == 47 ||
                magicBookData.Id == 48 ||
                magicBookData.Id == 49

                )
            {
                suhoSinDescription.SetText($"도깨비 나라\n도깨비 보스에서 획득!");
            }
            else if (
                magicBookData.Id == 45
                )
            {
                suhoSinDescription.SetText($"월간 패스\n12월 월간패스 보상에서 획득!");
            }
            else if (
                magicBookData.Id == 50
                )
            {
                suhoSinDescription.SetText($"월간 패스\n1월 월간패스 보상에서 획득!");
            }
            else if (
                magicBookData.Id == 56
                )
            {
                suhoSinDescription.SetText($"월간 패스\n2월 월간패스 보상에서 획득!");
            }

            else if (
               magicBookData.Id == 51 ||
               magicBookData.Id == 52 ||
               magicBookData.Id == 53

               )
            {
                suhoSinDescription.SetText($"도깨비 나라\n도깨비 우두머리에서 획득!");
            }
            else if (
               magicBookData.Id == 54 ||
               magicBookData.Id == 55 ||
               magicBookData.Id == 57 ||
               magicBookData.Id == 58

               )
            {
                suhoSinDescription.SetText($"수미산\n사천왕에서 획득!");
            }

            foxNorigaeGetButton.SetActive(magicBookData.Id == 28);
        }

        if (magicBookData != null)
        {
            norigaeDescription.SetText($"기본무공 강화\n{Utils.ConvertBigNum(magicBookData.Goldabilratio)}배");
        }

        if (weaponData != null)
        {
            norigaeDescription.SetText($"무공비급 강화\n{weaponData.Specialadd}배");
        }
        if (newGachaData != null)
        {
            norigaeDescription.SetText($"영혼의숲 능력치\n{newGachaData.Specialadd}배 상승\n<color=red>(공격 능력치)");
        }

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

            ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].AsObservable().Subscribe(WhenEquipWeapon_ViewChanged).AddTo(this);
        }
        else if (magicBookData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(WhenEquipMagicBookChanged).AddTo(this);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(this);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(this);

            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].AsObservable().Subscribe(WhenEquipMagicBook_ViewChanged).AddTo(this);
        }
        else if (newGachaData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.SoulRing].AsObservable().Subscribe(WhenEquipNewGachaChanged).AddTo(this);
            ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(this);
            ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(this);

        }


        if (weaponData != null)
        {
            ServerData.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(this);
        }
        else if (magicBookData != null)
        {
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(this);
        }
        else if (newGachaData != null)
        {
            ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(this);
        }
    }



    private void WhenItemLevelChanged(int level)
    {
        SetCurrentWeapon();
        UpdateLevelUpUi();
    }

    private void UpdateLevelUpUi()
    {
        if (weaponData == null && magicBookData == null && newGachaData == null) return;

        if ((weaponData != null && ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel) ||
            (magicBookData != null && ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel) ||
            (newGachaData != null && ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].level.Value >= newGachaData.Maxlevel))
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
        else if (magicBookData != null)
        {
            price = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        else
        {
            price = ServerData.newGachaServerTable.GetNewGachaLevelUpPrice(newGachaData.Stringid);
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
            upgradeButton.SetActive(amount >= weaponData.Requireupgrade && weaponData.Id < 19);
        }
        else if (magicBookData != null)
        {
            upgradeButton.SetActive(amount >= magicBookData.Requireupgrade && magicBookData.Id < 15);
        }
        else if (newGachaData != null)
        {
            upgradeButton.SetActive(amount >= newGachaData.Requireupgrade);
        }
    }

    private void WhenHasStageChanged(int state)
    {
        hasMask.SetActive(state == 0);

        SetEquipButton(state == 1);

        levelUpButton.gameObject.SetActive(state == 1);


        if (weaponData != null)
        {
            weaponViewEquipButton.gameObject.SetActive(state == 1);
            magicBookViewEquipButton.gameObject.SetActive(false);

            feelMul2Lock.SetActive(false);
            feelMul3Lock.SetActive(false);
            feelMul4Lock.SetActive(false);
            indraLock.SetActive(false);
            nataLock.SetActive(false);
            orochiLock.SetActive(false);
            feelPaeLock.SetActive(false);
            gumihoWeaponLock.SetActive(false);
            hellWeaponLock.SetActive(false);
            yeoRaeWeaponLock.SetActive(false);
            weaponLockObject.SetActive(false);

            //필멸2 필멸3  (23,24)
            if (weaponData.Id == 23 || weaponData.Id == 24 || weaponData.Id == 25
                || weaponData.Id == 26 || weaponData.Id == 27 || weaponData.Id == 28 || weaponData.Id == 29 || weaponData.Id == 30
                || weaponData.Id == 31 || weaponData.Id == 32 || weaponData.Id == 33 || weaponData.Id == 34 || weaponData.Id == 35
                || weaponData.Id == 36
                || weaponData.Id == 43 || weaponData.Id == 44 || weaponData.Id == 45
                || weaponData.Id == 46 || weaponData.Id == 47 || weaponData.Id == 48 || weaponData.Id == 49 || weaponData.Id == 50
                || weaponData.Id == 51 || weaponData.Id == 52 || weaponData.Id == 53 || weaponData.Id == 54 || weaponData.Id == 55
                || weaponData.Id == 56 || weaponData.Id == 57 || weaponData.Id == 58 || weaponData.Id == 59 || weaponData.Id == 60
                || weaponData.Id == 61 || weaponData.Id == 62 || weaponData.Id == 63 || weaponData.Id == 64 || weaponData.Id == 65
                || weaponData.Id == 66 || weaponData.Id == 67 || weaponData.Id == 68 || weaponData.Id == 69 || weaponData.Id == 70
                || weaponData.Id == 71 || weaponData.Id == 72 || weaponData.Id == 73 || weaponData.Id == 74 || weaponData.Id == 75
                || weaponData.Id == 76 || weaponData.Id == 77 || weaponData.Id == 78 || weaponData.Id == 79 || weaponData.Id == 80
                || weaponData.Id == 81 || weaponData.Id == 82 || weaponData.Id == 83 || weaponData.Id == 84 || weaponData.Id == 85
                || weaponData.Id == 86

                )
            {
                hasMask.SetActive(false);

                if (weaponData.Id == 23)
                {
                    feelMul2Lock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 24)
                {
                    feelMul3Lock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 25)
                {
                    feelMul4Lock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 26)
                {
                    indraLock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 27)
                {
                    nataLock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 28)
                {
                    orochiLock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 29)
                {
                    feelPaeLock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 30)
                {
                    gumihoWeaponLock.gameObject.SetActive(state == 0);
                }
                if (weaponData.Id == 31 || weaponData.Id == 32)
                {
                    hellWeaponLock.gameObject.SetActive(state == 0);
                }

                if (weaponData.Id == 33)
                {
                    yeoRaeWeaponLock.gameObject.SetActive(state == 0);
                }
                if (weaponData.Id == 34)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"요괴지옥\n강림도령에서 획득!");
                }
                if (weaponData.Id == 35)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"요괴지옥\n지옥불꽃에서 획득!");
                }
                if (weaponData.Id == 36)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"지옥 보스전\n일직차사에서 획득!");
                }
                if (weaponData.Id == 43)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"천상계\n신선 개에서 획득!");
                }
                if (weaponData.Id == 44)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"천상계\n신선 고양이에서 획득!");
                }

                if (weaponData.Id == 50)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"천상계\n천둥오리에서 획득!");
                }
                if (weaponData.Id == 51)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"천상계\n근두운에서 획득!");
                }
                if (weaponData.Id == 57)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n혈량에서 획득!");
                }
                if (weaponData.Id == 58)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n뇌량에서 획득!");
                }
                if (weaponData.Id == 59)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n암량에서 획득!");
                }
                if (weaponData.Id == 63)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n화량에서 획득!");
                }
                if (weaponData.Id == 64)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n설량에서 획득!");
                }
                if (weaponData.Id == 65)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n미량에서 획득!");
                }
                if (weaponData.Id == 66)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n흑량에서 획득!");
                }
                //
                if (weaponData.Id == 67)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"사신수\n현무에서 획득!");
                }
                if (weaponData.Id == 68)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"사신수\n백호에서 획득!");
                }
                if (weaponData.Id == 69)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"사신수\n주작에서 획득!");
                }
                if (weaponData.Id == 70)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"사신수\n청룡에서 획득!");
                }
                if (weaponData.Id == 77)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n남량에서 획득!");
                }
                if (weaponData.Id == 78)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n여량에서 획득!");
                }
                if (weaponData.Id == 79)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"도깨비나라\n우량에서 획득!");
                }
                if (weaponData.Id == 80)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"수미산\n지국천왕에서 획득!");
                }
                if (weaponData.Id == 84)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"수미산\n광목천왕에서 획득!");
                }
                if (weaponData.Id == 85)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"수미산\n증장천왕에서 획득!");
                }
                if (weaponData.Id == 86)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"수미산\n다문천왕에서 획득!");
                }

                //
                if (weaponData.Id == 45 || weaponData.Id == 46 || weaponData.Id == 47 || weaponData.Id == 48 || weaponData.Id == 49 ||
                    weaponData.Id == 52 || weaponData.Id == 53 || weaponData.Id == 54 || weaponData.Id == 55 || weaponData.Id == 56 ||
                    weaponData.Id == 60 || weaponData.Id == 61 || weaponData.Id == 62 || weaponData.Id == 71 || weaponData.Id == 72
                    || weaponData.Id == 73 || weaponData.Id == 74 || weaponData.Id == 75 || weaponData.Id == 76 || weaponData.Id == 82 || weaponData.Id == 83)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"십만대산\n추천보상으로 획득!");
                }
                if (weaponData.Id == 81)
                {
                    weaponLockObject.gameObject.SetActive(state == 0);
                    weaponLockDescription.SetText($"떡국 패스\n보상 교환에서 획득!");
                }

            }

        }
        else if (magicBookData != null)
        {
            feelMul2Lock.SetActive(false);
            feelMul3Lock.SetActive(false);
            feelMul4Lock.SetActive(false);
            indraLock.SetActive(false);
            nataLock.SetActive(false);
            orochiLock.SetActive(false);
            feelPaeLock.SetActive(false);
            gumihoWeaponLock.SetActive(false);
            hellWeaponLock.SetActive(false);
            yeoRaeWeaponLock.SetActive(false);
            weaponLockObject.SetActive(false);

            magicBookViewEquipButton.gameObject.SetActive(state == 1);
            weaponViewEquipButton.gameObject.SetActive(false);
        }
        else//스킬데이타 + ring
        {
            levelUpButton.gameObject.SetActive(false);
            feelMul2Lock.SetActive(false);
            feelMul3Lock.SetActive(false);
            feelMul4Lock.SetActive(false);
            indraLock.SetActive(false);
            nataLock.SetActive(false);
            orochiLock.SetActive(false);
            feelPaeLock.SetActive(false);
            gumihoWeaponLock.SetActive(false);
            hellWeaponLock.SetActive(false);
            yeoRaeWeaponLock.SetActive(false);
            weaponLockObject.SetActive(false);

            magicBookViewEquipButton.gameObject.SetActive(false);
            weaponViewEquipButton.gameObject.SetActive(false);
        }


    }
    private void WhenEquipWeaponChanged(int idx)
    {
        equipText.SetActive(idx == this.weaponData.Id);

        UpdateEquipButton();
    }
    private void WhenEquipWeapon_ViewChanged(int idx)
    {
        if (weaponViewEquipDesc != null)
        {
            weaponViewEquipDesc.SetText(idx == this.weaponData.Id ? "적용" : "외형적용");
            weaponViewEquipButton.sprite = (idx == this.weaponData.Id ? weaponViewEquipDisable : weaponViewEquipEnable);
        }
    }

    private void WhenEquipMagicBook_ViewChanged(int idx)
    {
        if (magicBookViewEquipDesc != null)
        {
            magicBookViewEquipDesc.SetText(idx == this.magicBookData.Id ? "적용" : "외형적용");
            magicBookViewEquipButton.sprite = (idx == this.magicBookData.Id ? weaponViewEquipDisable : weaponViewEquipEnable);
        }
    }

    private void WhenEquipMagicBookChanged(int idx)
    {
        equipText.SetActive(idx == this.magicBookData.Id);

        UpdateEquipButton();
    }
    private void WhenEquipNewGachaChanged(int idx)
    {
        equipText.SetActive(idx == this.newGachaData.Id);

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
        else if (magicBookData != null)
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
        else if (newGachaData != null)
        {
            int amount = ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].amount.Value;

            if (amount < newGachaData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.NewGachaData.TryGetValue(newGachaData.Id + 1, out var nextNewGacha))
            {
                int currentWeaponCount = ServerData.newGachaServerTable.GetCurrentNewGachaCount(newGachaData.Stringid);
                int nextWeaponCount = ServerData.newGachaServerTable.GetCurrentNewGachaCount(nextNewGacha.Stringid);

                int upgradeNum = currentWeaponCount / newGachaData.Requireupgrade;

                ServerData.newGachaServerTable.UpData(newGachaData, upgradeNum * newGachaData.Requireupgrade * -1);
                ServerData.newGachaServerTable.UpData(nextNewGacha, upgradeNum);

                ServerData.newGachaServerTable.SyncToServerAll(new List<int>() { newGachaData.Id, nextNewGacha.Id });
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
            else if (magicBookData != null)
            {
                UiEnventoryBoard.Instance.AllUpgradeMagicBook(magicBookData.Id);
            }
            else
            {
                UiEnventoryBoard.Instance.AllUpgradeNewGacha(newGachaData.Id);
            }

        }, null);
    }

    private void SetCurrentWeapon()
    {
        weaponView.Initialize(weaponData, magicBookData, null, newGachaData);

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
        else if (magicBookData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;
        }
        else if (newGachaData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.newGachaData.Effectid];
            weaponLevel = ServerData.newGachaServerTable.TableDatas[this.newGachaData.Stringid].level.Value;
            stringid = this.newGachaData.Stringid;
        }
        else
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;
        }
        string description = string.Empty;

        description += "<color=#ff00ffff>장착 효과</color>\n";

        float equipValue1 = 0f, equipValue1_max = 0f, equipValue2 = 0f, equipValue2_max = 0f;
        float hasValue1 = 0f, hasValue2 = 0f, hasValue3 = 0f, hasValue1_max = 0f, hasValue2_max = 0f, hasValue3_max = 0f;
        if (weaponData != null)
        {
            equipValue1 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.weaponData.Maxlevel);
            equipValue2 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.weaponData.Maxlevel);

            hasValue1 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.weaponData.Maxlevel);
            hasValue2 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.weaponData.Maxlevel);
            hasValue3 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.weaponData.Maxlevel);
        }
        else if (magicBookData != null)
        {
            equipValue1 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.magicBookData.Maxlevel);
            equipValue2 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.magicBookData.Maxlevel);

            hasValue1 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.magicBookData.Maxlevel);
            hasValue2 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.magicBookData.Maxlevel);
            hasValue3 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.magicBookData.Maxlevel);
        }
        else
        {
            equipValue1 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.newGachaData.Maxlevel);
            equipValue2 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.newGachaData.Maxlevel);

            hasValue1 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.newGachaData.Maxlevel);
            hasValue2 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.newGachaData.Maxlevel);
            hasValue3 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.newGachaData.Maxlevel);
        }

        if (effectData.Equipeffecttype1 != -1)
        {
            StatusType type = (StatusType)effectData.Equipeffecttype1;

            if (type.IsPercentStat())
            {
                float value = equipValue1 * 100f;
                float value_max = equipValue1_max * 100f;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
            else
            {
                float value = equipValue1;
                float value_max = equipValue1_max;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }

        }

        if (effectData.Equipeffecttype2 != -1)
        {
            StatusType type = (StatusType)effectData.Equipeffecttype2;

            if (type.IsPercentStat())
            {
                float value = equipValue2 * 100f;
                float value_max = equipValue2_max * 100f;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
            else
            {
                float value = equipValue2;
                float value_max = equipValue2_max;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
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

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
            else
            {
                float value = hasValue1;
                float value_max = hasValue1_max;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
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
            if (weaponData.Id >= 37 && weaponData.Id <= 41)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }
            if (weaponData.Id >= 45 && weaponData.Id <= 49)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }
            if (weaponData.Id >= 52 && weaponData.Id <= 56)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }

            if (weaponData.Id >= 60 && weaponData.Id <= 62)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }

            if (weaponData.Id >= 71 && weaponData.Id <= 76)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }
            if (weaponData.Id >= 81 && weaponData.Id < 84)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }


            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 무기를 변경 할까요?\n(외형도 함께 변경 됩니다.)", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon, weaponData.Id);
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon_View, weaponData.Id);
            }, () => { });
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
        else if (magicBookData != null)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 노리개를 변경 할까요?\n(외형도 함께 변경 됩니다.)", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook, magicBookData.Id);
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook_View, magicBookData.Id);
            }, () => { });

        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 반지를 변경 할까요?", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.SoulRing, newGachaData.Id);
            }, () => { });
        }

        UpdateEquipButton();
    }

    private void UpdateEquipButton()
    {
        int id = this.weaponData != null ? weaponData.Id : this.magicBookData != null ? magicBookData.Id : newGachaData.Id;

        int has = 0;

        if (weaponData != null)
        {
            has = ServerData.weaponTable.GetWeaponData(weaponData.Stringid).hasItem.Value;
        }
        else if (magicBookData != null)
        {
            has = ServerData.magicBookTable.GetMagicBookData(magicBookData.Stringid).hasItem.Value;
        }
        else
        {
            has = ServerData.newGachaServerTable.GetNewGachaData(newGachaData.Stringid).hasItem.Value;
        }

        SetEquipButton(has == 1);
        if (newGachaData == null)
        {
            levelUpButton.gameObject.SetActive(has == 1);
        }

        if (equipButton.gameObject.activeSelf)
        {
            string key = weaponData != null ? EquipmentTable.Weapon : magicBookData != null ? EquipmentTable.MagicBook : EquipmentTable.SoulRing;
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
            if (weaponData.Id >= 37 && weaponData.Id <= 42)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }

            if (weaponData.Id >= 45 && weaponData.Id <= 49)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }
            if (weaponData.Id >= 52 && weaponData.Id <= 56)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }

            if (weaponData.Id >= 60 && weaponData.Id <= 62)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }
            if (weaponData.Id >= 67 && weaponData.Id <= 70)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }

            if (weaponData.Id >= 71 && weaponData.Id <= 76)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }
            if (weaponData.Id >= 81 && weaponData.Id < 84)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }

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
        else if (magicBookData != null)
        {
            if (magicBookData.MAGICBOOKTYPE == MagicBookType.View)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }

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
        else
        {

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

    public void OnClickYoungMulCreateButton2()
    {
        if (magicBookData == null) return;

        UiYoungMulCraftBoard2.Instance.Initialize(magicBookData.Id);
    }

    private void OnEnable()
    {
        SetParent();
    }

    private void SetParent()
    {
        //정렬
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
        if (magicBookData != null)
        {
            if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.Value == 1)
            {
                this.transform.SetAsFirstSibling();
            }
        }
    }

    public void OnClickGetFeelMul2Button()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value < 400000)
        {
            PopupManager.Instance.ShowAlarmMessage("도깨비 대장간 레벨 40만 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon23"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon23"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon23");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!", null);
    }

    public void OnClickGetFeelMul3Button()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < 5000)
        {
            PopupManager.Instance.ShowAlarmMessage("검의 산 처치 5000 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon24"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon24"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon24");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!", null);
    }

    public void OnClickGetFeelMulLastButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < 6000)
        {
            PopupManager.Instance.ShowAlarmMessage("검의 산 처치 6000 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon25"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon25"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon25");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!", null);
    }

    public void OnClickGetFeelMulLastLastButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < 8000)
        {
            PopupManager.Instance.ShowAlarmMessage("검의 산 처치 8000 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon29"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon29"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon29");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "필멸(패) 획득!", null);
    }

    public void OnClickGetGumihoWeaponButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.gumiho0).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho1).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho2).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho3).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho4).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho5).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho6).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho7).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho8).Value == 0
            )
        {
            PopupManager.Instance.ShowAlarmMessage("구미호전 구미호 꼬리 모두 획득시 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon30"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon30"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon30");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우검 획득!", null);
    }

    public void OnClickGetGumihoNorigaeButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.gumiho7).Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("구미호전 구미호 꼬리8 획득시 획득 하실 수 있습니다.");
            return;
        }

        ServerData.magicBookTable.TableDatas["magicBook28"].amount.Value += 1;
        ServerData.magicBookTable.TableDatas["magicBook28"].hasItem.Value = 1;
        ServerData.magicBookTable.SyncToServerEach("magicBook28");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우 노리개 획득!", null);
    }

}
