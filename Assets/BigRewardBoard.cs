using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRewardBoard : MonoBehaviour
{
    [SerializeField]
    private UiBigRewardCell bigRewardCellPrefab;

    [SerializeField]
    private Transform cellParent;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.winterPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Reward1 != (int)Item_Type.DokebiFire)
            {
                continue;
            }
            var passInfo = new PassInfo();

            passInfo.require = tableData[i].Unlockamount;
            passInfo.id = tableData[i].Id;

            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = ChildPassServerTable.childFree;

            passInfo.rewardType_IAP = tableData[i].Reward2;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = ChildPassServerTable.childAd;

            var cell = Instantiate<UiBigRewardCell>(bigRewardCellPrefab, cellParent);

            cell.Initialize(passInfo);
        }
    }
}
