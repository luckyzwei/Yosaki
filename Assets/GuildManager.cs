using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GuildManager : SingletonMono<GuildManager>
{
    public ReactiveProperty<bool> hasGuild = new ReactiveProperty<bool>(false);

    // public List<GuildMemberInfo> guildMemberInfos;

    public string myGuildIndate { get; private set; } = string.Empty;

    public string myGuildName => guildInfoData.Value == null ? string.Empty : guildInfoData.Value["guildName"]["S"].ToString();

    public ReactiveProperty<LitJson.JsonData> guildInfoData = new ReactiveProperty<LitJson.JsonData>();

    public ReactiveProperty<int> guildLevelExp = new ReactiveProperty<int>(0);

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

            guildInfoData.Value = returnValue["guild"];

            this.myGuildIndate = returnValue["guild"]["inDate"]["S"].ToString();

            ChangeHasGuildState(true);

            guildIconIdx.Value = int.Parse(returnValue["guild"]["guildIcon"]["N"].ToString());

            LoadGuildLevelGoods();
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

    public void LoadGuildLevelGoods()
    {
        if (string.IsNullOrEmpty(myGuildIndate))
        {
            return;
        }

        //굿즈정보 요청
        var guildGoodsBro = Backend.Social.Guild.GetGuildGoodsByIndateV3(myGuildIndate);

        if (guildGoodsBro.IsSuccess())
        {
            guildLevelExp.Value = int.Parse(guildGoodsBro.GetReturnValuetoJSON()["goods"]["totalGoods4Amount"]["N"].ToString());
        }
        else
        {

        }
    }

    public int GetGuildLevel(int goodsAmount)
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        int level = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (goodsAmount >= tableData[i].Needamount)
            {
                level = tableData[i].Id;
            }
        }

        return level;
    }

    public int GetGuildMemberMaxNum(int exp = 0)
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        int maxAddNum = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].GUILDLEVELTYPE != guildLevelType.guildMemberPlus) continue;

            if (exp >= tableData[i].Needamount)
            {
                maxAddNum += (int)tableData[i].Value;
            }
        }

        return GameBalance.GuildMemberMax + maxAddNum;
    }

    public bool HasGuildBuff(int buffIdx)
    {
        int myGuildExp = guildLevelExp.Value;

        var tableData = TableManager.Instance.GuildLevel.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].GUILDLEVELTYPE != guildLevelType.guildBuff) continue;

            if (myGuildExp < tableData[i].Needamount) continue;

            if (buffIdx != (int)tableData[i].Value) continue;

            return true;
        }

        return false;
    }

    public int GetBuffGetExp(int buffIdx)
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].GUILDLEVELTYPE != guildLevelType.guildBuff) continue;

            if (buffIdx == (int)tableData[i].Value) return tableData[i].Needamount;
        }

        return 999;
    }

    public bool HasGuildIcon(int grade)
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        int myExp = GuildManager.Instance.guildLevelExp.Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].GUILDLEVELTYPE != guildLevelType.guildIcon) continue;
            if (myExp < tableData[i].Needamount) continue;
            if ((int)tableData[i].Value >= grade) return true;
        }

        return false;
    }

    public int GetGuildIconExp(int grade)
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        int myExp = GuildManager.Instance.guildLevelExp.Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].GUILDLEVELTYPE != guildLevelType.guildIcon) continue;
            if ((int)tableData[i].Value == grade)
            {
                return tableData[i].Needamount;
            }
        }

        return 0;
    }
}
