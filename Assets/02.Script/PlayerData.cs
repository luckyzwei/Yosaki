﻿using BackEnd;
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

    public void NickNameChanged(string nickName)
    {
        NickName = nickName;
        whenNickNameChanged.Execute(nickName);
    }

    public void LoadUserNickName()
    {
        Backend.BMember.GetUserInfo(WhenUserInfoLoadComplete);
    }

    private void WhenUserInfoLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
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
                    NickName = row["nickname"].ToString();
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
            Debug.LogError("User info load Failed");
        }
    }

    private void WhenUserInfoLoadComplete()
    {
        PreSceneStartButton.Instance.SetInteractive();
        ChatManager.Instance.ConnectToChattingServer();
        Subscribe();

        SaveManager.Instance.StartAutoSave();

        PushManager.Instance.Initialize();
    }

    private void Subscribe()
    {
        RankManager.Instance.Subscribe();
    }
}
