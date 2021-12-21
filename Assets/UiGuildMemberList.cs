using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiGuildMemberCell;

public class UiGuildMemberList : SingletonMono<UiGuildMemberList>
{
    [SerializeField]
    private UiGuildMemberCell memberCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiGuildMemberCell> memberCells;

    public int guildMemberCount = GameBalance.GuildMemberMax;

    public GuildGrade GetMyGuildGrade()
    {
        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName.Equals(PlayerData.Instance.NickName)) 
            {
                return memberCells[i].guildMemberInfo.guildGrade;
            }

        }
        return GuildGrade.Member;
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        memberCells = new List<UiGuildMemberCell>();

        for (int i = 0; i < GameBalance.GuildMemberMax + 5; i++)
        {
            var cell = Instantiate<UiGuildMemberCell>(memberCellPrefab, cellParent);

            cell.gameObject.SetActive(false);

            memberCells.Add(cell);
        }

        RefreshMemberList();
    }

    public void RefreshMemberList()
    {
        var bro = Backend.Social.Guild.GetGuildMemberListV3(GuildManager.Instance.myGuildIndate, 5);

        if (bro.IsSuccess())
        {
            var returnValue = bro.GetReturnValuetoJSON();

            var rows = returnValue["rows"];

            guildMemberCount = rows.Count;

            for (int i = 0; i < memberCells.Count; i++)
            {
                if (i < rows.Count)
                {
                    memberCells[i].gameObject.SetActive(true);

                    var data = rows[i];

                    string nickName = data["nickname"]["S"].ToString();
                    string position = data["position"]["S"].ToString();
                    string lastLogin = data["lastLogin"]["S"].ToString();
                    string gamerIndate = data["gamerInDate"]["S"].ToString();
                    int donateGoods = int.Parse(data["totalGoods5Amount"]["N"].ToString());

                    var memberData = new GuildMemberInfo(nickName, position, lastLogin, gamerIndate, donateGoods);

                    memberCells[i].Initialize(memberData);
                }
                else
                {
                    memberCells[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            memberCells.ForEach(e => e.gameObject.SetActive(false));
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "조회 실패\n잠시후 다시 시도해 주세요", null);
        }
    }
}
