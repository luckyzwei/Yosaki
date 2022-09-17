using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyRaidButton : MonoBehaviour
{
    public void OnClickPartyRaidButton()
    {
        PartyRaidManager.Instance.ActivePartyRaidBoard();
    }
}
