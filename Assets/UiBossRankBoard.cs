using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class UiBossRankBoard : MonoBehaviour
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

    [SerializeField]
    private ObscuredInt bossId;


    private void OnEnable()
    {
        UiTopRankerView.Instance.DisableAllCell();
        SetTitle();
        LoadRankInfo();
    }

    private void SetTitle()
    {
        title.SetText("랭킹(보스)");
    }

    private void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        //if (bossId == 0) 
        //{
        //    RankManager.Instance.WhenMyStageRankLoadComplete.AsObservable().Subscribe(e =>
        //    {
        //        if (e != null)
        //        {
        //            myRankView.Initialize($"{e.Rank}", e.NickName, $"{Utils.ConvertBigNum(e.Score)}", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.fightPointIdx);
        //        }
        //        else
        //        {
        //            myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1);
        //        }

        //    }).AddTo(this);
        //}
        //else if (bossId == 1) 
        //{
        //    RankManager.Instance.WhenMyBoss1RankLoadComplete.AsObservable().Subscribe(e =>
        //    {
        //        if (e != null)
        //        {
        //            myRankView.Initialize($"{e.Rank}", e.NickName, $"{Utils.ConvertBigNum(e.Score)}", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.fightPointIdx);
        //        }
        //        else
        //        {
        //            myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1);
        //        }

        //    }).AddTo(this);
        //}

    }
    private void LoadRankInfo()
    {
        //rankViewParent.gameObject.SetActive(false);
        //loadingMask.SetActive(false);
        //failObject.SetActive(false);
        //noDataObject.SetActive(false);

        //if (bossId == 0)
        //{
        //    RankManager.Instance.GetRankerList_level(RankManager.Rank_Stage_Uuid, 100, WhenAllRankerLoadComplete);
        //    RankManager.Instance.RequestMyStageRank();
        //}
        //else if (bossId == 1)
        //{
        //    RankManager.Instance.GetRankerList_level(RankManager.Rank_Boss_1_Uuid, 100, WhenAllRankerLoadComplete);
        //    RankManager.Instance.RequestMyBoss1Rank();
        //}
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

                        var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                        rankViewContainer[i].gameObject.SetActive(true);
                        string nickName = splitData[5];
                        int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                        float score = float.Parse(data["score"][ServerData.format_Number].ToString());
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

                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"{Utils.ConvertBigNum(score)}", rank, costumeId, petId, weaponId, magicBookId, fightPoint);
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
