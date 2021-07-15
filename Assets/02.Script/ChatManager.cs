using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChatManager : SingletonMono<ChatManager>
{
    private string chatGroupName = "Normal";

    ChannelType channelType = ChannelType.Public;

    private int retryCountMax = 3;
    private int retryCount = 0;
    WaitForSeconds retryDelay = new WaitForSeconds(3.0f);

    public ReactiveCommand<string> whenChatReceived = new ReactiveCommand<string>();

    private Coroutine aliveRoutine;

    private string serverAddress_str = "serverAddress";
    private string serverPort_str = "serverPort";
    private string inDate_str = "inDate";

    public bool chatConnected { get; private set; }

    public void ConnectToChattingServer()
    {
        LinkChatCallBacks();

        var bro = Backend.Chat.GetGroupChannelList(chatGroupName);

        for(int i=0;i< bro.Rows().Count; i++) 
        {
            JsonData channel = bro.Rows()[i];

            int maxUserNum = int.Parse(channel["maxUserCount"].ToString());
            int joinedUserCount = int.Parse(channel["joinedUserCount"].ToString());
            if (joinedUserCount >= maxUserNum) 
            {
                continue;
            }

            string address = channel[serverAddress_str].ToString();
            var serverPort = (ushort.Parse(channel[serverPort_str].ToString()));
            string inDate = channel[inDate_str].ToString();
            string groupName = chatGroupName;

            ErrorInfo errorInfo;
            bool join = Backend.Chat.JoinChannel(ChannelType.Public, address, serverPort, groupName, inDate, out errorInfo);

            if (join)
            {
                retryCount = 0;

                //비속어 필터링
                whenChatReceived.Execute(CommonString.ChatConnectString);
                Debug.LogError("채팅  JoinChannel success");
                chatConnected = true;
                Backend.Chat.SetFilterUse(true);
                Backend.Chat.OnChat = OnChatReceived;
            }
            else
            {
                Debug.LogError("채팅  JoinChannel failed");
                chatConnected = false;

                if (errorInfo != null)
                {
                    RetryConnect();
                }
            }

            return;
        }
    }
    private void LinkChatCallBacks()
    {
        Backend.Chat.OnJoinChannel = OnJoinChannel;
    }

    private void RetryConnect()
    {
        whenChatReceived.Execute("채팅 서버 연결이 끊겼습니다. 다시 연결 합니다.");
        chatConnected = false;
        StartCoroutine(RetryRoutine());
    }

    private IEnumerator RetryRoutine()
    {
        if (retryCount > retryCountMax)
        {
            whenChatReceived.Execute("채팅 서버와 연결이 끊겼습니다.");
            chatConnected = false;
            yield break;
        }

        retryCount++;
        yield return retryDelay;
        ConnectToChattingServer();
    }

    private void OnJoinChannel(JoinChannelEventArgs args)
    {
        Debug.Log(string.Format("OnJoinChannel {0}", args.ErrInfo));
        //입장에 성공한 경우
        if (args.ErrInfo == ErrorInfo.Success)
        {
            // 내가 접속한 경우 
            if (!args.Session.IsRemote)
            {
                Debug.Log("채널에 접속했습니다");
                chatConnected = true;

            }
            //다른유저가 접속한 경우
            else
            {
#if UNITY_EDITOR
                Debug.LogError(args.Session.NickName + "님이 접속했습니다");
#endif
            }
        }
        else
        {
            //에러가 발생했을 경우
            Debug.LogError("입장도중 에러가 발생했습니다 : " + args.ErrInfo.Reason);
            chatConnected = false;
        }
    }

    private void Update()
    {
        Backend.Chat.Poll();
    }

    bool paused = false;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            paused = true;
        }
        else if (paused == true && pause == false)
        {
            if (IsChatConnecting() == false)
            {
                RetryConnect();
            }
        }
    }

    private bool IsChatConnecting()
    {
        return Backend.Chat.IsChatConnect(ChannelType.Public);
    }


    public void SendChat(string message)
    {
        if (IsChatConnecting() == false)
        {
            RetryConnect();
            return;
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.chatBan).Value == 1f)
        {
            PopupManager.Instance.ShowAlarmMessage("채팅이 차단된 상태입니다.");
            return;
        }

        if (string.IsNullOrEmpty(message)) 
        {
            PopupManager.Instance.ShowAlarmMessage("메세지가 비었습니다.");
            return;
        }

#if UNITY_EDITOR
        Debug.LogError("채팅 SendMessage");
#endif

        AddRankInfo(ref message);

        AddCostumeInfo(ref message);

        Backend.Chat.ChatToChannel(ChannelType.Public, message);
    }

    private void AddCostumeInfo(ref string message)
    {
        message = $"{(int)ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value}{CommonString.ChatSplitChar}{message}";
    }

    private void AddRankInfo(ref string message)
    {
        RankType rankType = RankType.None;

        var e = RankManager.Instance.MyRankInfo.GetEnumerator();

        int bestRank = int.MaxValue;

        while (e.MoveNext())
        {
            if (e.Current.Value != null)
            {
                if (e.Current.Value.Rank < bestRank)
                {
                    bestRank = e.Current.Value.Rank;
                    rankType = e.Current.Key;
                }
            }
        }

        if (rankType != RankType.None && bestRank != int.MaxValue && bestRank < 10000)
        {
            string colorPrefix = null;
            if (bestRank < 100)
            {
                colorPrefix = "<color=#ff00ffff>";
            }
            else if (bestRank < 1000)
            {
                colorPrefix = "<color=#ffff00ff>";
            }
            else
            {
                colorPrefix = "<color=#ffffffff>";
            }

            message = $"{colorPrefix}({GetRankName(rankType)}{bestRank}위)</color>{CommonString.ChatSplitChar}{message}";
        }
        else
        {
            message = $"(초보자){CommonString.ChatSplitChar}{message}";
        }
    }

    public string GetRankName(RankType rankType)
    {
        switch (rankType)
        {
            case RankType.Level:
                return CommonString.RankPrefix_Level;
                break;
            case RankType.Boss:
                return CommonString.RankPrefix_Boss0;
                break;
            case RankType.Boss1:
                return CommonString.RankPrefix_Boss1;
                break;
            case RankType.Infinity:
                return CommonString.RankPrefix_Infinity;
                break;
        }

        return null;
    }

    public void OnChatReceived(ChatEventArgs args)
    {
        if (args.ErrInfo == ErrorInfo.Success)
        {
            // 자신의 메시지일 경우
            if (!args.From.IsRemote)
            {
                var split = args.Message.Split(CommonString.ChatSplitChar);
                whenChatReceived.Execute($"{split[0]}{CommonString.ChatSplitChar}{split[1]} 나:{split[2]}");
            }
            // 다른 유저의 메시지일 경우
            else
            {
                var split = args.Message.Split(CommonString.ChatSplitChar);
                whenChatReceived.Execute($"{split[0]}{CommonString.ChatSplitChar}{split[1]} {args.From.NickName}:{split[2]}");
            }
        }
        else if (args.ErrInfo.Category == ErrorCode.BannedChat)
        {
            // 도배방지 메세지 
            if (args.ErrInfo.Detail == ErrorCode.BannedChat)
            {
                PopupManager.Instance.ShowAlarmMessage("메시지를 너무 많이 입력하였습니다.");
            }
        }
    }
}
