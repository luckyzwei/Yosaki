using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using BackEnd;

public class UiRoulette : MonoBehaviour
{
    private Coroutine spinRoutine;

    [SerializeField]
    private Toggle toggle;

    private ObscuredBool isAuto;

    private WaitForSeconds stopDelay = new WaitForSeconds(0.4f);

    private List<BonusRouletteData> tableDataShuffled;

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        SetItemIcon();
    }

    private void SetItemIcon()
    {
        tableDataShuffled = TableManager.Instance.BonusRoulette.dataArray.ToList();

        tableDataShuffled.Shuffle();
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
        int spinNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value;

        if (spinNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.BonusSpinCoin}이 부족합니다");
            ToggleOff();
            return;
        }

      //  UiTutorialManager.Instance.SetClear(TutorialStep._9_GetBonusReward);

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
        float spinTime = 0.15f;

        animator.SetTrigger("Play");

        while (tick < spinTime)
        {
            tick += Time.deltaTime;
            yield return null;
        }

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
    }

    private void RewardItem(Item_Type rewardType, float rewardAmount)
    {
        ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value--;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BonusSpinKey, ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        //골드
        if (rewardType == Item_Type.Gold)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value += rewardAmount;
            goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
        }
        //티켓
        else if (rewardType == Item_Type.Ticket)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += rewardAmount;
            goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
        }
        //매직스톤
        else if (rewardType == Item_Type.GrowThStone)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += rewardAmount;
            goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        }
        //보석
        else if (rewardType == Item_Type.Jade)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardAmount;
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        }

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactionList);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value += 10;
        }
    }
#endif
}
