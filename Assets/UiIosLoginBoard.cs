using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using static GoogleManager;

public class UiIosLoginBoard : SingletonMono<UiIosLoginBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TMP_InputField inputField_password;

    [HideInInspector]
    public bool loginProcess = false;

    int passWordMinNum = 8;


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        rootObject.SetActive(false);
#endif
    }

    public void ShowCustomGuestCreateBoard()
    {
        rootObject.SetActive(true);
    }

    private bool CanMakeNickName()
    {
        bool isRightRangeChar = Regex.IsMatch(inputField.text, "^[0-9a-zA-Z]*$");
        bool hasBadWorld = Utils.HasBadWord(inputField.text);
        bool sizeOver = inputField.text.Length >= passWordMinNum;
        return isRightRangeChar && hasBadWorld == false && sizeOver == false;
    }

    public void OnClickConfirmButton()
    {
        if (CanMakeNickName() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"부적절한 문자가 포함되어 있습니다.\n영문 숫자만 가능합니다.\n최대 {passWordMinNum}자");
            return;
        }

        if (inputField_password.text.Equals(string.Empty) || inputField_password.text.Length < passWordMinNum)
        {
            PopupManager.Instance.ShowAlarmMessage($"패스워드는 {passWordMinNum}자 이상이어야 합니다.");
            return;
        }

        string nickName = inputField.text;

        //게스트 가입 진행
        PopupManager.Instance.ShowYesNoPopup("회원가입", $"입력하신 아이디는 {nickName}\n패스워드는 {inputField_password.text}입니다.\n로그인 또는 회원가입을 진행합니까?", () =>
         {
             GoogleManager.Instance.loginId = nickName;

             string password = inputField_password.text;

             PlayerPrefs.SetString(CommonString.SavedLoginTypeKey, GoogleManager.Instance.loginId);

             PlayerPrefs.SetString(CommonString.SavedLoginPassWordKey, password);

             PlayerPrefs.SetString(CommonString.IOS_loginType, IOS_LoginType.Custom.ToString());

             GoogleManager.Instance.SignIn(nickName, password);
         }, null);
    }

    public void OnClickGameCenterButton()
    {
        if (loginProcess == true) return;

        GoogleManager.Instance.GameCenterLogin();

        loginProcess = true;
    }
}
