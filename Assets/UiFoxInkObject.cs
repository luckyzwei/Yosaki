using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFoxInkObject : MonoBehaviour
{

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[69]);
    }

}
