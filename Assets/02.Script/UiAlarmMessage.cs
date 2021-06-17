using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiAlarmMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    public void Initialize(string description)
    {
        descriptionText.SetText(description);
    }
    public void SelfDestroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
