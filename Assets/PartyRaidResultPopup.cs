using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static NetworkManager;
using System;

public class PartyRaidResultPopup : SingletonMono<PartyRaidResultPopup>
{
    [SerializeField]
    private TextMeshProUGUI totalText;

    private void OnEnable()
    {
        UpdateScoreBoard();
    }

    public void OnClickRegistScoreButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"점수를 등록하고 나갈가요?\n<color=red>(주의)현재 결과화면에 기록된 점수의 합으로 등록 됩니다.\n총 : {Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}점", () =>
          {
              double totalScore = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

              //로컬 점수 등록

              var twelveBossTable = TableManager.Instance.TwelveBossTable.dataArray[55];

              var serverData = ServerData.bossServerTable.TableDatas[twelveBossTable.Stringid];

              if (string.IsNullOrEmpty(serverData.score.Value) == false)
              {
                  if (totalScore < double.Parse(serverData.score.Value))
                  {
                      //return;
                  }
                  else
                  {
                      //랭킹등록
                      RankManager.Instance.UpdateChunmaTop(totalScore);

                      serverData.score.Value = totalScore.ToString();

                      ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
                  }
              }
              else
              {
                  //랭킹등록
                  RankManager.Instance.UpdateChunmaTop(totalScore);

                  serverData.score.Value = totalScore.ToString();

                  ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
              }

              //서버 연결 끊기
              PartyRaidManager.Instance.OnClickCloseButton();

              //로비로 이동하기
              GameManager.Instance.LoadNormalField();


          }, () => { });
    }

    public void UpdateScoreBoard()
    {
        PartyRaidManager.Instance.NetworkManager.UpdateResultScore();

        totalText.SetText($"최종 피해량 : { Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}");
    }
}
