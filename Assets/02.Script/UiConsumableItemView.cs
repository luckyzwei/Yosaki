using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiConsumableItemView : MonoBehaviour
{
    [SerializeField]
    private string goodsId;

    [SerializeField]
    private ObscuredInt price;

    [SerializeField]
    private ObscuredInt amount;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI amountText;

    private List<string> syncDatList = new List<string>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (nameText != null)
        {
            nameText.SetText(goodsId);
        }

        if (priceText != null)
        {
            priceText.SetText($"{price}개");
        }

        if (amountText != null)
        {
            amountText.SetText($"{amount}개");
        }
    }

    public void OnClickBuyButton()
    {
        var currentBlueStone = DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone);

        if (currentBlueStone.Value < price)
        {
            PopupManager.Instance.ShowAlarmMessage($"재화가 부족합니다 {goodsId}");
            return;
        }

        var data = DatabaseManager.goodsTable.GetTableData(goodsId);

        if (data == null)
        {
            PopupManager.Instance.ShowAlarmMessage($"등록되지 않은 아이템 {goodsId}");
            return;
        }

        syncDatList.Clear();
        syncDatList.Add(GoodsTable.BlueStone);
        syncDatList.Add(goodsId);

        //로컬
        data.Value += amount;

        DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value -= price;
        //서버 업데이트

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutineWeapon());

    }

    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);
    private Coroutine syncRoutine;
    private IEnumerator SyncDataRoutineWeapon()
    {
        yield return syncWaitTime;

        DatabaseManager.goodsTable.SyncAllData(syncDatList);

        syncRoutine = null;
    }
}
