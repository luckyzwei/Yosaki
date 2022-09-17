using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static NetworkManager;

public class PartyRaidTotalScoreBoard : SingletonMono<PartyRaidTotalScoreBoard>
{
    [SerializeField]
    private List<TextMeshProUGUI> playerScores;

    [SerializeField]
    List<Color> gradeColor;

    public void UpdateScoreBoard(Dictionary<int, PlayerInfo> roomPlayerDatas)
    {

        var sortedDatas = from pair in roomPlayerDatas
                          orderby pair.Value.score descending
                          select pair;

        var sortedDict = sortedDatas.ToDictionary(x => x.Key, x => x.Value);

        var keys = sortedDict.Keys.ToList();


        for (int i = 0; i < playerScores.Count; i++)
        {
            if (i < keys.Count)
            {
                var data = sortedDict[keys[i]];

                playerScores[i].gameObject.SetActive(true);

                playerScores[i].SetText($"{data.nickName.Replace(CommonString.IOS_nick, "")} : {Utils.ConvertBigNum(data.score)}({(data.endGame ? "전투종료" : "전투중")})");

                playerScores[i].color = gradeColor[i];
            }
            else
            {
                playerScores[i].gameObject.SetActive(false);
            }
        }

    }

}
