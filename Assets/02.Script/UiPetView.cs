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
    Ad, Gem, Shop
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

    private void SetPetSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];
        skeletonGraphic.startingAnimation = "Walk";
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }


    private bool initialized = false;

    public void Initialize(PetTableData petData)
    {
        this.petData = petData;
        this.petServerData = DatabaseManager.petTable.TableDatas[petData.Stringid];

        SetPetSpine(petData.Id);

        petName.SetText(petData.Name);

        UpdateUi();

        if (initialized == false)
        {
            initialized = true;
            Subscribe();
        }

        SetAbilityText();
    }

    private void SetAbilityText()
    {
        string abilityStr = string.Empty;

        if (petData.Hasvalue1 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype1;
            bool isPercent = statusType.IsPercentStat();
            abilityStr += $"{CommonString.GetStatusName(statusType)} : {petData.Hasvalue1 * (isPercent ? 100 : 1f)}\n";
        }

        if (petData.Hasvalue2 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype2;
            bool isPercent = statusType.IsPercentStat();
            abilityStr += $"{CommonString.GetStatusName(statusType)} : {petData.Hasvalue2 * (isPercent ? 100 : 1f)}";
        }


        if (abilityStr.Equals(string.Empty))
        {
            abilityStr = "효과 없음";
        }

        description2.SetText(abilityStr);
    }

    private void Subscribe()
    {
        //무료펫
        if (petData.Id == 0)
        {
            DatabaseManager.petTable.TableDatas[petData.Stringid].remainSec.AsObservable().Subscribe(e =>
            {
                if (e == 0)
                {
                    UpdateUi();
                }
            }).AddTo(this);
        }

        DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(this);

        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            if (e == 1)
            {
                UpdateUi();
            }
        }).AddTo(this);

    }

    private void UpdateUi()
    {
        adIcon.SetActive(petData.PETGETTYPE == PetGetType.Ad);
        gemIcon.SetActive(petData.PETGETTYPE == PetGetType.Gem);

        button.sprite = normalButtonSprite;

        //장착중
        if (DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value == petData.Id)
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
                buttonDescription.SetText($"{Utils.ConvertBigNum(petData.Price)}");
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

    }

    public void OnClickGetButton()
    {
        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            //보유
            if (petServerData.hasItem.Value == 1 && petServerData.remainSec.Value > 0)
            {
                DatabaseManager.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
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
                DatabaseManager.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                int currentBlueStone = (int)DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value;

                if (currentBlueStone >= petData.Price)
                {
                    BuyPetRoutine();
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}가 부족합니다.");
                }
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Shop)
        {
            if (petServerData.hasItem.Value == 1)
            {
                DatabaseManager.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                PopupManager.Instance.ShowAlarmMessage("상점에서 구매 가능합니다.\n(패키지 상품)");
            }
        }

        UpdateUi();
    }

    private void BuyPetRoutine()
    {
        bool isTutorialStep = (TutorialStep)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value == TutorialStep._8_GetPet;
        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            if (isTutorialStep == false)
            {
                AdManager.Instance.ShowRewardedReward(BuyFreePet);
            }
            else
            {
                BuyFreePet();
            }

        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            DatabaseManager.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;
            DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value -= petData.Price;
            //
            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param petParam = new Param();
            petParam.Add(petData.Stringid, DatabaseManager.petTable.TableDatas[petData.Stringid].ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            DatabaseManager.SendTransaction(transactionList, successCallBack: UpdateUi);
            DatabaseManager.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
        }

    }

    public void BuyFreePet()
    {
        var localTableData = TableManager.Instance.PetDatas[petData.Id];
        DatabaseManager.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;
        DatabaseManager.petTable.TableDatas[petData.Stringid].remainSec.Value = (int)petData.Time;
        DatabaseManager.petTable.UpdateData(petData.Stringid);
        DatabaseManager.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
        PlayerPet.Instance.WhenPetEquipIdxChanged(0);
        UpdateUi();

        UiTutorialManager.Instance.SetClear(TutorialStep._8_GetPet);
        UiManagerDescription.Instance.SetManagerDescription(ManagerDescriptionType.blackDragon);
    }
}
