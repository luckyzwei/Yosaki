using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;

public class UiChunmaRewardBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Normal;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Recommend;

    [SerializeField]
    private TextMeshProUGUI RecommendCount;

    private void Start()
    {
        Initialize();
        Subscribe();


    }

    private void Subscribe()
    {
        ServerData.bossServerTable.TableDatas["b68"].score.AsObservable().Subscribe(e=>
        {
            if (RecommendCount != null)
            {
                if (string.IsNullOrEmpty(ServerData.bossServerTable.TableDatas["b68"].score.Value))
                {
                    RecommendCount.SetText($"받은 추천 : 0");
                }
                else
                {
                    RecommendCount.SetText($"받은 추천 : {ServerData.bossServerTable.TableDatas["b68"].score.Value}");
                }
            }
        }).AddTo(this);
    }

    private void Initialize()
    {
        bossContentsView_Normal.Initialize(TableManager.Instance.TwelveBossTable.dataArray[55]);

        bossContentsView_Recommend.Initialize(TableManager.Instance.TwelveBossTable.dataArray[68]);
    }


}
