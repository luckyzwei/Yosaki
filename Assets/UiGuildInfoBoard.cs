using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiGuildInfoBoard : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI title;

    private void OnEnable()
    {
        SetGuildTitle();
    }


    private void SetGuildTitle()
    {
        title.SetText(GuildManager.Instance.guildInfoData["guildName"]["S"].ToString());
    }
    public void OnClickExitButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "문파를 탈퇴 합니까?\n문주는 문파원이 0명일때 탈퇴 가능</color>", () =>
        {
            var bro = Backend.Social.Guild.WithdrawGuildV3();

            if (bro.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "탈퇴 완료", null);
                GuildManager.Instance.ChangeHasGuildState(false);

            }
            else
            {
                var errorCode = bro.GetMessage();

                switch (errorCode)
                {
                    case "memberExist 사전 조건을 만족하지 않습니다.":
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문주는 문파원이 한명도 없어야 탈퇴 가능 합니다!", null);
                        }
                        break;
                    case "subscribed guild 사전 조건을 만족하지 않습니다.":
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문파에 가입되지 않은 유저 입니다!", null);
                        }
                        break;
                }
            }
        }, null);
    }
}
