using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiPlayerHpBar : MonoBehaviour
{
    [SerializeField]
    private Transform barObject;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private Animator animator;

    private static string PlayTrigger = "Play";

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        PlayerStatusController.Instance.maxHp.AsObservable().Subscribe(WhenMaxHpChanged).AddTo(this);
        PlayerStatusController.Instance.hp.AsObservable().Subscribe(WhenHpChanged).AddTo(this);
    }

    private void WhenMaxHpChanged(double value)
    {
        WhenHpChanged(value);
    }

    private void WhenHpChanged(double value)
    {
        animator.SetTrigger(PlayTrigger);

        double maxHp = PlayerStatusController.Instance.maxHp.Value;
        double currentHp = PlayerStatusController.Instance.hp.Value;

        hpText.SetText($"{Utils.ConvertBigNum(currentHp)}/{Utils.ConvertBigNum(maxHp)}");
        barObject.transform.localScale = new Vector3((float)currentHp / (float)maxHp, barObject.transform.localScale.y, barObject.transform.localScale.z);
    }

}
