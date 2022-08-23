using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Purchasing;
using BackEnd;

public enum SellWhere
{
    Shop, StagePass
}

public class UiShop : SingletonMono<UiShop>
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
        IAPManager.Instance.BuyProduct(id);
    }

    public void GetPackageItem(string productId)
    {
        if (productId.Equals("removeadios"))
        {
            productId = "removead";
        }

        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {productId}", null);
            return;
        }
        else
        {
            // PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{tableData.Title} 구매 성공!", null);
        }

        if (tableData.SELLWHERE != SellWhere.Shop) return;
        if (tableData.BUYTYPE == BuyType.Pension) return;

        //아이템 수령처리
        Param goodsParam = null;
        Param costumeParam = null;
        Param petParam = null;
        Param iapParam = new Param();
        Param iapTotalParam = new Param();
        Param magicStoneBuffParam = null;
        Param weaponParam = null;
        Param norigaeParam = null;
        Param skillParam = null;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        string logString = string.Empty;

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
            else if (rewardType.IsWeaponItem())
            {
                AddWeaponParam(ref weaponParam, rewardType);
            }
            else if (rewardType.IsNorigaeItem())
            {
                AddNorigaeParam(ref norigaeParam, rewardType);
            }
            else if (rewardType.IsSkillItem())
            {
                AddSkillParam(ref skillParam, rewardType);
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

        if (weaponParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));
        }

        if (norigaeParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, norigaeParam));
        }

        if (skillParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));
        }

        if (ServerData.iapServerTable.TableDatas.ContainsKey(tableData.Productid) == false)
        {
            Debug.LogError($"@@@product Id {tableData.Productid}");
            return;
        }
        else
        {
            ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;
            ServerData.iAPServerTableTotal.TableDatas[tableData.Productid].buyCount.Value++;
        }

        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());

        iapTotalParam.Add(tableData.Productid, ServerData.iAPServerTableTotal.TableDatas[tableData.Productid].ConvertToString());

        transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        transactionList.Add(TransactionValue.SetUpdate(IAPServerTableTotal.tableName, IAPServerTableTotal.Indate, iapTotalParam));

        ServerData.SendTransaction(transactionList, successCallBack: WhenRewardSuccess);

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
                    ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                    param.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                }
                break;
            case Item_Type.GrowthStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                    param.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                }
                break;
            case Item_Type.Ticket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                    param.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                }
                break;
            case Item_Type.Marble:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                    param.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                }
                break;
            case Item_Type.Songpyeon:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Songpyeon).Value += amount;
                    param.Add(GoodsTable.Songpyeon, ServerData.goodsTable.GetTableData(GoodsTable.Songpyeon).Value);
                }
                break;
            case Item_Type.RelicTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += amount;
                    param.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
                }
                break;
            case Item_Type.Event_Item_0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value += amount;
                    param.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
                }
                break;

            case Item_Type.Event_Item_1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value += amount;
                    param.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);
                }
                break;
            case Item_Type.Event_Item_Summer:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_Summer).Value += amount;
                    param.Add(GoodsTable.Event_Item_Summer, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_Summer).Value);
                }
                break;

            case Item_Type.SulItem:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SulItem).Value += amount;
                    param.Add(GoodsTable.SulItem, ServerData.goodsTable.GetTableData(GoodsTable.SulItem).Value);
                }
                break;

            case Item_Type.FeelMulStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value += amount;
                    param.Add(GoodsTable.FeelMulStone, ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value);
                }
                break;

            case Item_Type.Asura0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura0).Value += amount;
                    param.Add(GoodsTable.Asura0, ServerData.goodsTable.GetTableData(GoodsTable.Asura0).Value);
                }
                break;

            case Item_Type.Asura1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura1).Value += amount;
                    param.Add(GoodsTable.Asura1, ServerData.goodsTable.GetTableData(GoodsTable.Asura1).Value);
                }
                break;

            case Item_Type.Asura2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura2).Value += amount;
                    param.Add(GoodsTable.Asura2, ServerData.goodsTable.GetTableData(GoodsTable.Asura2).Value);
                }
                break;

            case Item_Type.Asura3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura3).Value += amount;
                    param.Add(GoodsTable.Asura3, ServerData.goodsTable.GetTableData(GoodsTable.Asura3).Value);
                }
                break;
            case Item_Type.Asura4:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura4).Value += amount;
                    param.Add(GoodsTable.Asura4, ServerData.goodsTable.GetTableData(GoodsTable.Asura4).Value);
                }
                break;

            case Item_Type.Asura5:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura5).Value += amount;
                    param.Add(GoodsTable.Asura5, ServerData.goodsTable.GetTableData(GoodsTable.Asura5).Value);
                }
                break;
            case Item_Type.Aduk:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Aduk).Value += amount;
                    param.Add(GoodsTable.Aduk, ServerData.goodsTable.GetTableData(GoodsTable.Aduk).Value);
                }
                break;

            case Item_Type.LeeMuGiStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value += amount;
                    param.Add(GoodsTable.LeeMuGiStone, ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value);
                }
                break;
            case Item_Type.SP:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += amount;
                    param.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
                }
                break;   
            case Item_Type.Hel:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += amount;
                    param.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
                }
                break; 
            case Item_Type.Ym:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Ym).Value += amount;
                    param.Add(GoodsTable.Ym, ServerData.goodsTable.GetTableData(GoodsTable.Ym).Value);
                }
                break;

            //

            case Item_Type.Hae_Norigae:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Hae_Norigae).Value += amount;
                    param.Add(GoodsTable.Hae_Norigae, ServerData.goodsTable.GetTableData(GoodsTable.Hae_Norigae).Value);
                }
                break;

            case Item_Type.Hae_Pet:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Hae_Pet).Value += amount;
                    param.Add(GoodsTable.Hae_Pet, ServerData.goodsTable.GetTableData(GoodsTable.Hae_Pet).Value);
                }
                break;

            //
            case Item_Type.SinSkill0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill0).Value += amount;
                    param.Add(GoodsTable.SinSkill0, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill0).Value);
                }
                break;
            case Item_Type.SinSkill1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill1).Value += amount;
                    param.Add(GoodsTable.SinSkill1, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill1).Value);
                }
                break;
            case Item_Type.SinSkill2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill2).Value += amount;
                    param.Add(GoodsTable.SinSkill2, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill2).Value);
                }
                break;
            case Item_Type.SinSkill3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill3).Value += amount;
                    param.Add(GoodsTable.SinSkill3, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill3).Value);
                }
                break;      
            case Item_Type.NataSkill:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.NataSkill).Value += amount;
                    param.Add(GoodsTable.NataSkill, ServerData.goodsTable.GetTableData(GoodsTable.NataSkill).Value);
                }
                break;   
            case Item_Type.OrochiSkill:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.OrochiSkill).Value += amount;
                    param.Add(GoodsTable.OrochiSkill, ServerData.goodsTable.GetTableData(GoodsTable.OrochiSkill).Value);
                }
                break;  
            case Item_Type.GangrimSkill:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GangrimSkill).Value += amount;
                    param.Add(GoodsTable.GangrimSkill, ServerData.goodsTable.GetTableData(GoodsTable.GangrimSkill).Value);
                }
                break;
            //

            case Item_Type.SmithFire:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += amount;
                    param.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
                }
                break;

            case Item_Type.StageRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value += amount;
                    param.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);
                }
                break;

            case Item_Type.PeachReal:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += amount;
                    param.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
                }
                break;


            case Item_Type.GuildReward:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += amount;
                    param.Add(GoodsTable.GuildReward, ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value);
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

        if (ServerData.costumeServerTable.TableDatas.TryGetValue(key, out var serverData) == false)
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

        if (ServerData.petTable.TableDatas.TryGetValue(key, out var serverData) == false)
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
                ServerData.buffServerTable.TableDatas[buffTableData[i].Stringid].remainSec.Value = -1;

                param.Add(buffTableData[i].Stringid, ServerData.buffServerTable.TableDatas[buffTableData[i].Stringid].ConvertToString());
                return;
            }
        }
    }

    public void AddWeaponParam(ref Param param, Item_Type type)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();
        var serverTableData = ServerData.weaponTable.TableDatas[key];
        serverTableData.hasItem.Value = 1;
        serverTableData.amount.Value += 1;

        param.Add(key, serverTableData.ConvertToString());
    }

    public void AddNorigaeParam(ref Param param, Item_Type type)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();
        var serverTableData = ServerData.magicBookTable.TableDatas[key];
        serverTableData.hasItem.Value = 1;
        serverTableData.amount.Value += 1;

        param.Add(key, serverTableData.ConvertToString());
    }

    public void AddSkillParam(ref Param param, Item_Type type)
    {
        if (param == null)
        {
            param = new Param();
        }

        int skillIdx = ((int)type) % 3000;

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillIdx].Value = 1;
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][skillIdx].Value = 1;

        List<int> skillAmountSyncData = new List<int>();
        List<int> skillAlreadyHasSyncData = new List<int>();

        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
            skillAlreadyHasSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][i].Value);
        }

        param.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);
        param.Add(SkillServerTable.SkillAlreadyHas, skillAlreadyHasSyncData);
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }

}
