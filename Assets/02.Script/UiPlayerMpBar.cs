using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;


public class UiPlayerMpBar : MonoBehaviour
{
    [SerializeField]
    private Transform barObject;

    [SerializeField]
    private TextMeshProUGUI mpText;

    [SerializeField]
    private Animator animator;

    private static string PlayTrigger = "Play";

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        PlayerStatusController.Instance.maxMp.AsObservable().Subscribe(WhenMaxMpChanged).AddTo(this);
        PlayerStatusController.Instance.mp.AsObservable().Subscribe(WhenMpChanged).AddTo(this);
    }

    private void WhenMaxMpChanged(double value)
    {
        WhenMpChanged(value);
    }

    private void WhenMpChanged(double value)
    {
        animator.SetTrigger(PlayTrigger);

        float maxMp = (float)PlayerStatusController.Instance.maxMp.Value;
        float currentMp = (float)PlayerStatusController.Instance.mp.Value;

        mpText.SetText($"{(int)currentMp}/{(int)maxMp}");
        barObject.transform.localScale = new Vector3(currentMp / maxMp, barObject.transform.localScale.y, barObject.transform.localScale.z);
    }
}
