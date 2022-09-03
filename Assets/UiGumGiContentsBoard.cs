using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiGumGiContentsBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreDescription;
    public Button enterButton;
    public Button registerButton;

    public TextMeshProUGUI getButtonDesc;
    public TextMeshProUGUI expDescription;
    public TextMeshProUGUI abilDescription;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].AsObservable().Subscribe(e =>
        {

            scoreDescription.SetText($"{e}");

        }).AddTo(this);


        ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "획득함");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).AsObservable().Subscribe(e =>
        {
            expDescription.SetText($"{e}");
        }).AddTo(this);

    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.GumGi);
            enterButton.interactable = false;
        }, () => { });


    }

    public void OnClickEnterSoulButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.GumGiSoul);
            enterButton.interactable = false;
        }, () => { });
    }

    public void OnClickGetFireButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SP)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLogType("GumGi", "_", score.ToString());
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SP)} {score}개 획득!", null);
            });
        }, null);
    }
}
