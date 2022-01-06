﻿using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildIconCell : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private GameObject selectedFrame;

    private int idx;

    public void Initialize(int idx)
    {
        this.idx = idx;
        icon.sprite = CommonUiContainer.Instance.guildIcon[idx];

        Subscribe();
    }

    private void Subscribe()
    {

        GuildManager.Instance.guildIconIdx.AsObservable().Subscribe(e =>
        {

            selectedFrame.SetActive(idx == e);

        }).AddTo(this);

    }

    public void OnCliCkIconButton()
    {
        if (GuildManager.Instance.HasGuildIcon(CommonUiContainer.Instance.guildIconGrade[idx]) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("문파 경험치가 부족합니다.");
            return;
        }

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
