using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPetHomeBoard : MonoBehaviour
{
    [SerializeField]
    private UiPetHomeView petHomeViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;

    [SerializeField]
    private TextMeshProUGUI petHasCount;

    [SerializeField]
    private Button getPetHomeButton;

    [SerializeField]
    private TextMeshProUGUI rewardButtonDescription;
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.getPetHome].AsObservable().Subscribe(e =>
        {

            getPetHomeButton.interactable = e == 0;

            rewardButtonDescription.SetText(e == 0 ? "보상 받기" : "오늘 받음");

        }).AddTo(this);
    }

    public void OnClickGetPetHomeRewardButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getPetHome].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("보상은 하루에 한번 받으실 수 있습니다.");
            return;
        }

        var tableData = TableManager.Instance.petHome.dataArray;

        int petCount = PlayerStats.GetPetHomeHasCount();

        int rewardCount = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (petCount <= i) break;

            ServerData.AddLocalValue((Item_Type)tableData[i].Rewardtype, tableData[i].Rewardvalue);

            rewardCount++;
        }

        if (rewardCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("보상이 없습니다");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
        goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
        goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
        goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
        goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);

        ServerData.userInfoTable.TableDatas[UserInfoTable.getPetHome].Value = 1;
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.getPetHome, ServerData.userInfoTable.TableDatas[UserInfoTable.getPetHome].Value);


        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 전부 수령했습니다");
        });

    }

    private void OnEnable()
    {
        UpdateDescription();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.PetTable.dataArray;

        for (int i = 8; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiPetHomeView>(petHomeViewPrefab, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

    private void UpdateDescription()
    {
        SetAbilText();

        SetRewardText();

        petHasCount.SetText($"환수 보유 {PlayerStats.GetPetHomeHasCount()}");
    }

    private void SetAbilText()
    {
        int petHomeHasCount = PlayerStats.GetPetHomeHasCount();

        var tableData = TableManager.Instance.petHome.dataArray;

        Dictionary<StatusType, float> rewards = new Dictionary<StatusType, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (petHomeHasCount <= i) break;

            StatusType abilType = (StatusType)tableData[i].Abiltype;
            float abilValue = tableData[i].Rewardvalue;

            if (rewards.ContainsKey(abilType) == false)
            {
                rewards.Add(abilType, 0f);
            }

            rewards[abilType] += abilValue;
        }

        var e = rewards.GetEnumerator();

        string description = "";

        while (e.MoveNext())
        {
            description += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)}% 증가\n";
        }

        abilDescription.SetText(description);
    }

    private void SetRewardText()
    {
        int petHomeHasCount = PlayerStats.GetPetHomeHasCount();

        var tableData = TableManager.Instance.petHome.dataArray;

        Dictionary<Item_Type, float> rewards = new Dictionary<Item_Type, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (petHomeHasCount <= i) break;

            Item_Type rewardType = (Item_Type)tableData[i].Rewardtype;
            float rewardValue = tableData[i].Rewardvalue;

            if (rewards.ContainsKey(rewardType) == false)
            {
                rewards.Add(rewardType, 0f);
            }

            rewards[rewardType] += rewardValue;
        }

        var e = rewards.GetEnumerator();

        string description = "";

        while (e.MoveNext())
        {
            description += $"{CommonString.GetItemName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)}개\n";
        }

        rewardDescription.SetText(description);
    }
}
