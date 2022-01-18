using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BackEnd;
using TMPro;

public class YachaPetAwakeView : MonoBehaviour
{
    [SerializeField]
    private GameObject awakeButton;

    [SerializeField]
    private GameObject levelUpObjects;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private void Start()
    {
        Subscribe();

        Intialize();
    }

    private void Intialize()
    {
        priceText.SetText(Utils.ConvertBigNum(GameBalance.AwakePetUpgradePrice));
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.petAwake].AsObservable().Subscribe(e =>
        {
            awakeButton.SetActive(e == 0);
            levelUpObjects.SetActive(e == 1);
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).AsObservable().Subscribe(level =>
        {
            levelText.SetText($"LV : {level}");
            levelDescription.SetText($"환수 {1f + level * GameBalance.PetAwakeValuePerLevel}배 강해짐");
        }).AddTo(this);
    }

    public void OnClickAwakeButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.petAwake].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 각성 됐습니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.MonkeyStone).Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"십이지신(신) 최종 보상 {CommonString.GetItemName(Item_Type.MonkeyStone)}이 필요합니다.");
            return;
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.petAwake].Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(UserInfoTable.petAwake, ServerData.userInfoTable.TableDatas[UserInfoTable.petAwake].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "환수 지배 습득!!", null);
        });
    }

    public void OnClickLevelUpButton()
    {
        float currentGrowthStoneAmount = ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value;

        if (currentGrowthStoneAmount < GameBalance.AwakePetUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GrowthStone)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= GameBalance.AwakePetUpgradePrice;
        ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value += 1;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
    private Coroutine syncRoutine;
    private WaitForSeconds delay = new WaitForSeconds(0.3f);
    private IEnumerator SyncRoutine()
    {
        yield return delay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.PetAwakeLevel, ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value);


        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
           //   LogManager.Instance.SendLogType("PetAwake", "S", ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value.ToString());
          });
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += 1000000000f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value++;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value--;
        }
    }
#endif
}

