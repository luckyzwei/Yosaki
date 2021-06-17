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

    private void WhenMaxHpChanged(float value)
    {
        WhenHpChanged(value);
    }

    private void WhenHpChanged(float value)
    {
        animator.SetTrigger(PlayTrigger);

        float maxHp = PlayerStatusController.Instance.maxHp.Value;
        float currentHp = PlayerStatusController.Instance.hp.Value;

        hpText.SetText($"{(int)currentHp}/{(int)maxHp}");
        barObject.transform.localScale = new Vector3(currentHp / maxHp, barObject.transform.localScale.y, barObject.transform.localScale.z);
    }

}
