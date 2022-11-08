using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiDokebiFireBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI dokebiLevelText;

    [SerializeField]
    private TextMeshProUGUI dokebiAbilText1;

    public Button registerButton;

    public TextMeshProUGUI getButtonDesc;


    private void Start()
    {
        Initialize();
        Subscribe();
        SetFlowerReward();
    }

    //기능 보류
    private void SetFlowerReward()
    {
        //chunFlowerReward.Initialize(TableManager.Instance.TwelveBossTable.dataArray[65]);
    }
    private void OnEnable()
    {
        UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
    }
    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).AsObservable().Subscribe(level =>
        {
            dokebiLevelText.SetText($"LV : {level}");
            UpdateAbilText1((int)level);

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);
    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.dokebiAbilBase.dataArray;

        string abilDesc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type == StatusType.AttackAddPer)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetDokebiAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {PlayerStats.GetDokebiAbilHasEffect(type) * 100f}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        dokebiAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value)}");
    }

    public void OnClickDokebiEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.DokebiFire);
        }, () => { });
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += 2000;
        }
    }
#endif

    public void OnClickGetDokebiFireButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFire)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getDokebiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score}개 획득!", null);
            });
        }, null);
    }
}
