using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYoumulUpgradeBoard : SingletonMono<UiYoumulUpgradeBoard>
{
    [SerializeField]
    private GameObject rootObject;

    public void ShowUpgradePopup(bool show) 
    {
        rootObject.SetActive(show);
    }
}
