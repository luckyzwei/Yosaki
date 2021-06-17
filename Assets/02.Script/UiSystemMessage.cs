using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSystemMessage : SingletonMono<UiSystemMessage>
{
    [SerializeField]
    private TextMeshProUGUI systemMessage;

    [SerializeField]
    private Animator animator;

    private new void Awake()
    {
        base.Awake();
        systemMessage.gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        if (systemMessage.gameObject.activeInHierarchy == false)
        {
            systemMessage.gameObject.SetActive(true);
        }

        systemMessage.SetText(text);
        animator.Play(0);
    }
}
