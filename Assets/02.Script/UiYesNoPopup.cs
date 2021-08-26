using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiYesNoPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Button confirmButton;

    private Action yesCallBack;
    private Action noCallBack;


    public void Initialize(string title, string description, Action yesCallBack, Action noCallBack)
    {
        confirmButton.interactable = true;

        if (this.title != null)
        {
            this.title.SetText(title);
        }

        this.description.SetText(description);
        this.yesCallBack = yesCallBack;
        this.noCallBack = noCallBack;
    }

    public void OnClickYesButton()
    {
        confirmButton.interactable = false;
        yesCallBack?.Invoke();
        GameObject.Destroy(this.gameObject);
    }

    public void OnClickNoButton()
    {
        noCallBack?.Invoke();
        GameObject.Destroy(this.gameObject);
    }
}
