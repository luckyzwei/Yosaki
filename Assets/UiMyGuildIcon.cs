using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMyGuildIcon : MonoBehaviour
{
    private Image guildIcon;

    void Start()
    {
        guildIcon = GetComponent<Image>();
        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildIconIdx.AsObservable().Subscribe(e =>
        {
            guildIcon.sprite = CommonUiContainer.Instance.guildIcon[e];
        }).AddTo(this);
    }
}
