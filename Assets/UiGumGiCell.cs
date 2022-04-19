using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiGumGiCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI requireDescription;

    [SerializeField]
    private Image weaponIcon;

    [SerializeField]
    private Image weaponIconFrame;

    [SerializeField]
    private Image equip_View_btn;

    [SerializeField]
    private Image equip_btn;

    [SerializeField]
    private Sprite equipedSprite;

    [SerializeField]
    private Sprite unEquipedSprite;

    [SerializeField]
    private TextMeshProUGUI equipViewBtn_Desc;

    [SerializeField]
    private TextMeshProUGUI equipBtn_Desc;

    private GumGiTableData tableData;

    [SerializeField]
    private GameObject lockMask;

    public void Initialize(GumGiTableData tableData)
    {
        this.tableData = tableData;

        weaponIcon.material = CommonUiContainer.Instance.weaponEnhnaceMats[tableData.Id];

        abilDescription.SetText($"{tableData.Id}단계\n{CommonString.GetStatusName((StatusType)tableData.Abiltype)} {Utils.ConvertBigNum(tableData.Abilvalue)}");

        requireDescription.SetText($"{Utils.ConvertBigNum(tableData.Require)}이상");

        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].AsObservable().Subscribe(e =>
        {
            weaponIcon.sprite = CommonResourceContainer.GetWeaponSprite(e);
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponEnhance].AsObservable().Subscribe(e =>
        {
            equip_btn.sprite = e == tableData.Id ? equipedSprite : unEquipedSprite;
            equipBtn_Desc.SetText(e == tableData.Id ? "능력 적용됨" : "능력 장착");

        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].AsObservable().Subscribe(e =>
        {
            equip_View_btn.sprite = e == tableData.Id ? equipedSprite : unEquipedSprite;
            equipViewBtn_Desc.SetText(e == tableData.Id ? "외형 적용됨" : "외형 장착");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(tableData.Require > e);
        }).AddTo(this);
    }

    public void OnClickEquipButton()
    {
        float goods = ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value;
        if (goods < tableData.Require)
        {

            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "검기를 변경 할까요?\n(외형도 함께 변경 됩니다.)", () =>
         {
             ServerData.equipmentTable.ChangeEquip(EquipmentTable.WeaponEnhance, tableData.Id);
             ServerData.equipmentTable.ChangeEquip(EquipmentTable.WeaponE_View, tableData.Id);
         }, null);

    }
    public void OnClickEquipViewButton()
    {
        float goods = ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value;
        if (goods < tableData.Require)
        {

            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "검기 외형을 변경 할까요?", () =>
        {
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.WeaponE_View, tableData.Id);
        }, null);
    }
}
