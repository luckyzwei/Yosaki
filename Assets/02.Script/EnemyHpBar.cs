using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer frameRenderer;
    [SerializeField]
    private SpriteRenderer backRenderer;
    [SerializeField]
    private SpriteRenderer greenRenderer;
    [SerializeField]
    private SpriteRenderer greyRenderer;

    private Coroutine greyRoutine;

    private float originYScale = -1f;

    private float decreaseTime = 0.5f;

    [SerializeField]
    private int minOrderInLayer = 99;

    private bool notUpdate = false;

    private void OnEnable()
    {
        ResetGauge();

        SetOderInlayer();
    }

    private void Subscribe()
    {
        SettingData.HpBar.AsObservable().Subscribe(e =>
        {

            notUpdate = e == 0;

            this.gameObject.SetActive(notUpdate == false);

        }).AddTo(this);
    }

    private void SetOderInlayer()
    {
        if (notUpdate == true) return;

        int rand = Random.Range(0, 20);

        frameRenderer.sortingOrder = minOrderInLayer + rand;

        backRenderer.sortingOrder = minOrderInLayer + rand + 1;

        greyRenderer.sortingOrder = minOrderInLayer + rand + 2;

        greenRenderer.sortingOrder = minOrderInLayer + rand + 3;
    }

    private void Awake()
    {
        SetOriginYScale();

        Subscribe();
    }

    private void SetOriginYScale()
    {
        originYScale = greenRenderer.transform.localScale.y;
    }

    private void ResetGauge()
    {
        if (notUpdate == true) return;
        greyRoutine = null;
        greenRenderer.transform.localScale = new Vector2(1f, originYScale);
        greyRenderer.transform.localScale = new Vector2(1f, originYScale);
    }

    private IEnumerator GreyRoutine()
    {
        if (notUpdate == true) yield break;

        float tick = 0f;

        float startXScale = greyRenderer.transform.localScale.x;

        while (tick < decreaseTime)
        {
            tick += Time.deltaTime;

            float lerpValue = Mathf.Lerp(startXScale, greenRenderer.transform.localScale.x, tick / decreaseTime);

            greyRenderer.transform.localScale = new Vector2(Mathf.Max(0f, lerpValue), originYScale);

            yield return null;
        }

        greyRenderer.transform.localScale = greenRenderer.transform.localScale;
        greyRoutine = null;
    }

    public void UpdateGauge(double currentHp, double maxHp)
    {
        if (notUpdate == true) return;

        if (maxHp == 0f) return;

        greenRenderer.transform.localScale = new Vector2(Mathf.Max(0f, (float)(currentHp / maxHp)), originYScale);

        if (greyRoutine == null)
        {
            if (this.gameObject.activeInHierarchy)
            {
                greyRoutine = StartCoroutine(GreyRoutine());
            }
        }
    }
}
