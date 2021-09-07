using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiCollectionEvent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI eventDescription;

    private void Start()
    {
        SetDescriptionText();
    }

    private void SetDescriptionText()
    {
        string description = string.Empty;

        description += $"<color=red>송편 드랍 9월 {GameBalance.ChuseokDropEndDay}일까지</color>\n";
        description += $"<color=red>아이템 제작 9월 {GameBalance.ChuseokMakeEndDay}일까지</color>\n";
        description += $"<color=red>상품 판매 9월 {GameBalance.ChuseokPackageSaleEndDay}일까지</color>";

        eventDescription.SetText(description);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Songpyeon).Value += 1000;
        }
    }
#endif

}
