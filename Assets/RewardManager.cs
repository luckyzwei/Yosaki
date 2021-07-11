using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : SingletonMono<RewardManager>
{
    public void GetReward(Item_Type type, int amount)
    {
        switch (type)
        {
            case Item_Type.Gold:
                break;
            case Item_Type.Jade:
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                ServerData.goodsTable.UpData(GoodsTable.Jade, false);
                break;
            case Item_Type.GrowThStone:
                ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                ServerData.goodsTable.UpData(GoodsTable.GrowthStone, false);
                break;
            case Item_Type.Memory:
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value += amount;
                ServerData.statusTable.UpData(StatusTable.Memory, false);
                break;
        }
    }
}
