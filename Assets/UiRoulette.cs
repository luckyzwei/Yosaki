using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using BackEnd;

public class UiRoulette : MonoBehaviour
{
    [SerializeField]
    private Transform rotateObject;

    [SerializeField]
    private float spinSpeed = 100f;

    [SerializeField]
    private List<Image> itemIcons;

    private Coroutine spinRoutine;

    [SerializeField]
    private Toggle toggle;

    private ObscuredBool isAuto;

    private WaitForSeconds stopDelay = new WaitForSeconds(0.4f);

    private List<BonusRouletteData> tableDataShuffled;

    private void Start()
    {
        SetItemIcon();
    }

    private void SetItemIcon()
    {
        tableDataShuffled = TableManager.Instance.BonusRoulette.dataArray.ToList();

        tableDataShuffled.Shuffle();

        for (int i = 0; i < tableDataShuffled.Count; i++)
        {
            itemIcons[i].sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableDataShuffled[i].Itemtype);
        }
    }

    public void WhenToggleChanged(bool isOn)
    {
        isAuto = isOn;

        if (isOn && spinRoutine == null)
        {
            SpinSlot();
        }
    }

    private void ToggleOff()
    {
        toggle.isOn = false;
        WhenToggleChanged(false);
    }

    private void OnDestroy()
    {
        if (spinRoutine != null && CoroutineExecuter.Instance != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(spinRoutine);
        }
    }

    public void SpinSlot()
    {
        int spinNum = (int)DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value;

        if (spinNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.BonusSpinCoin}이 부족합니다");
            ToggleOff();
            return;
        }

        UiTutorialManager.Instance.SetClear(TutorialStep._9_GetBonusReward);

        spinRoutine = CoroutineExecuter.Instance.StartCoroutine(SlotSpinRoutine());
    }

    private int GetRandomIdx()
    {
        return Utils.GetRandomIdx(tableDataShuffled.Select(e => e.Prob).ToList());
    }

    private int GetRewardAmount(int randIdx)
    {
        var tableData = tableDataShuffled[randIdx];

        return Random.Range((int)tableData.Min, (int)tableData.Max);
    }

    private Item_Type GetRewardType(int randIdx)
    {
        var tableData = tableDataShuffled[randIdx];

        return (Item_Type)tableData.Itemtype;
    }

    private static string RoulletSpin = "RoulletSpin";
    private IEnumerator SlotSpinRoutine()
    {
        SoundManager.Instance.PlaySound(RoulletSpin);

        int randIdx = GetRandomIdx();

        //아이템 실적용
        Item_Type rewardType = GetRewardType(randIdx);

        float rewardAmount = GetRewardAmount(randIdx);

        RewardItem(rewardType, rewardAmount);

        float tick = 0f;
        float spinTime = 0.2f;

        while (tick < spinTime)
        {
            tick += Time.deltaTime;
            rotateObject.Rotate(Vector3.forward * spinSpeed);
            yield return null;
        }

        rotateObject.transform.localRotation = Quaternion.Euler(0f, 0f, randIdx * 45f);

        string description = $"{CommonString.GetItemName(rewardType)} {(int)rewardAmount}획득!!";

        PopupManager.Instance.ShowAlarmMessage(description);

        yield return stopDelay;

        spinRoutine = null;

        if (isAuto && this.gameObject.activeInHierarchy == true)
        {
            SpinSlot();
        }
        else
        {
            ToggleOff();
        }

    }

    private void OnEnable()
    {
        spinRoutine = null;
        rotateObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void RewardItem(Item_Type rewardType, float rewardAmount)
    {
        DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value--;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BonusSpinKey, DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        //골드
        if (rewardType == Item_Type.Gold)
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).Value += rewardAmount;
            goodsParam.Add(GoodsTable.Gold, DatabaseManager.goodsTable.GetTableData(GoodsTable.Gold).Value);
        }
        //티켓
        else if (rewardType == Item_Type.Ticket)
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value += rewardAmount;
            goodsParam.Add(GoodsTable.Ticket, DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        }
        //매직스톤
        else if (rewardType == Item_Type.MagicStone)
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.MagicStone).Value += rewardAmount;
            goodsParam.Add(GoodsTable.MagicStone, DatabaseManager.goodsTable.GetTableData(GoodsTable.MagicStone).Value);
        }
        //보석
        else if (rewardType == Item_Type.BlueStone)
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value += rewardAmount;
            goodsParam.Add(GoodsTable.BlueStone, DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value);
        }

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        DatabaseManager.SendTransaction(transactionList);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value += 10;
        }
    }
#endif
}
