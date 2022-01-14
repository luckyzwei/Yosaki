using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGuildMemberCell;

public class UiGuildGradeChangeBoard : SingletonMono<UiGuildGradeChangeBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI currentGradeText;

    private string indate;

    private string nickName;

    private GuildGrade currentGrade;

    [SerializeField]
    private GameObject memberButton;

    [SerializeField]
    private GameObject viceMasterButton;

    [SerializeField]
    private GameObject masterButton;

    public void Initialize(string nickName, string indate)
    {
        this.nickName = nickName;

        this.indate = indate;

        rootObject.SetActive(true);

        currentGrade = UiGuildMemberList.Instance.GetGuildGrade(nickName);

        currentGradeText.SetText(CommonString.GetGuildGradeName(currentGrade));

        memberButton.SetActive(currentGrade != GuildGrade.Member);

        viceMasterButton.SetActive(currentGrade != GuildGrade.ViceMaster);

        masterButton.SetActive(currentGrade != GuildGrade.Master);
    }


    //Success cases
    //지명,해제,위임에 성공한 경우
    //statusCode : 204
    //message : Success

    //Error cases
    //길드에 없는 유저일 경우
    //statusCode : 404
    //errorCode : NotFoundException
    //message : guildMember not found, guildMember을(를) 찾을 수 없습니다

    public void OnClickMemberButton()
    {
        GuildGrade myGuildGrade = UiGuildMemberList.Instance.GetMyGuildGrade();

        if (myGuildGrade != GuildGrade.Master)
        {
            PopupManager.Instance.ShowAlarmMessage("권한이 없습니다.");
            return;
        }

        if (currentGrade == GuildGrade.Member)
        {
            PopupManager.Instance.ShowAlarmMessage($"이미 {CommonString.GetGuildGradeName(currentGrade)}입니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{nickName.Replace(CommonString.IOS_nick, "")}를 {CommonString.GetGuildGradeName(GuildGrade.Member)}로 변경 합니까?", () =>
        {
            var bro = Backend.Social.Guild.ReleaseViceMasterV3(indate);

            if (bro.IsSuccess())
            {
                var cell = UiGuildMemberList.Instance.GetMemberCell(nickName);

                if (cell != null)
                {
                    cell.UpdateGradeText(GuildGrade.Member);
                }

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"변경 성공!", null);
                rootObject.SetActive(false);
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"변경 실패\n존재하지 않는 유저\n({bro.GetErrorCode()})", null);
            }
        }, null);
    }


    public void OnClickViceMasterButton()
    {
        GuildGrade myGuildGrade = UiGuildMemberList.Instance.GetMyGuildGrade();

        if (myGuildGrade != GuildGrade.Master)
        {
            PopupManager.Instance.ShowAlarmMessage("권한이 없습니다.");
            return;
        }

        if (currentGrade == GuildGrade.ViceMaster)
        {
            PopupManager.Instance.ShowAlarmMessage($"이미 {CommonString.GetGuildGradeName(currentGrade)}입니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{nickName.Replace(CommonString.IOS_nick, "")}를 {CommonString.GetGuildGradeName(GuildGrade.ViceMaster)}로 변경 합니까?", () =>
        {
            var bro = Backend.Social.Guild.NominateViceMasterV3(indate);

            if (bro.IsSuccess())
            {
                var cell = UiGuildMemberList.Instance.GetMemberCell(nickName);

                if (cell != null)
                {
                    cell.UpdateGradeText(GuildGrade.ViceMaster);
                }

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"변경 성공!", null);
                rootObject.SetActive(false);
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"변경 실패\n존재하지 않는 유저\n({bro.GetErrorCode()})", null);
            }
        }, null);



    }
    public void OnClickMasterButton()
    {
        GuildGrade myGuildGrade = UiGuildMemberList.Instance.GetMyGuildGrade();

        if (myGuildGrade != GuildGrade.Master)
        {
            PopupManager.Instance.ShowAlarmMessage("권한이 없습니다.");
            return;
        }

        if (currentGrade == GuildGrade.Master)
        {
            PopupManager.Instance.ShowAlarmMessage($"이미 {CommonString.GetGuildGradeName(currentGrade)}입니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{nickName.Replace(CommonString.IOS_nick, "")}를 {CommonString.GetGuildGradeName(GuildGrade.Master)}로 변경 합니까?", () =>
        {
            var bro = Backend.Social.Guild.NominateMasterV3(indate);

            if (bro.IsSuccess())
            {
                var cell = UiGuildMemberList.Instance.GetMemberCell(nickName);

                if (cell != null)
                {
                    cell.UpdateGradeText(GuildGrade.Master);
                }

                var myCell = UiGuildMemberList.Instance.GetMemberCell(PlayerData.Instance.NickName);

                if (myCell != null)
                {
                    myCell.UpdateGradeText(GuildGrade.Member);
                }

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"변경 성공!", null);
                rootObject.SetActive(false);
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"변경 실패\n존재하지 않는 유저\n({bro.GetErrorCode()})", null);
            }
        }, null);


    }


}
