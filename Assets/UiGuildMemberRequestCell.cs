using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiGuildMemberRequestCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nickNameText;

    public string nickName { get; private set; }

    private string indate;

    public void Initialize(string nickName, string indate)
    {
        this.nickName = nickName;
        this.indate = indate;

        nickNameText.SetText(this.nickName.Replace(CommonString.IOS_nick, ""));
    }

    public void OnClickAcceptButton()
    {
        if (UiGuildMemberList.Instance.guildMemberCount >= GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelGoods.Value))
        {
            PopupManager.Instance.ShowAlarmMessage($"문파원이 가득 찼습니다.(최대 {GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelGoods.Value)}명)");
            return;
        }

        var bro = Backend.Social.Guild.ApproveApplicantV3(this.indate);

        if (bro.IsSuccess())
        {
            UiGuildRequestMemberList.Instance.DisableInCell(nickName);
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"가입 승인 완료", null);
            UiGuildMemberList.Instance.guildMemberCount++;
        }
        else
        {
            switch (bro.GetStatusCode())
            {
                case "412":
                    {
                        UiGuildRequestMemberList.Instance.DisableInCell(nickName);
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"이미 문파에 가입된 유저 입니다.", null);
                    }
                    break;
                case "429":
                    {
                        UiGuildRequestMemberList.Instance.DisableInCell(nickName);
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"문파원이 가득 찼습니다.", null);
                    }
                    break;
                default:
                    {
                        PopupManager.Instance.ShowConfirmPopup("오류", $"서버가 불안정 합니다.\n{bro.GetStatusCode()} {bro.GetMessage()}", null);
                    }
                    break;
            }
        }
    }
    public void OnClickRejectButton()
    {
        var bro = Backend.Social.Guild.RejectApplicantV3(this.indate);

        if (bro.IsSuccess())
        {
            UiGuildRequestMemberList.Instance.DisableInCell(nickName);
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"가입 거절 완료", null);
        }
        else
        {
            switch (bro.GetStatusCode())
            {
                default:
                    {
                        PopupManager.Instance.ShowConfirmPopup("오류", $"서버가 불안정 합니다.\n{bro.GetStatusCode()} {bro.GetMessage()}", null);
                    }
                    break;
            }
        }
    }
}
