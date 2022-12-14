using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiYoguiSogulEnterPopup : MonoBehaviour
{
    [SerializeField]
    private UiYoguiSogulRewardCell cellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI lastClearStageDesc;

    private List<UiYoguiSogulRewardCell> cellLists = new List<UiYoguiSogulRewardCell>();

    [SerializeField]
    private TextMeshProUGUI passiveDescription;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.YoguisogulTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            var cell = Instantiate<UiYoguiSogulRewardCell>(cellPrefab, cellParent);

            cell.Initialize(tableDatas[i]);

            cellLists.Add(cell);
        }

        lastClearStageDesc.SetText($"최고 단계 : {(int)(ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value)}");

        passiveDescription.SetText($"{PlayerStats.sogulGab}단계당 {PlayerStats.sogulValuePerGab * 100f}% 상승!");
    }

    public void OnClickEnterButton()
    {
        GameManager.Instance.LoadContents(GameManager.ContentsType.YoguiSoGul);
    }

    public void OnClickAllReceiveButton()
    {

        int lastClearStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value;

        var tableDatas = TableManager.Instance.YoguisogulTable.dataArray;

        var GetYoguiSoguilRewardedList = ServerData.etcServerTable.GetYoguiSoguilRewardedList();

        int rewardReceiveCount = 0;

        string addStringValue = string.Empty;


        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            if (lastClearStageId < tableDatas[i].Stage) break;

            if (GetYoguiSoguilRewardedList.Contains(tableDatas[i].Stage) == true) continue;

            ServerData.AddLocalValue((Item_Type)tableDatas[i].Rewardtype, tableDatas[i].Rewardvalue);

            addStringValue += $"{BossServerTable.rewardSplit}{tableDatas[i].Stage}";

            rewardReceiveCount++;
        }


        if (rewardReceiveCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.yoguiSogulReward, ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLogType("Son", "all", "");
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value = string.Empty;
        }
    }
#endif
}
