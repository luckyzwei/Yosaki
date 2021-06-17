using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiConfirmPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    private Action confirmCallBack;

    private bool closeWhenConfirm;

    public void Initialize(string title, string description, Action confirmCallBack, bool closeWhenConfirm = true)
    {
        this.closeWhenConfirm = closeWhenConfirm;
        this.title.SetText(title);
        this.description.SetText(description);
        this.confirmCallBack = confirmCallBack;
    }

    public void OnClickConfirmButton()
    {
        confirmCallBack?.Invoke();

        if (closeWhenConfirm)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void OnClickCafeButton() 
    {
        Application.OpenURL("https://cafe.naver.com/madaki");
    }
}
