using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using static UiGuildMemberCell;

public class UiGuildMemberList : SingletonMono<UiGuildMemberList>
{
    [SerializeField]
    private UiGuildMemberCell memberCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiGuildMemberCell> memberCells;

    public int guildMemberCount = 0;

    [SerializeField]
    private TextMeshProUGUI guildNameInputBoard;

    [SerializeField]
    private TextMeshProUGUI memberNumText;

    private bool initialized = false;

    [SerializeField]
    private GameObject guildInfoButton;

    public ReactiveProperty<int> attenUserNum = new ReactiveProperty<int>();

    public GuildMemberInfo myMemberInfo { get; private set; }

    public UiGuildMemberCell GetMemberCell(string nickName)
    {
        nickName = nickName.Replace(CommonString.IOS_nick, "");

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null &&
                memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(nickName))
            {
                return memberCells[i];
            }
        }

        return null;
    }

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
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(PlayerData.Instance.NickName.Replace(CommonString.IOS_nick, "")))
            {
                return memberCells[i].guildMemberInfo.guildGrade;
            }

        }
        return GuildGrade.Member;
    }

    public GuildGrade GetGuildGrade(string nickName)
    {
        nickName = nickName.Replace(CommonString.IOS_nick, "");

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(nickName))
            {
                return memberCells[i].guildMemberInfo.guildGrade;
            }

        }
        return GuildGrade.Member;
    }

    void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildLevelExp.AsObservable().Subscribe(e =>
        {
            RefreshGuildMemberCountText();
        }).AddTo(this);
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

        for (int i = 0; i < 100; i++)
        {
            var cell = Instantiate<UiGuildMemberCell>(memberCellPrefab, cellParent);

            cell.gameObject.SetActive(false);

            memberCells.Add(cell);
        }

        RefreshMemberList();

        initialized = true;
    }

    private void RefreshGuildMemberCountText()
    {
        memberNumText.SetText($"문파 인원 : {guildMemberCount}/{GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value)}");
    }

    public void RefreshMemberList()
    {
        memberNumText.SetText(string.Empty);

        var bro = Backend.Social.Guild.GetGuildMemberListV3(GuildManager.Instance.myGuildIndate, GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value) + 5);

        if (bro.IsSuccess())
        {
            var returnValue = bro.GetReturnValuetoJSON();

            var rows = returnValue["rows"];

            guildMemberCount = rows.Count;
            guildMemberCount = Mathf.Min(guildMemberCount, GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value));

            memberNumText.SetText($"문파 인원 : {guildMemberCount}/{GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value)}");

            bool findMyData = false;
            string myNickName = PlayerData.Instance.NickName.Replace(CommonString.IOS_nick, "");

            attenUserNum.Value = 0;

            int attenNum = 0;

            for (int i = 0; i < memberCells.Count; i++)
            {
                if (i < guildMemberCount)
                {
                    memberCells[i].gameObject.SetActive(true);

                    var data = rows[i];

                    string nickName = data["nickname"]["S"].ToString();
                    string position = data["position"]["S"].ToString();
                    string lastLogin = data["lastLogin"]["S"].ToString();
                    string gamerIndate = data["gamerInDate"]["S"].ToString();
                    int donateGoods = int.Parse(data["totalGoods3Amount"]["N"].ToString());
                    int donateDogFeedAmounts = int.Parse(data["totalGoods5Amount"]["N"].ToString());
                    bool todayDonated = int.Parse(data["totalGoods9Amount"]["N"].ToString()) >= 1;
                    bool todayDonatedPetExp = int.Parse(data["totalGoods8Amount"]["N"].ToString()) >= 1;
                    bool todayGuildBoss = int.Parse(data["totalGoods6Amount"]["N"].ToString()) >= 1;

                    if (todayDonated)
                    {
                        attenNum++;

                    }

                    var memberData = new GuildMemberInfo(nickName, position, lastLogin, gamerIndate, donateGoods, todayDonated, todayDonatedPetExp, todayGuildBoss, donateDogFeedAmounts);

                    memberCells[i].Initialize(memberData);
                    memberCells[i].transform.SetAsFirstSibling();

                    if (findMyData == false)
                    {
                        findMyData = nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName);
                    }

                    if(nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName)) 
                    {
                        this.myMemberInfo = memberData;
                    }

                }
                else
                {
                    memberCells[i].gameObject.SetActive(false);
                }
            }

            attenUserNum.Value = attenNum;

#if UNITY_EDITOR
            attenUserNum.Value = 29;
#endif

            if (findMyData == false)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var data = rows[i];

                    string nickName = data["nickname"]["S"].ToString();

                    string position = data["position"]["S"].ToString();
                    string lastLogin = data["lastLogin"]["S"].ToString();
                    string gamerIndate = data["gamerInDate"]["S"].ToString();
                    int donateGoods = int.Parse(data["totalGoods3Amount"]["N"].ToString());
                    int donateDogFeedAmount = int.Parse(data["totalGoods5Amount"]["N"].ToString());
                    bool todayDonated = int.Parse(data["totalGoods9Amount"]["N"].ToString()) >= 1;
                    bool todayDonatedPetExp = int.Parse(data["totalGoods8Amount"]["N"].ToString()) >= 1;
                    bool todayGuildBoss = int.Parse(data["totalGoods6Amount"]["N"].ToString()) >= 1;

                    //내꺼찾음
                    if (nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName))
                    {
                        var memberData = new GuildMemberInfo(nickName, position, lastLogin, gamerIndate, donateGoods, todayDonated, todayDonatedPetExp, todayGuildBoss, donateDogFeedAmount);
                        memberCells[0].Initialize(memberData);
                        memberCells[0].transform.SetAsFirstSibling();

                        this.myMemberInfo = memberData;
                        break;
                    }
                }
            }

            for (int i = 0; i < memberCells.Count; i++)
            {
                if (i < guildMemberCount)
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
