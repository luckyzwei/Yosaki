using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChunmaRewardBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Horse;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView_Horse.Initialize(TableManager.Instance.TwelveBossTable.dataArray[55]);
    }


}
