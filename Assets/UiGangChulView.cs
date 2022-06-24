using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGangChulView : SingletonMono<UiGangChulView>
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

    [SerializeField]
    private GameObject guildRecordObject;

    public ObscuredInt rewardGrade = 0;

    [SerializeField]
    private Button recordButton;

    void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.hasGuild.AsObservable().Subscribe(e =>
        {
            guildRecordObject.SetActive(e);
        }).AddTo(this);
    }
    private void OnEnable()
    {
        var serverData = ServerData.bossServerTable.TableDatas["boss20"];

        if (serverData.score.Value != "0")
        {
            RankManager.Instance.UpdateRealBoss_Score_GangChul(double.Parse(serverData.score.Value));
        }
        //this.transform.SetAsLastSibling();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[20]);
    }

    public void RecordGuildScoreButton()
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

        bool alreadyRecord = ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value == 1;



        if (alreadyRecord)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 이미 점수를 추가했습니다.");
            return;
        }
        if (rewardGrade == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("추가할 점수가 없습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{rewardGrade}점 점수를 추가합니까?\n<color=red>점수는 하루에 한번만 추가할 수 있습니다.</color>\n문파별로 최대 인원만큼만 추가 가능합니다.\n(매일 오전 5시 초기화)",
            () =>
            {
                if (alreadyRecord)
                {
                    PopupManager.Instance.ShowAlarmMessage("오늘은 이미 점수를 추가했습니다.");
                    return;
                }
                if (rewardGrade == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("추가할 점수가 없습니다.");
                    return;
                }

                recordButton.interactable = false;

                var guildInfoBro = Backend.Social.Guild.GetMyGuildGoodsV3();

                if (guildInfoBro.IsSuccess())
                {
                    var returnValue = guildInfoBro.GetReturnValuetoJSON();

                    int addAmount = int.Parse(returnValue["goods"]["totalGoods7Amount"]["N"].ToString());

                    bool maxContributed = addAmount >= GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value);

                    if (maxContributed)
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{GuildManager.Instance.myGuildName} 문파는 \n오늘 더이상 점수를 추가할 수 없습니다!\n<color=red>최대 {GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value)}번 추가 가능</color>\n<color=red>(매일 오전 5시 초기화)</color>", null);
                        recordButton.interactable = true;
                        return;
                    }
                }
                else
                {
                    recordButton.interactable = true;
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오류가 발생했습니다. 잠시후 다시 시도해 주세요.", null);
                    return;
                }

                ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value = 1;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();

                userInfoParam.Add(UserInfoTable.sendGangChul, ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    var bro2 = Backend.URank.Guild.ContributeGuildGoods(RankManager.Rank_Guild_Reset_GangChul_Uuid, goodsType.goods7, 1);

                    if (bro2.IsSuccess())
                    {

                        var bro = Backend.URank.Guild.ContributeGuildGoods(RankManager.Rank_GangChul_Guild_Boss_Uuid, goodsType.goods6, rewardGrade);

                        if (bro.IsSuccess())
                        {
                            recordButton.interactable = true;

                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "점수 추가 완료!", null);

                            //var time = ServerData.userInfoTable.currentServerTime;

                            //UiGuildChatBoard.Instance.SendRankScore_System($"<color=yellow>{PlayerData.Instance.NickName}님이 {rewardGrade}점을 추가했습니다.({time.Month}월 {time.Day}일 {time.Hour}시)");

                            //var memberCell = UiGuildMemberList.Instance.GetMemberCell(PlayerData.Instance.NickName);

                            //if (memberCell != null)
                            //{
                            //    memberCell.UpdateGuildBossObject(true);
                            //}
                        }
                        else
                        {
                            recordButton.interactable = true;

                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 추가에 실패했습니다\n점수 갱신 시간이 아닙니다.\n({bro.GetStatusCode()})", null);

                            ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value = 0;

                            List<TransactionValue> transactions2 = new List<TransactionValue>();

                            Param userInfoParam2 = new Param();

                            userInfoParam2.Add(UserInfoTable.sendGangChul, ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value);

                            transactions2.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam2));

                            ServerData.SendTransaction(transactions2, successCallBack: () =>
                            {

                            });
                        }
                    }
                    else
                    {
                        recordButton.interactable = true;

                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 추가에 실패했습니다\n점수 갱신 시간이 아닙니다.\n({bro2.GetStatusCode()})", null);

                        ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value = 0;

                        List<TransactionValue> transactions2 = new List<TransactionValue>();

                        Param userInfoParam2 = new Param();

                        userInfoParam2.Add(UserInfoTable.sendGangChul, ServerData.userInfoTable.TableDatas[UserInfoTable.sendGangChul].Value);

                        transactions2.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam2));

                        ServerData.SendTransaction(transactions2, successCallBack: () =>
                        {

                        });
                    }

                });
            }, null);
    }
}
