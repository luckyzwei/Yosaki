using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UiNewAttendSystem : MonoBehaviour
{
    [SerializeField]
    private UiNewAttendCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiNewAttendCell> uiPassCellContainer = new List<UiNewAttendCell>();

    private ObscuredString passShopId;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value += 1;
        }
    }
#endif

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.AttendanceReward.dataArray;

        for (int i = 0; i < 30; i++)
        {
            var prefab = Instantiate<UiNewAttendCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();
                int adjustCount = 0;
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value > 20)
                {
                    adjustCount += (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value - 10;
                }
                if(i + adjustCount + 1>=tableData.Length)
                {
                    uiPassCellContainer[i].gameObject.SetActive(false);
                    continue;
                }
                passInfo.require = tableData[i + adjustCount + 1].Id;
                passInfo.id = tableData[i + adjustCount + 1].Id;

                passInfo.rewardType_Free = tableData[i + adjustCount + 1].Reward_Type;
                passInfo.rewardTypeValue_Free = tableData[i + adjustCount + 1].Reward_Value;
                passInfo.rewardType_Free_Key = AttendanceServerTable.attendFree;

                passInfo.rewardType_IAP = tableData[i + adjustCount + 1].Reward_Type1;
                passInfo.rewardTypeValue_IAP = tableData[i + adjustCount + 1].Reward_Value1;
                passInfo.rewardType_IAP_Key = AttendanceServerTable.attendAd;

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
        string freeKey = AttendanceServerTable.attendFree;
        string adKey = AttendanceServerTable.attendAd;

        List<int> splitData_Free = GetSplitData(AttendanceServerTable.attendFree);
        List<int> splitData_Ad = GetSplitData(AttendanceServerTable.attendAd);

        var tableData = TableManager.Instance.AttendanceReward.dataArray;

        int rewardedNum = 0;

        string free = ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value;
        string ad = ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value;

        bool hasCostumeItem = false;

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Id);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward_Type)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward_Type, tableData[i].Reward_Value);
                rewardedNum++;
            }

            //유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward_Type1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward_Type1, tableData[i].Reward_Value1);
                rewardedNum++;
            }
        }

        if (hasCostumeItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            return;
        }

        if (rewardedNum > 0)
        {
            ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value = free;
            ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
            goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
            goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
            goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
            goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
            goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(AttendanceServerTable.attendFree, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value);
            passParam.Add(AttendanceServerTable.attendAd, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value);

            transactions.Add(TransactionValue.SetUpdate(AttendanceServerTable.tableName, AttendanceServerTable.Indate, passParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }


    private bool CanGetReward(int require)
    {
        int killCountTotal = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value;
        return killCountTotal >= require;
    }
    public bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiNewAttendBuyButton.productKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.attendanceServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }

    public void RefreshCells()
    {
        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            uiPassCellContainer[i].RefreshParent();
        }
    }
}
