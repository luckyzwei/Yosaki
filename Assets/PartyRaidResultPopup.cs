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
              double score = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

              //랭킹등록
              RankManager.Instance.UpdateRealBoss_Score(score);
              //
              PartyRaidManager.Instance.OnClickCloseButton();

              GameManager.Instance.LoadNormalField();

          }, () => { });
    }

    public void UpdateScoreBoard()
    {
        PartyRaidManager.Instance.NetworkManager.UpdateResultScore();

        totalText.SetText($"최종 피해량 : { Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}");
    }
}
