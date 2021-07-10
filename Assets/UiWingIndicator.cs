using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;

public class UiWingIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI featherCount;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private UiWingView currentWingView;

    [SerializeField]
    private UiWingView nextWingView;

    private ObscuredInt currentGrade;

    [SerializeField]
    private GameObject arrowObject;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).AsObservable().Subscribe(WhenFeatherCountChanged).AddTo(this);

        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(WhenWingGradeChanged).AddTo(this);
    }

    private void WhenWingGradeChanged(float grade)
    {
        int idx = (int)grade;
        currentGrade = idx;
        currentWingView.Initialize(idx);
        nextWingView.Initialize(idx + 1);

        arrowObject.SetActive((currentGrade != -1) && idx + 1 != TableManager.Instance.WingTable.dataArray.Length);

        if (currentGrade + 1 < TableManager.Instance.WingTable.dataArray.Length)
        {
            var nextTableData = TableManager.Instance.WingTable.dataArray[currentGrade + 1];
            priceText.SetText($"승급:{Utils.ConvertBigNum(nextTableData.Requirejump)}개");
        }
        else
        {
            priceText.SetText("최고단계");
        }
    }

    private void OnEnable()
    {
        WhenFeatherCountChanged(DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
    }

    private void WhenFeatherCountChanged(float featherCount)
    {
        if (this.gameObject.activeInHierarchy == false) return;

        this.featherCount.SetText(Utils.ConvertBigNum(featherCount));
    }

    public void OnClickUpgradeButton()
    {
        if (currentGrade + 1 >= TableManager.Instance.WingTable.dataArray.Length)
        {            
            PopupManager.Instance.ShowAlarmMessage("최고단계 입니다.");
            return;
        }

        var nextTableData = TableManager.Instance.WingTable.dataArray[currentGrade + 1];

        if (DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value < nextTableData.Requirejump)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactionList = new List<TransactionValue>();

        DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= nextTableData.Requirejump;

        Param featherParam = new Param();
        featherParam.Add(GoodsTable.MarbleKey, DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, featherParam));

        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value += 1f;

        Param gradeParam = new Param();
        gradeParam.Add(UserInfoTable.marbleAwake, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gradeParam));

        SoundManager.Instance.PlaySound("GoldUse");

        DatabaseManager.SendTransaction(transactionList, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage($"{nextTableData.Id + 1}단계 날계 획득!!");
          });
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += 100000;
        }
    }
#endif
}
