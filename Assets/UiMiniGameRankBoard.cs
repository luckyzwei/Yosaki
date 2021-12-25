using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiMiniGameRankBoard : MonoBehaviour
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
        title.SetText($"랭킹({CommonString.RankPrefix_MiniGame})");
    }

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        RankManager.Instance.WhenMyMiniGameRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                myRankView.Initialize($"{e.Rank}", e.NickName, $"{e.Score.ToString("F2")}초", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.fightPointIdx, e.GuildName);
            }
            else
            {
                myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1,string.Empty);
            }


        }).AddTo(this);
    }
    private void LoadRankInfo()
    {
        rankViewParent.gameObject.SetActive(false);
        loadingMask.SetActive(false);
        failObject.SetActive(false);
        RankManager.Instance.GetRankerList(RankManager.Rank_MiniGame_Uuid, 100, WhenAllRankerLoadComplete);
        RankManager.Instance.RequestMyMiniGameRank();
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
                        string nickName = data["nickname"][ServerData.format_string].ToString();
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

#if UNITY_IOS
                    nickName = nickName.Replace(CommonString.IOS_nick, "");
#endif
                        string guildName = string.Empty;
                        if (splitData.Length >= 8)
                        {
                            guildName = splitData[7];
                        }
                        //myRankView.Initialize($"{e.Rank}", e.NickName, $"Lv {e.Score}");
                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"{score.ToString("F2")}초", rank, costumeId, petId, weaponId, magicBookId, fightPoint,guildName);
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
