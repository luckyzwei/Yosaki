using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSinBossBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView uiBossContentsViewPrefab;

    [SerializeField]
    private Transform cellParent;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.TwelveBossTable.dataArray;

        for (int i = 15; i < 19; i++)
        {
            var cell = Instantiate<UiTwelveBossContentsView>(uiBossContentsViewPrefab, cellParent);

            cell.Initialize(tableDatas[i]);
        }
    }
}
