using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildBossView : SingletonMono<UiGuildBossView>
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

    public ObscuredInt rewardGrade = 0;

    void Start()
    {
        Initialize();
    }
    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[12]);
    }

    public void RecordGuildScoreButton()
    {

        if (ServerData.userInfoTable.TableDatas[UserInfoTable.SendGuildPoint].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 이미 점수를 추가했습니다.");
            return;
        }
        if (rewardGrade == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("추가할 점수가 없습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{rewardGrade}점 점수를 추가합니까?\n<color=red>점수는 하루에 한번만 추가할 수 있습니다.",
            () =>
            {
                ServerData.userInfoTable.TableDatas[UserInfoTable.SendGuildPoint].Value = 1;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();

                userInfoParam.Add(UserInfoTable.SendGuildPoint, ServerData.userInfoTable.TableDatas[UserInfoTable.SendGuildPoint].Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    var bro = Backend.URank.Guild.ContributeGuildGoods(RankManager.Rank_Guild_Uuid, goodsType.goods1, rewardGrade);

                    if (bro.IsSuccess())
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "점수 추가 완료!", null);

                        UiGuildChatBoard.Instance.SendRankScore($"<color=yellow>{PlayerData.Instance.NickName}님이 {rewardGrade}점을 추가했습니다.");

                    }
                    else
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 추가에 실패했습니다\n월요일 오전 4시~오전 5시에는 갱신할 수 없습니다\n({bro.GetStatusCode()})", null);

                        ServerData.userInfoTable.TableDatas[UserInfoTable.SendGuildPoint].Value = 0;

                        List<TransactionValue> transactions2 = new List<TransactionValue>();

                        Param userInfoParam2 = new Param();

                        userInfoParam2.Add(UserInfoTable.SendGuildPoint, ServerData.userInfoTable.TableDatas[UserInfoTable.SendGuildPoint].Value);

                        transactions2.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam2));

                        ServerData.SendTransaction(transactions2, successCallBack: () =>
                        {

                        });
                    }

                });
            }, null);
    }

}
