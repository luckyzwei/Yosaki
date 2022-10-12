using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiCollectionEventFall : MonoBehaviour
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

        description = "-";
        //description += $"<color=red>벚꽃 획득 3월 31일까지</color>\n";
        //description += $"<color=red>아이템 제작 3월 31일까지</color>\n";
        //description += $"<color=red>상품 판매 3월 31일까지</color>";

        eventDescription.SetText(description);
    }

#if UNITY_EDITOR
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall).Value += 1000000;
        }
    }
#endif

    private void OnEnable()
    {
        //if (ServerData.userInfoTable.CanMakeEventItem() == false)
        //{
        //    this.gameObject.SetActive(false);
        //    PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다!");
        //}
    }

}
