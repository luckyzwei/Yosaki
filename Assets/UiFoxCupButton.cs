using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class UiFoxCupButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private GameObject foxcupObject;

    [SerializeField]
    private int lockCount;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.getFoxCup).AsObservable().Subscribe(e =>
        {
            if (e == 0)
            {
                buttonText.SetText("»πµÊ");
            }
            else 
            {
                buttonText.SetText("∫∏¡÷"); 
            }
        }).AddTo(this);
    }

    public void OnButtonClick()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getFoxCup).Value == 0)
        {
            if (lockCount <= ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.getFoxCup).Value = 1;
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"ø©øÏ »£∏Æ∫¥ »πµÊ!", null);
                ServerData.userInfoTable.UpData(UserInfoTable.getFoxCup, false);
                return;
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"»Øºˆ ¿Â∫Ò ∞≠»≠ \n {lockCount} ø° «ÿ±›!", null);
                return;
            }
        }
        else
        {
            foxcupObject.SetActive(true);
        }
    }
}
