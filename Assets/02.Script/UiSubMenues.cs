using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSubMenues : SingletonMono<UiSubMenues>
{

    public void ActiveOnlineRaidLobby() 
    {
        PartyRaidManager.Instance.ActivePartyRaidBoard();
    }
}
