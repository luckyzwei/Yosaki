using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYachaUpgradeBoard : SingletonMono<UiYachaUpgradeBoard>
{
    [SerializeField]
    private GameObject rootObject;

    public void ShowUpgradePopup(bool show)
    {
        rootObject.SetActive(show);
    }
}
