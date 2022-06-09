using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHaeTeaBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Three;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Kirin;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Rabit;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[22]);
        bossContentsView_Three.Initialize(TableManager.Instance.TwelveBossTable.dataArray[23]);
        bossContentsView_Kirin.Initialize(TableManager.Instance.TwelveBossTable.dataArray[25]);
        bossContentsView_Rabit.Initialize(TableManager.Instance.TwelveBossTable.dataArray[27]);
    }
}
