using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UiGuideMissionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI gaugeText;

    [SerializeField]
    private Button getButton;

    [SerializeField]
    private TextMeshProUGUI rewardNum;

    private GuideMissionData tableData;


    private int getAmountFactor;
    public void Initialize(GuideMissionData tableData)
    {
        if (tableData.Enable == false)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.tableData = tableData;

        title.SetText(tableData.Title);

        WhenMissionCountChanged();

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.GuideMissionReward].AsObservable().Subscribe(e =>
        {
            bool rewarded = ServerData.etcServerTable.GuideMissionRewarded(tableData.Id);

            this.gameObject.SetActive(!rewarded && PreviousGuideMissionCheck());

        }).AddTo(this);
    }
    private bool PreviousGuideMissionCheck()
    {
        if (tableData.Id != 0)
        {
            if(ServerData.etcServerTable.GuideMissionRewarded(tableData.Id -1))
            {
                return true;
            }
        }
        else
        {
            return !ServerData.etcServerTable.GuideMissionRewarded(tableData.Id);
        }
        return false;
    }
    private void OnEnable()
    {
        if (tableData != null)
        {
            WhenMissionCountChanged();
        }

    }

    private void WhenMissionCountChanged()
    {
        if (this.gameObject.activeInHierarchy == false) return;

        int count = 0;
        if (IsClearMission())
        {
            count = 1;
        }
        else
        {
            count = 0;
        }

        gaugeText.SetText($"{count}/{tableData.Rewardrequire}");

        getButton.interactable = count >= tableData.Rewardrequire;

        rewardNum.SetText($"{Utils.ConvertBigNum((double)tableData.Rewardvalue)}개");
    }

    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);

    public bool AlreadyReceiveReward()
    {
        return ServerData.etcServerTable.GuideMissionRewarded(tableData.Id);
    }
    public bool IsClearMission()
    {
        return ServerData.etcServerTable.GuideMissionCleared(tableData.Id);
    }

    public void OnClickGetButton()
    {
        if(!IsClearMission())
        {
            PopupManager.Instance.ShowAlarmMessage("해당 미션을 클리어해야 합니다.");
            return;
        }
        if(AlreadyReceiveReward())
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        int rewardGemNum = tableData.Rewardvalue;

        getButton.interactable = false;



        List<TransactionValue> transactions = new List<TransactionValue>();

        Param rewardParam = new Param();

        ServerData.etcServerTable.TableDatas[EtcServerTable.GuideMissionReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

        rewardParam.Add(EtcServerTable.GuideMissionReward, ServerData.etcServerTable.TableDatas[EtcServerTable.GuideMissionReward].Value);


        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

        Item_Type type = (Item_Type)tableData.Rewardtype;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)rewardGemNum));

        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(type)} {rewardGemNum}개 획득!!");
        SoundManager.Instance.PlaySound("GoldUse");

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
        });

        
    }

    private IEnumerator SyncDataRoutine()
    {
        yield return syncWaitTime;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param eventMissionParam = new Param();
        Param goodsParam = new Param();

        //미션 카운트 차감
        eventMissionParam.Add(tableData.Stringid, ServerData.eventMissionTable.TableDatas[tableData.Stringid].ConvertToString());
        transactionList.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));

        //재화 추가
        goodsParam.Add(GoodsTable.Event_NewYear, ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear).Value);
        if (ServerData.iapServerTable.TableDatas[UiNewYearPassBuyButton.productKey].buyCount.Value == 0)
        {
            goodsParam.Add(GoodsTable.Event_NewYear_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear_All).Value);
        }
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactionList);

        SyncRoutine = null;
    }
}
