using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PartyRaidButton : MonoBehaviour
{
    [SerializeField]
    private ContentsType contentsType = ContentsType.PartyRaid;

    private int GetDogFeedCount()
    {
        var guildInfoBro = Backend.Social.Guild.GetMyGuildGoodsV3();

        if (guildInfoBro.IsSuccess())
        {
            var returnValue = guildInfoBro.GetReturnValuetoJSON();

            int currentScore = int.Parse(returnValue["goods"]["totalGoods5Amount"]["N"].ToString());

            return currentScore / 10;

        }

        return 0;
    }

    public void OnClickPartyRaidButton()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 100000)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨 10만부터 입장하실 수 있습니다.");
            return;
        }

        if (contentsType == ContentsType.PartyRaid_Guild) 
        {
            if (GuildManager.Instance.hasGuild.Value == false)
            {
                PopupManager.Instance.ShowAlarmMessage("문파에 가입되어 있어야 합니다.");
                return;
            }

            int dogFeedCount = GetDogFeedCount();

            if (dogFeedCount < GameBalance.sanGoonDogFeedCount)
            {
                PopupManager.Instance.ShowAlarmMessage($"견공에게 먹이를 3회 이상 줘야 합니다.\n현재 {dogFeedCount}회");
                return;
            }
        }

        PartyRaidManager.Instance.NetworkManager.contentsType = contentsType;

        PartyRaidManager.Instance.ActivePartyRaidBoard();
    }
}
