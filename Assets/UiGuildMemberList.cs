using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private TextMeshProUGUI guildNameInputBoard;

    [SerializeField]
    private TextMeshProUGUI memberNumText;

    private bool initialized = false;

    [SerializeField]
    private GameObject guildInfoButton;

    public void RemovePlayer(string nickName)
    {
        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName == nickName)
            {
                memberCells[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    public GuildGrade GetMyGuildGrade()
    {
        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(PlayerData.Instance.NickName))
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

    private void OnEnable()
    {
        if (initialized)
        {
            RefreshMemberList();
        }
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

        initialized = true;
    }

    public void RefreshMemberList()
    {
        memberNumText.SetText(string.Empty);

        var bro = Backend.Social.Guild.GetGuildMemberListV3(GuildManager.Instance.myGuildIndate, 25);

        if (bro.IsSuccess())
        {
            var returnValue = bro.GetReturnValuetoJSON();

            var rows = returnValue["rows"];

            guildMemberCount = rows.Count;

            memberNumText.SetText($"문파 인원 : {guildMemberCount}/{GameBalance.GuildMemberMax}");

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
                    int donateGoods = int.Parse(data["totalGoods2Amount"]["N"].ToString());

                    var memberData = new GuildMemberInfo(nickName, position, lastLogin, gamerIndate, donateGoods);

                    memberCells[i].Initialize(memberData);
                    memberCells[i].transform.SetAsFirstSibling();

                }
                else
                {
                    memberCells[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < memberCells.Count; i++)
            {
                if (i < rows.Count)
                {
                    memberCells[i].RefreshKickButton();
                }

            }

            PopupManager.Instance.ShowAlarmMessage("갱신 완료");
        }
        else
        {
            memberCells.ForEach(e => e.gameObject.SetActive(false));
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "조회 실패\n잠시후 다시 시도해 주세요", null);
        }

        guildInfoButton.SetActive(GetMyGuildGrade() == GuildGrade.Master);
    }
}
