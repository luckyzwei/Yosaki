using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildRequestMemberList : SingletonMono<UiGuildRequestMemberList>
{
    [SerializeField]
    private UiGuildMemberRequestCell cellPrefab;

    [SerializeField]
    private Transform cellParents;

    private List<UiGuildMemberRequestCell> cells;

    void Start()
    {
        Refresh();
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
    }

    public void Refresh()
    {
        if (UiGuildMemberList.Instance.GetMyGuildGrade() == UiGuildMemberCell.GuildGrade.Member)
        {
            PopupManager.Instance.ShowAlarmMessage("권한이 없습니다.");
            return;
        }

        var bro = Backend.Social.Guild.GetApplicantsV3(50);

        if (bro.IsSuccess())
        {
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
        }
        else
        {
            if (cells != null)
            {
                cells.ForEach(e => e.gameObject.SetActive(false));
            }
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"조회 실패\n잠시후 다시 시도해 주세요\n{bro.GetStatusCode()} {bro.GetMessage()}", null);
        }
    }
}


