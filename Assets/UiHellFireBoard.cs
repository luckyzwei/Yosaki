using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiHellFireBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private UiHellRewardCell cellPrefab;

    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private TextMeshProUGUI sonLevelText;

    [SerializeField]
    private TextMeshProUGUI sonAbilText1;

    private List<UiHellRewardCell> rewardCells = new List<UiHellRewardCell>();

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Hel).AsObservable().Subscribe(level =>
        {
            sonLevelText.SetText($"LV : {level}");
            UpdateAbilText1((int)level);

        }).AddTo(this);


        ServerData.goodsTable.GetTableData(GoodsTable.HellPowerUp).AsObservable().Subscribe(level =>
        {
            UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
        }).AddTo(this);

    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.hellAbilBase.dataArray;

        string abilDesc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type == StatusType.AttackAddPer)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetHellAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {PlayerStats.GetHellAbilHasEffect(type) * 100f}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        sonAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        var tableData = TableManager.Instance.hellReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiHellRewardCell>(cellPrefab, cellParents);

            cell.Initialize(tableData[i]);

            rewardCells.Add(cell);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Hell);
        }, () => { });
    }

    public void OnClickAllReceiveButton()
    {
        bool hasreward = false;
        for (int i = 0; i < rewardCells.Count; i++)
        {
            hasreward |= rewardCells[i].OnClickGetButtonByScript();
        }

        if (hasreward)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.hellReward, ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                //  LogManager.Instance.SendLogType("Son", "all", "");
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
            ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += 2000;
        }
    }
#endif
}
