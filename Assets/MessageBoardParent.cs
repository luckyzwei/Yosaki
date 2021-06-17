using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoardParent : SingletonMono<MessageBoardParent>
{
    private void Start()
    {
        PopupManager.Instance.SetChatBoardMainGameCanvas();
    }
}
