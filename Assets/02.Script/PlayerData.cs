using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerData : SingletonMono<PlayerData>
{
    public string NickName { get; private set; }
    public string Indate { get; private set; }

    public ReactiveCommand<string> whenNickNameChanged = new ReactiveCommand<string>();

    [SerializeField]
    private UiNickNameInputBoard uiNickNameInputBoard;

#if UNITY_IOS
    public bool HasIOSFlag { get; private set; } = false;
#endif

    public void NickNameChanged(string nickName)
    {
        LogManager.Instance.SendLogType("NickChange", "pref", NickName);

        NickName = nickName;
        whenNickNameChanged.Execute(nickName);
    }

    public void LoadUserNickName()
    {
        Debug.Log("IOS_5");
        Backend.BMember.GetUserInfo(WhenUserInfoLoadComplete);
    }

    private void WhenUserInfoLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            Debug.Log("IOS_6");
            Debug.Log("UserInfo가 존재합니다.");
            var returnData = bro.GetReturnValuetoJSON();
            JsonData row = returnData["row"];

            if (row.Keys.Contains("inDate"))
            {
                Indate = row["inDate"].ToString();
            }

            if (row.Keys.Contains("nickname"))
            {
                if (row["nickname"] != null)
                {
#if UNITY_ANDROID
                    NickName = row["nickname"].ToString();
#endif
#if UNITY_IOS
                    NickName = row["nickname"].ToString().Replace(CommonString.IOS_nick, "");
#endif
                    Debug.Log("IOS_8");
                    WhenUserInfoLoadComplete();
                }
                else
                {
                    uiNickNameInputBoard.gameObject.SetActive(true);
                }
            }
            else
            {
                uiNickNameInputBoard.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("IOS_7");
            Debug.LogError("User info load Failed");
        }
    }

    private void WhenUserInfoLoadComplete()
    {
        Debug.Log("IOS_9");
        PreSceneStartButton.Instance.SetInteractive();
        ChatManager.Instance.ConnectToChattingServer();
        Subscribe();

        SaveManager.Instance.StartAutoSave();
        GameManager.Instance.Initialize();
        PushManager.Instance.Initialize();
        Debug.Log("IOS_10");
#if UNITY_IOS
        Backend.Chart.GetChartList((callback) =>
        {
            HasIOSFlag = callback.Rows().Count == 2;
            if(HasIOSFlag)
            {
                 SRDebug.Init();
            }
        });
#endif
    }

    private void Subscribe()
    {
        RankManager.Instance.Subscribe();
    }
}
