using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildIconCell : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    private int idx;

    public void Initialize(int idx)
    {
        this.idx = idx;
        icon.sprite = CommonUiContainer.Instance.guildIcon[idx];
    }
    public void OnCliCkIconButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "문파 아이콘을 변경 합니까?", () =>
        {
            Param param = new Param();

            param.Add("guildIcon", idx);

            var bro = Backend.Social.Guild.ModifyGuildV3(param);

            if (bro.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문파 아이콘 변경 완료!", null);
                GuildManager.Instance.guildIconIdx.Value = idx;
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"아이콘 변경에 실패했습니다.\n({bro.GetStatusCode()})", null);
            }
        }, null);
    }
}
