using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiGuildChatBoard : SingletonMono<UiGuildChatBoard>
{
    [SerializeField]
    private TextMeshProUGUI guildDescription;

    private void OnEnable()
    {
        if (GuildManager.Instance.guildInfoData != null)
        {
            string desc = GuildManager.Instance.guildInfoData["guildDesc"]["S"].ToString();
            guildDescription.SetText(desc);
        }
    }

    public void OnGuildDescriptionEditEnd(string desc)
    {
        if (string.IsNullOrEmpty(desc))
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "텍스트를 입력해 주세요", null);
            return;
        }

        Param param = new Param();

        param.Add("guildDesc", desc);

        var bro = Backend.Social.Guild.ModifyGuildV3(param);

        if (bro.IsSuccess())
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "소개글 등록 성공!", null);
            guildDescription.SetText(desc);
        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"소개글 등록에 실패했습니다.\n({bro.GetStatusCode()})", null);
        }

    }
}
