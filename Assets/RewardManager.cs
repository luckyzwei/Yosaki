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
                DatabaseManager.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                DatabaseManager.goodsTable.UpData(GoodsTable.Jade, false);
                break;
            case Item_Type.GrowThStone:
                DatabaseManager.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                DatabaseManager.goodsTable.UpData(GoodsTable.GrowthStone, false);
                break;
            case Item_Type.Memory:
                DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value += amount;
                DatabaseManager.statusTable.UpData(StatusTable.Memory, false);
                break;
        }
    }
}
