using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChunmaRewardBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Normal;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Recommend;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView_Normal.Initialize(TableManager.Instance.TwelveBossTable.dataArray[55]);

        bossContentsView_Recommend.Initialize(TableManager.Instance.TwelveBossTable.dataArray[68]);
    }


}
