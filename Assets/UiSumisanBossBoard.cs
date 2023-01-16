﻿using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSumisanBossBoard : MonoBehaviour
{
    [SerializeField]
    private List<UiTwelveBossContentsView> sumisanBoss;


    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        
        sumisanBoss[0].Initialize(TableManager.Instance.TwelveBossTable.dataArray[87]);
         
    }
}