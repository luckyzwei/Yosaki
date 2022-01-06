using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiGuildLevelAbilCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI unlockLevel;

    [SerializeField]
    private GameObject lockMask;

    private GuildLevelData guildLevelData;

    private bool subscribed = false;

    public void Initialize(GuildLevelData guildLevelData)
    {
        this.guildLevelData = guildLevelData;

        description.SetText(guildLevelData.Description);

        unlockLevel.SetText($"LV : {GuildManager.Instance.GetGuildLevel(guildLevelData.Needamount)}에 해금");

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildLevelExp.AsObservable().Subscribe(e =>
        {
            lockMask.gameObject.SetActive(e < guildLevelData.Needamount);
        }).AddTo(this);
    }
}
