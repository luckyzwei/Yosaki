using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Purchasing;
using BackEnd;

public class UiShop : MonoBehaviour
{
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("GoldUse");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(this);
    }

    public void BuyProduct(string id)
    {
#if UNITY_EDITOR
        GetPackageItem(id);
        return;
#endif
        IAPManager.Instance.BuyProduct(id);
    }

    public void GetPackageItem(string productId)
    {
        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {productId}", null);
            return;
        }
        else
        {
            // PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{tableData.Title} 구매 성공!", null);
        }

        //아이템 수령처리
        Param goodsParam = null;
        Param costumeParam = null;
        Param petParam = null;
        Param iapParam = new Param();
        Param magicStoneBuffParam = null;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        for (int i = 0; i < tableData.Rewardtypes.Length; i++)
        {
            Item_Type rewardType = (Item_Type)tableData.Rewardtypes[i];
            int rewardAmount = tableData.Rewardvalues[i];

            if (rewardType.IsGoodsItem())
            {
                AddGoodsParam(ref goodsParam, rewardType, rewardAmount);
            }
            else if (rewardType.IsStatusItem())
            {

            }
            else if (rewardType.IsCostumeItem())
            {
                AddCostumeParam(ref costumeParam, rewardType, rewardAmount);
            }
            else if (rewardType.IsPetItem())
            {
                AddPetParam(ref petParam, rewardType, rewardAmount);
            }
            else if (rewardType == Item_Type.MagicStoneBuff)
            {
                AddMagicStoneBuffParam(ref magicStoneBuffParam);
            }
        }

        //재화
        if (goodsParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }
        //코스튬
        if (costumeParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        //펫
        if (petParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));
        }

        if (magicStoneBuffParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(BuffServerTable.tableName, BuffServerTable.Indate, magicStoneBuffParam));
        }

        if (DatabaseManager.iapServerTable.TableDatas.ContainsKey(tableData.Productid) == false)
        {
            Debug.LogError($"@@@product Id {tableData.Productid}");
            return;
        }
        else
        {
            DatabaseManager.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;
        }

        iapParam.Add(tableData.Productid, DatabaseManager.iapServerTable.TableDatas[tableData.Productid].ConvertToString());

        transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        DatabaseManager.SendTransaction(transactionList, successCallBack: WhenRewardSuccess);

        currentItemIdx = productId;
    }

    private string currentItemIdx;

    private void WhenRewardSuccess()
    {
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "구매 감사합니다!\n아이템이 성공적으로 지급됐습니다.", null);

        IAPManager.Instance.SendLog("상품 구매후 수령 성공", currentItemIdx);
    }

    public void AddGoodsParam(ref Param param, Item_Type type, int amount)
    {
        if (param == null)
        {
            param = new Param();
        }

        switch (type)
        {
            case Item_Type.Jade:
                {
                    DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                    param.Add(GoodsTable.Jade, amount);
                }
                break;
            case Item_Type.GrowThStone:
                {
                    DatabaseManager.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                    param.Add(GoodsTable.GrowthStone, amount);

                }
                break;
            case Item_Type.Ticket:
                {
                    DatabaseManager.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                    param.Add(GoodsTable.Ticket, amount);
                }
                break;
            case Item_Type.Marble:
                {
                    DatabaseManager.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                    param.Add(GoodsTable.MarbleKey, amount);
                }
                break;

        }
    }

    public void AddCostumeParam(ref Param param, Item_Type type, int amount)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();

        if (DatabaseManager.costumeServerTable.TableDatas.TryGetValue(key, out var serverData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"미등록된 아이디 {key}", null);
            return;
        }

        serverData.hasCostume.Value = true;

        param.Add(key, serverData.ConvertToString());
    }

    public void AddPetParam(ref Param param, Item_Type type, int amount)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();

        if (DatabaseManager.petTable.TableDatas.TryGetValue(key, out var serverData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"미등록된 아이디 {key}", null);
            return;
        }

        serverData.hasItem.Value = 1;

        param.Add(key, $"{serverData.idx},{serverData.hasItem.Value},{serverData.level.Value},{serverData.remainSec.Value}");
    }

    public void AddMagicStoneBuffParam(ref Param param)
    {
        if (param == null)
        {
            param = new Param();
        }

        var buffTableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < buffTableData.Length; i++)
        {
            if (buffTableData[i].Bufftype == (int)StatusType.MagicStoneAddPer)
            {
                DatabaseManager.buffServerTable.TableDatas[buffTableData[i].Stringid].remainSec.Value = -1;

                param.Add(buffTableData[i].Stringid, DatabaseManager.buffServerTable.TableDatas[buffTableData[i].Stringid].ConvertToString());
                return;
            }
        }
    }

}
