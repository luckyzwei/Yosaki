using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDokebiBossBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView dokebiBoss_0;

    [SerializeField]
    private UiTwelveBossContentsView dokebiBoss_1;

    [SerializeField]
    private UiTwelveBossContentsView dokebiBoss_2;

    [SerializeField]
    private UiTwelveBossContentsView dokebiBoss_3;

    [SerializeField]
    private UiTwelveBossContentsView dokebiBoss_4;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        dokebiBoss_0.Initialize(TableManager.Instance.TwelveBossTable.dataArray[75]);

        dokebiBoss_1.Initialize(TableManager.Instance.TwelveBossTable.dataArray[76]);

        dokebiBoss_2.Initialize(TableManager.Instance.TwelveBossTable.dataArray[77]);

        dokebiBoss_3.Initialize(TableManager.Instance.TwelveBossTable.dataArray[78]);

        dokebiBoss_4.Initialize(TableManager.Instance.TwelveBossTable.dataArray[79]);

    }
}
