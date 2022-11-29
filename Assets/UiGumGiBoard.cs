using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiGumGiBoard : MonoBehaviour
{
    [SerializeField]
    private UiGumGiCell gumGiCellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UnityEngine.UI.Button lastGumgiButton;

    [SerializeField]
    private TMPro.TextMeshProUGUI lastGumgiDesc;

    void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).AsObservable().Subscribe(e =>
        {
            if(e< TableManager.Instance.gumGiTable.dataArray[200].Require)
            {   
                lastGumgiButton.interactable = false;
                lastGumgiDesc.text = $"검기\n200단계 획득시";
            }
            else
            {
                lastGumgiButton.interactable = true;
                lastGumgiDesc.text = $"무한 검기";
            }
        }).AddTo(this);
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.gumGiTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiGumGiCell>(gumGiCellPrefab, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += 100000;
        }
    }
#endif

}
