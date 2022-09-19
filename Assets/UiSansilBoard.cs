using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSansilBoard : MonoBehaviour
{
    [SerializeField]
    private float needSpAmount = 1000000;

    [SerializeField]
    private UiTwelveBossContentsView twelveBossContentsView;

    private void Start()
    {
        Initialize();
    }

    private void Initialize() 
    {
        twelveBossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[57]);
    }

    private void OnEnable()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value < needSpAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"검기 {Utils.ConvertBigNum(needSpAmount)}개 이상일때 해금 됩니다.");

            this.gameObject.SetActive(false);

            return;
        }
    }
}
