using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiInfinityRankBoard : MonoBehaviour
{
    [SerializeField]
    private UiRankView uiRankViewPrefab;

    [SerializeField]
    private Transform rankViewParent;

    [SerializeField]
    private UiRankView myRankView;

    List<UiRankView> rankViewContainer = new List<UiRankView>();

    [SerializeField]
    private GameObject loadingMask;

    [SerializeField]
    private GameObject failObject;

    [SerializeField]
    private GameObject noDataObject;

    [SerializeField]
    private TextMeshProUGUI title;

    private void OnEnable()
    {
        UiTopRankerView.Instance.DisableAllCell();
        LoadRankInfo();
        SetTitle();
    }

    private void SetTitle()
    {
        title.SetText($"랭킹({CommonString.ContentsName_InfinityTower})");
    }

    private void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        RankManager.Instance.WhenInfinityRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                myRankView.Initialize($"{e.Rank}", e.NickName, $"지하 {e.Score + 1}층", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.fightPointIdx);
            }
            else
            {
                myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1);
            }

        }).AddTo(this);
    }
    private void LoadRankInfo()
    {
        rankViewParent.gameObject.SetActive(false);
        loadingMask.SetActive(false);
        failObject.SetActive(false);
        noDataObject.SetActive(false);
        RankManager.Instance.GetRankerList_level(RankManager.Rank_Infinity_Uuid, 100, WhenAllRankerLoadComplete);
        RankManager.Instance.RequestInfinityTowerRank();
    }

    private void WhenAllRankerLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                noDataObject.SetActive(false);

                rankViewParent.gameObject.SetActive(true);

                int interval = rows.Count - rankViewContainer.Count;

                for (int i = 0; i < interval; i++)
                {
                    var view = Instantiate<UiRankView>(uiRankViewPrefab, rankViewParent);
                    rankViewContainer.Add(view);
                }

                for (int i = 0; i < rankViewContainer.Count; i++)
                {
                    if (i < rows.Count)
                    {
                        JsonData data = rows[i];

                        var splitData = data["NickName"][DatabaseManager.format_string].ToString().Split(CommonString.ChatSplitChar);

                        rankViewContainer[i].gameObject.SetActive(true);
                        string nickName = splitData[5];
                        int rank = int.Parse(data["rank"][DatabaseManager.format_Number].ToString());
                        int level = int.Parse(data["score"][DatabaseManager.format_Number].ToString());
                        int costumeId = int.Parse(splitData[0]);
                        int petId = int.Parse(splitData[1]);
                        int weaponId = int.Parse(splitData[2]);
                        int magicBookId = int.Parse(splitData[3]);
                        int fightPoint = int.Parse(splitData[4]);


                        Color color1 = Color.white;
                        Color color2 = Color.white;

                        //1등
                        if (i == 0)
                        {
                            color1 = Color.yellow;
                        }
                        //2등
                        else if (i == 1)
                        {
                            color1 = Color.yellow;
                        }
                        //3등
                        else if (i == 2)
                        {
                            color1 = Color.yellow;
                        }

                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"지하 {level + 1}층", rank, costumeId, petId, weaponId, magicBookId, fightPoint);
                    }
                    else
                    {
                        rankViewContainer[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                noDataObject.SetActive(true);
            }

        }
        else
        {
            failObject.SetActive(true);
        }
    }

}

//"rows": [
//       {
//           // 유저의 게이머 inDate
//           "gamerInDate": {
//               "S": "2021-03-11T06:38:46.817Z"
//           },
//           // 유저의 닉네임
//           "nickname": {
//    "S": "닉네임s"
//           },
//           // 추가항목
//           // 컬럼명과 값이 그대로 노출됩니다.
//           // 추가항목 컬럼명이 extraScore인 경우 extraScore
//           "extraScore": {
//    "N": "4577"
//           },
//           // 점수
//           // 컬럼명이 score로 통일됩니다.
//           // 랭킹항목 컬럼명이 power인 경우에도 score
//           "score": {
//    "N": 199
//           },
//           // offset
//           "index": {
//    "N": 0
//           },
//           // 유저의 랭킹
//           "rank": {
//    "N": 1
//           }
//       },
//       // and etc...
//    ],
