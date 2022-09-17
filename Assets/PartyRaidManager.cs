using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PartyRaidManager : SingletonMono<PartyRaidManager>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private NetworkManager networkManager;
    public NetworkManager NetworkManager=> networkManager;

    public void ActivePartyRaidBoard() 
    {
        rootObject.SetActive(true);
        networkManager.Connect();
    }

    public void OnClickCloseButton()
    {
        rootObject.SetActive(false);
        PhotonNetwork.Disconnect();
    }


}
