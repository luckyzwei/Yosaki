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
                buttonText.SetText("획득");
            }
            else 
            {
                buttonText.SetText("보주"); 
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"여우 호리병 획득!", null);
                ServerData.userInfoTable.UpData(UserInfoTable.getFoxCup, false);
                return;
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"환수 장비 강화 \n {lockCount} 에 해금!", null);
                return;
            }
        }
        else
        {
            foxcupObject.SetActive(true);
        }
    }
}
