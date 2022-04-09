using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuildRankInfo
{
    public string guildName;
    public string indate;
    public int rank;
    public int score;

    public GuildRankInfo(string name, string indate, int rank, int score)
    {
        this.guildName = name;
        this.indate = indate;
        this.rank = rank;
        this.score = score;
    }
}

public class UiGuildRankBoard : MonoBehaviour
{
    [SerializeField]
    private List<UiGuildRankCell> uiGuildRankCells;

    [SerializeField]
    private GameObject RefreshObject;



    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        RefreshObject.SetActive(true);

        Backend.URank.Guild.GetRankList(RankManager.Rank_Guild_Uuid, 100, bro =>
        {
            if (bro.IsSuccess())
            {
                RefreshObject.SetActive(false);

                var rows = bro.GetReturnValuetoJSON()["rows"];

                UiGuildTopRankView.Instance.currentIndex = 0;

                for (int i = 0; i < uiGuildRankCells.Count; i++)
                {
                    if (i < rows.Count)
                    {
                        uiGuildRankCells[i].gameObject.SetActive(true);

                        var rankInfo = new GuildRankInfo
                            (
                            rows[i]["guildName"]["S"].ToString(),
                            rows[i]["guildInDate"]["S"].ToString(),
                           int.Parse(rows[i]["rank"]["N"].ToString()),
                            int.Parse(rows[i]["score"]["N"].ToString())
                            );

                        uiGuildRankCells[i].Initialize(rankInfo);

                        UiGuildTopRankView.Instance.SetTopRankInfo(rankInfo);

                    }
                    else
                    {
                        uiGuildRankCells[i].gameObject.SetActive(false);

                    }
                }
            }
            else
            {

            }
            // 이후 처리
        });
    }
}
