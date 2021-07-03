using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private List<GameObject> hardIcon;

    private void OnEnable()
    {
        ResetGauge();

        SetOderInlayer();
    }

    private void SetOderInlayer()
    {
        int rand = Random.Range(0, 20);

        frameRenderer.sortingOrder = minOrderInLayer + rand;

        backRenderer.sortingOrder = minOrderInLayer + rand + 1;

        greyRenderer.sortingOrder = minOrderInLayer + rand + 2;

        greenRenderer.sortingOrder = minOrderInLayer + rand + 3;
    }

    private void Awake()
    {
        SetOriginYScale();
    }

    private void SetOriginYScale()
    {
        originYScale = greenRenderer.transform.localScale.y;
    }

    private void ResetGauge()
    {
        greenRenderer.transform.localScale = new Vector2(1f, originYScale);
        greyRenderer.transform.localScale = new Vector2(1f, originYScale);
    }

    private IEnumerator GreyRoutine()
    {
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

    public void SetHardIcon(int grade)
    {
        for (int i = 0; i < hardIcon.Count; i++)
        {
            hardIcon[i].gameObject.SetActive(grade == i);
        }
    }

    public void UpdateGauge(float currentHp, float maxHp)
    {
        if (maxHp == 0f) return;

        greenRenderer.transform.localScale = new Vector2(Mathf.Max(0f, currentHp / maxHp), originYScale);

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
