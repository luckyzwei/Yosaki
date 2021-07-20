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

    [SerializeField]
    private GameObject tutorialObject;

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

        petName.SetText(petData.Name);

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
        }).AddTo(this);

    }

    private void UpdateUi()
    {
        adIcon.SetActive(petData.PETGETTYPE == PetGetType.Ad);
        gemIcon.SetActive(petData.PETGETTYPE == PetGetType.Gem);

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
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}가 부족합니다.");
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
                PopupManager.Instance.ShowAlarmMessage("상점에서 구매 가능합니다.\n(패키지 상품)");
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
}
