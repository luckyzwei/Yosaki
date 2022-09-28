using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSubHpBar : SingletonMono<UiSubHpBar>
{
    [SerializeField]
    private Image greenRenderer;
    [SerializeField]
    private Image greyRenderer;

    private float fixedLerpSpeed = 5f;


    private void OnEnable()
    {
        ResetGauge();

        StartCoroutine(GreyRoutine());
    }


    private void ResetGauge()
    {
        greenRenderer.fillAmount = 1f;
        greyRenderer.fillAmount = 1f;
    }

    private IEnumerator GreyRoutine()
    {
        while (true)
        {
            float lerpValue = Mathf.Lerp(greyRenderer.fillAmount, greenRenderer.fillAmount, Time.deltaTime * fixedLerpSpeed);

            greyRenderer.fillAmount = lerpValue;

            yield return null;
        }
    }

    public void UpdateGauge(double currentHp, double maxHp)
    {
        if (maxHp == 0f) return;

        greenRenderer.fillAmount = (float)(currentHp / maxHp);
    }
}
