using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiDescriptionBoard : MonoBehaviour
{
    [SerializeField]
    private string description;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI description_TMP;

    [SerializeField]
    private bool initInspector = true;

    public void SetDescription(string description)
    {
        description_TMP.SetText(description);
    }

    private void Start()
    {
        if (initInspector)
        {
            description_TMP.SetText(description);
        }

        rootObject.SetActive(false);
    }

    public void OnClickButton()
    {
        SoundManager.Instance.PlayButtonSound();
        rootObject.SetActive(true);
    }

}
