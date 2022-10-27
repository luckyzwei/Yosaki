using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiPartyRaidRewardBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingMask;

    [SerializeField]
    private TextMeshProUGUI scoreDescription;


    private void OnEnable()
    {
        UpdateGuildScore();
    }

    private void UpdateGuildScore()
    {
        loadingMask.SetActive(true);

        scoreDescription.SetText("불러오는중..");

        Backend.Social.Guild.GetMyGuildGoodsV3((guildInfoBro) =>
        {
            if (guildInfoBro.IsSuccess())
            {
                var returnValue = guildInfoBro.GetReturnValuetoJSON();

                int currentScore = int.Parse(returnValue["goods"]["totalGoods7Amount"]["N"].ToString());

                //
                var serverData = ServerData.bossServerTable.TableDatas["b73"];

                serverData.score.Value = currentScore.ToString();

                ServerData.bossServerTable.UpdateData("b73");

                scoreDescription.SetText($"문파 점수 : {currentScore}점");
                //
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "점수 갱신 실패", null);
            }

            loadingMask.SetActive(false);
            // 이후 처리
        });

    }
}
