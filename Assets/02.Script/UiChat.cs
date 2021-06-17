using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiChat : MonoBehaviour
{
    [SerializeField]
    private List<UiMessageText> messagePool;

    [SerializeField]
    private TMP_InputField inputfield;

    private int currentIdx = 0;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ChatManager.Instance.whenChatReceived.Subscribe(e => { SetMessage(e, false); }).AddTo(this);
    }

    private void Initialize()
    {
        messagePool.ForEach(e => e.Initialize(string.Empty, true));

        if (ChatManager.Instance.chatConnected)
        {
            SetMessage(CommonString.ChatConnectString, true);
        }
    }

    private void SetMessage(string message, bool isSystem)
    {
        messagePool[currentIdx].Initialize(message, isSystem);
        messagePool[currentIdx].transform.SetAsFirstSibling();

        currentIdx++;
        if (currentIdx == messagePool.Count)
        {
            currentIdx = 0;
        }
    }

    public void SendChat(string chat)
    {
        if (chat.Length > 30)
        {
            chat = chat.Substring(0, 30);
        }
        ChatManager.Instance.SendChat(chat);
        inputfield.text = string.Empty;
    }
}
