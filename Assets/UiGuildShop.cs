using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildShop : MonoBehaviour
{

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += 100;
        }
    }
#endif
}
