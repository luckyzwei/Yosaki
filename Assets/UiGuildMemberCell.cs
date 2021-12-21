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
    }

}
