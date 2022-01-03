using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiSonBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private UiSonRewardCell cellPrefab;

    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private TextMeshProUGUI sonLevelText;

    [SerializeField]
    private TextMeshProUGUI sonAbilText1;

    [SerializeField]
    private TextMeshProUGUI upgradePriceText;

    [SerializeField]
    private Image sonCharacterIcon;

    private List<UiSonRewardCell> rewardCells = new List<UiSonRewardCell>();

    [SerializeField]
    private GameObject sonSkillBoard;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Son_Level).AsObservable().Subscribe(level =>
        {
            sonLevelText.SetText($"LV : {level}");
            UpdateAbilText1(level);

            sonCharacterIcon.sprite = CommonUiContainer.Instance.sonThumbNail[GameBalance.GetSonIdx()];
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Peach).AsObservable().Subscribe(amount =>
        {
            upgradePriceText.SetText($"+{amount}");

        }).AddTo(this);
    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.SonAbil.dataArray;

        string abilDesc = "보유 효과\n\n";

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentLevel < tableData[i].Unlocklevel) continue;

            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type.IsPercentStat() == false)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetSonAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetSonAbilHasEffect(type) * 100f)}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        sonAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        var tableData = TableManager.Instance.SonReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiSonRewardCell>(cellPrefab, cellParents);

            cell.Initialize(tableData[i]);

            rewardCells.Add(cell);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Son);
        }, () => { });
    }

    public void OnClickLevelUpButton()
    {
        float goodsNum = ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value;

        if (goodsNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PeachReal)}가 없습니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value -= goodsNum;
        ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value += (int)goodsNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private int GetUpgradePrice()
    {
        return 1;
    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);
    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.Son_Level, ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              LogManager.Instance.SendLogType("Son", "Level", ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value.ToString());
          });
    }

    public void OnClickAllReceiveButton()
    {
        for (int i = 0; i < rewardCells.Count; i++)
        {
            rewardCells[i].OnClickGetButtonByScript();
        }
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += 2000;
        }
    }
#endif

    public void OnClickSkillButton()
    {
        sonSkillBoard.SetActive(true);
    }
}
