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

    public void ActiveRootObject(bool active)
    {
        rootObject.SetActive(active);
    }


}
