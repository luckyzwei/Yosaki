using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class UiNickNameChangeBoard : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private TextMeshProUGUI nickNameChangeFee;

    private void Start()
    {
        SetPriceText();
    }

    private void SetPriceText()
    {
        this.nickNameChangeFee.SetText($"{Utils.ConvertBigNum(GameBalance.nickNameChangeFee)}");
    }

    public void OnClickConfirmButton()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            PopupManager.Instance.ShowAlarmMessage("닉네임을 입력 해주세요.");
            return;
        }

        if (PlayerData.Instance.NickName.Equals(inputField.text))
        {
            PopupManager.Instance.ShowAlarmMessage("현재 닉네임 입니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value < GameBalance.nickNameChangeFee)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        if (CanMakeNickName() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("부적절한 문자가 포함되어 있습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{inputField.text}로 닉네임을 변경 합니까?", () =>
         {
             confirmButton.interactable = false;
#if UNITY_ANDROID
             Backend.BMember.UpdateNickname(inputField.text, MakeNickNameCallBack);
#endif
#if UNITY_IOS
             Backend.BMember.UpdateNickname(inputField.text+CommonString.IOS_nick, MakeNickNameCallBack);
#endif

         }, null);

    }

    private void MakeNickNameCallBack(BackendReturnObject bro)
    {
        confirmButton.interactable = true;

        if (bro.IsSuccess())
        {
            PlayerData.Instance.NickNameChanged(inputField.text);
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닉네임 변경 성공!", null);

            //재화 소모
            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.nickNameChangeFee;
            ServerData.goodsTable.UpData(GoodsTable.Jade, false);


        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{GetNickNameErrorCode(bro.GetStatusCode())}", null);
        }
    }

    private bool CanMakeNickName()
    {
        bool isRightRangeChar = Regex.IsMatch(inputField.text, "^[0-9a-zA-Z가-힣]*$");
        bool hasBadWorld = Utils.HasBadWord(inputField.text);

        return isRightRangeChar && hasBadWorld == false;
    }

    public string GetNickNameErrorCode(string statusCode)
    {
        switch (statusCode)
        {
            case "409":
                {
                    return CommonString.NickNameError_409;
                }
                break;
            case "400":
                {
                    return CommonString.NickNameError_400;
                }
                break;
        }

        return "String empty";
    }
}
