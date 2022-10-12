using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinueOpenButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class PointerDownEvent : UnityEvent { }

    [SerializeField]
    private float clickDelay = 0.08f;

    [SerializeField]
    private PointerDownEvent OnEvent;
    public PointerDownEvent onEvent => OnEvent;

    private Coroutine autoClickRoutine;

    [SerializeField]
    private Image buttonGraphic;

    [SerializeField]
    private Color pressedColor;

    private Color originColor;

    public bool canExecute = true;

    private void Awake()
    {
        SetOriginColor();
    }

    private void OnEnable()
    {
        SetButtonColor(originColor);
    }

    private void SetOriginColor()
    {
        if (buttonGraphic != null)
        {
            originColor = buttonGraphic.color;
        }
    }

    private void SetButtonColor(Color color)
    {
        if (buttonGraphic != null)
        {
            buttonGraphic.color = color;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        StopAutoClickRoutine();
    }

    private void Update()
    {
        if (Input.touchCount >= 2)
        {
            StopAutoClickRoutine();
        }
    }

    public void StopAutoClickRoutine()
    {
        if (autoClickRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoClickRoutine);
            autoClickRoutine = null;
        }
    }

    private void StartClickRoutine()
    {
        StopAutoClickRoutine();
        autoClickRoutine = CoroutineExecuter.Instance.StartCoroutine(ButtonClickRoutine());
    }

    private IEnumerator ButtonClickRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(clickDelay);

        if (canExecute)
        {
            OnEvent?.Invoke();
        }

        //선딜레이
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (canExecute)
            {
                OnEvent?.Invoke();
            }
            yield return delay;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartClickRoutine();
        SetButtonColor(pressedColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAutoClickRoutine();
        SetButtonColor(originColor);
    }

}
