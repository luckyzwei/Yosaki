﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
public class ChuSeokAttenCountIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountChuSeok).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"{Utils.ConvertBigNum(e)}일차");
        }).AddTo(this);
    }
}
