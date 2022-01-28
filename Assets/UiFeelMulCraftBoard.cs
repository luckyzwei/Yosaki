using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;

public class UiFeelMulCraftBoard : SingletonMono<UiFeelMulCraftBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI createDescription;

    private WeaponData feelMulData;

    [SerializeField]
    private WeaponView feelMulView;

    [SerializeField]
    private TextMeshProUGUI itemAmount;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.PigStone).AsObservable().Subscribe(e =>
        {
            itemAmount.SetText($"({e}/{1})");
        }).AddTo(this);
    }
    private void Initialize()
    {
        feelMulData = TableManager.Instance.WeaponTable.dataArray[22];

        feelMulView.Initialize(feelMulData, null);

        createDescription.SetText($"12지신(해) 최종 보상 {CommonString.GetItemName(Item_Type.PigStone)}이 필요합니다.");
    }

    public void ShowPopup()
    {
        rootObject.SetActive(true);
    }

    public void OnClickCraftButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.PigStone).Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"12지신(해) 최종 보상 {CommonString.GetItemName(Item_Type.PigStone)}이 필요합니다.");
            return;
        }

        var feelMulServerData = ServerData.weaponTable.TableDatas[feelMulData.Stringid];

        if (feelMulServerData.hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중 입니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.weaponTable.TableDatas[feelMulData.Stringid].amount.Value += 1;
        ServerData.weaponTable.TableDatas[feelMulData.Stringid].hasItem.Value = 1;

        Param weaponParam = new Param();
        weaponParam.Add(feelMulData.Stringid, ServerData.weaponTable.TableDatas[feelMulData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "필멸 획득!", null);
            //  LogManager.Instance.SendLogType("야차제작", "make", "");
        });
    }
}
