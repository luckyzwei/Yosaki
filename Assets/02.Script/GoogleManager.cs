using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
#if UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
    [SerializeField]
    private UiNickNameInputBoard nickNameInputBoard;

    private bool isSignIn = false;

    private ObscuredString loginId;

    public static string email { get; private set; } = "Editor";

    private void Awake()
    {
        Backend.Initialize(HandleBackendCallBack, true);

#if UNITY_IOS
         SRDebug.Init();
#endif
    }

    private void HandleBackendCallBack()
    {
        if (Backend.IsInitialized)
        {
#if UNITY_EDITOR
            WhenVersionCheckSuccess();
            return;
#endif
            CheckCurrentVersion();

        }
        else
        {
            Debug.LogError("Failed to initialize the backend");
        }
    }

    private void CheckCurrentVersion()
    {

        int clientVersion = int.Parse(Application.version);

        var bro = Backend.Utils.GetLatestVersion();

        if (bro.IsSuccess())
        {
            var jsonData = bro.GetReturnValuetoJSON();
            string serverVersion = jsonData["version"].ToString();

            //버전이 높거나 같음
            if (clientVersion >= int.Parse(serverVersion))
            {
                Debug.LogError($"클라이언트 버전 {clientVersion} 서버 버전 {serverVersion} 같음");
                WhenVersionCheckSuccess();
            }
            else
            {
                Debug.LogError($"클라이언트 버전 {clientVersion} 서버 버전 {serverVersion} 다름");

                PopupManager.Instance.ShowVersionUpPopup(CommonString.Notice, "업데이트 버전이 있습니다. 스토어로 이동합니다.", () =>
                {
#if UNITY_ANDROID
                    Application.OpenURL("https://play.google.com/store/apps/details?id=com.DragonGames.yoyo");
#endif
#if UNITY_IOS
                    Application.OpenURL("itms-apps://itunes.apple.com/app/id1587651736");
#endif
                }, false);
            }

            Debug.LogError($"clientVersion = {clientVersion} serverVersion {serverVersion}");
        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "클라이언트 버전 정보 로드에 실패했습니다.\n버전 정보를 다시 요청합니다.", CheckCurrentVersion);
        }
    }

    private void WhenVersionCheckSuccess()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
    .Builder()
    .RequestServerAuthCode(false)
    .RequestEmail()                 // 이메일 요청
    .RequestIdToken()               // 토큰 요청
    .Build();

        //커스텀된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;


        //GPGS 시작.
        PlayGamesPlatform.Activate();
#endif

#if UNITY_EDITOR
        LoginByCustumId();
#elif UNITY_ANDROID
            GoogleAuth();
#elif UNITY_IOS
            GameCenterLogin();
#endif
    }
#if UNITY_IOS
    public void GameCenterLogin()
    {
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success to true");
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    loginId = Social.localUser.id;

                    LoginByCustumId();
                }
                else
                {
                    PopupManager.Instance.ShowYesNoPopup("알림", "로그인 실패 재시도 합니다", GameCenterLogin, () =>
                    {
                        Application.Quit();
                    });
                    return;
                }
            });
        }
    }
#endif

#if UNITY_ANDROID
    private void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    PopupManager.Instance.ShowYesNoPopup("알림", "구글 로그인 실패 재시도 합니다", GoogleAuth, () =>
                     {
                         Application.Quit();
                     });
                    return;
                }

                Debug.Log($"Hash {Backend.Utils.GetGoogleHash()}");
                // 로그인이 성공되었습니다.
                loginId = Social.localUser.id;
                email = ((PlayGamesLocalUser)Social.localUser).Email;
                Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("GoogleId - " + Social.localUser.id);
                Debug.Log("UserName - " + Social.localUser.userName);

                LoginByCustumId();
            });
        }
    }
#endif

    private IEnumerator SceneChangeRoutine()
    {
        ServerData.LoadTables();

        while (SendQueue.UnprocessedFuncCount != 0)
        {
            yield return null;
        }

        if (isSignIn == false)
        {
            PlayerData.Instance.LoadUserNickName();
        }
        else
        {
            while (true)
            {
                yield return null;

                if (SendQueue.UnprocessedFuncCount <= 0 && isSignIn)
                {
                    nickNameInputBoard.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void LoginByCustumId()
    {
        var bro = Backend.BMember.CustomLogin(GetGoogleLoginKey(), GetGoogleLoginKey());

        //회원가입 안됨
        if (bro.IsSuccess())
        {
            Debug.Log("Login success");
            StartCoroutine(SceneChangeRoutine());
        }
        else
        {
            Debug.Log($"LoginFail bro.GetStatusCode() {bro.GetStatusCode()}");

            if (bro.GetStatusCode() == "403")
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "서버에 문제가 있습니다. 앱을 종료합니다. \n 잠시후 다시 시도해주세요", () =>
                  {
                      Application.Quit();
                  });
            }

            if (bro.GetStatusCode() == "401")
            {
                SignIn();
            }
        }
    }

    public void SignIn()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(GetGoogleLoginKey(), GetGoogleLoginKey());

        if (BRO.IsSuccess())
        {
            isSignIn = true;
            Debug.Log("Sign in success");
            LoginByCustumId();
        }
        else
        {
            Debug.Log($"SignIn error {BRO.GetStatusCode()}");
            switch (BRO.GetStatusCode())
            {
                case "200":
                    Debug.Log("이미 회원가입된 회원");
                    break;

                case "403":
                    Debug.Log("차단된 사용자 입니다. 차단 사유 : " + BRO.GetErrorCode());
                    break;
            }

            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "로그인 실패 재시도 합니까?", () =>
             {
                 SignIn();
             });
        }
    }
    
    private string GetGoogleLoginKey()
    {
#if UNITY_EDITOR
        //  return "a_8846847867697156085"; //블랙핑크
        //  return "a_3961873472804492579"; //제니
        return "a_884684786722e697156085";
#endif
        Debug.LogError($"GetGoogleLoginKey {loginId}");
        return loginId;
        //return Social.localUser.id;
    }
}
