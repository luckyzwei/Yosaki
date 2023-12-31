﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class UiGradeTestBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private Image costumeIcon;

    private void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Subscribe() 
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            costumeIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail((int)e);
        }).AddTo(this);
    }


    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.gradeScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetGradeTestGrade();

        if (grade != -1)
        {
            gradeText.SetText($"{grade + 1}단계");
        }
        else
        {
            gradeText.SetText("없음");
        }


    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.GradeTest);
        }, () => { });
    }
}
