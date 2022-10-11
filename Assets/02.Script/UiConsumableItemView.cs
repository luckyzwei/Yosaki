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
        var currentBlueStone = ServerData.goodsTable.GetTableData(GoodsTable.Jade);

        if (currentBlueStone.Value < price)
        {
            PopupManager.Instance.ShowAlarmMessage($"옥이 부족합니다.");
            return;
        }

        var data = ServerData.goodsTable.GetTableData(goodsId);

        if (data == null)
        {
            PopupManager.Instance.ShowAlarmMessage($"등록되지 않은 아이템 {goodsId}");
            return;
        }

        //로컬
        data.Value += amount;

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= price;
        //서버 업데이트

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        if (goodsId.Equals("Potion_1") || goodsId.Equals("Potion_2"))
        {
            UiTutorialManager.Instance.SetClear(TutorialStep.SetPotion);
        }

    }

    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);
    private Coroutine syncRoutine;
    private IEnumerator SyncDataRoutineWeapon()
    {
        yield return syncWaitTime;

        syncRoutine = null;
    }
}
