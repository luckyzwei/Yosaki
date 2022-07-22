using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHellContentsBoard : MonoBehaviour
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
        //길드보스 -1
        for (int i = 40; i < 50; i++)
        {
            var cell = Instantiate<UiTwelveBossContentsView>(uiBossContentsViewPrefab, cellParent);

            cell.Initialize(tableDatas[i]);
        }

    }
}
