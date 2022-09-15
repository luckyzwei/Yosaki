using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhotonView : MonoBehaviourPun
{
    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg, PhotonMessageInfo info)
    {
        Debug.LogError($"Chat :{info.Sender.NickName} {msg}");
    }
}
