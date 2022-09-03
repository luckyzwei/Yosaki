using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHellBossBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;  
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_dukChoon;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[52]);
        bossContentsView_dukChoon.Initialize(TableManager.Instance.TwelveBossTable.dataArray[54]);
    }
}
