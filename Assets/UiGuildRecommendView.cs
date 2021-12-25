using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiGuildRecommendView : MonoBehaviour
{
    [SerializeField]
    private List<UiGuildListCell> guildListCell;

    [SerializeField]
    private GameObject loadFailObject;

    [SerializeField]
    private Button refreshButton;

    [SerializeField]
    private TMP_InputField guildNameInputField;

    void Start()
    {
        RefreshGuildList(false);
    }

    private void RefreshGuildList(bool showAlarmMessage = true)
    {
#if UNITY_ANDROID
        var bro = Backend.Social.Guild.GetRandomGuildInfoV3("isAnd", 1, 0, 5);
#endif
#if UNITY_IOS
        var bro = Backend.Social.Guild.GetRandomGuildInfoV3("isAnd",0,0,5);
#endif

        loadFailObject.SetActive(!bro.IsSuccess());

        if (bro.IsSuccess())
        {
            int rowCount = bro.Rows().Count;

            for (int i = 0; i < guildListCell.Count; i++)
            {
                if (i < rowCount)
                {
                    guildListCell[i].gameObject.SetActive(true);
                    guildListCell[i].Initialize(bro.Rows()[i]);
                }
                else
                {
                    guildListCell[i].gameObject.SetActive(false);
                }
            }

            if (showAlarmMessage)
            {
                PopupManager.Instance.ShowAlarmMessage("갱신 완료");
            }
            ////example
            //for (int i = 0; i < bro.Rows().Count; i++)
            //{

            //}
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("갱신 실패");
        }


    }
    private Coroutine refreshDelay;
    private WaitForSeconds refreshDelayWs = new WaitForSeconds(1.0f);
    private IEnumerator RefreshRoutine()
    {
        if (refreshButton != null)
        {
            refreshButton.interactable = false;
        }

        yield return refreshDelayWs;

        if (refreshButton != null)
        {
            refreshButton.interactable = true;
        }

        refreshDelay = null;
    }
    public void OnClickRefreshButton()
    {
        if (refreshDelay == null)
        {
            refreshDelay = CoroutineExecuter.Instance.StartCoroutine(RefreshRoutine());
            RefreshGuildList();
        }
    }

    public void OnClickNameSearchButton()
    {
        BackendReturnObject bro = Backend.Social.Guild.GetGuildIndateByGuildNameV3(guildNameInputField.text);

        if (bro.IsSuccess())
        {
            string guildIndate = bro.GetReturnValuetoJSON()["guildInDate"]["S"].ToString();

            var bro2 = Backend.Social.Guild.GetGuildInfoV3(guildIndate);

            if (bro2.IsSuccess())
            {
                int guildNum = int.Parse(bro2.GetReturnValuetoJSON()["guild"]["memberCount"]["N"].ToString());
                bool isInstantAcceptGuild = bro2.GetReturnValuetoJSON()["guild"].ContainsKey("_immediateRegistration") &&
                     bro2.GetReturnValuetoJSON()["guild"]["_immediateRegistration"]["BOOL"].ToString().Equals("True");

                bool isAndroidGuild = int.Parse(bro2.GetReturnValuetoJSON()["guild"]["isAnd"]["N"].ToString()) == 1;

#if UNITY_ANDROID
                if (isAndroidGuild == false) 
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "가입할 수 없는 문파 입니다.", null);
                    return;
                }
#endif

#if UNITY_IOS
                if (isAndroidGuild == true)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "가입할 수 없는 문파 입니다.", null);
                    return;
                }
#endif

                if (guildNum >= GameBalance.GuildMemberMax)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "인원이 가득찬 문파 입니다.", null);
                }
                else
                {
                    var bro3 = Backend.Social.Guild.ApplyGuildV3(guildIndate);

                    if (bro3.IsSuccess())
                    {
                        if (isInstantAcceptGuild == false)
                        {
                            PopupManager.Instance.ShowConfirmPopup("알림", "가입 신청 완료!", null);
                        }
                        else
                        {
                            PopupManager.Instance.ShowConfirmPopup("알림", "문파 가입 완료!!", null);
                            GuildManager.Instance.LoadGuildInfo();
                        }
                    }
                    else
                    {
                        switch (bro3.GetStatusCode())
                        {

                            //이미 가입한 길드
                            case "409":
                                {
                                    PopupManager.Instance.ShowConfirmPopup("알림", "이미 가입 요청된 문파 입니다.", null);
                                }
                                break;
                            //이미 길드가 있음
                            case "412":
                                {
                                    PopupManager.Instance.ShowConfirmPopup("알림", "이미 가입된 문파가 있습니다.", null);
                                }
                                break;
                            default:
                                {
                                    PopupManager.Instance.ShowConfirmPopup("알림", $"가입 요청 실패\n{bro.GetStatusCode()}", null);
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "존재하지 않는 문파 입니다.", null);
            }

        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "존재하지 않는 문파 입니다.", null);
        }

    }
}
