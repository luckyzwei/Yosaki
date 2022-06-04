using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSinBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView2;
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView3;
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView4;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView5;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView6;

    void Start()
    {
        Initialize();
    }
    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[13]);
        bossContentsView2.Initialize(TableManager.Instance.TwelveBossTable.dataArray[14]);
        bossContentsView3.Initialize(TableManager.Instance.TwelveBossTable.dataArray[19]);
        bossContentsView4.Initialize(TableManager.Instance.TwelveBossTable.dataArray[21]);
        bossContentsView5.Initialize(TableManager.Instance.TwelveBossTable.dataArray[24]);
        bossContentsView6.Initialize(TableManager.Instance.TwelveBossTable.dataArray[26]);
    }
}
