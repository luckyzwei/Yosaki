using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGuildRankBoard;

public class UiGuildRankCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rankText;

    [SerializeField]
    private TextMeshProUGUI guildName;

    [SerializeField]
    private TextMeshProUGUI score;

    [SerializeField]
    private List<GameObject> rankImage;

    public void Initialize(GuildRankInfo rankInfo)
    {
        rankText.gameObject.SetActive(rankInfo.rank >= 4);

        for (int i = 0; i < rankImage.Count; i++)
        {
            rankImage[i].gameObject.SetActive((i + 1) == rankInfo.rank);
        }

        guildName.SetText(rankInfo.guildName);

        rankText.SetText(rankInfo.rank.ToString());

        score.SetText($"{rankInfo.score}점");
    }
}
