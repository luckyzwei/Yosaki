using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;

public class UiWeaponCraftBoard : SingletonMono<UiWeaponCraftBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI weaponAmount;
    [SerializeField]
    private TextMeshProUGUI weaponStoneAmount;

    [SerializeField]
    private WeaponView yomulView;

    [SerializeField]
    private WeaponView legend1View;

    private WeaponData yomulData;

    private WeaponData legendWeaponData;

    public void ShowPopup()
    {
        rootObject.SetActive(true);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        yomulData = TableManager.Instance.WeaponTable.dataArray[20];
        yomulView.Initialize(yomulData, null);

        legendWeaponData = TableManager.Instance.WeaponTable.dataArray[19];
        legend1View.Initialize(legendWeaponData, null);

        ServerData.weaponTable.TableDatas[legendWeaponData.Stringid].amount.AsObservable().Subscribe(e =>
        {
            weaponAmount.SetText($"{e}/{1}");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).AsObservable().Subscribe(e =>
        {
            weaponStoneAmount.SetText($"{e}/{1}");
        }
        ).AddTo(this);

    }

    public void OnClickCraftButton()
    {
        int legendWeaponAmount = ServerData.weaponTable.TableDatas[legendWeaponData.Stringid].amount.Value;
        int upgradeStoneAmount = (int)ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).Value;

        if (legendWeaponAmount < 1 || upgradeStoneAmount < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("재료가 부족 합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.weaponTable.TableDatas[legendWeaponData.Stringid].amount.Value -= 1;
        ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).Value -= 1;

        ServerData.weaponTable.TableDatas[yomulData.Stringid].amount.Value += 1;
        ServerData.weaponTable.TableDatas[yomulData.Stringid].hasItem.Value = 1;

        Param weaponParam = new Param();
        weaponParam.Add(yomulData.Stringid, ServerData.weaponTable.TableDatas[yomulData.Stringid].ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.WeaponUpgradeStone, ServerData.goodsTable.GetTableData(GoodsTable.WeaponUpgradeStone).Value);

        transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              SoundManager.Instance.PlaySound("Reward");
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "누가 나를 깨웠지?", null);
              LogManager.Instance.SendLog("요물제작", "요물제작 성공");
          });
    }
}
