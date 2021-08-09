using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDokebiShop : MonoBehaviour
{
    [SerializeField]
    private UiDokebiShopCell uiDokebiShopCellPrefab;

    [SerializeField]
    private Transform cellParent;


    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.DokebiRewardTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiDokebiShopCell>(uiDokebiShopCellPrefab, cellParent);
            cell.Initialize(tableDatas[i]);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value += 1000;
        }
    }

#endif
}
