using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiReactionButton : SingletonMono<UiReactionButton>
{
    [SerializeField]
    private TextMeshProUGUI description;

    private Action callBack;

    [SerializeField]
    private Transform fitter1;

    [SerializeField]
    private Transform fitter2;

    [SerializeField]
    private GameObject rootObject;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        GameManager.Instance.whenSceneChanged.AsObservable().Subscribe(e =>
        {
            UiReactionButton.Instance.Show(false);
        }).AddTo(this);
    }

    public void Show(bool show)
    {
        rootObject.SetActive(show);
    }

    public void Initialize(string description, Action callBack)
    {
        if (this.description.text.Equals(description) == false)
        {
            this.description.SetText(description);
        }

        this.callBack = callBack;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter1.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter2.transform);
    }

    public void OnClickButton()
    {
        callBack?.Invoke();
    }
}
