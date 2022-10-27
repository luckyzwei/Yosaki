using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    public void Initialize(string description)
    {
        this.description.SetText(description);
    }
}
