using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSusanoBuff : SingletonMono<UiSusanoBuff>
{
    public static ReactiveProperty<bool> isImmune = new ReactiveProperty<bool>(false);
    public int immuneCount = 0;
    public void ActiveSusanoImmune()
    {
        if (immuneCount > 0) return;
        if (GameManager.contentsType == GameManager.ContentsType.PartyRaid) return;
        if (GameManager.contentsType == GameManager.ContentsType.PartyRaid_Guild) return;
        if (GameManager.contentsType == GameManager.ContentsType.DokebiTower) return;
        if (GameManager.contentsType == GameManager.ContentsType.Ok) return;
        if (GameManager.contentsType == GameManager.ContentsType.Yum) return;
        if (GameManager.contentsType == GameManager.ContentsType.Do) return;
        if (GameManager.contentsType == GameManager.ContentsType.Sasinsu) return;
        if (GameManager.contentsType == GameManager.ContentsType.Online_Tower) return;
        if (GameManager.contentsType == GameManager.ContentsType.GradeTest) return;
        if (GameManager.contentsType == GameManager.ContentsType.SumisanTower) return;
        //산신령
        if (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon &&
            (GameManager.Instance.bossId == 57 ||
            GameManager.Instance.bossId == 72 || // 서재
            GameManager.Instance.bossId == 75 ||//도깨비나라
            GameManager.Instance.bossId == 76 ||
            GameManager.Instance.bossId == 77 ||
            GameManager.Instance.bossId == 78 ||
            GameManager.Instance.bossId == 79 ||
            GameManager.Instance.bossId == 80 ||
            GameManager.Instance.bossId == 81 ||
            GameManager.Instance.bossId == 82 ||
            GameManager.Instance.bossId == 83 ||
            GameManager.Instance.bossId == 85 ||
            GameManager.Instance.bossId == 86 ||
            GameManager.Instance.bossId == 87 ||
            GameManager.Instance.bossId == 88
            //도깨비 지킴이

            )) return;


        int susanoGrade = PlayerStats.GetSusanoGrade();
        if (susanoGrade == -1) return;

        var tableData = TableManager.Instance.susanoTable.dataArray[susanoGrade];
        if (tableData.Buffsec == 0) return;

        immuneCount = 1;

        PlayerStatusController.Instance.SetHpToMax();

        isImmune.Value = true;

        StartCoroutine(ImmuneRoutine());
    }

    private IEnumerator ImmuneRoutine()
    {
        int susanoGrade = PlayerStats.GetSusanoGrade();

        var tableData = TableManager.Instance.susanoTable.dataArray[susanoGrade];

        float tick = 0f;

        while (tick < tableData.Buffsec)
        {
            tick += Time.deltaTime;

            yield return null;
        }

        isImmune.Value = false;
    }

}
