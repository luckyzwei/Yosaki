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

        description += $"<color=red>눈송이 획득 1월 {GameBalance.EventDropEndDay}일까지</color>\n";
        description += $"<color=red>아이템 제작 1월 {GameBalance.EventMakeEndDay}일까지</color>\n";
        description += $"<color=red>상품 판매 1월 {GameBalance.EventPackageSaleEndDay}일까지</color>";

        eventDescription.SetText(description);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value += 1000000;
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
