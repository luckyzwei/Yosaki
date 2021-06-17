using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundPlayer : MonoBehaviour
{
    public static string buttonSoundKey = "Button";

    [SerializeField]
    private string buttonKey = null;

    void Start()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                PlaySound();
            }
            );
        }
    }

    private void PlaySound()
    {
        if (buttonKey == null || string.IsNullOrEmpty(buttonKey))
        {
            SoundManager.Instance.PlayButtonSound();
        }
        else
        {
            SoundManager.Instance.PlaySound(buttonKey);
        }
    }
}
