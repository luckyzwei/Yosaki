using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PartyRaidButton : MonoBehaviour
{
    [SerializeField]
    private ContentsType contentsType = ContentsType.PartyRaid;

    public void OnClickPartyRaidButton()
    {
        PartyRaidManager.Instance.NetworkManager.contentsType = contentsType;

        PartyRaidManager.Instance.ActivePartyRaidBoard();
    }
}
