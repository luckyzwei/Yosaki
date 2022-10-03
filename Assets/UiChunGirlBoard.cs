using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiChunGirlBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView chunGirl_0;

    [SerializeField]
    private UiTwelveBossContentsView chunGirl_1;

    [SerializeField]
    private UiTwelveBossContentsView chunGirl_2;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        chunGirl_0.Initialize(TableManager.Instance.TwelveBossTable.dataArray[58]);

        chunGirl_1.Initialize(TableManager.Instance.TwelveBossTable.dataArray[59]);

        chunGirl_2.Initialize(TableManager.Instance.TwelveBossTable.dataArray[60]);
    }
}
