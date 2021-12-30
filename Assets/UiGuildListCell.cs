using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildListCell : MonoBehaviour
{
    [SerializeField]
    private Image guildIcon;

    [SerializeField]
    private TextMeshProUGUI guildName;

    [SerializeField]
    private TextMeshProUGUI guildDescription;

    [SerializeField]
    private TextMeshProUGUI guildScore;

    [SerializeField]
    private TextMeshProUGUI masterName;

    [SerializeField]
    private TextMeshProUGUI guildMemNumCount;

    [SerializeField]
    private TextMeshProUGUI acceptDescription;

    [SerializeField]
    private Button enterButton;

    [SerializeField]
    private TextMeshProUGUI enterDescription;

    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private Color instantAcceptColor;

    [SerializeField]
    private Color needAcceptColor;

    private JsonData jsonData;

    private int memberCount;

    private bool isInstantAcceptGuild = false;

    public void Initialize(JsonData jsonData)
    {
        //memberCount / masterNickname / guildName / param1(길드 소개글)

        this.jsonData = jsonData;

        string guildName = jsonData["guildName"].ToString();
        this.guildName.SetText(guildName);

        int guildIconIdx = int.Parse(jsonData["guildIcon"].ToString());

        string description = jsonData["guildDesc"].ToString();
        this.guildDescription.SetText(description);

        memberCount = int.Parse(jsonData["memberCount"].ToString());
        guildMemNumCount.SetText($"{memberCount}/{GameBalance.GuildMemberMax}");

        string masterName = jsonData["masterNickname"].ToString();
        this.masterName.SetText(masterName);

        isInstantAcceptGuild = jsonData.ContainsKey("_immediateRegistration") && jsonData["_immediateRegistration"].ToString().Equals("True");

        guildIcon.sprite = CommonUiContainer.Instance.guildIcon[int.Parse(jsonData["guildIcon"].ToString())];

        acceptDescription.SetText(isInstantAcceptGuild ? "즉시가입" : "가입신청");
        
        buttonImage.color = isInstantAcceptGuild ? instantAcceptColor : needAcceptColor;

        //점수 조회
        string indate = jsonData["inDate"].ToString();

        guildScore.SetText("(점수 조회중)");

        Backend.Social.Guild.GetGuildGoodsByIndateV3(indate, bro =>
        {
            // 이후 처리
            if (bro.IsSuccess())
            {
                var data = bro.GetReturnValuetoJSON();

                if (guildScore != null)
                    guildScore.SetText($"(점수:{int.Parse(data["goods"]["totalGoods3Amount"]["N"].ToString())}점)");
            }
            else
            {
                if (guildScore != null)
                    guildScore.SetText("(점수 조회 실패)");
            }
        });
    }

    public void OnClickEnterButton()
    {
        string indate = jsonData["inDate"].ToString();

        enterButton.interactable = false;
        enterDescription.SetText("신청됨");

        SendQueue.Enqueue(Backend.Social.Guild.GetGuildInfoV3, indate, callback =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                int guildNum = int.Parse(callback.GetReturnValuetoJSON()["guild"]["memberCount"]["N"].ToString());
                if (guildNum >= GameBalance.GuildMemberMax)
                {
                    PopupManager.Instance.ShowConfirmPopup("알림", "문파원이 가득차서 가입하실수 없습니다.", null);

                    enterButton.interactable = true;
                    enterDescription.SetText("가입 신청");

                    return;
                }
                else
                {
                    SendQueue.Enqueue(Backend.Social.Guild.ApplyGuildV3, indate, bro =>
                     {
                         enterButton.interactable = true;
                         enterDescription.SetText("가입 신청");

                         if (bro.IsSuccess())
                         {
                             if (isInstantAcceptGuild == false)
                             {
                                 PopupManager.Instance.ShowConfirmPopup("알림", "문파 가입 신청 완료!", null);
                             }
                             else
                             {
                                 PopupManager.Instance.ShowConfirmPopup("알림", "문파 가입 완료!!", null);
                                 GuildManager.Instance.LoadGuildInfo();
                             }
                         }
                         else
                         {
                             switch (bro.GetStatusCode())
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


                     });
                }
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup("알림", $"가입 요청 실패\n{callback.GetStatusCode()}", null);

                enterButton.interactable = true;
                enterDescription.SetText("가입 신청");
            }
        });
    }

    //Success cases
    //요청에 성공한 경우
    //statusCode : 204
    //message : Success
    //
    //Error cases
    //콘솔 설정 조건에 맞지 않는 유저가 길드 가입 요청 시도한 경우
    //statusCode : 403
    //errorCode : ForbiddenError
    //message : Forbidden applyGuild, 금지된 applyGuild
    //
    //이미 가입 요청한 길드에 다시 가입 요청 한 경우
    //statusCode : 409
    //errorCode : DuplicatedParameterException
    //message : Duplicated alreadyRequestGamer, 중복된 alreadyRequestGamer 입니다
    //
    //이미 속해있는 길드가 존재하는 경우
    //statusCode : 412
    //errorCode : PreconditionFailed
    //message : JoinedGamer 사전 조건을 만족하지 않습니다.
    //
    //길드원이 이미 100명 이상인 경우
    //statusCode : 429
    //errorCode : Too Many Request
    //message : guild member count 요청 횟수를 초과하였습니다.
}
