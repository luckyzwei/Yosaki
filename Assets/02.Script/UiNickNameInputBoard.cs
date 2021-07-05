using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class UiNickNameInputBoard : SingletonMono<UiNickNameInputBoard>
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button confirmButton;

    private ReactiveProperty<bool> nowConnection = new ReactiveProperty<bool>(false);

    [SerializeField]
    private GameObject termsPopup;

    [SerializeField]
    private Button termsAgreeButton;

    [SerializeField]
    private GameObject nickNamePopupRoot;

    private void Start()
    {
        SetDefatult();
    }

    private void SetDefatult()
    {
        termsAgreeButton.interactable = false;
        nickNamePopupRoot.SetActive(false);
    }

    public void WhenTermsToggleChanged(bool isOn)
    {
        termsAgreeButton.interactable = isOn;
    }
    public void OnClickTermsAgreeButton()
    {
        termsPopup.SetActive(false);
        nickNamePopupRoot.SetActive(true);
    }

    private void UpdateButtonState()
    {
        confirmButton.interactable = nowConnection.Value == false;
    }

    private bool HasSuckSsoText()
    {
        return (inputField.text.Contains("썩쏘"));
    }

    public void OnClickConfirmButton()
    {
        if (CanMakeNickName() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("부적절한 문자가 포함되어 있습니다.");
            return;
        }

        if (HasSuckSsoText())
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말 썩쏘님이 맞습니까?",
                () =>
                {
                    nowConnection.Value = true;

                    Backend.BMember.CreateNickname(inputField.text, MakeNickNameCallBack);
                },
                () =>
                {
                    nowConnection.Value = false;
                });
        }
        else
        {
            nowConnection.Value = true;

            Backend.BMember.CreateNickname(inputField.text, MakeNickNameCallBack);
        }

        DatabaseManager.userInfoTable.ClearDailyMission();
    }

    //빈 닉네임 혹은 string.empty로 닉네임 생성&수정을 시도 한 경우
    //statusCode : 400
    //errorCode: UndefinedParameterException
    //message : undefined nickname, nickname을(를) 확인할 수 없습니다
    //
    //이미 중복된 닉네임이 있는 경우
    //statusCode: 409
    //errorCode: DuplicatedParameterException
    //message : Duplicated nickname, 중복된 nickname 입니다
    //
    //20자 이상의 닉네임인 경우
    //statusCode: 400
    //errorCode: BadParameterException
    //message : bad nickname is too long, 잘못된 nickname is too long 입니다
    //
    //닉네임에 앞/ 뒤 공백이 있는 경우
    // statusCode : 400
    //errorCode: BadParameterException
    //message : bad beginning or end of the nickname must not be blank , 잘못된 beginning or end of the nickname must not be blank 입니다

    private void MakeNickNameCallBack(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            PlayerData.Instance.LoadUserNickName();
            PreSceneStartButton.Instance.SetInteractive();
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닉네임 생성 성공!", NickNameMakeComplete);
        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{GetNickNameErrorCode(bro.GetStatusCode())}", null);
            nowConnection.Value = false;
        }
    }

    private void NickNameMakeComplete()
    {
        DialogManager.Instance.StartDialog();
        Destroy(this.gameObject);
    }

    private bool CanMakeNickName()
    {
        bool isRightRangeChar = Regex.IsMatch(inputField.text, "^[0-9a-zA-Z가-힣]*$");
        bool hasBadWorld = Utils.HasBadWord(inputField.text);

        return isRightRangeChar && hasBadWorld == false;
    }

    public void OnClickTermsViewButton()
    {
        Application.OpenURL("https://cafe.naver.com/madaki/2");
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
