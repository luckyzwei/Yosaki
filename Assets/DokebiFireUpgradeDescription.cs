using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class DokebiFireUpgradeDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleDescription;


    void Start()
    {
        Subscribe();

    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireEnhance).AsObservable().Subscribe(e =>
        {

            int level = (int)ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value;

            var tabledata = TableManager.Instance.dokebiAbilBase.dataArray;

            marbleDescription.SetText($"우두머리 불꽃 1개당\n도깨비불 보유효과\n{PlayerStats.dokebiUpgradeValue * 100f}% 강화\n<color=yellow>총 {PlayerStats.GetDokebiFireEnhanceAbilPlusValue() * 100f}% 강화됨</color>");

        }).AddTo(this);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireEnhance).Value += 100;
        }
    }
#endif

}
