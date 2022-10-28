using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMileageRewardPopup : MonoBehaviour
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
        eventDescription.SetText(description);
    }

#if UNITY_EDITOR
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value += 1000000;
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
