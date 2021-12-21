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
    private TextMeshProUGUI masterName;

    [SerializeField]
    private TextMeshProUGUI guildMemNumCount;

    private JsonData jsonData;

    private int memberCount;

    public void Initialize(JsonData jsonData)
    {
        //memberCount / masterNickname / guildName / param1(길드 소개글)

        this.jsonData = jsonData;

        string guildName = jsonData["guildName"].ToString();
        this.guildName.SetText(guildName);

        int guildIconIdx = int.Parse(jsonData["param1"].ToString());

        string description = jsonData["param2"].ToString();
        this.guildDescription.SetText(description);

        memberCount = int.Parse(jsonData["memberCount"].ToString());
        guildMemNumCount.SetText($"{memberCount}/{GameBalance.GuildMemberMax}");

        string masterName = jsonData["masterNickname"].ToString();
        this.masterName.SetText(masterName);
    }

    public void OnClickInfoButton()
    {


    }

    public void OnClickEnterButton()
    {
        if (memberCount >= GameBalance.GuildMemberMax)
        {
            PopupManager.Instance.ShowAlarmMessage("문파원이 가득차서 가입하실수 없습니다.");
            return;
        }

        string indate = jsonData["inDate"].ToString();

        var bro = Backend.Social.Guild.ApplyGuildV3(indate);

        if (bro.IsSuccess())
        {
            PopupManager.Instance.ShowConfirmPopup("알림", "가입 신청 완료!", null);

            //GuildManager.Instance.ChangeHasGuildState(true);

            //ServerData.userInfoTable.TableDatas[UserInfoTable.CanEnterGuild].Value = 0;
            //ServerData.userInfoTable.UpData(UserInfoTable.CanEnterGuild, false);
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
