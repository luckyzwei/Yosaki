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

    private Coroutine greyRoutine;

    private float decreaseTime = 1f;

    private void OnEnable()
    {
        ResetGauge();
    }


    private void ResetGauge()
    {
        greenRenderer.fillAmount = 1f;
        greyRenderer.fillAmount = 1f;
    }

    private IEnumerator GreyRoutine()
    {
        float tick = 0f;

        float greenFillAmount = greenRenderer.fillAmount;

        while (tick < decreaseTime)
        {
            tick += Time.deltaTime;

            float lerpValue = Mathf.Lerp(greyRenderer.fillAmount, greenFillAmount, tick / decreaseTime);

            greyRenderer.fillAmount = lerpValue;

            yield return null;
        }

        greyRenderer.fillAmount = greenRenderer.fillAmount;
        greyRoutine = null;
    }

    public void UpdateGauge(float currentHp, float maxHp)
    {
        if (maxHp == 0f) return;

        greenRenderer.fillAmount = currentHp / maxHp;

        if (greyRoutine != null)
        {
            StopCoroutine(greyRoutine);
        }

        if (this.gameObject.activeInHierarchy)
        {
            greyRoutine = StartCoroutine(GreyRoutine());
        }
    }
}
