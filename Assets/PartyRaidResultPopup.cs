using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static NetworkManager;
using System;
using Photon.Pun;
using BackEnd;

public class PartyRaidResultPopup : SingletonMono<PartyRaidResultPopup>
{
    [SerializeField]
    private TextMeshProUGUI totalText;

    [SerializeField]
    private GameObject recordButton;

    [SerializeField]
    private GameObject leaveOnlyButton;

    [SerializeField]
    private GameObject rewardChartButton;

    private void UpdateButton()
    {
        rewardChartButton.SetActive(PartyRaidManager.Instance.NetworkManager.IsGuildBoss());

        if (PartyRaidManager.Instance.NetworkManager.IsGuildBoss() == false)
        {
            recordButton.SetActive(true);
            leaveOnlyButton.SetActive(true);
        }
        else
        {
            recordButton.SetActive(PhotonNetwork.IsMasterClient);
            leaveOnlyButton.SetActive(!PhotonNetwork.IsMasterClient);
        }

    }


    private void OnEnable()
    {
        UpdateScoreBoard();

        UpdateButton();
    }

    public void OnClickRegistScoreButton()
    {
        if (PartyRaidManager.Instance.NetworkManager.IsGuildBoss() == false)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"점수를 등록하고 나갈가요?\n<color=red>(주의)현재 결과화면에 기록된 점수의 합으로 등록 됩니다.\n총 : {Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}점", () =>
            {

                RecordPartyRaidScore();
                LeaveRoom();

            }, () => { });
        }
        else
        {
            int totalScore = GetGuildPartyTotalScore();

            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{totalScore}점을 갱신하고 나갈가요?\n<color=red>(주의)현재 결과화면에 기록된 점수의 합을 기반으로 계산 됩니다.", () =>
            {

                RecordGuildRaidScore();
                LeaveRoom();

            }, () => { });
        }


    }

    private void RecordPartyRaidScore()
    {
        double totalScore = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

        //로컬 점수 등록

        var twelveBossTable = TableManager.Instance.TwelveBossTable.dataArray[55];

        var serverData = ServerData.bossServerTable.TableDatas[twelveBossTable.Stringid];

        //랭킹등록
        RankManager.Instance.UpdateChunmaTop(totalScore);

        if (string.IsNullOrEmpty(serverData.score.Value) == false)
        {
            if (totalScore < double.Parse(serverData.score.Value))
            {
                //return;
            }
            else
            {

                serverData.score.Value = totalScore.ToString();

                ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
            }
        }
        else
        {


            serverData.score.Value = totalScore.ToString();

            ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
        }
    }

    private int GetGuildPartyTotalScore()
    {
        double totalScore = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

        int ret = 0;

        var tableData = TableManager.Instance.TwelveBossTable.dataArray[73];

        for (int i = 0; i < tableData.Rewardcut.Length; i++)
        {
            if (totalScore < tableData.Rewardcut[i])
            {
                ret = i + 1;
                break;
            }
        }

        return ret;
    }

    private void RecordGuildRaidScore()
    {
        bool canRecord = ServerData.userInfoTable.CanRecordGuildScore();

#if UNITY_EDITOR
        canRecord = true;
#endif
        if (canRecord == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오후11시~ 다음날 오전5시 까지는\n점수를 등록할 수 없습니다!", null);
            return;
        }

        int totalScore = GetGuildPartyTotalScore();

        if (totalScore == 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "추가할 점수가 없습니다.", null);
            return;
        }

        recordButton.gameObject.SetActive(false);


        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{totalScore}점 점수를 추가합니까?",
            () =>
            {
                if (totalScore == 0)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "추가할 점수가 없습니다.", null);
                    recordButton.gameObject.SetActive(true);
                    return;
                }

                var guildInfoBro = Backend.Social.Guild.GetMyGuildGoodsV3();

                if (guildInfoBro.IsSuccess())
                {
                    var returnValue = guildInfoBro.GetReturnValuetoJSON();

                    int currentScore = int.Parse(returnValue["goods"]["totalGoods7Amount"]["N"].ToString());

                    int interval = totalScore - currentScore;

                    if (interval > 0)
                    {
                        var bro2 = Backend.URank.Guild.ContributeGuildGoods(RankManager.Rank_Party_Guild_Uuid, goodsType.goods7, interval);

                        if (bro2.IsSuccess())
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "점수 추가 완료!", null);
                            recordButton.gameObject.SetActive(true);
                            return;
                        }
                        else
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 추가에 실패했습니다\n점수 갱신 시간이 아닙니다.\n({bro2.GetStatusCode()})", null);
                            recordButton.gameObject.SetActive(true);
                            return;
                        }
                    }
                    //낮은점수
                    else
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"최고점수를 갱신하지 못했습니다\n최고점수 {currentScore}점", null);
                        recordButton.gameObject.SetActive(true);
                        return;
                    }

                }
                else
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오류가 발생했습니다. 잠시후 다시 시도해 주세요.", null);
                    recordButton.gameObject.SetActive(true);
                    return;
                }

            }, () =>
            {

            });
    }

    public void LeaveRoom()
    {

        //서버 연결 끊기
        PartyRaidManager.Instance.OnClickCloseButton();

        //로비로 이동하기
        GameManager.Instance.LoadNormalField();
    }

    public void UpdateScoreBoard()
    {
        PartyRaidManager.Instance.NetworkManager.UpdateResultScore();

        totalText.SetText($"최종 피해량 : { Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}");
    }
}
