using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiGuildMemberCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nickName;
    [SerializeField]
    private TextMeshProUGUI lastLogin;
    [SerializeField]
    private TextMeshProUGUI donateAmount;
    public GuildMemberInfo guildMemberInfo { get; private set; }

    public GameObject kickButton;

    public enum GuildGrade
    {
        Member, ViceMaster, Master
    }

    public class GuildMemberInfo
    {
        public string nickName { get; private set; }
        public GuildGrade guildGrade { get; private set; }
        public string lastLogin { get; private set; }
        public string gamerIndate { get; private set; }

        public int donateGoods { get; private set; }


        public GuildMemberInfo(string nickName, string position, string lastLogin, string gamerIndate, int donateGoods)
        {
            this.nickName = nickName;

            switch (position)
            {
                case "master": { guildGrade = GuildGrade.Master; } break;
                case "member": { guildGrade = GuildGrade.Member; } break;
                default: { guildGrade = GuildGrade.ViceMaster; } break;
            }

            this.lastLogin = lastLogin;

            this.gamerIndate = gamerIndate;

            this.donateGoods = donateGoods;
        }
    }

    public void Initialize(GuildMemberInfo guildMemberInfo)
    {
        this.guildMemberInfo = guildMemberInfo;

        nickName.SetText($"{guildMemberInfo.nickName}({CommonString.GetGuildGradeName(this.guildMemberInfo.guildGrade)})");

        donateAmount.SetText(Utils.ConvertBigNum(guildMemberInfo.donateGoods));

        lastLogin.SetText(guildMemberInfo.lastLogin);

        RefreshKickButton();
    }

    public void RefreshKickButton()
    {
        if (UiGuildMemberList.Instance.GetMyGuildGrade() == GuildGrade.Member)
        {
            kickButton.SetActive(false);
        }
        else if (UiGuildMemberList.Instance.GetMyGuildGrade() == GuildGrade.ViceMaster)
        {
            kickButton.SetActive(PlayerData.Instance.NickName != guildMemberInfo.nickName && guildMemberInfo.guildGrade == GuildGrade.Member);
        }
        else if (UiGuildMemberList.Instance.GetMyGuildGrade() == GuildGrade.Master)
        {
            kickButton.SetActive(PlayerData.Instance.NickName != guildMemberInfo.nickName);
        }
    }

    public void OnClickKickButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", $"({guildMemberInfo.nickName})유저를 추방 하시겠습니다?", () =>
        {
            var bro = Backend.Social.Guild.ExpelMemberV3(guildMemberInfo.gamerIndate);

            if (bro.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "추방에 성공했습니다!", null);
                UiGuildMemberList.Instance.RemovePlayer(guildMemberInfo.nickName);

            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup("오류", $"권한이 없거나 이미 탈퇴한 유저 입니다.\n{bro.GetStatusCode()}", null);
            }
        }, null);

    }

}
