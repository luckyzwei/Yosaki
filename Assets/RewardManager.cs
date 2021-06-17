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
            case Item_Type.BlueStone:
                DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value += amount;
                DatabaseManager.goodsTable.UpData(GoodsTable.BlueStone, false);
                break;
            case Item_Type.MagicStone:
                DatabaseManager.goodsTable.GetTableData(GoodsTable.MagicStone).Value += amount;
                DatabaseManager.goodsTable.UpData(GoodsTable.MagicStone, false);
                break;
            case Item_Type.Memory:
                DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value += amount;
                DatabaseManager.statusTable.UpData(StatusTable.Memory, false);
                break;
        }
    }
}
