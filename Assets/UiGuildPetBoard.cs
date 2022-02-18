using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiGuildPetBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI petExpText;

    [SerializeField]
    private TextMeshProUGUI sendAmountText;

    [SerializeField]
    private TextMeshProUGUI petDescription;

    private ObscuredFloat exchangeGoodsNum;

    private ObscuredInt maxSendNum = 10;
    private ObscuredFloat eachMarbleNum = 2000000f;
    private ObscuredFloat eachGrowthStoneNum = 200000000f;

    private void Start()
    {
        Subscribe();
        WhenValueChanged("1");
    }

    private void OnEnable()
    {
        GuildManager.Instance.LoadGuildLevelGoods();
    }

    public void WhenValueChanged(string goodsNum)
    {
        int goods = int.Parse(goodsNum);

        if (goods == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"최소 {Utils.ConvertBigNum(eachMarbleNum)}개부터 먹이로 주실 수 있습니다.");
        }

        this.exchangeGoodsNum = Mathf.Clamp(goods, 1, maxSendNum);

        sendAmountText.SetText($"먹이 : {CommonString.GetItemName(Item_Type.Marble)} {Utils.ConvertBigNum(exchangeGoodsNum * eachMarbleNum)}개 \n레벨 {exchangeGoodsNum}상승\n{CommonString.GetItemName(Item_Type.GrowthStone)} {Utils.ConvertBigNum(eachGrowthStoneNum * exchangeGoodsNum)}개 획득");
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildPetExp.AsObservable().Subscribe(e =>
        {
            petExpText.SetText($"LV : {e}");

            petDescription.SetText($"{CommonString.GetStatusName(StatusType.ExpGainPer)} {PlayerStats.GetGuildPetEffect(StatusType.ExpGainPer)*100f}\n{CommonString.GetStatusName(StatusType.GoldGainPer)} {PlayerStats.GetGuildPetEffect(StatusType.GoldGainPer) * 100f}\n{CommonString.GetStatusName(StatusType.AttackAddPer)} {PlayerStats.GetGuildPetEffect(StatusType.AttackAddPer) * 100f}\n");
        }).AddTo(this);
    }

    public void SendPetExp()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 먹이를 줄 수 없습니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value < exchangeGoodsNum * eachMarbleNum)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Marble)} {Utils.ConvertBigNum(exchangeGoodsNum * eachMarbleNum)}개를 먹이로 줄까요?\n레벨 {exchangeGoodsNum}상승", () =>
          {
              ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value = 1;

              var broForGuildLevel = Backend.Social.Guild.ContributeGoodsV3(goodsType.goods5, (int)exchangeGoodsNum);

              if (broForGuildLevel.IsSuccess())
              {
                  GuildManager.Instance.guildPetExp.Value += (int)exchangeGoodsNum;

                  ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= exchangeGoodsNum * eachMarbleNum;
                  ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += exchangeGoodsNum * eachGrowthStoneNum;

                  List<TransactionValue> transactions = new List<TransactionValue>();

                  Param userInfoParam = new Param();

                  userInfoParam.Add(UserInfoTable.sendPetExp, ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value);

                  Param goodsParam = new Param();

                  goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                  goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);

                  transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                  transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                  ServerData.SendTransaction(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"레벨 {exchangeGoodsNum}증가,\n{CommonString.GetItemName(Item_Type.GrowthStone)} {Utils.ConvertBigNum(exchangeGoodsNum * eachGrowthStoneNum)}개 획득!", null);
                        GuildManager.Instance.LoadGuildLevelGoods();
                    });
              }
              else
              {
                  ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value = 0;
              }

          }, () => { });
    }

}
