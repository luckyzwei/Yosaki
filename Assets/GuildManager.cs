using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GuildManager : SingletonMono<GuildManager>
{
    public ReactiveProperty<bool> hasGuild = new ReactiveProperty<bool>(false);

    // public List<GuildMemberInfo> guildMemberInfos;

    public string myGuildIndate { get; private set; }

    public string myGuildName => guildInfoData == null ? string.Empty : guildInfoData["guildName"]["S"].ToString();

    public LitJson.JsonData guildInfoData;

    public ReactiveProperty<int> guildIconIdx = new ReactiveProperty<int>();



    //public struct GuildMemberInfo
    //{
    //    public string nickName;
    //    public GuildGrade guildGrade;


    //    public GuildMemberInfo(string nickName, GuildGrade guildGrade)
    //    {
    //        this.guildGrade = guildGrade;
    //        this.nickName = nickName;
    //    }
    //}


    public void ChangeHasGuildState(bool state)
    {
        hasGuild.Value = state;
    }
    public void LoadGuildInfo()
    {
        var bro = Backend.Social.Guild.GetMyGuildInfoV3();

        if (bro.IsSuccess())
        {
            // guildMemberInfos = new List<GuildMemberInfo>();

            var returnValue = bro.GetReturnValuetoJSON();

            guildInfoData = returnValue["guild"];

            this.myGuildIndate = returnValue["guild"]["inDate"]["S"].ToString();

            ChangeHasGuildState(true);

            guildIconIdx.Value = int.Parse(returnValue["guild"]["guildIcon"]["N"].ToString());

            //string masterNick = returnValue["guild"]["masterNickname"]["S"].ToString();


            // guildMemberInfos.Add(new GuildMemberInfo(masterNick, GuildGrade.Master));

            //for (int i = 0; i < returnValue["guild"]["viceMasterList"]["L"].Count; i++)
            //{
            //    string nickName = returnValue["guild"]["viceMasterList"]["L"][i]["S"].ToString();
            //    guildMemberInfos.Add(new GuildMemberInfo(nickName, GuildGrade.ViceMaster));
            //}

        }
        else
        {
            ChangeHasGuildState(false);

            switch (bro.GetStatusCode())
            {
                //Old guild의 유저가 조회한 경우
                //statusCode : 412
                //errorCode: PreconditionFailed
                //message : guild's version is different 사전 조건을 만족하지 않습니다.

                //guild가 없는 유저가 조회한 경우
                //statusCode : 412
                //errorCode: PreconditionFailed
                //message : notGuildMember 사전 조건을 만족하지 않습니다.
                case "412":
                    {
                        bro.GetErrorCode().Equals("PreconditionFailed");
                    }
                    break;
            }
        }

    }
}
