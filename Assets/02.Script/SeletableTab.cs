using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeletableTab : MonoBehaviour
{
    [SerializeField]
    private List<Image> tabs;
    [SerializeField]
    private List<TextMeshProUGUI> tabTexts;
    [SerializeField]
    private List<GameObject> objects;

    [SerializeField]
    private Color enableColor = Color.white;
    [SerializeField]
    private Color disableColor = Color.grey;

    [SerializeField]
    private Color enableColor_text = Color.white;
    [SerializeField]
    private Color disableColor_text = Color.grey;

    private void Awake()
    {
        SetDefault();
    }

    private void SetDefault()
    {
        OnSelect(0);
    }

    private void OnEnable()
    {
        //OnSelect(0);
    }

    public void OnSelect(int select)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabTexts.Count == tabs.Count)
            {
                tabTexts[i].color = i == select ? enableColor_text : disableColor_text;
            }

            tabs[i].color = i == select ? enableColor : disableColor;
            objects[i].gameObject.SetActive(i == select);
        }
    }

}
