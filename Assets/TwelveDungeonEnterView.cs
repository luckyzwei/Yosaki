using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class TwelveDungeonEnterView : MonoBehaviour
{
    [SerializeField]
    private RectTransform popupBg;

    [SerializeField]
    private float popupOriginWidth;

    [SerializeField]
    private float popupDokebiWidth;

    [SerializeField]
    private GameObject enterButton;

    private void OnEnable()
    {
        popupBg.sizeDelta = new Vector2(popupDokebiWidth, popupBg.sizeDelta.y);
        enterButton.SetActive(false);
    }

    private void OnDisable()
    {
        popupBg.sizeDelta = new Vector2(popupOriginWidth, popupBg.sizeDelta.y);
    }
}
