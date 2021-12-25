using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiGuildRankBoard;

public class UiGuildTopRankerCell : MonoBehaviour
{
    [SerializeField]
    private Image guildIcon;

    [SerializeField]
    private TextMeshProUGUI guildName;

    [SerializeField]
    private TextMeshProUGUI score;

    public void Initialize(GuildRankInfo rankInfo)
    {
        guildIcon.gameObject.SetActive(false);
        score.gameObject.SetActive(false);

        Backend.Social.Guild.GetGuildInfoV3(rankInfo.indate, bro =>
        {
            // 이후 처리
            if (bro.IsSuccess())
            {
                guildIcon.gameObject.SetActive(true);
                score.gameObject.SetActive(true);

                var returnValue = bro.GetReturnValuetoJSON();
                var data = returnValue["guild"];

                guildIcon.sprite = CommonUiContainer.Instance.guildIcon[int.Parse(returnValue["guild"]["guildIcon"]["N"].ToString())];

                guildName.SetText(returnValue["guild"]["guildName"]["S"].ToString());

                score.SetText($"{rankInfo.score}점");

            }
            else
            {

            }
        });
    }
}
