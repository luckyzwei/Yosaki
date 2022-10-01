using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiFlowerBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreDescription;
    public Button enterButton;
    public Button registerButton;

    public TextMeshProUGUI getButtonDesc;
    public TextMeshProUGUI levelUpDesc;
    public TextMeshProUGUI expDescription;
    public TextMeshProUGUI abilDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].AsObservable().Subscribe(e =>
        {

            scoreDescription.SetText($"{e}");

        }).AddTo(this);


        ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Cw).AsObservable().Subscribe(e =>
        {
            levelUpDesc.SetText($"+{e}");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Cw).AsObservable().Subscribe(e =>
        {

            expDescription.SetText($"LV:{e}");

            UpdateAbilDescription();

        }).AddTo(this);
    }

    private void UpdateAbilDescription()
    {
        string abil = string.Empty;

        abil += $"<color=#ff00ffff>{CommonString.GetStatusName(StatusType.growthStoneUp)} {PlayerStats.GetSmithValue(StatusType.growthStoneUp)}개\n";
        abil += $"<color=red>{CommonString.GetStatusName(StatusType.WeaponHasUp)} {PlayerStats.GetSmithValue(StatusType.WeaponHasUp)}배\n";
        abil += $"<color=blue>{CommonString.GetStatusName(StatusType.NorigaeHasUp)} {PlayerStats.GetSmithValue(StatusType.NorigaeHasUp)}배\n";
        abil += $"<color=yellow>{CommonString.GetStatusName(StatusType.PetEquipHasUp)} {PlayerStats.GetSmithValue(StatusType.PetEquipHasUp)}배\n";
        abil += $"<color=#00ffffff>{CommonString.GetStatusName(StatusType.PetEquipProbUp)} {PlayerStats.GetSmithValue(StatusType.PetEquipProbUp)}%";

        abilDescription.SetText(abil);
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.ChunFlower);
            enterButton.interactable = false;
        }, () => { });
    }

    public void OnClickGetFireButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Cw)}는 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += score;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getFlower, ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Cw)} {score}개 획득!", null);
            });
        }, null);
    }



#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Fw).Value += 5000;
        }
    }
#endif
}
