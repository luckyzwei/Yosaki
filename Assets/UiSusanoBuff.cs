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
        //산신령
        if (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 57) return;

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
