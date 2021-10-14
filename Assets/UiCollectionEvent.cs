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

        description += $"<color=red>도깨비방망이 획득 10월 {GameBalance.EventDropEndDay}일까지</color>\n";
        description += $"<color=red>아이템 제작 11월 {GameBalance.EventMakeEndDay}일까지</color>\n";
        description += $"<color=red>상품 판매 11월 {GameBalance.EventPackageSaleEndDay}일까지</color>";

        eventDescription.SetText(description);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value += 10000;
        }
    }
#endif

    private void OnEnable()
    {
        if (ServerData.userInfoTable.CanMakeEventItem() == false) 
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다!");
        }
    }

}
