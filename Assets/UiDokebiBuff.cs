using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiDokebiBuff : SingletonMono<UiDokebiBuff>
{
    public static ReactiveProperty<bool> isImmune = new ReactiveProperty<bool>(false);

    public int immuneCount = 0;


    public void ActiveDokebiImmune()
    {
        if (immuneCount > 0) return;

        if (ServerData.weaponTable.TableDatas[CommonString.weapon79Key].hasItem.Value != 1)
        {
            return;
        }

        PlayerStatusController.Instance.SetHpToMax();

        immuneCount = 1;

        isImmune.Value = true;

        StartCoroutine(ImmuneRoutine());
    }

    private IEnumerator ImmuneRoutine()
    {

        float tick = 0f;

        while (tick < GameBalance.dokebiImmuneTime)
        {
            tick += Time.deltaTime;

            yield return null;
        }

        isImmune.Value = false;
    }

}
