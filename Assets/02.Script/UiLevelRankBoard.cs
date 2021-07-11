using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiLevelRankBoard : MonoBehaviour
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
    private TextMeshProUGUI title;


    private void OnEnable()
    {
        UiTopRankerView.Instance.DisableAllCell();
        SetTitle();
        LoadRankInfo();
    }

    private void SetTitle()
    {
        title.SetText("랭킹(레벨)");
    }

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        RankManager.Instance.WhenMyLevelRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                myRankView.Initialize($"{e.Rank}", e.NickName, $"Lv {e.Score}", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.fightPointIdx);
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
        RankManager.Instance.GetRankerList_level(RankManager.Rank_Level_Uuid, 100, WhenAllRankerLoadComplete);
        RankManager.Instance.RequestMyLevelRank();
    }

    private void WhenAllRankerLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
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

                        var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                        rankViewContainer[i].gameObject.SetActive(true);
                        string nickName = splitData[5];
                        int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                        int level = int.Parse(data["score"][ServerData.format_Number].ToString());
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

                        //myRankView.Initialize($"{e.Rank}", e.NickName, $"Lv {e.Score}");
                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"Lv {level}", rank, costumeId, petId, weaponId, magicBookId, fightPoint);
                    }
                    else
                    {
                        rankViewContainer[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                //데이터 없을때
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
