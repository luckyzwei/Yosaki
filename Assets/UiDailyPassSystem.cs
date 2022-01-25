using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiDailyPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiDailyPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiDailyPassCell> uiPassCellContainer = new List<UiDailyPassCell>();

    private List<int> splitData_Free;
    private List<int> splitData_Ad;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.DailyPass.dataArray;

        int interval = tableData.Length - uiPassCellContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var prefab = Instantiate<UiDailyPassCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();

                passInfo.require = tableData[i].Unlockamount;
                passInfo.id = tableData[i].Id;

                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = DailyPassServerTable.DailypassFreeReward;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = DailyPassServerTable.DailypassAdReward;

                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }

        // cellParent.transform.localPosition = new Vector3(0f, cellParent.transform.localPosition.y, cellParent.transform.localPosition.z);
    }

    public void OnClickAllReceiveButton()
    {
        splitData_Free = GetSplitData(DailyPassServerTable.DailypassFreeReward);
        splitData_Ad = GetSplitData(DailyPassServerTable.DailypassAdReward);

        var tableData = TableManager.Instance.DailyPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassFreeReward].Value;
        string ad = ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassAdReward].Value;


        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                rewardedNum++;
            }

            //유로보상
            if (HasRemoveAdProduct() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                rewardedNum++;
            }
        }

        if (rewardedNum > 0)
        {
            ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassFreeReward].Value = free;
            ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassAdReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(DailyPassServerTable.DailypassFreeReward, ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassFreeReward].Value);
            passParam.Add(DailyPassServerTable.DailypassAdReward, ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassAdReward].Value);

            transactions.Add(TransactionValue.SetUpdate(DailyPassServerTable.tableName, DailyPassServerTable.Indate, passParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                LogManager.Instance.SendLogType("DailyPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }

    }

    public bool CanGetReward(int require)
    {
        int dailyMobKillCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value;
        return dailyMobKillCount >= require;
    }

    private bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.dailyPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }


    private string removeAdItemName = "removead";

    public bool HasRemoveAdProduct()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[removeAdItemName].buyCount.Value > 0;

        return hasIapProduct;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value += 30000;
        }
    }
#endif
}
