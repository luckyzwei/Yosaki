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

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[22]);
        bossContentsView_Three.Initialize(TableManager.Instance.TwelveBossTable.dataArray[23]);
        bossContentsView_Kirin.Initialize(TableManager.Instance.TwelveBossTable.dataArray[25]);
    }
}
