using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UiDailyMissionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI gaugeText;

    [SerializeField]
    private Button getButton;

    [SerializeField]
    private TextMeshProUGUI rewardNum;

    private DailyMissionData tableData;

    private int getAmountFactor;
    public void Initialize(DailyMissionData tableData)
    {
        if (tableData.Enable == false) 
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.tableData = tableData;

        title.SetText(tableData.Title);

        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.dailyMissionTable.TableDatas[tableData.Stringid].AsObservable().Subscribe(WhenMissionAccountChanged).AddTo(this);
    }

    private void OnEnable()
    {
        if (tableData != null)
        {
            WhenMissionAccountChanged(DatabaseManager.dailyMissionTable.TableDatas[tableData.Stringid].Value);
        }
    }

    private void WhenMissionAccountChanged(int account)
    {
        if (this.gameObject.activeInHierarchy == false) return;

        gaugeText.SetText($"{account}/{tableData.Rewardrequire}");

        getButton.interactable = account >= tableData.Rewardrequire;

        getAmountFactor = account / tableData.Rewardrequire;

        rewardNum.SetText($"{getAmountFactor * tableData.Rewardvalue }개");

        if (getButton.interactable)
        {
            this.transform.SetAsFirstSibling();
        }
    }

    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);

    public void OnClickGetButton()
    {
        int amountFactor = getAmountFactor;
        int rewardGemNum = tableData.Rewardvalue * amountFactor;
        //로컬 갱신
        DailyMissionManager.UpdateDailyMission((DailyMissionKey)(tableData.Id), -tableData.Rewardrequire * amountFactor);
        DatabaseManager.goodsTable.AddLocalData(GoodsTable.Jade, rewardGemNum);

        PopupManager.Instance.ShowAlarmMessage($"보석 {rewardGemNum}개 획득!!");
        SoundManager.Instance.PlaySound("GoldUse");

        if (SyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine);
        }

        SyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutine());

        this.transform.SetAsLastSibling();

       // UiTutorialManager.Instance.SetClear(TutorialStep._7_MissionReward);
    }

    private IEnumerator SyncDataRoutine()
    {
        yield return syncWaitTime;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param dailyMissionParam = new Param();
        Param goodsParam = new Param();

        //미션 카운트 차감
        dailyMissionParam.Add(tableData.Stringid, DatabaseManager.dailyMissionTable.TableDatas[tableData.Stringid].Value);
        transactionList.Add(TransactionValue.SetUpdate(DailyMissionTable.tableName, DailyMissionTable.Indate, dailyMissionParam));

        //재화 추가
        goodsParam.Add(GoodsTable.Jade, DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        DatabaseManager.SendTransaction(transactionList);

        SyncRoutine = null;
    }
}
