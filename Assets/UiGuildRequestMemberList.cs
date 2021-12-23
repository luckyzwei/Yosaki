using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildRequestMemberList : SingletonMono<UiGuildRequestMemberList>
{
    [SerializeField]
    private UiGuildMemberRequestCell cellPrefab;

    [SerializeField]
    private Transform cellParents;

    private List<UiGuildMemberRequestCell> cells;

    [SerializeField]
    private GameObject emptyText;

    [SerializeField]
    private Button allAcceptButton;

    [SerializeField]
    private Button requireAgreeButton;

    [SerializeField]
    private GameObject acceptRequireObjects;

    private void OnEnable()
    {
        acceptRequireObjects.SetActive(UiGuildMemberList.Instance.GetMyGuildGrade() == UiGuildMemberCell.GuildGrade.Master);

        RefreshRequreButtons();
    }
    void Start()
    {
        Refresh();
    }

    private void RefreshRequreButtons()
    {
        if (UiGuildMemberList.Instance.GetMyGuildGrade() != UiGuildMemberCell.GuildGrade.Master) return;

        if (GuildManager.Instance.guildInfoData != null)
        {
            if (GuildManager.Instance.guildInfoData.ContainsKey("_immediateRegistration")
                && GuildManager.Instance.guildInfoData["_immediateRegistration"]["BOOL"].ToString().Equals("True"))
            {
                allAcceptButton.interactable = false;
                requireAgreeButton.interactable = true;
            }
            else
            {
                allAcceptButton.interactable = true;
                requireAgreeButton.interactable = false;
            }
        }

    }



    public void OnClickRequireAcceptButton()
    {
        var bro = Backend.Social.Guild.SetRegistrationValueV3(false);

        if (bro.IsSuccess())
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "변경 완료! 문파에 가입 하려면 승인이 필요합니다.", null);

            GuildManager.Instance.LoadGuildInfo();

            RefreshRequreButtons();
        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문주가 아니거나,문파에 가입하지 않은 유저 입니다.", null);
        }
    }

    public void OnClickAllAcceptButton()
    {
        var bro = Backend.Social.Guild.SetRegistrationValueV3(true);

        if (bro.IsSuccess())
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "변경 완료! 승인 없이 유저가 가입 됩니다.", null);

            GuildManager.Instance.LoadGuildInfo();

            RefreshRequreButtons();
        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문주가 아니거나,문파에 가입하지 않은 유저 입니다.", null);
        }

    }

    public void DisableInCell(string nickName)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].nickName.Equals(nickName))
            {
                cells[i].gameObject.SetActive(false);
            }
        }

        CheckDisableText();
    }

    private void CheckDisableText()
    {
        if (cells == null)
        {
            emptyText.SetActive(false);
            return;
        }

        int activeCount = 0;

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].gameObject.activeInHierarchy)
            {
                activeCount++;
                break;
            }
        }

        emptyText.SetActive(activeCount == 0);
    }

    public void Refresh()
    {
        if (UiGuildMemberList.Instance.GetMyGuildGrade() == UiGuildMemberCell.GuildGrade.Member)
        {
            PopupManager.Instance.ShowAlarmMessage("권한이 없습니다.");
            CheckDisableText();
            return;
        }

        var bro = Backend.Social.Guild.GetApplicantsV3(50);

        if (bro.IsSuccess())
        {
            if (cells != null)
            {
                cells.ForEach(e => e.gameObject.SetActive(false));
            }

            cells = new List<UiGuildMemberRequestCell>();

            var returnValue = bro.GetReturnValuetoJSON();

            var rows = returnValue["rows"];

            int makeCount = rows.Count - cells.Count;

            for (int i = 0; i < makeCount; i++)
            {
                cells.Add(Instantiate<UiGuildMemberRequestCell>(cellPrefab, cellParents));
            }

            for (int i = 0; i < cells.Count; i++)
            {
                if (i < rows.Count)
                {
                    var data = rows[i];
                    cells[i].gameObject.SetActive(true);
                    cells[i].Initialize(data["nickname"]["S"].ToString(), data["inDate"]["S"].ToString());
                }
                else
                {
                    cells[i].gameObject.SetActive(false);
                }
            }

            PopupManager.Instance.ShowAlarmMessage("갱신 완료");
        }
        else
        {
            if (cells != null)
            {
                cells.ForEach(e => e.gameObject.SetActive(false));
            }
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"조회 실패\n잠시후 다시 시도해 주세요\n{bro.GetStatusCode()} {bro.GetMessage()}", null);
        }


        CheckDisableText();
    }
}


