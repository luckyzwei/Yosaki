using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHellBossBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;  
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_dukChoon;
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_haeWonMak;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[52]);
        bossContentsView_dukChoon.Initialize(TableManager.Instance.TwelveBossTable.dataArray[54]);
        bossContentsView_haeWonMak.Initialize(TableManager.Instance.TwelveBossTable.dataArray[56]);
    }
}
