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

    [SerializeField]
    private TextMeshProUGUI abilList;


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
            float abilValue = tableData[i].Abilvalue;

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
            description += $"{CommonString.GetStatusName(e.Current.Key)} {e.Current.Value * 100f}% 증가\n";
        }

        if (rewards.Count == 0)
        {
            abilDescription.SetText("환수가 없습니다.");
        }
        else
        {
            abilDescription.SetText(description);
        }

        string abils = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            abils += $"보유 {i + 1} : {CommonString.GetStatusName((StatusType)tableData[i].Abiltype)} {tableData[i].Abilvalue * 100f}%\n";
        }

        abils += "<color=red>모든 효과는 중첩됩니다!</color>";


        abilList.SetText(abils);
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

        if (rewards.Count == 0) 
        {
            rewardDescription.SetText("환수가 없습니다.");
        }
        else 
        {
            rewardDescription.SetText(description);
        }

     
    }
}
