using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;

public class UiFeelMulUpgradeBoard : SingletonMono<UiFeelMulUpgradeBoard>
{

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private WeaponView feelMulView;


    public void ShowUpgradePopup(bool show)
    {
        rootObject.SetActive(show);
    }

    void Start()
    {

        Initialize();
    }

    private void Initialize()
    {
        feelMulView.Initialize(TableManager.Instance.WeaponTable.dataArray[22], null, null);
    }

}
